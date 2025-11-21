namespace Lumina.Core.Patterns
{
    public class EditorContext
    {
        private IEditorState _currentState;

        public event Action<string>? OnLog;

        public EditorContext(IEditorState initialState)
        {
            SetState(initialState);
        }

        public void SetState(IEditorState newState)
        {
            _currentState?.Exit();
            _currentState = newState;
            _currentState.SetContext(this);
            _currentState.Enter();
            Log($"Switched to {_currentState.GetType().Name}");
        }

        public void OnMouseDown(double x, double y) => _currentState.HandleMouseDown(x, y);
        public void OnMouseMove(double x, double y) => _currentState.HandleMouseMove(x, y);
        public void OnMouseUp(double x, double y) => _currentState.HandleMouseUp(x, y);

        public void Log(string message)
        {
            OnLog?.Invoke($"[{DateTime.Now:T}] {message}");
        }
    }
}
