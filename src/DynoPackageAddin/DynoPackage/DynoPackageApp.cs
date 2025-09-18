using System.IO;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using Dyno;
using Dyno.ViewModels;
using Prorubim.DynoRevitCore;
using Prorubim.DynoRevitCore.ViewModels;

namespace Prorubim.DynoPackage
{
    [Transaction(TransactionMode.Manual),
     Regeneration(RegenerationOption.Manual)]
    public class DynoPackageApp : DynoAppBase
    {
        public static string StorageFolder = Path.Combine(DynoPath, "Storage");

        public override Result OnStartup(UIControlledApplication application)
        {
            base.OnStartup(application);
            var man = new DynoManager();
            
            man.InitExternalEvents();
            return Result.Succeeded;
        }
    }
}