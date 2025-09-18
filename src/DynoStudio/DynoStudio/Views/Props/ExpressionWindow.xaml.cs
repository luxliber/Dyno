using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Dyno.ViewModels;

namespace Prorubim.DynoStudio.Props
{
    /// <summary>
    /// Логика взаимодействия для ExpressionWindow.xaml
    /// </summary>
    internal partial class ExpressionWindow
    {
        private readonly CalcEngine.CalcEngine _ce;

        public ExpressionWindow()
        {
            InitializeComponent();

            _ce = new CalcEngine.CalcEngine();

            var logical = _ce.Functions.Where(x => x.Value.Category == "Logical");
            var text = _ce.Functions.Where(x => x.Value.Category == "Text");
            var math = _ce.Functions.Where(x => x.Value.Category == "Math");
            var stat = _ce.Functions.Where(x => x.Value.Category == "Statistical");

            foreach (var p in DynoManagerBase.SelectedWorkspacePreset.Parameters)
                Nodes.Items.Add(p.Name);


            foreach (var p in DynoManagerBase.SelectedWorkspacePreset.Workspace.WorkspaceForm.UserPars)
                Nodes.Items.Add(p.Name);


            foreach (var f in logical)
                LogicalF.Items.Add(f.Key);

            foreach (var f in text)
                TextF.Items.Add(f.Key);

            foreach (var f in math)
                MathF.Items.Add(f.Key);

            foreach (var f in stat)
                StatF.Items.Add(f.Key);

            var itemContainerStyle = new Style(typeof(ListBoxItem));
            itemContainerStyle.Setters.Add(new Setter(HorizontalAlignmentProperty, HorizontalAlignment.Center));
            itemContainerStyle.Setters.Add(new EventSetter(PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(Nodes_SelectionChanged)));
            Nodes.ItemContainerStyle = itemContainerStyle;
            LogicalF.ItemContainerStyle = itemContainerStyle;
            TextF.ItemContainerStyle = itemContainerStyle;
            MathF.ItemContainerStyle = itemContainerStyle;
            StatF.ItemContainerStyle = itemContainerStyle;
        }

        private void Nodes_SelectionChanged(object sender, MouseButtonEventArgs e)
        {
            if (!(sender is ListBoxItem listBoxItem))
                return;

            if (
                DynoManagerBase.SelectedWorkspacePreset.Parameters.FirstOrDefault(x => x.Name == listBoxItem.Content.ToString()) != null ||
                DynoManagerBase.SelectedWorkspacePreset.Workspace.WorkspaceForm.UserPars.FirstOrDefault(x => x.Name == listBoxItem.Content.ToString()) != null
                )
            {
                DeleteSelectedText();

                var selectionIndex = Text.SelectionStart;
                Text.Text = Text.Text.Insert(selectionIndex, listBoxItem.Content.ToString());
                Text.SelectionStart = selectionIndex + listBoxItem.Content.ToString().Length;
            }
            else
            {
                DeleteSelectedText();

                var f = _ce.Functions.FirstOrDefault(x => x.Key == listBoxItem.Content.ToString());
                var selectionIndex = Text.SelectionStart;

                var parCount = f.Value.ParmMax > 3 ? 3 : f.Value.ParmMax;

                var insText = f.Key.ToLower();

                if (parCount > 0)
                    insText += parCount > 0 ? "( " : "";

                for (var i = 0; i < parCount - 1; i++)
                    insText += " , ";

                if (parCount > 0)
                    insText += " )";

                Text.Text = Text.Text.Insert(selectionIndex, insText);
                Text.SelectionStart = selectionIndex + $"{f.Key}( ".Length;

            }

            Text.Focus();
            e.Handled = true;
        }

        private void DeleteSelectedText()
        {
            if (Text.SelectionLength > 0)
            {
                var start = Text.SelectionStart;
                var selectText = Text.Text.Substring(Text.SelectionStart, Text.SelectionLength);
                Text.Text = Text.Text.Replace(selectText, "");
                Text.SelectionStart = start;
            }
        }

        private void ButtonOk_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void ButtonCancel_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        protected override void OnActivated(EventArgs e)
        {
            Text.Focus();
           
            Text.SelectAll();
            base.OnActivated(e);
        }

        private void ButtonClear_OnClick(object sender, RoutedEventArgs e)
        {
            Text.Text = "";
        }
    }
}
