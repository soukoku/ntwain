All TWAIN operations are done through the a combination of 
Data Group (DG), Data Argument Type (DAT), and Message (MSG) 
triplets. Rather than dealing with all the combinations 
directly in the DSM pinvokes and risk passing the wrong thing, 
all valid triplet combinations are made available under this namespace.

Example:
To get the status of the DS, the triplet is
`DG_Control / DAT_STATUS / MSG_GET` in the documentation. 
With this wrapper you can use a similar call path:

```cs
DGControl.Status.Get(...);
```

Only triplets usable by the application-side are defined here.

These are still relatively low-level calls and `TwainAppSession` is the next higher
level abstraction with some state keeping and other dotnet-friendly methods
and events for regular use.
