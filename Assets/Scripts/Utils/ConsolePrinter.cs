using UnityEngine;

namespace Utils
{
    public static class ConsolePrinter
    {
        public static void PrintSaveSystemMessage(string msg)
        {
            Debug.Log($"<color=teal>{msg}</color>");
        }
    }
}