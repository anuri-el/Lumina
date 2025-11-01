namespace Lumina.Core.Patterns
{
    public class SelectState : IEditorState
    {
        private EditorContext? _context;

        public void SetContext(EditorContext context) => _context = context;
        public void Enter() => _context?.Log("Entering Select mode");
        public void Exit() => _context?.Log("Leaving Select mode");

        public void HandleMouseDown(double x, double y)
            => _context?.Log($"Selected object at ({x:0}, {y:0})");

        public void HandleMouseMove(double x, double y) { }
        public void HandleMouseUp(double x, double y) { }
    }
}
