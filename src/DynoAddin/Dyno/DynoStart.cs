#region

using System.Windows;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Dynamo.Applications;
using Dyno.Manager;
using Prorubim.DynoRevitCore;

#endregion

namespace Dyno
{
    [Transaction(TransactionMode.Manual),
     Regeneration(RegenerationOption.Manual)]
    public class DynoStart : IExternalCommand
    {
        internal static bool IsFirstStart = true;

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            DynoManager.CommandData = commandData;

            if (IsFirstStart)
            {
                IsFirstStart = false;

                if (DynamoProductsManager.CheckActiveDynamoProduct(false))
                    DynamoProductsManager.LaunchDynamoCommandForInit();

                return Result.Succeeded;
            }

            if (DynoManager.OpenedWorkspaceInDynamo != "")
            {
                commandData.JournalData.Clear();
                commandData.JournalData.Add(JournalKeys.DynPathKey, DynoManager.OpenedWorkspaceInDynamo);
                commandData.JournalData.Add(JournalKeys.DynPathExecuteKey, "false");

                DynoManager.OpenedWorkspaceInDynamo = "";
                DynamoProductsManager.LaunchDynamoCommandForOpen();

                return Result.Succeeded;
            }

            if (DynoApp.GetApp().DynoManager.Settings.WindowShowing) DynoApp.GetApp().WindowHide();
            else
                DynoApp.GetApp().WindowShow();

            return Result.Succeeded;
        }
    }
}