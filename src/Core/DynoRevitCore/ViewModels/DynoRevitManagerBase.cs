using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Dynamo.Applications;
using Dynamo.Applications.Models;
using Dynamo.Nodes;
using Dyno.Models;
using Dyno.Models.Parameters;
using Dyno.Models.Workspaces;
using Dyno.ViewModels;
using RevitServices.Persistence;
using DateTime = System.DateTime;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using View = System.Windows.Forms.View;
using WorkspaceFormWindow = Dyno.Views.WorkspaceFormWindow;

namespace Prorubim.DynoRevitCore.ViewModels
{
    public partial class DynoRevitManagerBase : DynoManagerBase
    {
        public static RevitDynamoModel RevitDynamoModelInstance
        {
            get => DynamoRevit.RevitDynamoModel;
            set => DynamoRevit.RevitDynamoModel = value;
        }

        public static ObservableCollection<WorkspaceGroupPackage> Packages { get; set; } =
            new ObservableCollection<WorkspaceGroupPackage>();


        public static ExternalCommandData CommandData { get; set; }

        internal static bool IsInitCoreModelNeeded = true;
        private static WorkspacePreset _runnedWorkspacePreset;
        private static WorkspaceFormWindow _openedForm;

        private static readonly List<Element> SelectedElements = new List<Element>();
        private static ElementSelection<Element> _selElNode;

        protected static ExternalEvent PrepareExternalEvent;
        private static ExternalEvent _runExternalEvent;

        public static bool NeedEvaluation;
        private static DateTime _startingTime;
        public static bool IsSilentMode;

        public static void Evaluate(WorkspacePreset wt)
        {
            if (!DynamoProductsManager.CheckActiveDynamoProduct()) return;

            SelectedWorkspacePreset = wt;
            PrepareExternalEvent.Raise();
        }

        public static void Evaluate(WorkspacePlaylist wpl)
        {
            if (!DynamoProductsManager.CheckActiveDynamoProduct()) return;

            SelectedWorkspacePreset = null;
            SelectedWorkspaceList = wpl;
            SelectedWorkspaceListIndex = 0;
            PrepareExternalEvent.Raise();
        }

        private static IEnumerable<Element> ListToElements(IEnumerable<object> selectedElements) =>
            selectedElements.Select(element => element as Element).ToList();

        private static IEnumerable<Reference> ListToReferences(IEnumerable<object> selectedObjects) =>
            selectedObjects.Select(element => element as Reference).ToList();

        public virtual void InitExternalEvents()
        {
            var handler = new PrepareExternalIvent();
            PrepareExternalEvent = ExternalEvent.Create(handler);

            var runExternalIvent = new RunExternalIvent();
            _runExternalEvent = ExternalEvent.Create(runExternalIvent);
        }

        public static void ExecuteWorkspaceFromCommand(string id, string packageName)
        {
            var package = GetPackageByName(packageName);
            var wp = package?.Root.GetWorkspacePresetByInnerName(id);

            if (wp == null) return;
            Evaluate(wp);
        }

        public static WorkspaceGroupPackage GetPackageByName(string packageName)
        {
            foreach (var package in Packages)
                if (package.Name == packageName)
                    return package;

            return null;
        }

        public override object SelectObject()
        {
            var reference = DynoAppBase.UiApp.ActiveUIDocument.Selection.PickObject(ObjectType.Element);
            return reference != null ? DynoAppBase.UiApp.ActiveUIDocument.Document.GetElement(reference) : null;
        }

        public override List<object> SelectObjectsByRectangle()
        {
            var elements = DynoAppBase.UiApp.ActiveUIDocument.Selection.PickElementsByRectangle();
            return elements?.Cast<object>().ToList();
        }

        public override List<object> SelectObjectsInOrder()
        {
            var elementIds = new List<ElementId>();
            var flag = true;

            while (flag)
            {
                try
                {
                    Reference reference = DynoAppBase.UiApp.ActiveUIDocument.Selection.PickObject(ObjectType.Element,
                        "Pick elements in the desired order and hit ESC to stop picking.");
                    elementIds.Add(reference.ElementId);
                }
                catch
                {
                    flag = false;
                }
            }

            return elementIds.Select(x => DynoAppBase.UiApp.ActiveUIDocument.Document.GetElement(x)).Cast<object>()
                .ToList();
        }

        public override object SelectFace()
        {
            var reference = DynoAppBase.UiApp.ActiveUIDocument.Selection.PickObject(ObjectType.Face);
            return reference;
        }

        public override List<object> SelectFaces()
        {
            var references = DynoAppBase.UiApp.ActiveUIDocument.Selection.PickObjects(ObjectType.Face);
            return references?.Cast<object>().ToList();
        }

        public override object SelectEdge()
        {
            var reference = DynoAppBase.UiApp.ActiveUIDocument.Selection.PickObject(ObjectType.Edge);
            return reference;
        }

        public override List<object> SelectEdges()
        {
            var references = DynoAppBase.UiApp.ActiveUIDocument.Selection.PickObjects(ObjectType.Edge);
            return references?.Cast<object>().ToList();
        }

        public new object SelectPointOnFace()
        {
            var reference = DynoAppBase.UiApp.ActiveUIDocument.Selection.PickObject(ObjectType.PointOnElement);
            return reference;
        }

        public override List<object> SelectObjects()
        {
            var references = DynoAppBase.UiApp.ActiveUIDocument.Selection.PickObjects(ObjectType.Element);
            var els = new List<object>();

            if (references == null) return els;

            foreach (var r in references)
                els.Add(DynoAppBase.UiApp.ActiveUIDocument.Document.GetElement(r));

            return els;
        }


        public override string GetElementId(object element)
        {
            if (element is Element el) return el.Id.ToString();
            if (element is Reference rf) return rf.ElementId.ToString();

            return "none";
        }

        public override void SelectElements(SelectElementParameter par)
        {
            try
            {
                if (par.Mode == "one")
                {
                    var obj = SelectObject();
                    if (obj != null)
                    {
                        par.SelectedElements.Clear();
                        par.SelectedElements.Add(obj);
                    }
                }
                else if (par.Mode == "many")
                {
                    var objList = SelectObjects();

                    if (objList != null)
                    {
                        par.SelectedElements.Clear();
                        par.SelectedElements = objList;
                    }
                }
                else if (par.Mode == "order")
                {
                    var objList = SelectObjectsInOrder();

                    if (objList != null)
                    {
                        par.SelectedElements.Clear();
                        par.SelectedElements = objList;
                    }
                }
                else if (par.Mode == "rectangle")
                {
                    var objList = SelectObjectsByRectangle();
                    par.SelectedElements = objList;
                }

                par.UpdateValue();
                par.OnPropertyChanged("Value");
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public override void SelectReference(SelectReferenceParameter par)
        {
            try
            {
                if (par.Type == "face")
                {
                    var obj = SelectFace();
                    if (obj != null)
                    {
                        par.SelectedReferences.Clear();
                        par.SelectedReferences.Add(obj);
                    }
                }

                if (par.Type == "faces")
                {
                    var objList = SelectFaces();
                    if (objList != null)
                    {
                        par.SelectedReferences.Clear();
                        par.SelectedReferences = objList;
                    }
                }
                else if (par.Type == "edge")
                {
                    var obj = SelectEdge();
                    if (obj != null)
                    {
                        par.SelectedReferences.Clear();
                        par.SelectedReferences.Add(obj);
                    }
                }
                else if (par.Type == "edges")
                {
                    var objList = SelectEdges();
                    if (objList != null)
                    {
                        par.SelectedReferences.Clear();
                        par.SelectedReferences = objList;
                    }
                }
                else if (par.Type == "pointOnFace")
                {
                    var obj = SelectPointOnFace();
                    if (obj != null)
                    {
                        par.SelectedReferences.Clear();
                        par.SelectedReferences.Add(obj);
                    }
                }

                par.UpdateValue();
                par.OnPropertyChanged("Value");
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public override void SelectFile(PathParameter par)
        {
            if (par.Mode == "file")
            {
                var openFileDialog = new OpenFileDialog();
                var parDir = Path.GetDirectoryName(par.Value.ToString());

                if (Path.IsPathRooted(par.Value.ToString()))
                    openFileDialog.InitialDirectory = parDir;
                else
                {
                    var scriptDir = Path.GetDirectoryName(par.Workspace.Workspace.WorkspacePath);
                    if (scriptDir != null && parDir != null)
                        openFileDialog.InitialDirectory = PathParameter.GetAbsolutePath(parDir, scriptDir);
                }

                openFileDialog.Filter = par.Filter;
                openFileDialog.FilterIndex = par.Filterindex;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() != false)
                {
                    par.Value = openFileDialog.FileName;
                    par.OnPropertyChanged("Value");
                }
            }
            else if (par.Mode == "directory")
            {
                var folderBrowserDialog =
                    new FolderBrowserDialog {SelectedPath = Path.GetDirectoryName(par.Value.ToString())};

                if (folderBrowserDialog.ShowDialog() != DialogResult.OK) return;

                par.Value = folderBrowserDialog.SelectedPath;
                par.OnPropertyChanged("Value");
            }
        }

        public override void OnDocumentChanged()
        {
            if (DynamoProductsManager.SelectedProduct != null)
            {
                DocumentManager.Instance.CurrentUIDocument = new UIDocument(DynoAppBase.Doc);
                RevitDynamoModelInstance?.SetRunEnabledBasedOnContext(DynoAppBase.Doc.ActiveView,true);
            }
        }
    }
}