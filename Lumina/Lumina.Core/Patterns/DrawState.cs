namespace Lumina.Core.Patterns
{
    public class DrawState : IEditorState
    {
        private EditorContext? _context;

        public void SetContext(EditorContext context) => _context = context;
        public void Enter() => _context?.Log("Entering Draw mode");
        public void Exit() => _context?.Log("Leaving Draw mode");

        public void HandleMouseDown(double x, double y)
            => _context?.Log($"Starting to draw at ({x:0}, {y:0})");

        public void HandleMouseMove(double x, double y)
            => _context?.Log($"Drawing line to ({x:0}, {y:0})");

        public void HandleMouseUp(double x, double y)
            => _context?.Log($"Stopped drawing at ({x:0}, {y:0})");
    }
}
