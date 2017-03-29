FreeTextBox 3.0 Source Code

***************************************
* Visual Studio Solutions
***************************************
There are several VS.NET Solution Files included, depending on the version of Visual Studio and 
target .NET framework you are using. Please choose the appropriate solution.

Each solution has an individual AssemblyInfo-NET-x-x.cs file which references a 
"strong name" file. To compile FreeTextBox you need to either
1. Create your own strong name file (see http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cptools/html/cpgrfstrongnameutilitysnexe.asp)
2. Remove the reference to the strong name file. It looks like [assembly: AssemblyKeyFile("FreeTextBox.snk")]

If you would like to create a version of FreeTextBox that has the pro features enabled by default,
you can do one of the following two things:

***************************************
* Licensing
***************************************

1. Delete all uses of FtbLicense within FreeTextBox.cs (or create a license object 
   that is always set to pro)

2. Open the LicenseGenerator folder and create your own license. There are three license options
   For SingleLicense and DistributionLicense (which do the same thing) simply enter a company name
   for the license in the second blank. You can also create an ExpiringLicense by entering a date
   into the second blank. Saved the encrypted output to a file called FreeTextBox.lic and put this 
   in the /bin/ folder with FreeTextBox.dll

When creating a license, please ensure that the variable "encryptionKeyBytes" is the same in 
   both the /Licensing/FtbLicenseProvider.cs file and in /LicenseGenerator/Encryptor.cs