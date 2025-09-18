using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Interop;
using Autodesk.Revit.UI;
using CoreNodeModels;
using Dyno;
using Dyno.Models.Parameters;
using WorkspaceFormWindow = Dyno.Views.WorkspaceFormWindow;

namespace Prorubim.DynoRevitCore.ViewModels
{
    public partial class DynoRevitManagerBase
    {
        public class PrepareExternalIvent : IExternalEventHandler
        {
            public void Execute(UIApplication app)
            {
                if (!DynamoProductsManager.CheckActiveDynamoProduct()) return;
                DynamoProductsManager.LaunchDynamoCommandForInit();

                if (SelectedWorkspaceList != null)
                    SelectedWorkspacePreset = SelectedWorkspaceList.Presets.FirstOrDefault();

                if (SelectedWorkspacePreset == null) return;

                var presetVersion = new Version($"{SelectedWorkspacePreset.Workspace.DynamoVersion}.0");
                if (DynamoProductsManager.SelectedProduct.VersionInfo < presetVersion)
                {
                    MessageBox.Show($"You are trying to start workspace version {SelectedWorkspacePreset.Workspace.DynamoVersion} but your active Dynamo version is {DynamoProductsManager.SelectedProduct.VersionInfo}", "Dyno");
                    return;
                }


                try
                {
                    if (SelectedWorkspaceList == null && SelectedWorkspacePreset.IsForm && !IsSilentMode)
                    {
                        _openedForm?.Close();
                        _openedForm = null;

                        if (RevitDynamoModelInstance.CurrentWorkspace.FileName !=
                            SelectedWorkspacePreset.Workspace.WorkspacePath)
                            RevitDynamoModelInstance.OpenFileFromPath(SelectedWorkspacePreset.Workspace
                                .WorkspacePath);

                        if (SelectedWorkspacePreset.Parameters.FirstOrDefault(x => x is DropDownParameter) != null)
                            foreach (var node in RevitDynamoModelInstance.CurrentWorkspace.Nodes)
                                if (node is DSDropDownBase)
                                {
                                    var dd = node as DSDropDownBase;
                                    dd.PopulateItems();
                                    var par = SelectedWorkspacePreset.GetParameterByGuid(node.GUID);

                                    if (par != null && node.GetType().FullName == "DSRevitNodesUI.Categories")
                                        par.Values =
                                            new ObservableCollection<object>(dd.Items.Select(x => x.Name));
                                    else if (par != null)
                                        par.Values = new ObservableCollection<object>(dd.Items.Select(x => x.Name));
                                }

                        UpdateFormIfExists();

                        var wfw = new WorkspaceFormWindow(SelectedWorkspacePreset);
                        _openedForm = wfw;
                        wfw.RunButton.Click += (sender, args) => { _runExternalEvent.Raise(); };

                        wfw.RunAndCloseButton.Click += (sender, args) =>
                        {
                            _runExternalEvent.Raise();

                            wfw.Close();
                        };

                        wfw.Closed += (sender, args) =>
                        {
                            wfw = null;
                            _openedForm = null;
                        };

                        // ReSharper disable once UseObjectOrCollectionInitializer
                        var windowInteropHelper = new WindowInteropHelper(wfw)
                        {
                            Owner = Autodesk.Windows.ComponentManager.ApplicationWindow
                        };

                        wfw.Show();
                    }
                    else
                        PrepareAndRunWorkspace();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Preparing Evaluation");
                }
            }

            public string GetName() => "Evaluate Workspace";
        }
    }
}