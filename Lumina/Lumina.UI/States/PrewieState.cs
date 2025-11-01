namespace Lumina.UI.States
{
    public class PreviewState : IEditorState
    {
        public string Name => "Preview";

        public string OnAction()
        {
            return "Editor is in preview mode. You can see the final image.";
        }
    }
}
