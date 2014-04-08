TWAIN Application-Side Library
==============================

Info
--------------------------------------
This is a dotnet library created to work with [TWAIN](http://twain.org/) interface. 
This project follows these general goals:

* Targets latest TWAIN version (2.3 at the moment)
* Supports all the TWAIN functions in the spec (mostly there)
* Eventally work on platforms other than Windows (just a dream)
 
The solution contains sample projects in winforms, wpf, and even (gasp!) console. 
A nuget package is also [available here](https://www.nuget.org/packages/ntwain)

Using the lib
--------------------------------------
To properly use this lib you will need to be reasonably familiar with the TWAIN spec
and how it works in general. Except for certain "important" calls that drive the
TWAIN state change, most triplet operations are availble as-is so you will need to know
when to use them. There are no high-level, single-line scan-a-page-for-me-now functions here.
