TWAIN Application-Side Library
==============================

Info
--------------------------------------
This is a dotnet library created to work with [TWAIN](http://twain.org/) interface. 
This project follows these general goals:

* Targets latest TWAIN version (2.3 at the moment)
* Supports all the TWAIN functions in the spec
* Eventally work on platforms other than Windows (I can dream)
 
The solution contains sample projects in winforms, wpf, and even (gasp!) console. 
A nuget package is also [available here](https://www.nuget.org/packages/ntwain)

Using the lib
--------------------------------------
To properly use this lib you will need to be reasonably familiar with the TWAIN spec
and how it works in general. Except for certain "important" calls that drive the
TWAIN state change, most triplet operations are only availble as-is so you will need to know
when and how to use them. There are no high-level, single-line scan-a-page-for-me-now functions.

At the moment this lib does not provide ways to parse transferred image data and require
consumers to do the conversion. The winform project contains one such 
example for handling DIB image in native transfer.