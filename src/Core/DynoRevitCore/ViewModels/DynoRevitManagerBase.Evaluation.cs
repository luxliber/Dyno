using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using Autodesk.Revit.DB;
using CoreNodeModels;
using CoreNodeModels.Input;
using Dynamo.Graph.Nodes;
using Dynamo.Nodes;
using Dyno.Models.Parameters;
using Dyno.Models.Workspaces;
using DateTime = System.DateTime;
using DoubleSlider = CoreNodeModels.Input.DoubleSlider;
using IntegerSlider = CoreNodeModels.Input.IntegerSlider;
using View = System.Windows.Forms.View;

namespace Prorubim.DynoRevitCore.ViewModels
{
    public partial class DynoRevitManagerBase
    {
        private static void RevitDynamoModel_EvaluationCompleted(object sender, EventArgs e)
        {
            RevitDynamoModelInstance.EvaluationCompleted -= RevitDynamoModel_EvaluationCompleted;
            
            SelectedElements.Clear();

            _runnedWorkspacePreset.Status =
                RevitDynamoModelInstance.CurrentWorkspace.Nodes.Any(n => n.State == ElementState.Warning)
                    ? WorkspaceBase.WorkspaceStatus.Error
                    : WorkspaceBase.WorkspaceStatus.Ok;

            var timeNow = DateTime.Now;
            var timeEval = DateTime.Now - _startingTime;

            var errorsPar = (OutputParameter) _runnedWorkspacePreset.GetParameterByName("Errors and Warnings");
            var errorsCounter = 0;

            if (errorsPar == null)
            {
                errorsPar = new OutputParameter("Errors and Warnings", "");
                _runnedWorkspacePreset.Parameters.Insert(0, errorsPar);
            }
            errorsPar.Childs.Clear();

            foreach (var node in RevitDynamoModelInstance.CurrentWorkspace.Nodes)
            {
                var nodeName = "";
                if (DynamoProductsManager.SelectedProduct.VersionInfo.ToString(1) == "1")
                    nodeName = GetNodeName1(node);

                if (DynamoProductsManager.SelectedProduct.VersionInfo.ToString(1) == "2")
                    nodeName = GetNodeName2(node);

                if (node.State == ElementState.Warning)
                {
                    errorsCounter++;

                    if (node.GetType() == typeof(DSModelElementSelection) ||
                        node.GetType() == typeof(DSModelElementsSelection) || node is ReferenceSelection)
                        errorsPar.Childs.Add($"Warning in node: {nodeName} - Nothing selected");
                    else
                        errorsPar.Childs.Add(node.ToolTipText != ""
                            ? $"Error in node: {nodeName} - {node.ToolTipText}"
                            : $"Error in node: {nodeName} - Unknown error");
                }

                if (!(_runnedWorkspacePreset.GetParameterByName("Starting Time") is OutputParameter startingTimePar))
                {
                    startingTimePar = new OutputParameter("Starting Time", "");
                    _runnedWorkspacePreset.AddParameter(startingTimePar);
                }

                startingTimePar.Value = $"{_startingTime.Hour}:{_startingTime.Minute}:{_startingTime.Second}";
                startingTimePar.OnPropertyChanged("Value");
                startingTimePar.OnPropertyChanged("Values");


                if (!(_runnedWorkspacePreset.GetParameterByName("Evaluation Time") is OutputParameter evalTimePar))
                {
                    evalTimePar = new OutputParameter("Evaluation Time", "");
                    _runnedWorkspacePreset.AddParameter(evalTimePar);
                }

                evalTimePar.Value = $"{timeEval.Hours}:{timeEval.Minutes}:{timeEval.Seconds}";
                evalTimePar.OnPropertyChanged("Value");
                evalTimePar.OnPropertyChanged("Values");

                if (!(_runnedWorkspacePreset.GetParameterByName("Ending Time") is OutputParameter endingTimePar))
                {
                    endingTimePar = new OutputParameter("Ending Time", "");
                    _runnedWorkspacePreset.AddParameter(endingTimePar);
                }

                endingTimePar.Value = $"{timeNow.Hour}:{timeNow.Minute}:{timeNow.Second}";
                endingTimePar.OnPropertyChanged("Value");
                endingTimePar.OnPropertyChanged("Values");

                if (node.GetType() == typeof(Watch) && node.IsOutputNode)
                {
                    var par = _runnedWorkspacePreset.GetParameterByName(nodeName);
                    if (par == null)
                    {
                        par = new OutputParameter(nodeName, "");
                        _runnedWorkspacePreset.AddParameter(par);
                    }

                    var tnode = (Watch)node;

                    var nodeVal = tnode.CachedValue;


                    var outputPar = (OutputParameter)par;
                    outputPar.Childs.Clear();
                    if (nodeVal != null)
                        if (nodeVal.GetType() == typeof(ArrayList))
                        {
                            var arrayVal = (ArrayList)nodeVal;
                            outputPar.Value = $"{arrayVal.Count} Items";

                            foreach (var val in arrayVal)
                                outputPar.Childs.Add($"[{arrayVal.IndexOf(val)}] {val}");
                        }
                        else if (nodeVal is double)
                        {
                            var v = nodeVal as double? ?? 0;
                            outputPar.Value = v.ToString(CultureInfo.InvariantCulture);
                        }
                        else
                            outputPar.Value = nodeVal.ToString();
                    else
                        outputPar.Value = "None";

                    par.OnPropertyChanged("Value");
                    par.OnPropertyChanged("Values");
                }
            }
            if (errorsCounter > 0)
            {
                errorsPar.Value = $"{errorsCounter} item(s)";

                errorsPar.OnPropertyChanged("Value");
                errorsPar.OnPropertyChanged("Values");
            }
            else
                _runnedWorkspacePreset.Parameters.Remove(errorsPar);

            //  if(_runnedWorkspacePreset.IsSaveAfterComplete)
          //    RevitDynamoModelInstance.CurrentWorkspace.Save(RevitDynamoModelInstance.EngineController.LiveRunnerRuntimeCore);

            IsSilentMode = false;

            StartNextPlaylistPreset();
        }

        private static void StartNextPlaylistPreset()
        {

            if (SelectedWorkspaceList == null)
                return;

            SelectedWorkspaceListIndex++;

            if (SelectedWorkspaceListIndex < SelectedWorkspaceList.Presets.Count)
            {
                SelectedWorkspacePreset = SelectedWorkspaceList.Presets[SelectedWorkspaceListIndex];
                PrepareAndRunWorkspace();
            }
            else
            {
                SelectedWorkspaceList = null;
                SelectedWorkspacePreset = null;
                SelectedWorkspaceListIndex = 0;
            }
        }

        private static void PrepareAndRunWorkspace()
        {
            _runnedWorkspacePreset = SelectedWorkspacePreset;

            var selectedElIds = DynoAppBase.UiApp.ActiveUIDocument.Selection.GetElementIds();
            foreach (var id in selectedElIds)
                SelectedElements.Add(DynoAppBase.UiApp.ActiveUIDocument.Document.GetElement(id));

            _startingTime = DateTime.Now;

            if (_runnedWorkspacePreset == null)
            {
                if (_openedForm != null && _openedForm.IsActive)
                    _openedForm.Close();

                MessageBox.Show("Active workspace focus is lost. Please select and run workspace again");

                return;
            }

            if (_runnedWorkspacePreset.ForceReopen || RevitDynamoModelInstance.CurrentWorkspace.FileName !=
                _runnedWorkspacePreset.Workspace.WorkspacePath)
                RevitDynamoModelInstance.OpenFileFromPath(_runnedWorkspacePreset.Workspace.WorkspacePath, true);

            _selElNode = null;

            foreach (var node in RevitDynamoModelInstance.CurrentWorkspace.Nodes)
            {
                var nodeName = "";
                if (DynamoProductsManager.SelectedProduct.VersionInfo.ToString(1) == "1")
                    nodeName = GetNodeName1(node);

                if (DynamoProductsManager.SelectedProduct.VersionInfo.ToString(1) == "2")
                    nodeName = GetNodeName2(node);

                if (_runnedWorkspacePreset.UseSelected != "" && nodeName == _runnedWorkspacePreset.UseSelected)
                {
                    if (node is ElementSelection<Element>)
                        _selElNode = node as ElementSelection<Element>;
                }
                else
                {
                    var par = _runnedWorkspacePreset.GetParameterByGuid(node.GUID) ??
                              _runnedWorkspacePreset.GetParameterByNameWithoutGuid(nodeName);

                    if (par == null) continue;

                    if (node is DSDropDownBase && par.GetType() != typeof(SelectElementParameter) &&
                        par.GetType() != typeof(SelectReferenceParameter))
                    {
                        var tnode = (DSDropDownBase)node;

                        DynamoDropDownItem dropDownItem;
                        if (tnode is DSRevitNodesUI.Categories)
                            dropDownItem = tnode.Items.FirstOrDefault(x => x.Name == par.Value.ToString());
                        else
                            dropDownItem = tnode.Items.FirstOrDefault(x => x.Name == par.Value.ToString());
                        if (dropDownItem != null)
                            tnode.SelectedIndex = tnode.Items.IndexOf(dropDownItem);
                    }
                    else if (node.GetType() == typeof(DoubleInput))
                    {
                        var tnode = (DoubleInput)node;
                        tnode.Value = par.AsDouble().ToString(CultureInfo.InvariantCulture);
                    }
                    else if (node.GetType() == typeof(IntegerSlider))
                    {
                        var tnode = (IntegerSlider)node;
                        tnode.Value = par.AsInt();
                    }
                    else if (node.GetType() == typeof(DoubleSlider))
                    {
                        var tnode = (DoubleSlider)node;
                        tnode.Value = par.AsDouble();
                    }

                    else if (node.GetType() == typeof(StringInput))
                    {
                        var tnode = (StringInput)node;
                        tnode.Value = par.Value.ToString();
                    }
                    else if (node.GetType() == typeof(DoubleInput) &&
                             par.GetType() == typeof(NumberSliderParameter))
                    {
                        var tnode = (DoubleInput)node;
                        var nfi = new NumberFormatInfo { NumberDecimalSeparator = "." };

                        tnode.Value = ((NumberSliderParameter)par).Value.ToString();
                    }
                    else if (node.GetType() == typeof(BoolSelector) &&
                             par.GetType() == typeof(BooleanParameter))
                    {
                        var tnode = (BoolSelector)node;
                        tnode.Value = par.AsBoolean();
                    }
                    else if (node.GetType() == typeof(DSModelElementsSelection) &&
                             par.GetType() == typeof(SelectElementParameter))
                    {
                        var tnode = (DSModelElementsSelection)node;
                        var spar = (SelectElementParameter)par;

                        tnode.UpdateSelection(ListToElements(spar.SelectedElements));
                    }
                    else if (node.GetType() == typeof(DSModelElementSelection) &&
                             par.GetType() == typeof(SelectElementParameter))
                    {
                        var tnode = (DSModelElementSelection)node;
                        var spar = (SelectElementParameter)par;
                        tnode.UpdateSelection(ListToElements(spar.SelectedElements));
                    }
                    else if (node is ReferenceSelection &&
                             par.GetType() == typeof(SelectReferenceParameter))
                    {
                        var tnode = (ReferenceSelection)node;
                        var spar = (SelectReferenceParameter)par;
                        tnode.UpdateSelection(ListToReferences(spar.SelectedReferences));
                    }
                    else if (node is FileSystemBrowser &&
                             par.GetType() == typeof(PathParameter))
                    {
                        var tnode = (FileSystemBrowser)node;
                        var spar = (PathParameter)par;
                        tnode.Value = spar.Value.ToString();
                    }
                }
            }

            if (_selElNode != null && SelectedElements.Count > 0)
                if (!_runnedWorkspacePreset.UseAsSequence)
                    _selElNode.UpdateSelection(SelectedElements);
                else
                {
                    _selElNode.UpdateSelection(new List<Element> { SelectedElements.First() });
                    _selElNode.MarkNodeAsModified(true);
                }

            RevitDynamoModelInstance.EvaluationCompleted += RevitDynamoModel_EvaluationCompleted;
            RevitDynamoModelInstance.ForceRun();

            _runnedWorkspacePreset.Status = WorkspaceBase.WorkspaceStatus.Working;
        }

        private static string GetNodeName1(NodeModel node) => node.CreationName;
        private static string GetNodeName2(NodeModel node) => node.Name;
        
    }
}