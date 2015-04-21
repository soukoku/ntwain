TWAIN Application-Side Library
==============================

Info
--------------------------------------
This is a library created to make working with [TWAIN](http://twain.org/) interface possible in dotnet. 
This project has these features/goals:

* Targets latest TWAIN version (2.3 as of this writing)
* Supports all the TWAIN functions in the spec.
* Can host an internal message loop so there's no need to hook into application UI thread.
 
The solution contains tester projects in winform, wpf, and even console usage. 
A nuget package is also [available here](https://www.nuget.org/packages/ntwain) 
(NOTE: this doc describes v3. For older version go to Source and choose v2 branch for its doc.)

Using the lib
--------------------------------------
To properly use this lib you will need to be reasonably familiar with the TWAIN spec
and understand how it works in general (especially capability). 
The TWAIN spec can be downloaded from [twain.org](http://twain.org/). 

Except for those that have been abstracted away with .net equivalents, most triplet operations are 
provided as-is so you will need to know when and how to use them. 
There are no high-level, scan-a-page-for-me-now functions.

The main class to use is TwainSession. You can either use it directly by subscribing
to its events or sub-class it and override the On* methods related to those events.
The sample projects contain both usages. Note that an application process should only
have one active (opened) TwainSession at any time.

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

TwainSession class provides many events, but these 3 are the most important

* TransferReady - fired before a transfer occurs. You can cancel the current transfer 
or all subsequent transfers using the event object.
* DataTransferred - fired after the transfer has occurred. The data available depends on 
what you've specified using the TWAIN API before starting the transfer. If using native image transfer, the event arg provides a quick GetNativeImageStream() method to convert the
data pointer to a System.IO.Stream for use in .net.
* TransferError - fired when exceptions are encountered during the transfer phase. 

NOTE: do not try to close the source/session in the handler of these 3 events or 
unpredictable things will happen. Either let the scan run its course or cancel the scan using the flag 
in the TransferReady event arg.

Once you've setup and opened the session, you can get available sources, pick one to use,
and call the Open() method of the data source to start using it as the example below:


```
#!c#

// choose and open the first source found
// note that TwainSession implements IEnumerable<DataSource> so we can use this extension method.
DataSource myDS = session.FirstOrDefault();
myDS.Open();

```

At this point you can use all the typical TWAIN triplet API for working with a data source.


```
#!c#

// All low-level triplet operations are defined through these properties.
// If the operation you want is not available, that most likely means 
// it's not for consumer use or it's been abstracted away with an equivalent API in this lib.
myDS.DGControl...;
myDS.DGImage...;

```

Additionally, the DataSource class has a Capabilities property with the pre-defined wrappers for capability negotiation. You can use the wrapper properties to see what capabilities or their operations are supported. You also won't have to keep track of what enum type to use for a capability. Most capabilities only require a simple single value to set
and the wrapper makes it easy to do that (see example below):


```
#!c#

// The wrapper has many methods that corresponds to the TWAIN capability triplet msgs like
// GetValues(), GetCurrent(), GetDefault(), SetValue(), etc.
// (see TWAIN spec for reference)


// This example sets pixel type of scanned image to BW and
// IPixelType is the wrapper property on the data source.
// The name of the wrapper property is the same as the CapabilityId enum.
PixelType myValue = PixelType.BlackWhite; 

if (myDS.Capabilities.ICapPixelType.CanSet  &&
    myDS.Capabilities.ICapPixelType.GetValues().Contains(myValue))
{
    myDS.Capabilities.ICapPixelType.SetValue(myValue);
}


```


When you're ready to get into transfer mode, just call Enable() on the data source object.

```
#!c#

myDS.Enable(...);

```

After transfer has completed (you are notified of this with the SourceDisabled event from the session object) and you're done with TWAIN, you can close the source and the session in sequence to clean things up.

```
#!c#

myDS.Close();
session.Close();

```


Caveats
--------------------------------------
At the moment the DataTransferredEventArgs only provides conversion routine to 
an image stream when using native transfer.
If other transfer methods are used you'll have to deal with them yourself.

If you just call session.Open() without passing a message loop hook parameter, an 
internal message loop will be started behind the scenes. When this happens the session events will be raised from another thread. 
If you would like things marshalled to the UI thread then set the experimental SynchronizationContext property the one from your UI thread. 

```
#!c#
// set this while in a UI thread
session.SynchronizationContext = SynchronizationContext.Current;

```
Note that on certain scanner drivers this may hang the 
application due to their use of modal dialogs, so if you find yourself in that position 
you'll have to find another way to synchronize data to UI threads. 


Using the new twaindsm.dll
--------------------------------------
By default NTwain will use the newer [data source manager](http://sourceforge.net/projects/twain-dsm/files/TWAIN%20DSM%202%20Win/)
(twaindsm.dll) if available. To override this behavior
set the PlatformInfo's PreferNewDSM flag to false. This is necessary due to some older sources not working with the newer dsm.

```
#!c#
// go back to using twain_32.dll under windows,
// do this once at app startup.
NTwain.PlatformInfo.Current.PreferNewDSM = false;

```


If the application process is going to be running in 64-bit then this flag will have no effect and you will 
always need to have the twaindsm installed. 

If the scanner's TWAIN driver is still 32-bit then you'll have need to compile the application exe in x86 or you won't see the driver.