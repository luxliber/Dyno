using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Autodesk.DesignScript.Runtime;
using Dyno.Models.Forms;

namespace Prorubim.DynoNodes
{
#pragma warning disable 1591
    /// <summary>
    /// Formutilites class
    /// </summary>
    [IsVisibleInDynamoLibrary(false)]
    public class DynoScriptHelper
    {
        internal static Dictionary<string, PortParameter> GenerateBindingValues(List<string> bindingNames, object bindingValues, WorkspaceForm form)
        {
            var formBindings = new Dictionary<string, PortParameter>();

            ArrayList bindingValuesList;

            if (bindingValues is ArrayList)
                bindingValuesList = (ArrayList) bindingValues;
            else
                bindingValuesList = new ArrayList {bindingValues};


            var minCount = Math.Min(bindingNames.Count, bindingValuesList.Count);
            for (var i = 0; i < minCount; i++)
                if (bindingValuesList[i] is ArrayList)
                {
                    var arr = bindingValuesList[i] as ArrayList;
                    if (arr.Count > 0)
                        formBindings.Add(bindingNames[i],
                            new PortParameter()
                            {
                                Name = bindingNames[i],
                                Value = arr[0],
                                Values = new List<object>(arr.Cast<object>())
                            });
                }
                else
                    formBindings.Add(bindingNames[i],
                        new PortParameter
                        {
                            Name = bindingNames[i],
                            Value = bindingValuesList[i],
                            Values = new List<object> {bindingValuesList[i]}
                        });

            foreach (var testPar in form.UserPars)
                if (!formBindings.ContainsKey(testPar.Name))
                    formBindings.Add(testPar.Name,
                        new PortParameter
                        {
                            Name = testPar.Name,
                            Value = testPar.Value,
                            Values = new List<object> {testPar.Value}
                        });

            return formBindings;
        }

        internal static WorkspaceForm ScanFormFile(string scriptPath)
        {
            var formPath = new FileInfo(scriptPath).DirectoryName;
            if (formPath == null) return null;

            var name = Path.GetFileNameWithoutExtension(scriptPath);

            var formFile = Path.Combine(formPath, name + ".dfm");

            if (File.Exists(formFile))
                return LoadForm(formFile);

            return null;
        }

        private static WorkspaceForm LoadForm(string path)
        {
            return WorkspaceForm.LoadFromPath(path);
        }

        public static object ShowFormFunc(string tabName, IList args, int ind)
        {
            /*      if (ind == -1)
                  {

                      var f = new WorkspaceDialogWindow(Form, FormBindings, args);
                      f.ShowDialog();
                      Form.UpdateNodeBindings(FormBindings);
                  }


                  if (ind == -1)
                      return tabName;
                  else
                  {
                      return ind < FormBindings.Count ? FormBindings[FormBindings.Keys.ElementAt(ind)] : null;
                  }*/
            return null;
        }
    }

#pragma warning restore 1591
}
