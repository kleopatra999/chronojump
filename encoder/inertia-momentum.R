# 
#  This file is part of ChronoJump
# 
#  ChronoJump is free software; you can redistribute it and/or modify
#   it under the terms of the GNU General Public License as published by
#    the Free Software Foundation; either version 2 of the License, or   
#     (at your option) any later version.
#     
#  ChronoJump is distributed in the hope that it will be useful,
#   but WITHOUT ANY WARRANTY; without even the implied warranty of
#    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the 
#     GNU General Public License for more details.
# 
#  You should have received a copy of the GNU General Public License
#   along with this program; if not, write to the Free Software
#    Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
# 
#   Copyright (C) 2014-2016   	Xavier Padullés <x.padulles@gmail.com>
#   Copyright (C) 2014-2016  	Xavier de Blas <xaviblas@gmail.com> 
# 
#
#     ----------
#    /          \
#   /         W  \
#  /         /    \
# |         /      |
# |        o       |
# |                |
#  \              /
#   \            /  
#    \          /
#      --------
#
#Weight has not to be on the top of the axis (has to be "sided")
#Measure weight
#Measure distance between centre of axis and centre of weight

calculate <- function (displacement, mass, length)
{
	print("at inertia-momentum.R calculate")

	#cumulative movement of the encoder
	x <- cumsum(displacement)
	#print(c("x",x))

	#time in milliseconds
	t <- seq(1,length(displacement))
	#print(c("t",t))

	#all the information about local maximums and minimums and crossings
	ex <- extrema(x)
	print(c("ex",ex))

	#times where the maximums are found in milliseconds
	tmax <- rowMeans(ex$maxindex)
	print(c("tmax",tmax))
	print(tmax)
	
	tmin <- rowMeans(ex$minindex)
	print(c("tmin",tmin))

	tall = sort(as.numeric(c(tmin, tmax)))
	print("tall")
	print(tall)

	#the last maximum is discarded
	#tmax <- tmax[1:(length(tmax)-1)]
	#print(c("tmax",tmax))
	tall <- tall[1:(length(tall)-1)]
	print(c("tall",tall))

	#Periods of the oscillations
	T = NULL
	#T <- diff(tmax[1:length(tmax)])
	T <- 2 * diff(tall[1:length(tall)])
	
	if(is.null(T) || length(T) <= 3)
		return (-1)

	#Coefficients of a Logarithmic regression
	logT <- lm( log(T) ~ I(log(tall[1:(length(tall)-1)])))
	print(c("logT",logT))

	#The final period of the oscillation in seconds
	finalT <- exp(logT$coefficients[1] + logT$coefficients[2]*log(tall[length(tall)]))/1000
	print(c("finalT",finalT))

	#Inertia momentum using the pendulus formula
	I <- ( (finalT / (2 * pi))^2 ) * mass * 9.81 * length - (mass * length^2)
	print(c("I",I))

	return(as.numeric(I))
}


args <- commandArgs(TRUE)
print(args)

optionsFile = args[1]
print(optionsFile)

options = scan(optionsFile, comment.char="#", what=character(), sep="\n")

fileInput = options[1]
fileOutput = options[2]
mass = as.numeric(options[3]) / 1000.0 	# g -> Kg
length = as.numeric(options[4]) / 100.0	#cm -> m
scriptUtilR = options[5]
		
source(scriptUtilR)

displacement = scan(file=fileInput, sep=",")

inertia = calculate(displacement, mass, length)

print (inertia)
write(inertia, fileOutput)
