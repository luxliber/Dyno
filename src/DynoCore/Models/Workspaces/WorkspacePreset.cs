using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Media;
using System.Xml;
using Dyno.Models.Parameters;
using Dyno.ViewModels;
using LitJson;

namespace Dyno.Models.Workspaces
{
    public class WorkspacePreset : WorkspaceBase
    {
        public string Tag => "preset";

        public string UseSelected;
        public bool UseAsSequence;
        public bool ForceReopen;

        public new string DisplayName => Name == Workspace.Name ? "" : Name;
        public new string ButtonName => WrapText(InnerName, 6);
        public new string InnerName => Name == "" ? Workspace.Name : Name;

        public ObservableCollection<WorkspaceParameter> Parameters { get; set; }
        public ObservableCollection<WorkspaceParameter> Childs => Parameters;

        private bool _isChanged;

        public bool IsChanged
        {
            get => _isChanged;
            set
            {
                _isChanged = value;
                Workspace.OnPropertyChanged(nameof(Workspace.IsChanged));
            }
        }

        public bool IsForm => Workspace.WorkspaceForm != null && ShownTabItems;

        public bool ShownTabItems => Workspace.WorkspaceForm.TabItems.Any(x => FormTabs.Contains(x.Header));

        private WorkspaceStatus _status = WorkspaceStatus.Nothing;


        public WorkspaceStatus Status
        {
            set
            {
                _status = value;
                OnPropertyChanged(nameof(StatusColor));
                Workspace.OnPropertyChanged(nameof(Workspace.HeaderBrush));
            }
            get => _status;
        }

        public Brush StatusColor
        {
            get
            {
                switch (Status)
                {
                    case WorkspaceStatus.Ok:
                        return new SolidColorBrush(Color.FromArgb(150, 150, 255, 150));
                    case WorkspaceStatus.Working:
                        return new SolidColorBrush(Color.FromArgb(150, 150, 250, 255));
                    case WorkspaceStatus.Error:
                        return new SolidColorBrush(Color.FromArgb(150, 255, 200, 100));
                }

                return new SolidColorBrush(Colors.Transparent);
            }
        }

        public Workspace Workspace { get; set; }

        public List<string> FormTabs = new List<string>();

        public WorkspacePreset()
        {
            Parameters = new ObservableCollection<WorkspaceParameter>();
        }

        public void AddParameter(WorkspaceParameter par)
        {
            var exPar = GetParameterByName(par.Name);
            if (exPar != null)
                Parameters.Remove(exPar);

            Parameters.Add(par);
        }

        public WorkspaceParameter GetParameterByName(string name) => Parameters.FirstOrDefault(par => par.Name == name);

        public WorkspaceParameter GetParameterByNameWithoutGuid(string name) =>
            Parameters.FirstOrDefault(par => par.Name == name && String.IsNullOrEmpty(par.Guid));

        public WorkspaceParameter GetParameterByGuid(Guid guid) =>
            Parameters.FirstOrDefault(par => par.Guid == guid.ToString());

        public void ScanParameters(JsonData data)
        {
            foreach (KeyValuePair<string, JsonData> par in data)
                if (par.Value.IsObject)
                    ScanExtendedParameter(par.Key, par.Value);
                else
                    ScanSimpleParameter(par.Key, par.Value);
        }

        private void ScanSimpleParameter(string key, JsonData value)
        {
            var lowerKey = key.ToLower();

            if (lowerKey == "desc")
                Desc = (string)value;
            else if (lowerKey == "useselected")
                UseSelected = (string)value;
            else if (lowerKey == "formtabs")
            {
                FormTabs.Clear();

                if (value.IsString && !String.IsNullOrEmpty((string)value))
                    FormTabs.Add((string)value);
                else if (value.IsArray)
                    foreach (JsonData v in value)
                        if (v.IsString)
                            FormTabs.Add(v.ToString());
            }
            else if (lowerKey == "forcereopen")
                ForceReopen = (bool)value;
            else if (lowerKey == "useassequence")
                UseAsSequence = (bool)value;
            else if (value.IsDouble)
                AddParameter(
                    new StringParameter(key,
                            ((double)value).ToString(CultureInfo.InvariantCulture))
                    { Workspace = this });
            else if (value.IsBoolean)
                AddParameter(new BooleanParameter(key, (bool)value) { Workspace = this });
            else if (value.IsInt)
                AddParameter(new StringParameter(key, value.ToString()) { Workspace = this });
            else if (value.IsString)
                AddParameter(new StringParameter(key, value.ToString()) { Workspace = this });
            else if (value.IsArray)
                AddParameter(new StringParameter(key, value) { Workspace = this });
        }

        private void ScanExtendedParameter(string key, JsonData data)
        {
            if (data.Keys.Contains("type"))
            {
                if (data["type"].ToString() == "element")
                    AddParameter(new SelectElementParameter(key, data) { Workspace = this });
                if (data["type"].ToString() == "face")
                    AddParameter(new SelectReferenceParameter(key, data) { Workspace = this });
                if (data["type"].ToString() == "faces")
                    AddParameter(new SelectReferenceParameter(key, data) { Workspace = this });
                if (data["type"].ToString() == "edges")
                    AddParameter(new SelectReferenceParameter(key, data) { Workspace = this });
                if (data["type"].ToString() == "edge")
                    AddParameter(new SelectReferenceParameter(key, data) { Workspace = this });
                if (data["type"].ToString() == "pointOnFace")
                    AddParameter(new SelectReferenceParameter(key, data) { Workspace = this });
                if (data["type"].ToString() == "path")
                    AddParameter(new PathParameter(key, data) { Workspace = this });
            }
            else if (data.Keys.Contains("value"))
            {
                if (data["value"].IsBoolean)
                    AddParameter(new BooleanParameter(key, data) { Workspace = this });
                if (data["value"].IsString)
                    AddParameter(new StringParameter(key, data) { Workspace = this });
                if (data["value"].IsDouble || data["value"].IsInt || data["value"].IsLong)
                    AddParameter(new NumberParameter(key, data) { Workspace = this });
            }
        }

        public override string Save()
        {
            var sb = new StringBuilder();
            var writer = new JsonWriter(sb) { PrettyPrint = true };

            writer.WriteObjectStart();
            writer.WritePropertyName(Workspace.Name);

            writer.WriteObjectStart();

            if (UseSelected != null)
            {
                writer.WritePropertyName("UseSelected");
                writer.Write(UseSelected);
            }

            if (UseAsSequence)
            {
                writer.WritePropertyName("UseAsSequence");
                writer.Write(true);
            }

            foreach (var par in Parameters)
            {
                writer.WritePropertyName(par.Name);
                par.writeJson(writer);
            }

            writer.WriteObjectEnd();
            writer.WriteObjectEnd();

            return writer.ToString();
        }

        public void ClearWarnings()
        {
            foreach (var par in Parameters)
                if (par.Format == "warning")
                    Parameters.Remove(par);
        }

        public void ScanWorkspaceFile()
        {
            if (Workspace.WorkspacePath == null) return;
            if (!File.Exists(Workspace.WorkspacePath)) return;

            try
            {
                ScanXmlWorkspaceFile();
                Workspace.DynamoVersion = "1";
            }
            catch (Exception)
            {
                try
                {
                    ScanJsonWorkspaceFile();
                    Workspace.DynamoVersion = "2";
                }
                catch (Exception)
                {
                    MessageBox.Show("Can`t parse workspace file: " + Workspace.WorkspacePath);
                }
            }
        }

        public bool IsShowDynamoFileVersion => Workspace.ManagerCollector.GetSettingsBase() != null &&
                                               Workspace.ManagerCollector.GetSettingsBase().IsShowDynamoFileVersion;



        public void ScanJsonWorkspaceFile()
        {
            var r = new StreamReader(Workspace.WorkspacePath);
            var json = r.ReadToEnd();
            r.Close();

            var jReader = new JsonReader(json) { AllowComments = true };
            var jObject = JsonMapper.ToObject(jReader);
            var jNodes = jObject["Nodes"];
            var jNodeViews = jObject["View"]["NodeViews"];
            var nodes = jNodeViews.Cast<JsonData>().Zip(jNodes.Cast<JsonData>(), (v, n) => new { View = v, Node = n });

            var tuneParsList = new[] { "desc", "formtabs", "useselected", "forcereopen" };

            foreach (var n in nodes)
            {
                var isInput = (bool)n.View["IsSetAsInput"];
                var type = ((string)n.Node["ConcreteType"]).Split(',').First().Trim();
                var nickName = (string)n.View["Name"];


                if (GetParameterByName(nickName) != null) continue;

                if (tuneParsList.Contains(nickName.ToLower()) || (isInput && type != null && nickName != null))
                {
                    if (n.Node.Keys.Contains("SelectedIndex"))
                        AddParameter(new DropDownParameter(nickName, (int)n.Node["SelectedIndex"])
                        {
                            Workspace = this,
                            Guid = Guid.Parse((string)n.Node["Id"]).ToString()
                        });

                    if (type == "CoreNodeModels.Input.StringInput")
                    {
                        if (nickName.ToLower() == "desc")
                            Desc = (string)n.Node["InputValue"];
                        else if (nickName.ToLower() == "formtabs")
                        {
                            var value = (string)n.Node["InputValue"];
                            if (!string.IsNullOrEmpty(value) && !FormTabs.Contains(value))
                                FormTabs.Add(value);
                        }
                        else if (nickName.ToLower() == "useselected")
                            UseSelected = (string)n.Node["InputValue"];
                        else if (nickName.ToLower() == "desc")
                            Desc = (string)n.Node["InputValue"];
                        else if (nickName.ToLower() == "formtabs")
                        {
                            var value = (string)n.Node["InputValue"];
                            if (!String.IsNullOrEmpty(value) && !FormTabs.Contains(value))
                                FormTabs.Add(value);
                        }
                        else
                            AddParameter(new StringParameter(nickName, (string)n.Node["InputValue"])
                            {
                                Workspace = this,
                                Guid = Guid.Parse((string)n.Node["Id"]).ToString()
                            });
                    }

                    else if (type == "CoreNodeModels.Input.BoolSelector")
                        if (nickName.ToLower() == "forcereopen")
                            ForceReopen = (bool)n.Node["InputValue"];
                        else
                            AddParameter(
                                new BooleanParameter(nickName, (bool)n.Node["InputValue"])
                                {
                                    Workspace = this,
                                    Guid = Guid.Parse((string)n.Node["Id"]).ToString()
                                });

                    else if (type == "CoreNodeModels.Input.DoubleInput")
                        AddParameter(new NumberParameter(nickName, (double)n.Node["InputValue"])
                        { Workspace = this, Guid = Guid.Parse((string)n.Node["Id"]).ToString() });

                    else if (type == "CoreNodeModels.Input.DoubleSlider")
                        AddParameter(new NumberParameter(nickName, (double)n.Node["InputValue"])
                        { Workspace = this, Guid = Guid.Parse((string)n.Node["Id"]).ToString() });

                    //Paths
                    else if (type == "CoreNodeModels.Input.Directory")
                        AddParameter(new PathParameter(nickName, "directory", n.Node["InputValue"].ToString())
                        {
                            Workspace = this,
                            Guid = Guid.Parse((string)n.Node["Id"]).ToString()
                        });
                    else if (type == "CoreNodeModels.Input.Filename")
                        AddParameter(new PathParameter(nickName, "file", n.Node["InputValue"].ToString())
                        {
                            Workspace = this,
                            Guid = Guid.Parse((string)n.Node["Id"]).ToString()
                        });

                    //Model Elements
                    else if (type == "Dynamo.Nodes.DSModelElementSelection")
                        AddParameter(new SelectElementParameter(nickName, "one")
                        {
                            Workspace = this,
                            Guid = Guid.Parse((string)n.Node["Id"]).ToString()
                        });
                    else if (type == "Dynamo.Nodes.DSModelElementsSelection")
                        AddParameter(new SelectElementParameter(nickName, "many")
                        {
                            Workspace = this,
                            Guid = Guid.Parse((string)n.Node["Id"]).ToString()
                        });

                    //Faces
                    else if (type == "Dynamo.Nodes.DSFaceSelection")
                        AddParameter(new SelectReferenceParameter(nickName, "face")
                        {
                            Workspace = this,
                            Guid = Guid.Parse((string)n.Node["Id"]).ToString()
                        });
                    else if (type == "Dynamo.Nodes.SelectFaces")
                        AddParameter(new SelectReferenceParameter(nickName, "faces")
                        {
                            Workspace = this,
                            Guid = Guid.Parse((string)n.Node["Id"]).ToString()
                        });

                    //Edges
                    else if (type == "Dynamo.Nodes.DSEdgeSelection")
                        AddParameter(new SelectReferenceParameter(nickName, "edge")
                        {
                            Workspace = this,
                            Guid = Guid.Parse((string)n.Node["Id"]).ToString()
                        });
                    else if (type == "Dynamo.Nodes.SelectEdges")
                        AddParameter(new SelectReferenceParameter(nickName, "edges")
                        {
                            Workspace = this,
                            Guid = Guid.Parse((string)n.Node["Id"]).ToString()
                        });

                    //Point
                    else if (type == "Dynamo.Nodes.DSPointOnElementSelection")
                        AddParameter(new SelectReferenceParameter(nickName, "pointOnFace")
                        {
                            Workspace = this,
                            Guid = Guid.Parse((string)n.Node["Id"]).ToString()
                        });
                }
            }
        }

        public void ScanXmlWorkspaceFile()
        {
            var doc = new XmlDocument();
            doc.Load(Workspace.WorkspacePath);
            var elementsNode = doc.DocumentElement.SelectSingleNode("Elements");
            foreach (XmlNode node in elementsNode.ChildNodes)
            {
                var isInput = node.Attributes["isSelectedInput"] != null
                    ? bool.Parse(node.Attributes["isSelectedInput"].Value)
                    : false;
                var type = node.Attributes["type"]?.Value;
                var nickName = node.Attributes["nickname"]?.Value;

                if (GetParameterByName(nickName) == null)
                    if (isInput && type != null && nickName != null)
                    {
                        if (node.Attributes["index"] != null)
                            AddParameter(new DropDownParameter(nickName, node.Attributes["index"].Value)
                            {
                                Workspace = this,
                                Guid = node.Attributes["guid"].Value
                            });

                        if (type == "CoreNodeModels.Input.StringInput")
                        {
                            if (nickName.ToLower() == "useselected")
                                UseSelected = node.ChildNodes[0].InnerText;
                            else if (nickName.ToLower() == "desc")
                                Desc = node.ChildNodes[0].InnerText;
                            else if (nickName.ToLower() == "formtabs")
                            {
                                var value = node.ChildNodes[0].InnerText;
                                if (!String.IsNullOrEmpty(value) && !FormTabs.Contains(value))
                                    FormTabs.Add(value);
                            }
                            else
                                AddParameter(new StringParameter(nickName, node.ChildNodes[0].InnerText)
                                {
                                    Workspace = this,
                                    Guid = node.Attributes["guid"].Value
                                });
                        }

                        else if (type == "CoreNodeModels.Input.BoolSelector")
                            if (nickName.ToLower() == "forcereopen")
                                ForceReopen = bool.Parse(node.ChildNodes[0].InnerText);
                            else
                                AddParameter(
                                    new BooleanParameter(nickName, bool.Parse(node.ChildNodes[0].InnerText))
                                    {
                                        Workspace = this,
                                        Guid = node.Attributes["guid"].Value
                                    });
                        else if (type == "CoreNodeModels.Input.DoubleInput")
                            AddParameter(new NumberParameter(nickName, double
                                    .Parse(node.ChildNodes[0].Attributes["value"].Value,
                                        CultureInfo.InvariantCulture))
                            { Workspace = this, Guid = node.Attributes["guid"].Value });

                        else if (type == "CoreNodeModels.Input.IntegerSlider")
                            AddParameter(new NumberParameter(nickName, double
                                    .Parse(node.ChildNodes[0].InnerText, CultureInfo.InvariantCulture)
                                )
                            { Workspace = this, Guid = node.Attributes["guid"].Value });

                        else if (type == "CoreNodeModels.Input.DoubleSlider")
                            AddParameter(new NumberParameter(nickName, double
                                    .Parse(node.ChildNodes[0].InnerText, CultureInfo.InvariantCulture)
                                )
                            { Workspace = this, Guid = node.Attributes["guid"].Value });

                        //Paths
                        else if (type == "CoreNodeModels.Input.Directory")
                            AddParameter(
                                new PathParameter(nickName, "directory", node.ChildNodes[0].InnerText)
                                {
                                    Workspace = this,
                                    Guid = node.Attributes["guid"].Value
                                });
                        else if (type == "CoreNodeModels.Input.Filename")
                            AddParameter(new PathParameter(nickName, "file", node.ChildNodes[0].InnerText)
                            {
                                Workspace = this,
                                Guid = node.Attributes["guid"].Value
                            });

                        //Model Elements
                        else if (type == "Dynamo.Nodes.DSModelElementSelection")
                            AddParameter(new SelectElementParameter(nickName, "one")
                            {
                                Workspace = this,
                                Guid = node.Attributes["guid"].Value
                            });
                        else if (type == "Dynamo.Nodes.DSModelElementsSelection")
                            AddParameter(new SelectElementParameter(nickName, "many")
                            {
                                Workspace = this,
                                Guid = node.Attributes["guid"].Value
                            });

                        //Faces
                        else if (type == "Dynamo.Nodes.DSFaceSelection")
                            AddParameter(new SelectReferenceParameter(nickName, "face")
                            {
                                Workspace = this,
                                Guid = node.Attributes["guid"].Value
                            });
                        else if (type == "Dynamo.Nodes.SelectFaces")
                            AddParameter(new SelectReferenceParameter(nickName, "faces")
                            {
                                Workspace = this,
                                Guid = node.Attributes["guid"].Value
                            });

                        //Edges
                        else if (type == "Dynamo.Nodes.DSEdgeSelection")
                            AddParameter(new SelectReferenceParameter(nickName, "edge")
                            {
                                Workspace = this,
                                Guid = node.Attributes["guid"].Value
                            });
                        else if (type == "Dynamo.Nodes.SelectEdges")
                            AddParameter(new SelectReferenceParameter(nickName, "edges")
                            {
                                Workspace = this,
                                Guid = node.Attributes["guid"].Value
                            });

                        //Point
                        else if (type == "Dynamo.Nodes.DSPointOnElementSelection")
                            AddParameter(new SelectReferenceParameter(nickName, "pointOnFace")
                            {
                                Workspace = this,
                                Guid = node.Attributes["guid"].Value
                            });
                    }
            }
        }
    }
}