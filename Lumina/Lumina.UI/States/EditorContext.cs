namespace Lumina.UI.States
{
    public class EditorContext
    {
        private IEditorState _state;
        public IEditorState State => _state;

        public EditorContext()
        {
            _state = new NormalState();
        }

        public void SetState(IEditorState newState)
        {
            _state = newState;
        }

        public string PerformAction()
        {
            return _state.OnAction();
        }
    }
}
