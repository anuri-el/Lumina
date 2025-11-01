namespace Lumina.UI.States
{
    public class EditState : IEditorState
    {
        public string Name => "Edit";

        public string OnAction()
        {
            return "Editor is in edit mode. You can apply transformations and effects.";
        }
    }
}
