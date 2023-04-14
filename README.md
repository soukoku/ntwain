# TWAIN dotnet library

NOTE: This is a rewrite of the original NTwain lib and is still
in early stages. Use the V3 branch for the current version.

## Info

This is a dotnet library created to make working with 
[TWAIN](http://twain.org/) devices easier in dotnet. 
It internally uses some parts of the
[twaincs](https://github.com/twain/twain-cs) code from
the TWAIN Working Group.

V4 of this lib has these features:

* Targets TWAIN version 2.5.
* Runs under supported framework (4.6.2+) and netcore variants (6.0+).
* Easier to use than the low-level C API with many dotnet niceties.
* Attempt at reducing heap allocations compared to previous versions.


## Compred to older versions

These are not implemented yet in this early version:

* Image memory transfer (DAT_IMAGEMEMXFER). 
* Audio native transfer (probably never will).

As with previous versions, only Windows has been tested on and thus 
supported really. Other changes include

* All TWAIN data types are now struct instead of class (and they come
from [twaincs](https://github.com/twain/twain-cs) for correctness. It may have
been easier to implement them as classes when starting out this lib, but 
it's not really ideal anymore. The change also makes them match the twain.h
names and the spec pdf.

* All lower-level TWAIN APIs are public instead of hidden away.

## Using the lib

Before using this lib, you are required to be reasonably 
familiar with the TWAIN spec and understand how it works in general. 
The TWAIN spec pdf can be downloaded from [twain.org](http://twain.org/). 

The main class to use is `TwainAppSession`. This is the highest abstraction level
provided by this lib. A lower-level abstraction is the triplet calls 
(under `NTwain.Triplets` namespace). The lowest level is the pinvoke dsm calls 
(under `NTwain.DSM` namespace).

You use `TwainAppSession` by subscribing to its events and calling methods to do TWAIN things.
There is a sample winform project (both 32 and 64 bit variants) on how it can be used. 
Note that an application process should only have one active (opened) 
`TwainAppSession` at any time.
