/*
 * This file is part of ChronoJump
 *
 * ChronoJump is free software; you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *   the Free Software Foundation; either version 2 of the License, or   
 *    (at your option) any later version.
 *    
 * ChronoJump is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the 
 *    GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 *  along with this program; if not, write to the Free Software
 *   Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 *
 * Xavier de Blas: 
 */

using System;
using System.Data;
using System.IO;
using System.Collections; //ArrayList
using Mono.Data.Sqlite;

class SqlitePersonSessionNotUpload : Sqlite
{
	protected internal static void CreateTable()
	 {
		dbcmd.CommandText = 
			"CREATE TABLE " + Constants.PersonNotUploadTable + " ( " +
			"personID INT, " + //foreign key
			"sessionID INT ) ";  //foreign key 
		/*
		   no option of all sessions 
		   because it will be confusing to user to select
		   "don't upload here and in other sessions"
		   he maybe will think that when uploading a session, 
		   persons tests that are here and in other sessions,
		   will be uploaded now
		   */

		dbcmd.ExecuteNonQuery();
	 }

	public static ArrayList SelectAll(int sessionID)
	 {
		dbcon.Open();
		dbcmd.CommandText = "SELECT personID FROM " + Constants.PersonNotUploadTable +
			" WHERE sessionID == " + sessionID;
		Log.WriteLine(dbcmd.CommandText.ToString());
		dbcmd.ExecuteNonQuery();

		SqliteDataReader reader;
		reader = dbcmd.ExecuteReader();

		ArrayList myArray = new ArrayList(1);
		while(reader.Read()) 
			myArray.Add (reader[0].ToString());
		
		reader.Close();
		dbcon.Close();
		return myArray;
	 }

	public static void Add(int personID, int sessionID)
	{
		dbcon.Open();
		dbcmd.CommandText = "INSERT INTO " + Constants.PersonNotUploadTable +  
			" (personID, sessionID)" +
			" VALUES (" + personID + ", " + sessionID +")";
		Log.WriteLine(dbcmd.CommandText.ToString());
		dbcmd.ExecuteNonQuery();

		dbcon.Close();
	 }

	public static void Delete(int personID, int sessionID)
	 {
		 dbcon.Open();
		 dbcmd.CommandText = "Delete FROM " + Constants.PersonNotUploadTable +
			 " WHERE personID == " + personID + " AND sessionID == " + sessionID;
		 Log.WriteLine(dbcmd.CommandText.ToString());
		 dbcmd.ExecuteNonQuery();
		 dbcon.Close();
	 }

}
