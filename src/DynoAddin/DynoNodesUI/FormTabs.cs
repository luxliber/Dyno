using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Autodesk.DesignScript.Runtime;
using CoreNodeModels;
using Dynamo.Graph;
using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;


namespace Prorubim.DynoNodesUI
{
    [IsDesignScriptCompatible]
    [SupressImportIntoVM]
    [NodeName("Form Tabs")]
    [NodeCategory("Prorubim.Dyno.Forms.Actions")]
    [NodeDescription("Set global variable by name and value")]
    [OutPortNames("tabName")]
    [OutPortTypes("String")]
    public class FormTabs : DSDropDownBase
    {
        public FormTabs()
        {
            RegisterAllPorts();
            Items.Add(new DynamoDropDownItem("None", "None"));
            
        }

        internal void UpdateTabs()
        {
    /*        Items.Clear();
            Items.Add(new DynamoDropDownItem("None", "None"));
            
            if (DynoScriptHelper.Form == null)
                return;

            Items.Clear();

            foreach (var tab in DynoScriptHelper.Form.TabItems)
                Items.Add(new DynamoDropDownItem(tab.Header, tab.Header));

            SelectedIndex = Items.Select(x => x.Name).ToList().IndexOf(SelectedTab);

            if (SelectedIndex == -1)
                SelectedIndex = 0;

            RaisePropertyChanged(nameof(Items));
            RaisePropertyChanged(nameof(SelectedIndex));
            OnNodeModified(true);*/
            
        }

        public override IEnumerable<AssociativeNode> BuildOutputAst(List<AssociativeNode> inputAstNodes)
        {
      /*      if (DynoScriptHelper.Workspace == null)
                Error("Preset is not found");

            if (DynoScriptHelper.Form == null)
                Error("Preset form is not found");*/

            return new[]
            {
                AstFactory.BuildAssignment(
                GetAstIdentifierForOutputIndex(0), AstFactory.BuildStringNode(Items[SelectedIndex].Name))
        };
        }

        protected override SelectionState PopulateItemsCore(string currentSelection)
        {
            UpdateTabs();
            return SelectionState.Done;
            //         State = ElementState.Active;
            //       return SelectionState.Done;
        }

        protected override void SerializeCore(XmlElement element, SaveContext context)
        {
            base.SerializeCore(element, context);

            if (SelectedIndex < 0 || Items.Count==0 ) return;

            SelectedTab = Items[SelectedIndex].Name;

            var xmlDocument = element.OwnerDocument;
            if (xmlDocument == null) return;

            var subNode = xmlDocument.CreateElement(nameof(SelectedTab));
            subNode.InnerText = SelectedTab;
            element.AppendChild(subNode);
        }

        public string SelectedTab { get; set; }
        internal string ScriptPath { get; set; }

        protected override void DeserializeCore(XmlElement nodeElement, SaveContext context)
        {
            base.DeserializeCore(nodeElement, context);
            SelectedIndex = 0;
            State = ElementState.Active;

            foreach (var subNode in nodeElement.ChildNodes.Cast<XmlNode>())
                switch (subNode.Name)
                {
                    case nameof(SelectedTab):
                        SelectedTab = subNode.InnerText;
                        break;
                }
        }
    }
}
