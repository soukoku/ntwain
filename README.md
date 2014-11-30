TWAIN Application-Side Library
==============================

Info
--------------------------------------
This is a library created to make working with [TWAIN](http://twain.org/) interface possible in dotnet. 
This project has these features/goals:

* Targets latest TWAIN version (2.3 at this writing)
* Supports all the TWAIN functions in the spec
* Optionally hosts an internal message loop so there's no need to hook into application UI thread
 
The solution contains tester projects in winform, wpf, and even (gasp!) console. 
A nuget package is also [available here](https://www.nuget.org/packages/ntwain) 
(NOTE: this doc describes v3. For older version go to Source and choose v2 branch.)

Using the lib
--------------------------------------
To properly use this lib you will need to be reasonably familiar with the TWAIN spec
and understand how it works in general (especially capability). 
The spec can be downloaded from [twain.org](http://twain.org/). 

Except for those that have been abstracted away with .net equivalents, most triplet operations are 
provided as-is so you will need to know when and how to use them. 
There are no high-level, single-line scan-a-page-for-me-now functions yet.

The main class to use is TwainSession. You can either use it directly by subscribing
to the important events or sub-class it and override the On* methods related to those events.
The sample projects contain both usages. Note that an application process should only
have one active (open) TwainSession at a time.

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

// choose and open the first source found
// note that TwainSession implements IEnumerable<DataSource> so we can do this
DataSource myDS = session.FirstOrDefault();
myDS.Open();

```

At this point you can use all the typical TWAIN triplet API for working with a data source.


```
#!c#

// all exposed triplet operations are defined through these properties.
// if the operation you want is not available, that most likely means 
// it's not for consumer use or there's an equivalent API in this lib.
myDS.DGControl...;
myDS.DGImage...;

```

Additionally, the DataSource class itself has some handy pre-defined wrappers for common capability 
negotiation. You can use the wrapper properties to see what capabilities or their operations are 
supported. You also won't have to keep track of what value types to use since the wrapper defines it
for you. Most capabilities only require a simple single value to set
and the wrapper makes it easy to do that (see example below):


```
#!c#

// The wrapper has many methods that corresponds to the TWAIN capability triplet msgs like
// Get(), GetCurrent(), GetDefault(), Set(), etc.
// (see TWAIN pdf doc for reference)


// This example sets pixel type of scanned image to BW and
// IPixelType is the wrapper property on the data source.
// (note: the name of the wrapper property is the same as the CapabilityId enum)
PixelType myValue = PixelType.BlackWhite; 

if (myDS.ICapPixelType.CanSet  &&
    myDS.ICapPixelType.Get().Contains(myValue))
{
    myDS.ICapPixelType.Set(myValue);
}


```


When you're ready to get into transfer mode, just call Enable() on the source object.

```
#!c#

myDS.Enable(...);

```

After transfer has completed (remember that you are notified of this with the SourceDisabled event from session) 
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

If you call session.Open() without passing a message loop hook argument, it will run an 
internal message loop behind the scenes. When this happens the events will be raised from another thread. 
If you would like things marshalled to a UI thread then set the experimental SynchronizationContext property
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

In fact, installing the new DSM is recommended whether you're running in 64-bit or not.

If the scanner's TWAIN driver is still 32-bit (and most likely it will be) then you'll have no choice but to
compile the app as x86.
