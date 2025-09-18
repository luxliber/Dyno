using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Dynamo.Applications;
using Dynamo.Graph.Nodes.ZeroTouch;
using WorkspaceDialogWindow = Dyno.Views.WorkspaceDialogWindow;

namespace Prorubim.DynoNodes
{
    /// <summary>
    /// sds
    /// </summary>
    public class DynoStudio
    {
        internal DynoStudio()
        {
        }

        /// <summary>
        /// Shows script form as dialog window. Node allows to enter input values into dialog controls and get output data
        /// </summary>
        /// <returns>
        /// Output data
        /// </returns>
        public static IList ShowDialog(List<string> tabNames, List<string> bindingNames, IList bindingValues, bool willBeShown = true)
        {
            if (willBeShown)
            {


                var revitDynamoModel = DynamoRevit.RevitDynamoModel;
                if (revitDynamoModel == null) return null;

                var scriptPath = revitDynamoModel.CurrentWorkspace.FileName;
                var scriptName = Path.GetFileNameWithoutExtension(scriptPath);
                var form = DynoScriptHelper.ScanFormFile(scriptPath);

                if (form == null) return null;

                var formBindings = DynoScriptHelper.GenerateBindingValues(bindingNames, bindingValues, form);

                var f = new WorkspaceDialogWindow(scriptName, form, formBindings, tabNames);
                f.ShowDialog();

                var res = new ArrayList();

                foreach (var bindingName in bindingNames)
                    if (form.PortPars.Keys.Contains(bindingName))
                        res.Add(PrepareValue(form.PortPars[bindingName].Value));
                    else if (formBindings.Keys.Contains(bindingName))
                        res.Add(PrepareValue(formBindings[bindingName].Value));
                    else
                        res.Add(null);


                var dialogNodes = revitDynamoModel.CurrentWorkspace.Nodes.Where(
                    x => (x as DSFunction) != null &&
                         ((DSFunction)x).CreationName.Contains("Prorubim.DynoNodes.DynoStudio.ShowDialog")).Cast<DSFunction>();

                foreach (var dialogNode in dialogNodes)
                    dialogNode.MarkNodeAsModified(true);

                return res;
            }

            return bindingValues;
        }

       

        private static void EngineController_AstBuilt(object sender, Dynamo.Engine.CodeGeneration.CompiledEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private static object PrepareValue(object value)
        {
            if (value == null)
                return null;

            float floatRes;
            int intRes;
            bool boolRes;


            if (bool.TryParse(value.ToString(), out boolRes))
                return boolRes;
            if (float.TryParse(value.ToString(), out floatRes))
                return floatRes;
            if (int.TryParse(value.ToString(), out intRes))
                return intRes;

            return value;
        }
    }
}
