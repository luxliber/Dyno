using System.ComponentModel;
using Prorubim.DynoStudio.History;
using Prorubim.DynoStudio.ViewModels;

namespace Dyno.Forms.History
{
    public class HistoryPropChanging :IHistoryAction
    {
        private readonly object _element;
        private readonly PropertyDescriptor _prop;
        private readonly object _oldValue;
        private readonly object _newValue;

        public HistoryPropChanging(object element, PropertyDescriptor prop, object oldValue, object newValue)
        {
            _element = element;
            _prop = prop;
            _oldValue = oldValue;
            _newValue = newValue;
        }

        public void RedoAction()
        {
            _prop.SetValue(_element, _newValue);
        }

        public void UndoAction()
        {
            if(_oldValue==null)
                return;
            _prop.SetValue(_element,_oldValue);
        }
    }
}