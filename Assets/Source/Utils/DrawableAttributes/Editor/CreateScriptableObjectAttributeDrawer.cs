using UnityEngine;
using UnityEngine.UIElements;

#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEditor.UIElements;
#endif
#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(CreateScriptableObjectAttribute), true)]
public class CreateScriptableObjectAttributeDrawer:PropertyDrawer
{
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        VisualElement container = new();
        PropertyField propertyField = new PropertyField(property);
        Button createButton = new Button(() =>
        {
            string path = EditorUtility.SaveFilePanel($"Save a new {fieldInfo.FieldType.ToString()}",
                Application.dataPath, $"new {fieldInfo.FieldType}", "asset"). Replace(Application.dataPath,"Assets");
            Debug.Log(path);
            Debug.Log(Application.dataPath);
            AssetDatabase.CreateAsset( ScriptableObject.CreateInstance(fieldInfo.FieldType), path);
            property.objectReferenceValue = AssetDatabase.LoadAssetAtPath(path, fieldInfo.FieldType);
            property.serializedObject.ApplyModifiedProperties();
        });
        createButton.text = "New";

        var containerStyle = container.style;
        containerStyle.flexDirection = FlexDirection.Row;
        
        var propertyStyle = propertyField.style;
        propertyStyle.flexGrow = 1;
        propertyStyle.flexShrink = 1;
        
        
        container.Add(propertyField);
        container.Add(createButton);
        return container;
    }
}
#endif
