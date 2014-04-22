TWAIN Application-Side Library
==============================

Info
--------------------------------------
This is a dotnet library created to work with [TWAIN](http://twain.org/) interface. 
This project has these features:

* Targets latest TWAIN version (2.3 at the moment)
* Supports all the TWAIN functions in the spec
* Hosts internal message loop so no need to hook into application UI thread
 
The solution contains tester projects in winform, wpf, and even (gasp!) console. 
A nuget package is also [available here](https://www.nuget.org/packages/ntwain)

Using the lib
--------------------------------------
To properly use this lib you will need to be reasonably familiar with the TWAIN spec
and how it works in general. Except for certain "important" calls that drive the
TWAIN state change, most triplet operations are only availble as-is so you will need to know
when and how to use them. There are no high-level, single-line scan-a-page-for-me-now functions.

The main class to use is TwainSession. New it up, hook into the events, and start calling
all the TWAIN functions provided through it.

Caveats
--------------------------------------
At the moment this lib does not provide ways to parse transferred image data and require
consumers to do the conversion themselves. The winform project contains one such 
example for handling DIB image in native transfer using the CommonWin32 lib.

Because it hosts its own message thread, the event callbacks will likely be from another thread. 
If you would like things marshalled to a "UI" thread then set the SynchronizationContext property
to the one from the UI thread. This part is highly experimental.

64-bit OS
--------------------------------------
If the application process is running in 64-bit then you will need to make sure you have the 
newer data source manager (twaindsm.dll) installed. 
[DSM from TWAIN.org](http://sourceforge.net/projects/twain-dsm/files/TWAIN%20DSM%202%20Win/)

Otherwise just compile and run the app as x86 and it'll use the 32-bit version (twain_32.dll) that comes with Windows.

If you really want to test 64-bit drivers for whatever reason, you most likely will have to use 
the test one from TWAIN.org since there are no known 64-bit TWAIN DS drivers at the time of writing.
[Sample DS from TWAIN.org](http://sourceforge.net/projects/twain-samples/files/TWAIN%202%20Sample%20Data%20Source/TWAIN%20DS%202.1.3/)