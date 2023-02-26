// See https://aka.ms/new-console-template for more information
using Breezee.Core.Adapter.IBLL;
using Breezee.Core.IOC;

Console.WriteLine("Hello, World!");


IADPLog log = ContainerContext.AutofacContainer.Resolve<IADPLog>();
log.LogInfo("AutofacContainer log info.");
log.LogError("AutofacContainer error log info.");

Console.WriteLine("IOC OK!");