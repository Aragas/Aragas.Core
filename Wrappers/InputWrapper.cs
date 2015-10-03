using System;

namespace Aragas.Core.Wrappers
{
    public class KeyPressedEventArgs : EventArgs
    {
        public string Key { get; set; }

        public KeyPressedEventArgs(string key)
        {
            Key = key;
        }
    }

    public interface IInputWrapper
    {
        event EventHandler<KeyPressedEventArgs> KeyPressed;

        void ShowKeyboard();

        void HideKeyboard();

        void ConsoleWrite(String message);
        
        void LogWriteLine(String message);
    }

    public static class InputWrapper
    {
        private static IInputWrapper _instance;
        public static IInputWrapper Instance
        {
            private get
            {
                if (_instance == null)
                    throw new NotImplementedException("This instance is not implemented. You need to implement it in your main project");
                return _instance;
            }
            set { _instance = value; }
        }
        
        public static event EventHandler<KeyPressedEventArgs> OnKey { add { Instance.KeyPressed += value; } remove { Instance.KeyPressed -= value; } }

        public static void ShowKeyboard() { Instance.ShowKeyboard();}
        public static void HideKeyboard() { Instance.HideKeyboard(); }

        public static void ConsoleWrite(string message) { Instance.ConsoleWrite(message); }

        public static void LogWriteLine(string message) { Instance.LogWriteLine(message); }
    }
}
