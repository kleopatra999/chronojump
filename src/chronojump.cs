/*
 * This file is part of ChronoJump
 *
 * Chronojump is free software; you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *   the Free Software Foundation; either version 2 of the License, or   
 *    (at your option) any later version.
 *    
 * Chronojump is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the 
 *    GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 *  along with this program; if not, write to the Free Software
 *   Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 *
 * Xavier de Blas: 
 * http://www.xdeblas.com, http://www.deporteyciencia.com (parleblas)
 */


using System;
using Gtk;
using Mono.Unix;
using System.IO; //"File" things


public class ChronoJump 
{
	ChronoJumpWindow chronoJumpWin;
	
	private static string [] authors = {"Xavier de Blas", "Juan Gonzalez", "Juan Fernando Pardo"};
	private static string progversion = ""; //now in "version" file
	private static string progname = "Chronojump";
	
	private string runningFileName; //useful for knowing if there are two chronojump instances
	private string messageToShowOnBoot = "";
	private bool chronojumpHasToExit = false;
		
	//isFirstTime we run chronojump in this machine? 
	//(or is there a DB file?)
	private bool isFirstTime = false;



	public static void Main(string [] args) 
	{
		Catalog.Init ("chronojump", "./locale");
		new ChronoJump(args);
	}

	public ChronoJump (string [] args) 
	{
		bool timeLogPassedOk = Log.Start(args);
		Log.WriteLine(string.Format("Time log passed: {0}", timeLogPassedOk.ToString()));
		
		checkIfChronojumpExitAbnormally();

		/* SERVER COMMUNICATION TESTS */
		/*
		try {
			ChronojumpServer myServer = new ChronojumpServer();

			//example of list a dir in server
			string [] myListDir = myServer.ListDirectory("/home");
			foreach (string myResult in myListDir) 
				Log.WriteLine(myResult);

			Log.WriteLine(myServer.ConnectDatabase());
			//select name of person with uniqueid 1
			Log.WriteLine(myServer.SelectPersonName(1));
		}
		catch {
			Log.WriteLine("Unable to call server");
		}
		*/
		/* END OF SERVER COMMUNICATION TESTS */

		//print version of chronojump
		Log.WriteLine(string.Format("Chronojump version: {0}", readVersion()));

		//move database to new location if chronojump version is before 0.7
		moveDatabaseToInstallJammerLocationIfNeeded();

		Sqlite.Connect();

		//Chech if the DB file exists
		if (!Sqlite.CheckTables()) {
			Log.WriteLine ( Catalog.GetString ("no tables, creating ...") );
			Sqlite.CreateFile();
			File.Create(runningFileName);
			Sqlite.CreateTables();

			isFirstTime = true;
		} else {
			//backup the database
			Util.BackupDirCreateIfNeeded();

			Util.BackupDatabase();
			Log.WriteLine ("made a database backup"); //not compressed yet, it seems System.IO.Compression.DeflateStream and
			//System.IO.Compression.GZipStream are not in mono


			if(! Sqlite.IsSqlite3()) {
				bool ok = Sqlite.ConvertFromSqlite2To3();
				if (!ok) {
					Log.WriteLine("******\n problem with sqlite \n******");
					//check (spanish)
					//http://mail.gnome.org/archives/chronojump-devel-list/2008-March/msg00011.html
					string errorMessage = Catalog.GetString("Failed database conversion, ensure you have libsqlite3-0 installed. \nIf problems persist ask in chronojump-list");
					Log.WriteLine(errorMessage);
					messageToShowOnBoot += errorMessage;
					chronojumpHasToExit = true;
				}
				Sqlite.Connect();
			}

			bool softwareIsNew = Sqlite.ConvertToLastChronojumpDBVersion();
			if(! softwareIsNew) {
				//Console.Clear();
				string errorMessage = string.Format(Catalog.GetString ("Sorry, this Chronojump version ({0}) is too old for your database."), readVersion()) + "\n" +  
						Catalog.GetString("Please update Chronojump") + ":\n"; 
				errorMessage += "http://www.gnome.org/projects/chronojump/installation"; 
				errorMessage += "\n\n" + Catalog.GetString("Press any key");
				Log.WriteLine(errorMessage);
				messageToShowOnBoot += errorMessage;
				chronojumpHasToExit = true;
			}

			Log.WriteLine ( Catalog.GetString ("tables already created") ); 

			//check for bad Rjs (activate if program crashes and you use it in the same db before v.0.41)
			//SqliteJump.FindBadRjs();
		}


		messageToShowOnBoot += recuperateBrokenEvents();

		//start as "simulated"
		SqlitePreferences.Update("simulated", "True", false); //false (dbcon not opened)

		Util.IsWindows();	//only as additional info here
		
		Application.Init();

		if(messageToShowOnBoot.Length > 0) {
			ErrorWindow errorWin;
			if(chronojumpHasToExit) {
				messageToShowOnBoot += "\n<b>" + string.Format(Catalog.GetString("Chronojump will exit now.")) + "</b>\n";
				errorWin = ErrorWindow.Show(messageToShowOnBoot);
				errorWin.Button_accept.Clicked += new EventHandler(on_message_boot_accepted_quit);
				Application.Run();
			} else { 
				errorWin = ErrorWindow.Show(messageToShowOnBoot);
				errorWin.Button_accept.Clicked += new EventHandler(on_message_boot_accepted_continue);
				Application.Run();
			}
		} else {
			startChronojump();
			Application.Run();
		}
			
	}
		
	private void on_message_boot_accepted_continue (object o, EventArgs args) {
		startChronojump();
	}

	private void on_message_boot_accepted_quit (object o, EventArgs args) {
		try {
			File.Delete(runningFileName);
		} catch {
			//done because if database dir is moved in a chronojump conversion (eg from before installer to installjammer) maybe it will not find this runningFileName
		}
		Log.End();
		Log.Delete();
		Application.Quit();
	}

	private void startChronojump() {	
		chronoJumpWin = new ChronoJumpWindow(isFirstTime, authors, readVersion(), progname, runningFileName);
	}

	private void chronojumpCrashedBeforeMessage() {
		Console.Clear();
		string windowsTextLog = "";
		if(Util.IsWindows())
			windowsTextLog = "\n" + Log.GetLast().Replace(".txt", "-crash.txt");

		string errorMessage = "\n" +
				string.Format(Catalog.GetString("Chronojump crashed before. Please, <b>write an email</b> to {0} including this file:"), "xaviblas@gmail.com") + "\n\n" +
					Log.GetLast() +
					windowsTextLog +
				       "\n\n" +	
				Catalog.GetString("Subject should be something like \"bug in Chronojump\". Your help is needed.") + "\n";

		/*
		 * This are the only outputs to Console. Other's use Log that prints to console and to log file
		 * this doesn't go to log because it talks about log
		 */
		Console.WriteLine(errorMessage);
		
		messageToShowOnBoot += errorMessage;	
		return;
	}

	//recuperate temp jumpRj or RunI if chronojump hangs
	private string recuperateBrokenEvents() 
	{
		string returnString = "";
		
		string tableName = "tempJumpRj";
		int existsTempData = Sqlite.TempDataExists(tableName);
		if(existsTempData > 0)
		{
			JumpRj myJump = SqliteJump.SelectRjJumpData("tempJumpRj", existsTempData);
			SqliteJump.InsertRj("jumpRj", "NULL", myJump.PersonID, myJump.SessionID,
					myJump.Type, myJump.TvMax, myJump.TcMax, 
					myJump.Fall, myJump.Weight, "", //fall, weight, description
					myJump.TvAvg, myJump.TcAvg,
					myJump.TvString, myJump.TcString,
					myJump.Jumps, Util.GetTotalTime(myJump.TcString, myJump.TvString), myJump.Limited
					);

			Sqlite.DeleteTempEvents(tableName);
			returnString = "Recuperated last Reactive Jump";
		}

		tableName = "tempRunInterval";
		existsTempData = Sqlite.TempDataExists(tableName);
		if(existsTempData > 0)
		{
			RunInterval myRun = SqliteRun.SelectIntervalRunData("tempRunInterval", existsTempData);
			SqliteRun.InsertInterval("runInterval", "NULL", myRun.PersonID, myRun.SessionID,
					myRun.Type, myRun.DistanceTotal, myRun.TimeTotal, 
					myRun.DistanceInterval, myRun.IntervalTimesString,  
					myRun.Tracks, "", //description
					myRun.Limited
					);

			Sqlite.DeleteTempEvents(tableName);
			returnString = "Recuperated last Intervallic Run";
		}
		
		return returnString;
	}

	private string readVersion() {
		string version = "";
		try  {
			StreamReader reader = File.OpenText(Constants.FileNameVersion);
			version = reader.ReadToEnd();
			reader.Close();

			//delete the '\n' that ReaderToEnd() has put
			version = version.TrimEnd(new char[1] {'\n'});
		} catch {
			version = "not available";
		}
		return version;
	}	
		
	private void checkIfChronojumpExitAbnormally() {
		runningFileName = Util.GetDatabaseDir() + Path.DirectorySeparatorChar + "chronojump_running";
		if(File.Exists(runningFileName)) 
			chronojumpCrashedBeforeMessage();
		else {
			if (Sqlite.CheckTables()) 
				File.Create(runningFileName);
		}
	}

	//move database to new location if chronojump version is before 0.7
	private void moveDatabaseToInstallJammerLocationIfNeeded() {
		string oldDB = Util.GetOldDatabaseDir();
		string newDB = Util.GetDatabaseDir();


		if(! Directory.Exists(newDB) && Directory.Exists(oldDB)) {
			try {
				Directory.Move(oldDB, newDB);
			}
			catch {
				string feedback = "";
				feedback += string.Format(Catalog.GetString("Cannot move database directory from {0} to {1}"), 
						oldDB, Path.GetFullPath(newDB)) + "\n";
				feedback += string.Format(Catalog.GetString("Trying to move/copy each file now")) + "\n";

				int fileMoveProblems = 0;
				int fileCopyProblems = 0;

				try {
					Directory.CreateDirectory(newDB);
					Directory.CreateDirectory(newDB + Path.DirectorySeparatorChar + "backup");

					string [] filesToMove = Directory.GetFiles(oldDB);
					foreach (string oldFile in filesToMove) {
						string newFile = newDB + Path.DirectorySeparatorChar + oldFile.Substring(oldDB.Length);
						try {
							File.Move(oldFile, newFile);
						}
						catch {
							fileMoveProblems ++;
							try {
								Log.WriteLine(string.Format("{0}-{1}", oldFile, newFile));
								File.Copy(oldFile, newFile);
							}
							catch {
								fileCopyProblems ++;
							}
						}
					}

				}
				catch {
					feedback += string.Format(Catalog.GetString("Cannot create directory {0}"), Path.GetFullPath(newDB)) + "\n";
					feedback += string.Format(Catalog.GetString("Please, do it manually.")) + "\n"; 
					feedback += string.Format(Catalog.GetString("Chronojump will exit now.")) + "\n";
					messageToShowOnBoot += feedback;	
					Log.WriteLine(feedback);
					chronojumpHasToExit = true;
				}
				if(fileCopyProblems > 0) {
					feedback += string.Format(Catalog.GetString("Cannot copy {0} files from {1} to {2}"), fileCopyProblems, oldDB, Path.GetFullPath(newDB)) + "\n";
					feedback += string.Format(Catalog.GetString("Please, do it manually.")) + "\n"; 
					feedback += string.Format(Catalog.GetString("Chronojump will exit now.")) + "\n";
					messageToShowOnBoot += feedback;	
					Log.WriteLine(feedback);
					chronojumpHasToExit = true;
				}
				if(fileMoveProblems > 0) {
					feedback += string.Format(Catalog.GetString("Cannot move {0} files from {1} to {2}"), fileMoveProblems, oldDB, Path.GetFullPath(newDB)) + "\n";
					feedback += string.Format(Catalog.GetString("Please, do it manually")) + "\n";
					messageToShowOnBoot += feedback;	
					Log.WriteLine(feedback);
				}
			}
					
			string dbMove = string.Format(Catalog.GetString("Database is now here: {0}"), Path.GetFullPath(newDB));
			messageToShowOnBoot += dbMove;	
			Log.WriteLine(dbMove);
		}
	}

	/*
	private void quitFromConsole() {
		try {
			File.Delete(runningFileName);
		} catch {
			//done because if database dir is moved in a chronojump conversion (eg from before installer to installjammer) maybe it will not find this runningFileName
		}
		Log.End();
		Environment.Exit(1);
	}
	*/

}
