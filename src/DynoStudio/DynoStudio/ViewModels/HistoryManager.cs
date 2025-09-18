using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Dyno.Forms.Annotations;
using Dyno.ViewModels;

namespace Prorubim.DynoStudio.ViewModels
{
    public class HistoryManager : INotifyPropertyChanged
    {
        private readonly FixedSizedQueue<IHistoryAction> _undoStack = new FixedSizedQueue<IHistoryAction>();
        private readonly Stack<IHistoryAction> _redoStack = new Stack<IHistoryAction>();

        public bool IsUndo => _undoStack.Count > 0;
        public bool IsRedo => _redoStack.Count > 0;

        public void AddAction(IHistoryAction action)
        {
            _undoStack.Push(action);

            if (_redoStack.Count>0)
                _redoStack.Clear();

            DynoManagerBase.SelectedWorkspacePreset.IsChanged = true;
            OnPropertyChanged(nameof(IsUndo));
            OnPropertyChanged(nameof(IsRedo));
        }

        public void Undo()
        {
            var action = _undoStack.Pop();

            if(action!=null)
            {
                _redoStack.Push(action);
                action.UndoAction();
            }

            DynoManagerBase.SelectedWorkspacePreset.IsChanged = true;

            OnPropertyChanged(nameof(IsUndo));
            OnPropertyChanged(nameof(IsRedo));
        }

        public void Redo()
        {
            if (_redoStack.Count <= 0) return;

            var action = _redoStack.Pop();
            action.RedoAction();
            _undoStack.Push(action);

            DynoManagerBase.SelectedWorkspacePreset.IsChanged = true;

            OnPropertyChanged(nameof(IsUndo));
            OnPropertyChanged(nameof(IsRedo));
        }

       

       

        public void Clear()
        {
            _undoStack.Clear();
            _redoStack.Clear();

            OnPropertyChanged(nameof(IsUndo));
            OnPropertyChanged(nameof(IsRedo));

        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected internal virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public interface IHistoryAction
    {
        void RedoAction();
        void UndoAction();
    }

    public class FixedSizedQueue<T>
    {
        readonly List<T> _q = new List<T>();

        public int Limit { get; set; } = 20;
        public int Count => _q.Count;

        public void Push(T obj)
        {
            lock (this)
            {
                _q.Add(obj);
                while (_q.Count > Limit)
                    _q.RemoveAt(0);
            }
        }

        public T Pop()
        {
            lock (this)
            {
                var item = _q.LastOrDefault();
                if(item!=null)
                    _q.RemoveAt(_q.Count-1);

                return item;
            }
        }

        public void Clear()
        {
            _q.Clear();
        }
    }
}