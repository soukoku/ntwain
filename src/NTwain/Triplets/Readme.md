All TWAIN operations are done through the a combination of 
Data Group (DG), Data Argument Type (DAT), and Message (MSG) 
triplets. Rather than letting consumers of this lib deal 
with all the combinations themselves and risk passing the 
wrong thing, all valid triplet combinations are simply 
made available under this namespace.

Example:
To get the status of the DS, just use the 
"Get" method (represents MSG), in the
"Status" property (represnts DAT), in the
"DGControl" class (represents DG).

or better explained in code:

DGControl.Status.Get(...)

Only triplets usable by the
application-side are defined here.

Due to the number of things involved, the properties are 
lazy-loaded until used.
