using Lumina.Core.Memento;

namespace Lumina.Core.EditorLogic
{
    public class EditorHistory
    {
        private readonly Stack<EditorMemento> _undoStack = new();
        private readonly Stack<EditorMemento> _redoStack = new();

        public void Push(EditorMemento state)
        {
            _undoStack.Push(state);
            _redoStack.Clear();
        }

        public EditorMemento? Undo()
        {
            if (_undoStack.Count <= 1)
                return null;

            var current = _undoStack.Pop();
            _redoStack.Push(current);

            return _undoStack.Peek();
        }

        public EditorMemento? Redo()
        {
            if (_redoStack.Count == 0)
                return null;

            var state = _redoStack.Pop();
            _undoStack.Push(state);
            return state;
        }
    }
}
