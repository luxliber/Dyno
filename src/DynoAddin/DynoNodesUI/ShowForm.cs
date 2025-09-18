using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using Dynamo.Events;
using Dynamo.Graph;
using Dynamo.Graph.Nodes;
using Dynamo.Graph.Workspaces;
using Dynamo.Models;
using ProtoCore.AST.AssociativeAST;


namespace Prorubim.DynoNodesUI
{/*
    [IsDesignScriptCompatible]
    [NodeName("ShowForm")]
    [NodeCategory("Prorubim.Dyno.Actions")]
    [NodeDescription("")]
    public class ShowForm : VariableInputNode, INotifyPropertyChanged
    {
        public ObservableCollection<FormBinding> FormBindings { get; set; } = new ObservableCollection<FormBinding>();

        public class FormBinding
        {
            public string Name { get; set; }
        }

        public string ScriptName
        {
            get
            {
                if (DynoScriptHelper.Form == null)
                    return "Workspace hasn`t form";

                return DynoScriptHelper.ScriptName + ".dfm";
            }
        }

        public bool IsItalic => DynoScriptHelper.ScriptName == "" || DynoScriptHelper.Form == null;

        public ShowForm()
        {
            InPorts.Add(new PortModel(PortType.Input, this, new PortData("formTabs", "")));
            OutPorts.Add(new PortModel(PortType.Output, this, new PortData("formTabs", "")));
            RegisterAllPorts();
        }


        public override IEnumerable<AssociativeNode> BuildOutputAst(List<AssociativeNode> inputAstNodes)
        {
            //      if (IsItalic)
            {
                //        Error(ScriptPath);
                return null;
            }

            DynoScriptHelper.IsFirstPortRequest = true;

            var res = new List<AssociativeNode>();
            var astFuncShowForm = AstFactory.BuildFunctionCall(
               new Func<string, IList, int, object>(DynoScriptHelper.ShowFormFunc),
               new List<AssociativeNode> { inputAstNodes[0], AstFactory.BuildExprList(inputAstNodes.Skip(1).ToList()), AstFactory.BuildIntNode(-1) }
               );
            res.Add(AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), astFuncShowForm));


            for (var i = 0; i < DynoScriptHelper.FormBindings.Count; i++)
            {
                var astFuncShowForm2 = AstFactory.BuildFunctionCall(
                new Func<string, IList, int, object>(DynoScriptHelper.ShowFormFunc),
                new List<AssociativeNode> { inputAstNodes[0], AstFactory.BuildExprList(inputAstNodes.Skip(1).ToList()), AstFactory.BuildIntNode(i) }
                );
                res.Add(AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(i + 1), astFuncShowForm2));
            }

            return res;
        }


        /*
       

        protected override void SerializeCore(XmlElement element, SaveContext context)
        {
            base.SerializeCore(element, context);

            var xmlDocument = element.OwnerDocument;
            foreach (var binding in DynoScriptHelper.FormBindings)
            {
                var pNode = xmlDocument.CreateElement("Port");
                pNode.InnerText = binding.Key;
                element.AppendChild(pNode);
            }
        }*

        /*  protected override void DeserializeCore(XmlElement nodeElement, SaveContext context)
          {
              base.DeserializeCore(nodeElement, context);

              foreach (XmlNode subNode in nodeElement.ChildNodes.Cast<XmlNode>()
                  .Where(subNode => subNode.Name.Equals("Port")))
              {
                  var name = subNode.InnerText;
                  InPortData.Add(new PortData(name, ""));
                  OutPortData.Add(new PortData(name, ""));
              }
              RegisterAllPorts();

              if (!string.IsNullOrEmpty(nodeElement.BaseURI))
                  DynoScriptHelper.ScriptPath = new Uri(nodeElement.BaseURI).LocalPath;

              UpdatePorts();
              RaisePropertiesChanged();
          }*

        public void RaisePropertiesChanged()
        {
            RaisePropertyChanged("ScriptPath");
            RaisePropertyChanged("IsItalic");
        }

        protected override string GetInputTooltip(int index)
        {

            return "";
        }

        protected override string GetInputName(int index)
        {
            return $"in{index}";
        }



        protected override void RemoveInput()
        {
            if (InPorts.Count > 1)
            {
                base.RemoveInput();
                FormBindings.RemoveAt(FormBindings.Count - 1);
                OutPorts.RemoveAt(InPorts.Count);
                OnPropertyChanged(nameof(FormBindings));
            }
        }

        protected override void AddInput()
        {
            base.AddInput();
            FormBindings.Add( new FormBinding {Name = "ddd"} );

            OutPorts.Add(new PortModel(PortType.Output, this, new PortData(InPorts.LastOrDefault().PortName, "")));
            OnPropertyChanged(nameof(FormBindings));
        }

        [field: NonSerialized]
        public new event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }*/
}
