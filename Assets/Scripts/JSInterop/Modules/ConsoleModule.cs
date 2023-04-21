using UnityEngine;

namespace JSInterop.Modules
{
    public class ConsoleModule
    {
        public static void log(string text)
        {
            Debug.Log(text);
        }
    }
}