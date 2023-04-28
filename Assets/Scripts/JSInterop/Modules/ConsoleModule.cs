using UnityEngine;

namespace JSInterop.Modules
{
    public class ConsoleModule
    {
        public static void log(string text)
        {
            Debug.Log(text);
        }

        public static void log(object obj)
        {
            Debug.Log(obj.ToString());
        }

        public static void error(dynamic obj)
        {
            Debug.LogError("JS:" + obj.hostException.ToString());
        }
    }
}