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
 * Copyright (C) 2008   Sharad Shankar & Onkar Nath Mishra http://www.logicbrick.com/
 * Copyright (C) 2008   Xavier de Blas <xaviblas@gmail.com> 
 *
 */


//config variables
bool showContour = false;
bool debug = false;
int playDelay = 10; //milliseconds between photogrammes wen playing. Used as a waitkey.
//not put values lower than 5 or the enter when executing will be the first pause
//eg: 5 (fast) 1000 (one second each photogramme)
//int playDelayFoundAngle = 150; //as above, but used when angle is found.
int playDelayFoundAngle = 50; //as above, but used when angle is found.
//Useful to see better the detected angle when something is detected
//recommended values: 50 - 200



/* recommended:
	   showAtLinesPoints = true
	   ...DiffPoints = true
	   ...SamePoints = true
	   ...OnlyStartMinEnd = true;
	   */

bool showStickThePoints = true;
bool showStickTheLinesBetweenSamePoints = true;
bool showStickTheLinesBetweenDifferentPoints = true;
bool showStickOnlyStartMinEnd = true;
bool mixStickWithMinAngleWindow = true;

int startAt = 1;
int programMode;


CvScalar WHITE = CV_RGB(255,255,255);
CvScalar BLACK = CV_RGB(0 ,0 , 0);
CvScalar RED = CV_RGB(255, 0, 0);
CvScalar GREEN = CV_RGB(0 ,255, 0);
CvScalar BLUE = CV_RGB(0 ,0 ,255);
CvScalar GREY = CV_RGB(128,128,128);
CvScalar YELLOW = CV_RGB(255,255, 0);
CvScalar MAGENTA = CV_RGB(255, 0,255);
CvScalar CYAN = CV_RGB( 0,255,255); 
CvScalar LIGHT = CV_RGB( 247,247,247); 

enum { SMALL = 1, MID = 2, BIG = 3 }; 

//used on menu gui and programMode
//currently validation and blackWithoutMarkers are synonymous (until statistical anylisys is not done)
/*
 * validation uses markers and black pants to try to find relationship between both
 * blackWithoutMarkers uses only black pants and finds the place where the markers should be
 *    (when validation study for lots of people isdone)
 * skinOnlyMarkers uses markers to find three points and angle (easiest)
 * skinOnlyMarkers uses markers to find three points and angle but in pants (it uses findLargestContour and finds inside it)
 */
//NOTE: if this changes, change also in kneeangle.cpp menu
enum { quit = -1, undefined = 0, validation = 1, blackWithoutMarkers = 2, skinOnlyMarkers = 3, blackOnlyMarkers = 4}; 

//black only markers will try to use contour
//and controls will be only threshold + -
//but if there's any problem with contour or the toe or hip go outside contour,
//then usingContour will be false and it will be used same method than skinOnlyMarkers
//interface will change also
//difference with skinOnlyMarkers is that user can return to: usingContour and play with threshold
//if is not successuful (three points are not found in contour) the usingContour will be false again
bool usingContour;

//used on gui
enum { 
	QUIT = -1,
	UNDEFINED = 0, 
	YES = 1, NO = 2, NEVER = 3, 
	PLAYPAUSE = 4, FORWARDONE = 5, FORWARD = 6, FASTFORWARD = 7, BACKWARD = 8,
	HIPMARK = 9, KNEEMARK = 10, TOEMARK = 11, ZOOM = 12,
	THIPMORE = 13, THIPLESS = 14, 
	TKNEEMORE = 15, TKNEELESS = 16, 
	TTOEMORE = 17, TTOELESS = 18, 
	TGLOBALMORE = 19, TGLOBALLESS = 20,
	SHIPMORE = 21, SHIPLESS = 22, 
	SKNEEMORE = 23, SKNEELESS = 24, 
	STOEMORE = 25, STOELESS = 26,
	TCONTOURMORE = 27, TCONTOURLESS = 28,
	BACKTOCONTOUR = 29,
	MINIMUMFRAMEVIEW = 30, MINIMUMFRAMEDELETE = 31
}; 

enum { TOGGLENOTHING = -1, TOGGLEHIP = 0, TOGGLEKNEE = 1, TOGGLETOE = 2};

CvPoint markedMouse;
int forceMouseMark;
int mouseClicked = undefined;
bool mouseMultiplier = false; //using shift key

bool zoomed = false;
double zoomScale = 2; 

//predictions stuff
bool usePrediction = false;	//unneded at 300 fps
RInside R = RInside();		// create an embedded R instance 
