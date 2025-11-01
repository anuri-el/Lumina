namespace Lumina.Core.Patterns
{
    public interface IEditorState
    {
        void Enter();
        void HandleMouseDown(double x, double y);
        void HandleMouseMove(double x, double y);
        void HandleMouseUp(double x, double y);
        void Exit();
        void SetContext(EditorContext context);
    }
}
