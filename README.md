TWAIN Application-Side Library
==============================

Info
--------------------------------------
This is a library created to make working with [TWAIN](http://twain.org/) interface possible in dotnet. 
This project has these features/goals:

* Targets latest TWAIN version (2.3 at this writing)
* Supports all the TWAIN functions in the spec
* Hosts an internal message loop so there's no need to hook into application UI thread
 
The solution contains tester projects in winform, wpf, and even (gasp!) console. 
A nuget package is also [available here](https://www.nuget.org/packages/ntwain)

Using the lib
--------------------------------------
To properly use this lib you will need to be reasonably familiar with the TWAIN spec
and how it works in general (especially capability). 
The spec can be downloaded from [twain.org](http://twain.org/). 

Except for certain "important" state-changing calls that have been 
abstract away, most triplet operations are 
provided as-is so you will need to know when and how to use them. 
There are no high-level, single-line scan-a-page-for-me-now functions.

The main class to use is TwainSession. You can either use it directly by subscribing
to the important events or sub-class it and override the OnMethods related to those events.
The sample projects contain both usages. Note that an application process should only
have one TwainSession, unless you really know what you're doing.

```
#!c#
// can use the utility method to create appId or make one yourself
var appId = TWIdentity.CreateFromAssembly(DataGroups.Image, Assembly.GetExecutingAssembly());

// new it up and handle events
var session = new TwainSession(appId);

session.TransferReady += ...
session.DataTransferred += ...

```

TwainSession class provides many events, but these 2 are the most important

* TransferReady - fired before a transfer occurs. You can cancel the current transfer 
or all subsequent transfers using the event object.
* DataTransferred - fired after the transfer has occurred. The data available depends on 
what you've specified using the TWAIN API before starting the transfer.


To get into transfer mode, you'll have to call these methods after setting up the session:

1. OpenManager() - opens the TWAIN data source manager (DSM). You can really keep this open throughout the app life time with no ill effect.
2. OpenSource() - opens a target device. You can continue to open and close sources as long as DSM is open.
3. EnableSource() - starts transferring

After transfer has completed (you are notified of this with the SourceDisabled event) 
and you're done with TWAIN, you can call their equivalents in correct hierarchical order like html

1. CloseSource()
2. CloseManager()


While most TWAIN APIs are done via the low-level triplet calls, this lib does provide some
commonly used functions as extension methods to TwainSession (especially capability functions).
This should make setting simple things such as DPI, bitdepth, or paper size easier. 
More of these extensions may come in later versions.

Caveats
--------------------------------------
At the moment this lib does not provide ways to parse transferred image data and require
consumers to do the conversion themselves. The winform project contains one such 
example for handling DIB image in native transfer using the CommonWin32 lib.

Because it hosts its own message thread, the events will likely be raised from another thread. 
If you would like things marshalled to a UI thread then set the SynchronizationContext property
to the one from the UI thread. 


```
#!c#
// set this while in a UI thread
session.SynchronizationContext = SynchronizationContext.Current;

```

64-bit OS
--------------------------------------
If the application process is running in 64-bit then you will need to make sure you have the 
newer data source manager (twaindsm.dll) from below installed. 
[DSM from TWAIN.org](http://sourceforge.net/projects/twain-dsm/files/TWAIN%20DSM%202%20Win/)

Otherwise just compile and run the app as x86 and it'll use the 32-bit version (twain_32.dll) that comes with Windows.

Note that there are no known 64-bit TWAIN DS drivers at the time of writing, so most likely you will have to
compile the application as x86 or run on 32-bit OS to work with a real device.
If you really want to test in 64-bit for whatever reason, you can use 
the test one from TWAIN.org below.
[Sample DS from TWAIN.org](http://sourceforge.net/projects/twain-samples/files/TWAIN%202%20Sample%20Data%20Source/TWAIN%20DS%202.1.3/)