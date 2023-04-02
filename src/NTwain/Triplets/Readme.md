All TWAIN operations are done through the a combination of 
Data Group (DG), Data Argument Type (DAT), and Message (MSG) 
triplets. Rather than dealing with all the combinations 
directly and risk passing the wrong thing, all valid triplet 
combinations are simply made available under this namespace.

Example:
To get the status of the DS, just use the 
"Get" method (represents MSG), in the
"Status" property (represnts DAT), in the
"DGControl" class (represents DG).

or better explained in code:

DGControl.Status.Get(...)

Only triplets usable by the
application-side are defined here.

These are still low-level calls and TwainSession is the higher
level abstraction with some state keeping and other checks for ease of use.
