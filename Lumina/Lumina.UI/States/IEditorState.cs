namespace Lumina.UI.States
{
    public interface IEditorState
    {
        string Name { get; }
        string OnAction();
    }
}
