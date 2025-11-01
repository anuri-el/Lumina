namespace Lumina.Core.Patterns
{
    public class CropState : IEditorState
    {
        private EditorContext? _context;
        private double startX, startY;

        public void SetContext(EditorContext context) => _context = context;
        public void Enter() => _context?.Log("Entering Crop mode");
        public void Exit() => _context?.Log("Leaving Crop mode");

        public void HandleMouseDown(double x, double y)
        {
            startX = x;
            startY = y;
            _context?.Log($"Started cropping at ({x:0}, {y:0})");
        }

        public void HandleMouseMove(double x, double y)
            => _context?.Log($"Cropping area: from ({startX:0}, {startY:0}) to ({x:0}, {y:0})");

        public void HandleMouseUp(double x, double y)
            => _context?.Log($"Crop finalized at ({x:0}, {y:0})");
    }
}
