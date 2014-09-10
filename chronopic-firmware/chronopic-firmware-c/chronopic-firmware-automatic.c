/*
 * Version 8.1
 * Translating firmware to SDCC:
 * source: http://git.gnome.org/browse/chronojump/plain/chronopic-firmware/chronopic-firmware.asm


encoder:
  * brown: VCC	  
  * blue : GND	  
  * black: CLK	  pin 21
  * white: VATA	  pin 23
  
PIC16F87
  * RB5 : option  pin 26

History:
  2011-01-01 complete TMR0 interrupt
  2011-05-13 ERROR: TIME = 04 ED
             modify configuration Bits: close WDT
             modify ISR and MAIN LOOP's if --> else if
             limit COUNTDEBOUNCE overflow in INTERRUPT(isr)
	     assembler is more efficient than C
  2012-04-19 if PC send command 'J' for port scanning, Chronopic will return 'J'  2014-08-30 if PC send command 'V' for getting version, ex: 2.1\n
  	     if PC send command 'a' get debounce time , ex:0x01
	     if PC send command 'bx' for setting debounce time, x is from byte value 0~255(\x0 ~ \xFF) 
	     if PC send command 'c' for starting contiuned to send encoder value
	     if PC send command 'd' for stopping contiuned to send encoder value
*/

//-- this PIC is going to be used:
//#include <pic16f876A.h> 	//sdcc for Windows
#include <pic16f876a.h>		//sdcc for Linux


//*****************************************
//*         CONSTANTS                     *
//*****************************************
//-- Logical status of the input (not physical status)
//-- It's the information sent to the frame of changing
unsigned char INPUT_OFF = 0;  //-- Push button not pushed
unsigned char INPUT_ON  = 1;  //-- Push button pushed

//-- Header of the asyncronous frame of "changing"
unsigned char FCHANGE = 'X';

//-- Header of the "status" frame
unsigned char FSTATUS = 'E';  // Petition of status of the platform
unsigned char RSTATUS = 'E';  // Response of the status frame

//-- Initialization value of the TIMER0 to have TICKS with a duration of 10ms
//-- It's used for the debouncing time
unsigned char TICK = 0xD9;

//-- Value of the debouncing time (in units of 10 milliseconds)
//-- This value can be changed, in order to select the most suitable
//-- Signals with a duration lower than this value are considered spurious
unsigned char DEBOUNCE_TIME = 0x05;

//-- Status of main automaton
unsigned char STAT_WAITING_EVENT = 0x00;
unsigned char STAT_DEBOUNCE = 0x01;
unsigned char STAT_FRAMEX = 0x02;

//*******************************************
//*               VARIABLES                 *
//*******************************************

//-- Save the context when interruptions arrive
unsigned char savew;   // Temporal storage of W register
unsigned char saves;   // temporal storage of the STATUS register

//-- Extension of timer 1
unsigned char TMR1HH;

//-- Counter of the debouncer
unsigned char COUNTDEBOUNCE;

//-- Timestamp of an event
unsigned char TIMESTAMP_HH; //-- high-high part
unsigned char TIMESTAMP_H;  //-- high part
unsigned char TIMESTAMP_L;  //-- low part

//-- Pushbutton status
unsigned char input;         //-- Current stable input Value: (0-1)
unsigned char input_new;     //-- New stable input Value: (0-1)

//-- Automaton status
unsigned char status;

//-- Reset: Shows if has been done a reset of events
//-- Reset=1, means that next incoming event will be considered the first
//-- that means that it's timestamp has no sense, and a 0 must be returned
//-- Reset=0, it's normal status.
unsigned char reset;

//-- 16 bits variable to do a pause
//-- (used by the pause routine)
unsigned char pause_h;
unsigned char pause_l;

//-- Store a character of the received frame
unsigned char my_char;

//-- wade's addition variables
unsigned char i = 0, j = 0;
unsigned char option = 0;     // option: 0 button enable, 1 encoder enable
unsigned char command_port_scanning = 'J';	// for port scanning, it will return 'J'
unsigned char command_get_version = 'V';	// for getting version, it will return '2.1'
unsigned char command_get_debounce_time = 'a';	// for setting debounce time, pc send two unsigned char, 'Sx' -- x:0~255
unsigned char command_set_debounce_time = 'b';	// for getting debounce time, it will return x:0~255(HEX)
//unsigned char command_start_send_encoder_value = 'c';	// starting continued send encoder's value
//unsigned char command_stop_send_encoder_value = 'd';	// stopping continued send encoder's value

char version_major = '1';
char version_minor = '1';

//-- encoder's valus
//char encoder_count = 0; //wade


// Interruptions routine
// Find the interruption cause
void isr(void) __interrupt 0
{
	//while (!TXIF);
	//TXREG = 0xaa;

	if (option == 0) 
	{

		//******************************************************
		//* Routine of interruption of timer0
		//* timer0 is used to control debouncing time
		//* A change on input signal is stable if time it's at minimum equal to debouncing time
		//* This timer is working all the time.
		//* Main automaton know when there's valid information
		//******************************************************  
		// Cause by timer0
		if (T0IF == 1)
		{	
			T0IF = 0;		 // Remove overflow flag
			TMR0 = TICK;		 //-- Execute timer again inside a click
			if (COUNTDEBOUNCE > 0)	 // wade : limit COUNTDEBOUNCE overflow
				COUNTDEBOUNCE--;	 //-- Decrese debouncing counter
			//while (!TXIF); //wade
			//TXREG = COUNTDEBOUNCE; //wade
		}
		//****************************************************
		// Routine of port B interruption
		// Called everytime that's a change on bit RB4
		// This is the main part.
		// Everytime that's a change on input signal,
		// it's timestamp is recorded on variable (TIMESTAMP, 3 bytes)
		// and we start debouncing time phase
		//****************************************************    
		// Caused by a change on B port
		else if (RBIF == 1)
		{
			if (reset == 1)
			{
				//-- It's the first event after reset
				//-- Put counter on zero and go to status reset=0
				TMR1HH = 0;
				TMR1H = 0;
				TMR1L = 0;
				reset = 0;
			}
			//-- Store the value of chronometer on TIMESTAMP
			//-- This is the timestamp of this event
			TIMESTAMP_HH = TMR1HH;
			TIMESTAMP_H = TMR1H;
			TIMESTAMP_L = TMR1L;
			//-- Initialize timer 1
			TMR1HH = 0;
			TMR1H = 0;
			TMR1L = 0;
			//-- Initialize debouncing counter
			COUNTDEBOUNCE =  DEBOUNCE_TIME;

			//-- start debouncing status
			status = STAT_DEBOUNCE;
			//-- start debouncing timer on a tick
			TMR0 = TICK;
			//-- Remove interruption flag
			RBIF = 0;
			//-- Inhabilite B port interruption
			// wade : take care
			RBIE = 0;	
		}
		//********************************************************
		//* Routine of interruption of timer1
		//* timer 1 controls the cronometring
		//* This routine is invoked when there's an overflow
		//* Timer 1 gets extended with 1 more byte: TMR1HH
		//********************************************************
		// Caused by timer1
		else if (TMR1IF == 1)
		{
			TMR1IF = 0;  // Remove overflow flag
			if (TMR1HH != 0xFF)
			{
				//-- Overflow control
				//-- Check if counter has arrived to it's maximum value
				//-- If it's maximum, then not increment
				TMR1HH++;
			}   
		}
	} // end of (option == 0)
}

//---------------------------------------
//-- CONFIGURATION OF SERIAL PORT
//-- 9600 BAUD, N81
//---------------------------------------
void sci_configuration()
{
	// wade : start
	// formula: Baud = Frequency / ( 16(x+1) )
	// SPBRG = 0X19;   // crystal:  4MHz Speed: 9600 baud 
	// SPBRG = 0X81;   // crystal: 20MHz Speed: 9600 baud
	// SPBRG = 0X0A;   // crystal: 20MHz Speed: 115200 baud
	// SPBRG = 0X01;   // crystal: 20MHz Speed: 625000
	SPBRG = 0X19;   // Speed: 9600 baud

	// wade : end
	TXSTA = 0X24;   // Configure transmitter
	RCSTA = 0X90;   // Configure receiver
}

//**************************************************
//* Receive a character by the SCI
//-------------------------------------------------
// OUTPUTS:
//    Register W contains received data
//**************************************************
unsigned char sci_readchar()
{
    while (!RCIF);	// P1R1:RCIF  Receive Interrupt Flag
    return RCREG;
}

//*****************************************************
//* Transmit a character by the SCI
//*---------------------------------------------------
//* INPUTS:
//*    Register W:  character to send
//*****************************************************
//-- Wait to Flag allows to send it to 1 (this comment may be bad translated)
void sci_sendchar(unsigned char my_w)
{
    while (!TXIF);
    TXREG = my_w;
}

void sci_sendline(unsigned char my_w)
{
    while (!TXIF);
    TXREG = my_w;
    while (!TXIF);
    TXREG = '\n';
}



/************************************
 * Send version
 ************************************/
void send_version()
{
    while (!TXIF);
    TXREG = version_major;
    while (!TXIF);
    TXREG = '.';
    while (!TXIF);
    TXREG = version_minor;
    while (!TXIF);
    TXREG = '\n';
}

void send_error()
{
    while (!TXIF);
    TXREG = '-';
    while (!TXIF);
    TXREG = '1';
    while (!TXIF);
    TXREG = '\n';
}


//*******************************************
//* Routine of pause, by active waiting
//*******************************************
void pause()
{
    unsigned char i,j;
    for (i = 0; i < 0xff; i++)
    {
	__asm
	CLRWDT	
	__endasm;
	for (j = 0; j < 0xff; j++);
    }
}


// wade : long delay
/*
void pause1()
{
    int i;int j;
    for (i = 0; i < 2; i++)
	for (j = 0; j < 10000; j++)
}
*/

//***************************************************************************
//* Read input on RB4 (push button status)
//* INPUTS: None
//* OUTPUTS: None
//* RETURNS: W contains input status (INPUT_ON, INPUT_OFF)
//***************************************************************************
unsigned char read_input()
{
    //-- Check status of bit RB4
    if (RB4 == 0)
	return INPUT_ON;
    return INPUT_OFF;
}

//*************************************************************
//* Update led with stable status of input
//* Stable status is on variable: input
//* INPUTS: None
//* OUTPUTS: None
//* RETURNS: Nothing
//*************************************************************
void update_led()
{
    //-- Led is on bit RB1. Input variable contains
    //-- only an information bit (1,0) on less signficant bit
    RB1 = !input;
    // 2012-04-02
    if (option == 1)
    {
	RB1 = 1;
    }
}

//*****************************************************
//* Activate interruption of changing of port B
//* INPUT: None
//* OUTPUT:  None
//* RETURN: Nothing
//*****************************************************
void portb_int_enable()
{
    // Remove inerruption flag, just in case it's activated
    RBIF = 0;
    // Activate interruption of change
    RBIE = 1;
}

//****************************************************************
//* Status service. Returns the status of the platform           *
//****************************************************************
void status_serv()
{
    //--Deactivate interruption of change while frame is sent
    RBIE = 0;
    //-- Send response code
    sci_sendchar(RSTATUS);
    //-- Send status of the push button
    sci_sendchar(input);
    //-- Activate interruption of change
    RBIE = 1;
}

static void asm_ledon()
{
    __asm
    BANKSEL PORTC
    MOVLW   0XFF
    MOVWF PORTC
    __endasm;
}

void main(void)
{
    // =========
    //   START
    // =========
    
    //-----------------------------
    //- Detect option pin 26
    //-----------------------------
    
    //-----------------------------
    //- CONFIGURE PORT B
    //-----------------------------
    //-- Pins I/O: RB0,RB4 inputs, all the other are outputs
    // 2012-04-02 wade: start
    TRISB = 0x11;
    
    // 2012-04-02 wade: end
    //-- Pull-ups of port B enabled
    //-- Prescaler of timer0 at 256
    //--   RBPU = 0, INTEDG=0(1:rising edge, 0:falling edge), T0CS=0, T0SE=0, PSA=0, [PS2,PS1,PS0]=111
    OPTION_REG = 0x07;

    //----------------------------------------------
    //  CONFIGURATION OF SERIAL COMMUNICATIONS
    //----------------------------------------------
    sci_configuration();

    //----------------------------------------------
    //- CONFIGURATION OF TIMER 0
    //----------------------------------------------
    //-- Remove interruption flag, just in case it's activated
    T0IF = 0;	//  Remove overflow flag
    //-- Activate timer. Inside an interruption tick
    // wade : start
    if (option == 0)
	TMR0 = TICK;
    if (option == 1)
	TMR0 = 0xFF;
    // wade : end

    //----------------------------------------------
    //- CONFIGURATION OF TIMER 1
    //----------------------------------------------
    //-- Activate timer 
    //   set prescaler
    // wade : start
    if (option == 0)
	T1CON = 0X31;
    // wade : end
    //-- Zero value
    TMR1HH = 0;
    TMR1H = 0;
    TMR1L = 0;
    //-- Enable interruption
    // wade : sart
    if (option == 0)
	TMR1IE = 1;
    // wade : end

    //----------------------------
    //- Interruptions port B
    //----------------------------
    //-- Wait to port B get's stable
    pause();
    //-- Enable interruption of change on port B
    // wade : start
    if (option == 0)
	portb_int_enable();
    // wade : end

    //------------------------------
    //- INITIALIZE VARIABLES
    //------------------------------
    //-- Initialize extended counteri
    TMR1HH = 0;
    //-- Initialize debounce counter
    COUNTDEBOUNCE = DEBOUNCE_TIME;
    //-- Initialize automaton
    status = STAT_WAITING_EVENT;
    //-- At start, system is on Reset
    reset = 1;
    //-- Read input status and update input variable   
    input = read_input();
    
    //----------------------
    //- INITIAL LED STATUS
    //----------------------
    //-- Update led with the stable status of input variable
    update_led();

    //--------------------------
    //- Interruption TIMER 0
    //--------------------------
    T0IE = 1;	// Activate interruption overflow TMR0
   
    //--------------------------
    //- Interruption INT RB0
    //-------------------------- 
    // wade : start
    if (option == 1)
	INTE = 1;
    // wade : end
    // wade : for testing
    /*
    for (i = 0; i <= 0x77; i++)
    {
	TXEN = 1;
	for(j = 0; j < 0xFF; j++)
	{
	    while (!TXIF);
	    TXREG = i;
	    while (!TXIF);
	    TXREG = j;
	}
	TXEN = 0;	
    }
    */
    // wade : for testing

    //------------------------------------------
    //- ACTIVATE GLOBAL INTERRUPTIONS
    //- The party starts, now!!!
    //------------------------------------------
    //-- Activate peripheral interruptions
    PEIE = 1;
    //-- Activate global interruptions
    GIE = 1;

    //****************************
    //*   MAIN LOOP
    //**************************** 
    while(1)
    {
        //-- used on the simulation
	__asm
	CLRWDT
	__endasm;
        // wade : start
        // sci_sendchar(FCHANGE);
        //sci_sendchar(0xFF);
        //sci_sendchar(0xFF);
	// wade : end

	if (option == 0)
	{
	    //-- Analize serial port waiting to a frame
	    if (RCIF == 1)	// Yes--> Service of platform status
	    {	        
		my_char = sci_readchar();
		if (my_char == FSTATUS)
			status_serv();
	    	else if (my_char == command_port_scanning)	// 'J'
			sci_sendline(command_port_scanning);
		else if (my_char == command_get_version)	// 'V'
			send_version();
		else if (my_char == command_get_debounce_time)	// 'a'
			sci_sendline(DEBOUNCE_TIME + '0'); 	//if DEBOUNCE is 50ms (0x05), returns a 5 (5 * 10ms = 50ms)
		else if (my_char == command_set_debounce_time)	// 'b'
			DEBOUNCE_TIME = sci_readchar();
		else
			send_error();
	    }

	    //------------------------------------------------------
	    //- DEPENDING AUTOMATON STATUS, GO TO DIFFERENT ROUTINES
	    //------------------------------------------------------
	    if (status == STAT_WAITING_EVENT)   // status = STAT_WAITING_EVENT?
	    {
	    }
	    else if (status == STAT_DEBOUNCE)   // status = DEBOUNCE?
	    {
		//----------------------------
		//- STATUS DEBOUNCING
		//----------------------------
		if (COUNTDEBOUNCE == 0)
		{
		    //-- End of debounce
		    //-- Remove interruption flag of port B: to clean. We do not want or need
		    //-- what came during that time
		    RBIF = 0;
		    //-- input_new = input status
		    input_new = read_input();
		    //-- Compare new input with stable input
		    if (input_new == input)
		    {
			//-- It came an spurious pulse (change with a duration
			//-- lower than debounce time). It's ignored.
			//-- We continue like if nothing happened
			//-- The value of the counter should be: actual + TIMESTAMP
			//-- TMR1 = TIMR1 + TIMESTAMP
			TMR1L = TMR1L + TIMESTAMP_L;
			//-- Add carry, if any
			if (C == 1)	    //-- Yes--> Add it to high part
			    TMR1H++;
			TMR1H = TMR1H + TIMESTAMP_H;
			//-- Add carry, if any
			if (C == 1)	    //-- Yes--> Add it to "higher weight" part
			    TMR1HH++;
			TMR1HH = TMR1HH + TIMESTAMP_HH;
			//-- Change status to waiting event
			status = STAT_WAITING_EVENT;
			//-- Activate interruption port B
			portb_int_enable();
		    }
		    else
		    { 
			//-- input!=input_new: Happened an stable change
			//-- Store new stable input
			input = input_new;
			//-- Change to status FRAMEX to send frame with the event
			status = STAT_FRAMEX;
		    }
		}
	    }
	    else if (status == STAT_FRAMEX)	    // status = FRAMEX?
	    {
		//----------------------------
		//- STATUS FRAMEX
		//----------------------------
		//-- Send frame of changing input
		//-- First the frame identifier
		sci_sendchar(FCHANGE);
		//-- Send push button status
		sci_sendchar(input);
		//-- Send timestamp
		sci_sendchar(TIMESTAMP_HH);
		sci_sendchar(TIMESTAMP_H);
		sci_sendchar(TIMESTAMP_L);
		//-- Change to next status
		status = STAT_WAITING_EVENT;
		//-- Update led status, depending on stable input status
		update_led();
		//-- Activate port B interruption
		portb_int_enable();
	    }
	} // end of if (option == 0)
	
    } // end of while

}
