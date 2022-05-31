#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace NK.ScriptsCreator.NKEditor
{
    public class NKScriptsCreatorSettings : ScriptableObject
    {
        public string Namespace = "NK";

        private static NKScriptsCreatorSettings FindSettings()
        {
            string[] guids = AssetDatabase.FindAssets("t:NKScriptsCreatorSettings");
            if (guids.Length > 1) Debug.LogWarning("Found multiple settings files, using the first.");

            switch (guids.Length)
            {
                case 0:
                    return null;
                default:
                    string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                    return AssetDatabase.LoadAssetAtPath<NKScriptsCreatorSettings>(path);
            }
        }

        public static NKScriptsCreatorSettings GetOrCreateSettings()
        {
            NKScriptsCreatorSettings settings = FindSettings();
            if (settings == null)
            {
                settings = CreateInstance<NKScriptsCreatorSettings>();
                AssetDatabase.CreateAsset(settings, "Assets/Plugins/NKTools/NKScriptsCreator/Settings/Editor/NKScriptsCreatorSettings.asset");
                AssetDatabase.SaveAssets();
            }
            return settings;
        }

        public static SerializedObject GetSerializedSettings()
        {
            return new SerializedObject(GetOrCreateSettings());
        }
    }

    // Register a SettingsProvider using UIElements for the drawing framework:
    public static class NKScriptsCreatorSettingsCustomUI
    {
        [SettingsProvider]
        public static SettingsProvider CreateMyCustomSettingsProvider()
        {
            // First parameter is the path in the Settings window.
            // Second parameter is the scope of this setting: it only appears in the Settings window for the Project scope.
            var provider = new SettingsProvider("Project/NKTools/NKScriptsCreator", SettingsScope.Project)
            {
                label = "NK Scripts Creator",
                // activateHandler is called when the user clicks on the Settings item in the Settings window.
                activateHandler = (searchContext, rootElement) =>
                {
                    SerializedObject settings = NKScriptsCreatorSettings.GetSerializedSettings();

                    // rootElement is a VisualElement. If you add any children to it, the OnGUI function
                    // isn't called because the SettingsProvider uses the UIElements drawing framework.
                    var title = new Label()
                    {
                        text = "NK Scripts Creator"
                    };
                    title.AddToClassList("title");
                    rootElement.Add(title);

                    var properties = new VisualElement()
                    {
                        style = { flexDirection = FlexDirection.Column }
                    };
                    properties.AddToClassList("property-list");
                    rootElement.Add(properties);

                    properties.Add(new InspectorElement(settings));

                    rootElement.Bind(settings);
                },
            };

            return provider;
        }
    }
}
#endif