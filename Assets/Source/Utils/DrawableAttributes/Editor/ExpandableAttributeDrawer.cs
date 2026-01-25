using UnityEngine;
using UnityEngine.UIElements;

#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEditor.UIElements;
#endif

#if UNITY_EDITOR
/// <summary>
/// Draws the property field for any field marked with ExpandableAttribute.
/// </summary>
[CustomPropertyDrawer(typeof(ExpandableAttribute), true)]
public class ExpandableAttributeDrawer : PropertyDrawer
{
    public static void RepaintInspector(SerializedObject BaseObject)
    {
        foreach (var item in ActiveEditorTracker.sharedTracker.activeEditors)
            if (item.serializedObject == BaseObject)
            { item.Repaint(); return; }
    }
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        VisualElement container = new();
        
        PropertyField propertyField = new PropertyField(property);

        container.Add(propertyField);
        
        //propertyField.RegisterValueChangeCallback((_)=>RepaintInspector(property.serializedObject));
        
        property.serializedObject.ApplyModifiedProperties();
        if (property.objectReferenceValue == null) return container;

        SerializedObject targetObject = new(property.objectReferenceValue);
        targetObject.Update();
        Foldout foldout = new ();
        
        var foldoutStyle = foldout.style;
        foldoutStyle.paddingLeft = 15.0f;
        container.Add(foldout);

        SerializedProperty field = targetObject.GetIterator();
        field.NextVisible(true);
        while (field.NextVisible(false)) 
        {
            PropertyField next = null;
            try
            {
                next = new(field);
                next.Bind(field.serializedObject);
            }
            catch (StackOverflowException)
            {
                field.objectReferenceValue = null;
                Debug.LogError("Detected self-nesting causing a StackOverflowException, avoid using the same object inside a nested structure.");
            }
            catch (Exception e) 
            {
                Debug.Log(e);
            }
            
            if (next != null) foldout.Add(next);
        }

        targetObject.ApplyModifiedProperties();
        return container;
    }
}
#endif