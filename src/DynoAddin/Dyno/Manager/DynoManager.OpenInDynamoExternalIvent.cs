using Autodesk.Revit.UI;
using Prorubim.DynoRevitCore;

namespace Dyno.Manager
{
    public partial class DynoManager
    {
        public class OpenInDynamoExternalIvent : IExternalEventHandler
        {
            public void Execute(UIApplication app)
            {
                if (!DynamoProductsManager.CheckActiveDynamoProduct()) return;

                var id = RevitCommandId.LookupCommandId("CustomCtrl_%CustomCtrl_%Add-Ins%Dyno%Dyno Browser");
                DynoAppBase.UiApp.PostCommand(id);
            }

            public string GetName()
            {
                return "Open In Dynamo Workspace";
            }
        }
    }
}