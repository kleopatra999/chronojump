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
 * Copyright (C) 2004-2015   Xavier de Blas <xaviblas@gmail.com> 
 */


using System;
using Gtk;
using Gdk;
using Glade;
using System.IO.Ports;
using System.IO; //"File" things
using System.Collections; //ArrayList
using System.Collections.Generic; //List
	
public partial class ChronoJumpWindow 
{
	//RFID
	[Widget] Gtk.Box hbox_rfid;
	[Widget] Gtk.Label label_rfid;
	
	//better raspberry controls
	[Widget] Gtk.Entry entry_raspberry_extra_weight;
	[Widget] Gtk.Box hbox_encoder_capture_extra_mass_no_raspberry;
	[Widget] Gtk.Box hbox_encoder_capture_extra_mass_raspberry;
	[Widget] Gtk.HBox hbox_encoder_im_weights_n;
	
	//config.EncoderNameAndCapture
	[Widget] Gtk.Box hbox_encoder_person;
	[Widget] Gtk.Label label_encoder_person_name;
	[Widget] Gtk.Button button_encoder_person_change;
	
	//shown when menu is hidden
	[Widget] Gtk.Button button_preferences_not_menu;


	private bool useVideo = true;
	private Config.AutodetectPortEnum configAutodetectPort = Config.AutodetectPortEnum.ACTIVE;

	private enum linuxTypeEnum { NOTLINUX, LINUX, RASPBERRY, NETWORKS }
	private linuxTypeEnum linuxType;
	private bool encoderConfigurationDefinedFromFile = false;


	private void configInit() 
	{
		//trying new Config class
		Config config = new Config();
		config.Read();
		LogB.Information("Config:\n" + config.ToString());

		if(config.Maximized)
			app1.Maximize();
		if(config.CustomButtons) {
			hbox_encoder_capture_extra_mass_no_raspberry.Visible = false;
			hbox_encoder_capture_extra_mass_raspberry.Visible = true;
		
			button_encoder_select.HeightRequest = 40;
			//this will make all encoder capture controls taller	
			//88 then buttons will be the same height than button_encoder_select. The 8 extra is for the vertical space
			button_encoder_capture.SetSizeRequest(150,88);
			//button_encoder_capture.HeightRequest = 88;

			spin_encoder_im_weights_n.Visible = false;
			hbox_encoder_im_weights_n.Visible = true;
		}
		if(! config.UseVideo) {
			useVideo = false;
			alignment_video_encoder.Visible = false;
		}
		
		configAutodetectPort = config.AutodetectPort;
		
		//show only power
		if(config.OnlyEncoder)
			select_menuitem_mode_toggled(menuitem_modes.POWER);
		
		if(config.EncoderCaptureShowOnlyBars) {
			vpaned_encoder_capture_video_and_set_graph.Visible = false;
			alignment_treeview_encoder_capture_curves.Visible = false;
		}
		
		if(config.PersonWinHide) {
			//vbox_persons.Visible = false;
			notebook_session_person.Visible = false;
			hbox_encoder_person.Visible = true;
		}
		
		if(config.EncoderAnalyzeHide) {
			hbox_encoder_sup_capture_analyze_two_buttons.Visible = false;
		}
		
		if(config.Econf != null) {
			encoderConfigurationDefinedFromFile = true;
			encoderConfigurationCurrent = config.Econf;
			encoderConfigurationGUIUpdate();
			//TODO: allow to see full data, but don't allow to change it (Open window content as unsensitive)
		}
		
		if(config.SessionMode == Config.SessionModeEnum.UNIQUE)	
		{
			main_menu.Visible = false;
			button_preferences_not_menu.Visible = true;

			if(! Sqlite.Exists(false, Constants.SessionTable, "session")) {
				//this creates the session and inserts at DB
				currentSession = new Session(
						"session", "", DateTime.Today,	//name, place, dateTime
						Constants.SportUndefinedID, Constants.SpeciallityUndefinedID, Constants.LevelUndefinedID,
						"", Constants.ServerUndefinedID); //comments, serverID
			} else
				currentSession = SqliteSession.SelectByName("session");
			
			on_load_session_accepted();
		}

		//TODO
		//RunScriptOnExit

		/*
		if(linuxType == linuxTypeEnum.NETWORKS) {
			//mostrar directament el power
			select_menuitem_mode_toggled(menuitem_modes.POWER);
			
			//no mostrar menu
			main_menu.Visible = false;
			
			//no mostrar persones
			//vbox_persons.Visible = false;
			//TODO: rfid can be here, also machine, maybe weight, other features
			//time, gym, ...

			//show rfid
			hbox_rfid.Visible = true;

			//to test display, just make sensitive the top controls, but beware there's no session yet and no person
			notebook_sup.Sensitive = true;
			hbox_encoder_sup_capture_analyze.Sensitive = true;
			notebook_encoder_sup.Sensitive = false;
		}
		*/
	}

	//rfid
	private void rfid_test() {
		Networks networks = new Networks();
		networks.Test();
	}
	void on_button_rfid_read_clicked (object o, EventArgs args) {
		string file = "/tmp/chronojump_rfid.txt";

		if(Util.FileExists(file))
			label_rfid.Text = Util.ReadFile(file, true);
	}

}

