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
 * http://www.xdeblas.com, http://www.deporteyciencia.com (parleblas)
 */

using System;
using Gtk;
using Glade;
using GLib; //for Value
using System.Text; //StringBuilder
using System.Collections; //ArrayList
using Mono.Unix;


//--------------------------------------------------------
//---------------- JUMP TYPE ADD WIDGET ------------------
//--------------------------------------------------------

public class JumpTypeAddWindow 
{
	[Widget] Gtk.Window jump_type_add;
	[Widget] Gtk.Button button_accept;
	[Widget] Gtk.Entry entry_name;
	[Widget] Gtk.RadioButton radiobutton_simple;
	[Widget] Gtk.RadioButton radiobutton_repetitive;
	[Widget] Gtk.RadioButton radiobutton_unlimited;
	[Widget] Gtk.VBox vbox_limited;
	[Widget] Gtk.HBox hbox_fixed;
	[Widget] Gtk.RadioButton radiobutton_limited_jumps;
	[Widget] Gtk.CheckButton checkbutton_limited_fixed;
	[Widget] Gtk.SpinButton spin_fixed_num;

	[Widget] Gtk.RadioButton radiobutton_startIn_yes;
	[Widget] Gtk.RadioButton radiobutton_startIn_no;
	[Widget] Gtk.RadioButton radiobutton_extra_weight_yes;
	[Widget] Gtk.RadioButton radiobutton_extra_weight_no;
	[Widget] Gtk.TextView textview_description;

	static JumpTypeAddWindow JumpTypeAddWindowBox;
	Gtk.Window parent;
	ErrorWindow errorWin;

	JumpTypeAddWindow (Gtk.Window parent) {
		Glade.XML gladeXML;
		gladeXML = Glade.XML.FromAssembly (Util.GetGladePath() + "chronojump.glade", "jump_type_add", null);
		gladeXML.Autoconnect(this);
		this.parent =  parent;
		
		//put an icon to window
		UtilGtk.IconWindow(jump_type_add);
	}
	
	static public JumpTypeAddWindow Show (Gtk.Window parent)
	{
		if (JumpTypeAddWindowBox == null) {
			JumpTypeAddWindowBox = new JumpTypeAddWindow (parent);
		}
		
		JumpTypeAddWindowBox.jump_type_add.Show ();
		JumpTypeAddWindowBox.fillDialog ();

		return JumpTypeAddWindowBox;
	}
	
	private void fillDialog ()
	{
		vbox_limited.Sensitive = false;	
		hbox_fixed.Sensitive = false;	
		button_accept.Sensitive = false;
		spin_fixed_num.Sensitive = false;
		radiobutton_extra_weight_no.Active = true;
	}
		
	void on_button_cancel_clicked (object o, EventArgs args)
	{
		JumpTypeAddWindowBox.jump_type_add.Hide();
		JumpTypeAddWindowBox = null;
	}
	
	void on_jump_type_add_delete_event (object o, DeleteEventArgs args)
	{
		JumpTypeAddWindowBox.jump_type_add.Hide();
		JumpTypeAddWindowBox = null;
	}
	
	void on_button_accept_clicked (object o, EventArgs args)
	{
		//check if this jump type exists, and check it's name is not "All jumps"
		bool jumpTypeExists = Sqlite.Exists (Constants.JumpTypeTable, Util.RemoveTildeAndColonAndDot(entry_name.Text));
		if(Util.RemoveTildeAndColonAndDot(entry_name.Text) == Catalog.GetString("All jumps")) {
			jumpTypeExists = true;
		}
		
		if(jumpTypeExists) {
			//string myString =  Catalog.GetString ("Jump type: '") + 
			//	Util.RemoveTildeAndColonAndDot(entry_name.Text) + 
			//	Catalog.GetString ("' exists. Please, use another name");
			
			string myString = string.Format(Catalog.GetString("Jump type: '{0}' exists. Please, use another name"), Util.RemoveTildeAndColonAndDot(entry_name.Text) );
			
			Console.WriteLine (myString);
			errorWin = ErrorWindow.Show(myString);
		} else {
			string myJump = "";
			myJump = Util.RemoveTildeAndColonAndDot(entry_name.Text);
			if(radiobutton_startIn_yes.Active) {
				myJump = myJump + ":1"; 
			} else {
				myJump = myJump + ":0"; 
			}
			if(radiobutton_extra_weight_yes.Active) {
				myJump = myJump + ":1"; 
			} else {
				myJump = myJump + ":0"; 
			}
			
			if(radiobutton_simple.Active) {
				myJump = myJump + ":" + 
					Util.RemoveTildeAndColon(textview_description.Buffer.Text);
			
				SqliteJumpType.JumpTypeInsert(myJump, false); //false, because dbcon is not opened
			} else {
				
				if(radiobutton_unlimited.Active) {
					//unlimited (but in jumps do like if it's limited by jumps 
					//(explanation in sqlite/jumpType.cs)
					myJump = myJump + ":1:-1"; 
				} else {
					if(radiobutton_limited_jumps.Active) {
						myJump = myJump + ":1"; 
					} else {
						myJump = myJump + ":0";
					}
				
					if(checkbutton_limited_fixed.Active) {
						myJump = myJump + ":" + spin_fixed_num.Value; 
					} else {
						myJump = myJump + ":0"; 
					}
				}
			
				myJump = myJump + ":" + 
					Util.RemoveTildeAndColon(textview_description.Buffer.Text);
				
				SqliteJumpType.JumpRjTypeInsert(myJump, false); //false, because dbcon is not opened
			}
			
			Console.WriteLine("Inserted: {0}", myJump);
		}

		JumpTypeAddWindowBox.jump_type_add.Hide();
		JumpTypeAddWindowBox = null;
	}

	void on_radiobutton_simple_toggled (object o, EventArgs args)
	{
		vbox_limited.Sensitive = false;	
		hbox_fixed.Sensitive = false;	
	}
	
	void on_radiobutton_repetitive_toggled (object o, EventArgs args)
	{
		vbox_limited.Sensitive = true;	
		if( ! radiobutton_unlimited.Active) {
			hbox_fixed.Sensitive = true;	
		}
	}
	
	void on_radiobutton_limited_jumps_or_time_toggled (object o, EventArgs args)
	{
		hbox_fixed.Sensitive = true;	
	}
	
	void on_radiobutton_unlimited_toggled (object o, EventArgs args)
	{
		hbox_fixed.Sensitive = false;	
	}
	
	void on_checkbutton_limited_fixed_clicked (object o, EventArgs args)
	{
		if(checkbutton_limited_fixed.Active) {
			spin_fixed_num.Sensitive = true;
		} else {
			spin_fixed_num.Sensitive = false;
		}
	}
	
	void on_entries_required_changed (object o, EventArgs args)
	{
		if(entry_name.Text.ToString().Length > 0) {
			button_accept.Sensitive = true;
		}
		else {
			button_accept.Sensitive = false;
		}
	}
		

	public Button Button_accept 
	{
		set { button_accept = value;	}
		get { return button_accept;	}
	}

}


