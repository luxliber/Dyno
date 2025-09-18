using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Autodesk.Revit.UI;
using CoreNodeModels;
using Dyno.Models.Parameters;
using DynoUI;
using Prorubim.DynoRevitCore;

namespace Dyno.Manager
{
    public partial class DynoManager
    {
        public class DropDownExternalIvent : IExternalEventHandler
        {
            public DropDownParameter Par;
            public Point Point;

            public void Execute(UIApplication app)
            {
                if (!DynamoProductsManager.CheckActiveDynamoProduct()) return;
                DynamoProductsManager.LaunchDynamoCommandForInit();

                try
                {
                    if (RevitDynamoModelInstance.CurrentWorkspace.FileName != Par.Workspace.Workspace.WorkspacePath)
                        RevitDynamoModelInstance.OpenFileFromPath(Par.Workspace.Workspace.WorkspacePath);

                    foreach (var node in RevitDynamoModelInstance.CurrentWorkspace.Nodes)
                        if (node.GUID.ToString() == Par.Guid)
                        {
                            if (!(node is DSDropDownBase dd)) continue;

                            dd.PopulateItems();
                            if (node.GetType().FullName == "DSRevitNodesUI.Categories")
                                Par.Values =
                                    new ObservableCollection<object>(
                                        dd.Items.Select(x => x.Name/*.Substring(4)*/));
                            else
                                Par.Values = new ObservableCollection<object>(dd.Items.Select(x => x.Name));

                            if (Par.Index >= 0 && Par.Index < Par.Values.Count)
                                Par.Value = Par.Values[Par.Index];
                            else
                                Par.Value = "";

                            break;
                        }

                    var window = new StringWindow
                    {
                        Values = Par.Values,
                        ValueText = Par.Value.ToString(),
                        Left = Point.X,
                        Top = Point.Y,
                        Desc = Par.Desc,
                        Title = Par.Name
                    };
                    if (window.ShowDialog() == true)
                    {
                        Par.Workspace.IsChanged = true;
                        Par.Value = window.ValueText;
                        Par.Workspace.OnPropertyChanged("IsChanged");
                    }
                    Par.OnPropertyChanged("Value");
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            public string GetName()
            {
                return "Evaluate Workspace";
            }
        }
    }
}