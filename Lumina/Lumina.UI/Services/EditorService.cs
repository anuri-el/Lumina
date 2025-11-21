using Lumina.Core.Patterns;
using System.Windows.Media.Imaging;

namespace Lumina.Core.Services
{
    public class EditorState : IPrototype<EditorState>
    {
        public BitmapImage? Image { get; set; }
        public double Brightness { get; set; }
        public double Contrast { get; set; }

        public EditorState Clone()
        {
            return new EditorState
            {
                Image = Image,
                Brightness = Brightness,
                Contrast = Contrast
            };
        }
    }

    public class EditorService
    {
        private readonly Stack<EditorState> _undo = new();
        private readonly Stack<EditorState> _redo = new();

        public EditorState Current { get; private set; } = new();

        public event Action<string>? Log;

        private void WriteLog(string msg)
        {
            Log?.Invoke(msg);
        }

        public EditorService()
        {
            WriteLog("EditorService initialized");
        }

        public void SetImage(BitmapImage image)
        {
            SaveUndo();
            Current.Image = image;

            WriteLog("Завантажено зображення");
        }

        public void SetBrightness(double value)
        {
            SaveUndo();
            Current.Brightness = value;
            WriteLog($"Brightness = {value}");
        }

        public void SetContrast(double value)
        {
            SaveUndo();
            Current.Contrast = value;
            WriteLog($"Contrast = {value}");
        }

        private void SaveUndo()
        {
            _undo.Push(Current.Clone());
            _redo.Clear();
        }

        public void Undo()
        {
            if (_undo.Count == 0)
            {
                WriteLog("Undo неможливий");
                return;
            }

            _redo.Push(Current.Clone());
            Current = _undo.Pop();

            WriteLog("Undo виконано");
        }

        public void Redo()
        {
            if (_redo.Count == 0)
            {
                WriteLog("Redo неможливий");
                return;
            }

            _undo.Push(Current.Clone());
            Current = _redo.Pop();

            WriteLog("Redo виконано");
        }

        public EditorState CreateSnapshot()
        {
            WriteLog("Створено Prototype snapshot");
            return Current.Clone();
        }

        public void RestoreSnapshot(EditorState snapshot)
        {
            SaveUndo();
            Current = snapshot.Clone();
            WriteLog("Відновлено стан з Prototype");
        }
    }
}
