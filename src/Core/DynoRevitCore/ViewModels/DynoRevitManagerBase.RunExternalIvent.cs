using System;
using System.Windows;
using Autodesk.Revit.UI;

namespace Prorubim.DynoRevitCore.ViewModels
{
    public partial class DynoRevitManagerBase
    {
        public class RunExternalIvent : IExternalEventHandler
        {
            public void Execute(UIApplication app)
            {
                try
                {
                    PrepareAndRunWorkspace();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Starting Evaluation");
                }
            }

            public string GetName()
            {
                return "Evaluate Workspace";
            }
        }
    }
}