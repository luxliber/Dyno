

namespace Prorubim.DynoNodesUI
{/*
    class ShowFormNodeViewCustomization : VariableInputNodeViewCustomization, INodeViewCustomization<ShowForm>
    {
        private ShowForm _model;
        private NodeView _nodeView;

        public void CustomizeView(ShowForm model, NodeView nodeView)
        {
            base.CustomizeView(model, nodeView);

            var l = new ShowFormControl { DataContext = model };
            

            _nodeView = nodeView;
            nodeView.inputGrid.Children.Add(l);
            _model = model;

            nodeView.ViewModel.DynamoViewModel.Model.EvaluationCompleted += delegate
            {
                
                //_model.MarkNodeAsModified();
            };
            
            //DynoScriptHelper.RegisterModelHandlers(nodeView.ViewModel.DynamoViewModel.Model);

            nodeView.UpdateLayout();
        }

        public void Dispose()
        {
            
        }
    }*/
}
