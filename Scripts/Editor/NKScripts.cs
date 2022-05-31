#if UNITY_EDITOR
using UnityEngine;
using System.IO;
using UnityEditor;
using System;

namespace NK.ScriptsCreator.NKEditor
{
    public class NKScripts
    {
        public struct ScriptTemplate
        {
            public TextAsset TemplateFile;
            public string DefaultFileName;
        }

        private const string DEFAULT_PATH = "Assets/Plugins/NKTools/NKScriptsCreator/Templates/Editor";

        private static readonly ScriptTemplate[] s_ScriptsTemplates =
        {
            new ScriptTemplate { TemplateFile = GetTextAssetFile("DefaultMonoBehaviour.cs.txt"), DefaultFileName = "NewMonoBehaviour.cs" },
            new ScriptTemplate { TemplateFile = GetTextAssetFile("DefaultScriptableObject.cs.txt"), DefaultFileName = "NewScriptableObject.cs" },
            new ScriptTemplate { TemplateFile = GetTextAssetFile("DefaultClass.cs.txt"), DefaultFileName = "NewDefaultClass.cs" },
            new ScriptTemplate { TemplateFile = GetTextAssetFile("DefaultStruct.cs.txt"), DefaultFileName = "NewDefaultStruct.cs" },
            new ScriptTemplate { TemplateFile = GetTextAssetFile("DefaultInterface.cs.txt"), DefaultFileName = "NewDefaultInterface.cs" }
        };

        private static TextAsset GetTextAssetFile(string templateFileName)
        {
            string path = Path.Combine(DEFAULT_PATH, templateFileName);
            return AssetDatabase.LoadAssetAtPath<TextAsset>(path);
        }

        private static void CreateNewScript(ScriptTemplate template)
        {
            string templatePath = AssetDatabase.GetAssetPath(template.TemplateFile);

            string[] lines = template.TemplateFile.text.Split('\n');
            string definedNamespace = NKScriptsCreatorSettings.GetOrCreateSettings().Namespace;
            if (!lines[2].Contains(definedNamespace))
            {
                lines[2] = $"namespace {definedNamespace}\n";
                string final = string.Empty;
                Array.ForEach(lines, line => final += line);
                File.WriteAllText(templatePath, final);
                AssetDatabase.Refresh();
            }

            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, template.DefaultFileName);
        }

        [MenuItem("Assets/Create/Scripts/New MonoBehaviour", priority = 52)]
        public static void CreateMonoBehaviour()
        {
            CreateNewScript(s_ScriptsTemplates[0]);
        }

        [MenuItem("Assets/Create/Scripts/New ScriptableObject")]
        public static void CreateScriptableObject()
        {
            CreateNewScript(s_ScriptsTemplates[1]);
        }

        [MenuItem("Assets/Create/Scripts/New Class")]
        public static void CreateDefaultClass()
        {
            CreateNewScript(s_ScriptsTemplates[2]);
        }

        [MenuItem("Assets/Create/Scripts/New Struct")]
        public static void CreateDefaultStruct()
        {
            CreateNewScript(s_ScriptsTemplates[3]);
        }

        [MenuItem("Assets/Create/Scripts/New Interface")]
        public static void CreateDefaultInterface()
        {
            CreateNewScript(s_ScriptsTemplates[4]);
        }
    }
}
#endif
