using System.IO;
using UnityEditor;
using UnityEngine;

namespace Resolve.BaseServices.Editor
{
    public class ResolveEditorToolls
    {
        [MenuItem("EEA Tools/Clear PlayerPrefs")]
        public static void ClearPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
        }

        [MenuItem("EEA Tools/Open Data Path")]
        public static void OpenDataFolder()
        {
            string filePath = Application.persistentDataPath;

            string itemPath = filePath.Replace(@"/", @"\");   // explorer doesn't like front slashes
            System.Diagnostics.Process.Start("explorer.exe", "/select," + itemPath);
        }
    }
}