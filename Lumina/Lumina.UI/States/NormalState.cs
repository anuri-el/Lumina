namespace Lumina.UI.States
{
    public class NormalState : IEditorState
    {
        public string Name => "Normal";

        public string OnAction()
        {
            return "Editor is in normal mode. You can view and navigate the image.";
        }
    }
}
