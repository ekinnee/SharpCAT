# SharpCAT
C#, .NET Standard based CAT control library.

I am targeting .Net Standard so that the assembly may be used with .Net Core or the .Net framework.

I'm starting with the FT818, I then plan on adding my ID-4100a, and TH-D74A.

If you wish to help let me know, or create a pull request. I'm not a pro developer, just a hack
that normally builds small tools for himself.

Ideas on how to do this are appreciated!

# Why?
Yes, there's Hamlib, and yes there's HamLibSharp.

https://github.com/N0NB/hamlib

https://github.com/k5jae/HamLibSharp

I don't speak C++ and PInvoke is nasty.

That being said, there's also not a pure .Net (C#) CAT control lib out there that I know of.

# What needs done?
The project is in an early very phase. The following is in no particular order, except the IRadio bit.

1. Implement an IRadio interface.

2. Settle on how the radios and commands are defined.

    JSON?
  
    Or just use a .cs file (current)?
  
3. Need to support opening / using an arbitrary number of radios.

    RigControl can do 2.
  
4. Fully support asynchronous operations.

    There's some handy events exposed by .net for data received on the Serial Port.
  
5. Add support for remote sharing of serial ports.

6. Implemtment the flrig control protocol.
  
    Use this in place of flrig if desired.

7. Implement the ability to run as a service.

8. Implement CAT and CIV control.

    This will come with the first Icom radio implemented I figure.
    
    I have an Icom 4100a and an 880H onhand.
