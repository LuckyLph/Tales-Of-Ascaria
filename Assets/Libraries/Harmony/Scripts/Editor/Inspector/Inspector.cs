using System;
using System.Collections.Generic;
using Rotorz.ReorderableList;
using Rotorz.ReorderableList.Internal;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Harmony
{
    /// <summary>
    /// Base pour créer facilement un inspecteur personalisé sous Unity, avec quelques fonctionalitées supplémentaires.
    /// </summary>
    public abstract class Inspector : Editor
    {
        protected SerializedProperty GetBasicProperty(string name)
        {
            return serializedObject.FindProperty(name);
        }

        protected SerializedProperty GetListProperty(string name)
        {
            return GetBasicProperty(name);
        }

        protected EnumProperty GetEnumProperty(string name, Type enumType)
        {
            return new EnumProperty(GetBasicProperty(name), enumType);
        }

        protected void DrawBasicProperty(SerializedProperty property)
        {
            if (property != null && property.IsValid())
            {
                EditorGUILayout.PropertyField(property);

                property.serializedObject.ApplyModifiedProperties();
            }
        }

        protected void DrawBasicPropertyWithTitleLabel(SerializedProperty property)
        {
            if (property != null && property.IsValid())
            {
                DrawTitleLabel(property.displayName);
                EditorGUILayout.PropertyField(property, GUIContent.none);

                property.serializedObject.ApplyModifiedProperties();
            }
        }

        protected void DrawListProperty(SerializedProperty property, bool isFixed = false, int startIndex = 0)
        {
            if (property != null && property.IsValid())
            {
                DrawTitleLabel(property.displayName);
                ReorderableListGUI.ListField(new ListPropertyAdapter(property, startIndex),
                                             () => DrawLabel("Empty"),
                                             isFixed
                                                 ? ReorderableListFlags.DisableReordering |
                                                   ReorderableListFlags.HideAddButton |
                                                   ReorderableListFlags.HideRemoveButtons
                                                 : ReorderableListFlags.DisableContextMenu);

                property.serializedObject.ApplyModifiedProperties();
            }
        }

        protected void DrawEnumPropertyDropDown(EnumProperty property)
        {
            if (property != null && property.IsValid())
            {
                DrawTitleLabel(property.Name);
                property.CurrentValueIndex = EditorGUILayout.Popup(property.CurrentValueIndex,
                                                                   property.ValuesNames);

                property.ApplyModifiedProperties();
            }
        }

        protected void DrawEnumPropertyGrid(EnumProperty property)
        {
            if (property != null && property.IsValid())
            {
                DrawTitleLabel(property.Name);
                property.CurrentValueIndex = GUILayout.SelectionGrid(property.CurrentValueIndex,
                                                                     property.ValuesNames,
                                                                     2,
                                                                     EditorStyles.radioButton);
                EditorGUILayout.Space();

                property.ApplyModifiedProperties();
            }
        }

        protected void DrawSeparator()
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        }

        protected void BeginHorizontal()
        {
            EditorGUILayout.BeginHorizontal();
        }

        protected void EndHorizontal()
        {
            EditorGUILayout.EndHorizontal();
        }

        protected void BeginVertical()
        {
            EditorGUILayout.BeginVertical();
        }

        protected void EndVertical()
        {
            EditorGUILayout.EndVertical();
        }

        protected void BeginTable(string title)
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
        }

        protected void EndTable()
        {
            EditorGUILayout.EndHorizontal();
        }

        protected void BeginTableRow()
        {
            EditorGUILayout.BeginVertical();
        }

        protected void EndTableRow()
        {
            EditorGUILayout.EndVertical();
        }

        protected void DrawTitleLabel(string text)
        {
            DrawBoldLabel(text);
        }

        protected void DrawLabel(string text)
        {
            EditorGUILayout.LabelField(text);
        }

        protected void DrawBoldLabel(string text)
        {
            EditorGUILayout.LabelField(text, EditorStyles.boldLabel);
        }

        protected void DrawSection(string text)
        {
            GUIStyle style = EditorStyles.largeLabel;
            style.fontStyle = FontStyle.Bold;
            style.fontSize = 15;
            style.fixedHeight = 25;
            EditorGUILayout.LabelField(text, style);
            DrawSeparator();
        }

        protected void DrawImage(Texture2D image)
        {
            GUIStyle centeredStyle = GUI.skin.GetStyle("Label");
            centeredStyle.alignment = TextAnchor.UpperCenter;
            GUILayout.Label(image, centeredStyle);
        }

        protected void DrawTableCell(string text)
        {
            EditorGUILayout.TextArea(text, EditorStyles.label);
        }

        protected void DrawTableCell(string text, Color color)
        {
            GUIStyle guiStyle = new GUIStyle(EditorStyles.label);
            guiStyle.normal.textColor = color;
            EditorGUILayout.TextArea(text, guiStyle);
        }

        protected void DrawTableHeader(string text)
        {
            EditorGUILayout.TextArea(text, EditorStyles.boldLabel);
        }

        protected void DrawInfoBox(string text)
        {
            EditorGUILayout.HelpBox(text, MessageType.Info);
        }

        protected void DrawWarningBox(string text)
        {
            EditorGUILayout.HelpBox(text, MessageType.Warning);
        }

        protected void DrawErrorBox(string text)
        {
            EditorGUILayout.HelpBox(text, MessageType.Error);
        }

        protected void DrawButton(string text, UnityAction actionOnClick)
        {
            if (GUILayout.Button(text))
            {
                actionOnClick();
            }
        }

        protected void DrawDisabledButton(string text)
        {
            EditorGUI.BeginDisabledGroup(true);
            GUILayout.Button(text);
            EditorGUI.EndDisabledGroup();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            OnDraw();
            serializedObject.ApplyModifiedProperties();
        }

        protected abstract void OnDraw();

        protected sealed class EnumProperty
        {
            private readonly SerializedProperty property;

            public string Name
            {
                get { return property.displayName; }
            }

            public List<EnumPropertyValue> Values { get; private set; }
            public string[] ValuesNames { get; private set; }

            public int CurrentValue
            {
                get { return property.intValue; }
                set { property.intValue = value; }
            }

            public int CurrentValueIndex
            {
                get
                {
                    for (int i = 0; i < Values.Count; i++)
                    {
                        if (CurrentValue == Values[i].Value)
                        {
                            return i;
                        }
                    }
                    return -1;
                }
                set { CurrentValue = Values[value].Value; }
            }

            public EnumProperty(SerializedProperty property, Type enumType)
            {
                this.property = property;

                //Enum values
                Array enumValues = Enum.GetValues(enumType);
                //Enum value names
                string[] enumNames = Enum.GetNames(enumType);

                //Create a list of enum values to display and sort it by name
                //If there is a "None" value, make it first.
                Values = new List<EnumPropertyValue>();
                int noneValueIndex = -1;
                for (int i = 0; i < enumValues.Length; i++)
                {
                    if (enumNames[i] == "None")
                    {
                        noneValueIndex = i;
                    }
                    else
                    {
                        Values.Add(new EnumPropertyValue((int) enumValues.GetValue(i),
                                                         enumNames[i]));
                    }
                }
                Values.Sort((displayable1, displayable2) => displayable1.Name.CompareTo(displayable2.Name));
                if (noneValueIndex != -1)
                {
                    Values.Insert(0, new EnumPropertyValue((int) enumValues.GetValue(noneValueIndex),
                                                           enumNames[noneValueIndex]));
                }

                //Create array of enum value names
                ValuesNames = new string[Values.Count];
                for (int i = 0; i < enumNames.Length; i++)
                {
                    ValuesNames[i] = Values[i].Name;
                }
            }

            public bool IsValid()
            {
                return property.IsValid();
            }

            public bool NeedRefresh()
            {
                return property.NeedRefresh();
            }

            public void ApplyModifiedProperties()
            {
                property.serializedObject.ApplyModifiedProperties();
            }
        }

        protected sealed class EnumPropertyValue
        {
            public int Value { get; private set; }
            public string Name { get; private set; }

            public EnumPropertyValue(int value, string name)
            {
                Value = value;
                Name = name;
            }
        }

        private sealed class ListPropertyAdapter : IReorderableListAdaptor
        {
            private readonly SerializedProperty property;
            private readonly int startIndex;

            public ListPropertyAdapter(SerializedProperty property,
                                       int startIndex)
            {
                this.property = property;
                this.startIndex = startIndex;
            }

            public int Count
            {
                get
                {
                    int count = property.arraySize - startIndex;
                    return count < 0 ? 0 : count;
                }
            }

            public bool CanDrag(int index)
            {
                return true;
            }

            public bool CanRemove(int index)
            {
                return true;
            }

            public void Add()
            {
                int arraySize = property.arraySize;
                ++property.arraySize;
                SerializedPropertyUtility.ResetValue(property.GetArrayElementAtIndex(arraySize));
            }

            public void Insert(int index)
            {
                property.InsertArrayElementAtIndex(index + startIndex);
                SerializedPropertyUtility.ResetValue(property.GetArrayElementAtIndex(index));
            }

            public void Duplicate(int index)
            {
                property.InsertArrayElementAtIndex(index + startIndex);
            }

            public void Remove(int index)
            {
                SerializedProperty arrayElementAtIndex = property.GetArrayElementAtIndex(index + startIndex);
                if (arrayElementAtIndex.propertyType == SerializedPropertyType.ObjectReference)
                {
                    arrayElementAtIndex.objectReferenceValue = null;
                }
                property.DeleteArrayElementAtIndex(index + startIndex);
            }

            public void Move(int sourceIndex, int destIndex)
            {
                if (destIndex > sourceIndex)
                {
                    --destIndex;
                }
                property.MoveArrayElement(sourceIndex + startIndex, destIndex + startIndex);
            }

            public void Clear()
            {
                property.ClearArray();
            }

            public void BeginGUI()
            {
            }

            public void EndGUI()
            {
            }

            public void DrawItemBackground(Rect position, int index)
            {
            }

            public void DrawItem(Rect position, int index)
            {
                EditorGUI.PropertyField(position, property.GetArrayElementAtIndex(index + startIndex), GUIContent.none, false);
            }

            public float GetItemHeight(int index)
            {
                return EditorGUI.GetPropertyHeight(property.GetArrayElementAtIndex(index + startIndex), GUIContent.none, false);
            }
        }
    }
}