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

Except for those that have been abstracted away with .net equivalents, most triplet operations are 
provided as-is so you will need to know when and how to use them. 
There are no high-level, single-line scan-a-page-for-me-now functions yet.

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

// finally open it
session.Open();

```

TwainSession class provides many events, but these 2 are the most important

* TransferReady - fired before a transfer occurs. You can cancel the current transfer 
or all subsequent transfers using the event object.
* DataTransferred - fired after the transfer has occurred. The data available depends on 
what you've specified using the TWAIN API before starting the transfer.


Once you've setup and opened the session, you can get available sources, pick one to use,
and call Open() to start using it.


```
#!c#

// choose and open a source
IEnumerable<TwainSources> sources = session.GetSources();
var myDS = sources.First();
myDS.Open();

```

At this point you can negotiate with the source using all the typical TWAIN triplet API.
The TwainSource class itself has some handy pre-defined methods for common capability negotiation
such as DPI, bitdepth, or paper size to get you started.

When you're ready to get into transfer mode, just call StartTransfer() on the source object.

```
#!c#

var myDS = sources.StartTransfer(...);

```

After transfer has completed (you are notified of this with the SourceDisabled event from session) 
and you're done with TWAIN, you can close the source and the session in sequence to clean things up.

```
#!c#

myDS.Close();
session.Close();

```


Caveats
--------------------------------------
At the moment this lib does not provide ways to parse transferred image data and require
consumers to do the conversion themselves. The winform project contains one such 
example for handling DIB image in native transfer using the CommonWin32 lib.

Because it hosts its own message thread, the events will be raised from another thread. 
If you would like things marshalled to a UI thread then set the SynchronizationContext property
to the one from the UI thread. 

```
#!c#
// set this while in a UI thread
session.SynchronizationContext = SynchronizationContext.Current;

```
Note that on certain scanner drivers this may hang the 
application due to their use of modal dialogs, so if you find yourself in that position 
you'll have to find another way to synchronize data to UI threads. 


64-bit OS
--------------------------------------
If the application process is running in 64-bit then you will need to make sure you have the 
newer data source manager (twaindsm.dll) from below installed. 

[DSM from TWAIN.org](http://sourceforge.net/projects/twain-dsm/files/TWAIN%20DSM%202%20Win/)

Otherwise just compile and run the app as x86 and it'll use the 32-bit version (twain_32.dll) that comes with Windows.
If your scanner driver is still 32-bit (and most likely it will be) you'll have no choice but to
compile as x86 anyway, even if you have installed the newer dsm dll.
