using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MotionStateMachineRegistry))]
public class MotionStateMachineRegistryEditor : Editor
{
    public TextAsset registryTemplate;
    public TextAsset registryScript;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Generate Runtime Class"))
        {
            string pathFolder = Path.GetDirectoryName(AssetDatabase.GetAssetPath(registryScript));
            string pattern =  @"\{[^}]*\}"; 
            string result = Regex.Replace(registryTemplate.name, pattern, registryScript.name);
            string newScriptPath = Path.Combine(Path.GetDirectoryName(Application.dataPath), pathFolder, result);
            string templateText = registryTemplate.text;

            TypeInfo typeInfo = typeof(MotionStateMachineRegistry).GetTypeInfo();
            
            templateText = templateText.Replace("#CLASS_NAME#", registryScript.name);
            Type parentClass = typeInfo.BaseType;
            templateText = templateText.Replace("#DERIVED_TYPE#", parentClass == null ? "" : $": {parentClass.Name}");

            StringBuilder propertyLoopString = new StringBuilder();

            var properties = typeInfo.GetProperties().Where(prop => prop.IsDefined(typeof(StateMachinePropertyAttribute), false));

            foreach (PropertyInfo propertyInfo in properties)
            {
                propertyLoopString.Append($"properties.Add(nameof({propertyInfo.Name}), new (() => {propertyInfo.Name}, (obj) => {propertyInfo.Name} = ({propertyInfo.PropertyType})obj)); \n\t");
            }

            templateText = templateText.Replace("#INITIALIZE_PROPERTY_LOOP#", propertyLoopString.ToString());
            
            Debug.Log(propertyLoopString.ToString());
            
            File.WriteAllText(newScriptPath, templateText);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
