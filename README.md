Isncsci Algorithm
=================

This algorithm is designed to produce a spinal cord injury classification consistent with the International Standards for Neurological Classification of Spinal Cord Injury developed and maintained by the American Spinal Injury Association (ASIA).

It was written in C# using the .Net framework 4.5.  The current version of this code is v1.0.3.  A working version of this code is available at http://www.isncscialgorithm.com/  The code is also available as a NuGet dowloadable dynamic-link library (dll) at https://www.nuget.org/packages/RhiIsncsci/

Information on how to use our NuGet package is available in our [getting started guide](http://www.isncscialgorithm.com/SourceCode/GettingStarted).  You can also download a sample project from our [user guide](http://www.isncscialgorithm.com/SourceCode).


###Version History:

* __1.0.3__ - _Wednesday, January 28 2015_: A new NuGet package was published, containing the changes in version 1.0.2, to support both .Net framewors: 4.0 and 4.5.

* __1.0.2__ - _Tuesday, January 27 2015_: We have fixed a minor issue where, in some special cases only, the algorithm was not comparing both left and right when determining if motor function was present more than three levels below the motor level on that side.  This error was causing these cases, which should have been evaluated as AIS C, to be evaluated as UTD with options of AIS B and AIS C.

* __1.0.1__ - _Friday, April 11 2014_: A new NuGet package was published to support both .Net framewors: 4.0 and 4.5.

* __1.0.0__ - _Wednesday, April 09 2014_: First public release of the ISNCSCI Algorithm.
