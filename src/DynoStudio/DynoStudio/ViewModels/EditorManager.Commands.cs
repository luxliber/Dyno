using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using Dyno.FormControls;
using Dyno.Models.Forms;
using Dyno.ViewModels;
using Dyno.Views.FormControls;
using Prorubim.DynoStudio.History;
using Prorubim.DynoStudio.Utils;

namespace Prorubim.DynoStudio.ViewModels
{
    public partial class EditorManager
    {
        public RelayCommand TranslateXCommand => new RelayCommand(o =>
        {
            if (double.TryParse(o.ToString(), out var offset))
                TranslateSelectedElements(offset, 0);

            OnPropertyChanged(nameof(PropertyGridItem));
        });

        public RelayCommand TranslateYCommand => new RelayCommand(o =>
        {

            if (double.TryParse(o.ToString(), out var offset))
                TranslateSelectedElements(0, offset);

            OnPropertyChanged(nameof(PropertyGridItem));
        });

        public RelayCommand DeleteCommand => new RelayCommand(o => DeleteControl());
        public RelayCommand CopyCommand => new RelayCommand(o => CopyControl());
        public RelayCommand CutCommand => new RelayCommand(o => CutControl());
        public RelayCommand PasteCommand => new RelayCommand(o => PasteControl());

        public RelayCommand FrontCommand => new RelayCommand(o => BringToFrontControl());
        public RelayCommand BackCommand => new RelayCommand(o => SendToBackControl());

        public RelayCommand DropCommand => new RelayCommand(o =>
        {
            UnselectAll();

            var pars = o as IList;
            var e = pars[1] as DragEventArgs;
            var sender = pars[0];

            var droppedControlName = e.Data.GetData(typeof(Button)) as Button;

            var formTabControl = sender as Grid;
            if (formTabControl == null) return;

            var formTab = formTabControl.Tag as FormTab;

            var pos = e.GetPosition(formTabControl);

            pos.X = Math.Floor(pos.X / 10) * 10;
            pos.Y = Math.Floor(pos.Y / 10) * 10;


            FormControl c = null;

            switch (droppedControlName.Name)
            {
                case "tSelect":
                    c = new FormSelectButton();
                    break;
                case "tLabel":
                    c = new FormLabel();
                    break;
                case "tTextBox":
                    c = new FormTextBox();
                    break;
                case "tNumberBox":
                    c = new FormNumberBox();
                    break;
                case "tCheckBox":
                    c = new FormCheckBox();
                    break;
                case "tRadioButton":
                    c = new FormRadioButton();
                    break;
                case "tListBox":
                    c = new FormListBox();
                    break;
                case "tComboBox":
                    c = new FormComboBox();
                    break;
                case "tImage":
                    c = new FormImage();
                    break;
            }

            if (c == null) return;

            c.Form = DynoManagerBase.SelectedWorkspacePreset.Workspace.WorkspaceForm;

            formTab.Items.Add(c);

            c.SetPosition(pos.X, pos.Y);
            c.UpdatePosition();
            c.IsActivated = true;
            
            FormControlHelper.UpdateControlValuesFromBindings(c);
            FormControlHelper.UpdateControlValuesFromExpressions(c);

            DynoManager.SelectedHistory.AddAction(new HistoryElementCreating(c, formTab));
            
            Select(c);
            PropertyGridItem = c;
        });

        public RelayCommand CloseTabCommand => new RelayCommand(o =>
        {
            var formTab = o as FormTab;

            if (formTab?.WorkspaceForm != null)
            {
                UnselectAll();
                formTab.WorkspaceForm.DeleteTab(formTab);
            }
        });

        public RelayCommand AddTabCommand => new RelayCommand(o =>
        {
            UnselectAll();
            WorkspaceForm.AddTab();
        });

        public RelayCommand CopyTabCommand => new RelayCommand(o =>
        {
            UnselectAll();
            WorkspaceForm.CopyTab();
        });
    }
}
