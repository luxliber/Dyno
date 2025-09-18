using Dyno;
using Dyno.FormControls;
using Dyno.Models.Forms;
using Dyno.Views.FormControls;
using Prorubim.DynoStudio.ViewModels;

namespace Prorubim.DynoStudio.History
{
    public class HistoryElementCreating : IHistoryAction
    {
        private readonly FormControl _element;
        private readonly FormTab _tab;
        
        public HistoryElementCreating(FormControl element, FormTab tab)
        {
            _element = element;
            _tab = tab;
        }

        public void RedoAction()
        {
            if (!_tab.Items.Contains(_element))
                _tab.Items.Add(_element);
        }

        public void UndoAction()
        {
            if (_tab.Items.Contains(_element))
                _tab.Items.Remove(_element);
        }
    }
}