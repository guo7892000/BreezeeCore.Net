// See https://aka.ms/new-console-template for more information
using Breezee.Core.Adapter.IBLL;
using Breezee.Core.IOC;

Console.WriteLine("Hello, World!");


IADPLog log = ContainerContext.AutofacContainer.Resolve<IADPLog>();
log.LogInfo("AutofacContainer log info.");
log.LogError("AutofacContainer error log info.");

Console.WriteLine("IOC OK!");

string path = "C:/folder1/folder2/file.txt";
string dirFull =Path.GetDirectoryName(path);
string dirName = Path.GetFileName(dirFull);
Console.WriteLine(dirFull);
Console.WriteLine(dirName);