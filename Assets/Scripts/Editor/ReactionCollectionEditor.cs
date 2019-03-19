using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(ReactionCollection))]
public class ReactionCollectionEditor : EditorWithSubEditors<ReactionEditor, Reaction>
{
    private ReactionCollection reactionCollection;       
    private SerializedProperty reactionsProperty;      

    private Type[] reactionTypes;                      
    private string[] reactionTypeNames;                 
    private int selectedIndex;                          

    private const float dropAreaHeight = 50f;            
    private const float controlSpacing = 5f;            
    private const string reactionsPropName = "reactions";  

    private readonly float verticalSpacing = EditorGUIUtility.standardVerticalSpacing;

    private void OnEnable()
    {
        reactionCollection = (ReactionCollection)target;

        reactionsProperty = serializedObject.FindProperty(reactionsPropName);
        CheckAndCreateSubEditors(reactionCollection.reactions);
        SetReactionNamesArray();
    }

    private void OnDisable()
    {
        CleanupEditors();
    }

    protected override void SubEditorSetup(ReactionEditor editor)
    {
        editor.reactionsProperty = reactionsProperty;
    }

    #region InspectorGUI

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        CheckAndCreateSubEditors(reactionCollection.reactions);

        for (int i = 0; i < subEditors.Length; i++)
        {
            subEditors[i].OnInspectorGUI();
        }

        if (reactionCollection.reactions.Length > 0)
        {
            EditorGUILayout.Space();
            EditorGUILayout.Space();
        }

        Rect fullWidthRect = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, GUILayout.Height(dropAreaHeight + verticalSpacing));
        Rect leftAreaRect = fullWidthRect;

        leftAreaRect.y += verticalSpacing * 0.5f;

        leftAreaRect.width *= 0.5f;
        leftAreaRect.width -= controlSpacing * 0.5f;

        leftAreaRect.height = dropAreaHeight;

        Rect rightAreaRect = leftAreaRect;

        rightAreaRect.x += rightAreaRect.width + controlSpacing;

        TypeSelectionGUI(leftAreaRect);

        DragAndDropAreaGUI(rightAreaRect);

        DraggingAndDropping(rightAreaRect, this);

        serializedObject.ApplyModifiedProperties();
    }


    private void TypeSelectionGUI(Rect containingRect)
    {
        Rect topHalf = containingRect;
        topHalf.height *= 0.5f;
        Rect bottomHalf = topHalf;
        bottomHalf.y += bottomHalf.height;

        // Display a popup in the top half showing all the reaction types.
        selectedIndex = EditorGUI.Popup(topHalf, selectedIndex, reactionTypeNames);

        // Display a button in the bottom half that if clicked...
        if (GUI.Button(bottomHalf, "Add Selected Reaction"))
        {
            // ... finds the type selected by the popup, creates an appropriate reaction and adds it to the array.
            Type reactionType = reactionTypes[selectedIndex];
            Reaction newReaction = ReactionEditor.CreateReaction(reactionType);
            reactionsProperty.AddToObjectArray(newReaction);
        }
    }


    private static void DragAndDropAreaGUI(Rect containingRect)
    {
        // Create a GUI style of a box but with middle aligned text and button text color.
        GUIStyle centredStyle = GUI.skin.box;
        centredStyle.alignment = TextAnchor.MiddleCenter;
        centredStyle.normal.textColor = GUI.skin.button.normal.textColor;

        // Draw a box over the area with the created style.
        GUI.Box(containingRect, "Drop new Reactions here", centredStyle);
    }


    private static void DraggingAndDropping(Rect dropArea, ReactionCollectionEditor editor)
    {
        // Cache the current event.
        Event currentEvent = Event.current;

        // If the drop area doesn't contain the mouse then return.
        if (!dropArea.Contains(currentEvent.mousePosition))
            return;

        switch (currentEvent.type)
        {
            // If the mouse is dragging something...
            case EventType.DragUpdated:

                // ... change whether or not the drag *can* be performed by changing the visual mode of the cursor based on the IsDragValid function.
                DragAndDrop.visualMode = IsDragValid() ? DragAndDropVisualMode.Link : DragAndDropVisualMode.Rejected;

                // Make sure the event isn't used by anything else.
                currentEvent.Use();

                break;

            // If the mouse was dragging something and has released...
            case EventType.DragPerform:

                // ... accept the drag event.
                DragAndDrop.AcceptDrag();

                // Go through all the objects that were being dragged...
                for (int i = 0; i < DragAndDrop.objectReferences.Length; i++)
                {
                    // ... and find the script asset that was being dragged...
                    MonoScript script = DragAndDrop.objectReferences[i] as MonoScript;

                    // ... then find the type of that Reaction...
                    Type reactionType = script.GetClass();

                    // ... and create a Reaction of that type and add it to the array.
                    Reaction newReaction = ReactionEditor.CreateReaction(reactionType);
                    editor.reactionsProperty.AddToObjectArray(newReaction);
                }

                // Make sure the event isn't used by anything else.
                currentEvent.Use();

                break;
        }
    }


    private static bool IsDragValid()
    {
        // Go through all the objects being dragged...
        for (int i = 0; i < DragAndDrop.objectReferences.Length; i++)
        {
            // ... and if any of them are not script assets, return that the drag is invalid.
            if (DragAndDrop.objectReferences[i].GetType() != typeof(MonoScript))
                return false;

            // Otherwise find the class contained in the script asset.
            MonoScript script = DragAndDrop.objectReferences[i] as MonoScript;
            Type scriptType = script.GetClass();

            // If the script does not inherit from Reaction, return that the drag is invalid.
            if (!scriptType.IsSubclassOf(typeof(Reaction)))
                return false;

            // If the script is an abstract, return that the drag is invalid.
            if (scriptType.IsAbstract)
                return false;
        }

        // If none of the dragging objects returned that the drag was invalid, return that it is valid.
        return true;
    }

    #endregion

    private void SetReactionNamesArray()
    {
        Type reactionType = typeof(Reaction);
        Type[] allTypes = reactionType.Assembly.GetTypes();
        List<Type> reactionSubTypeList = new List<Type>();

        for (int i = 0; i < allTypes.Length; i++)
        {
            if (allTypes[i].IsSubclassOf(reactionType) && !allTypes[i].IsAbstract)
            {
                reactionSubTypeList.Add(allTypes[i]);
            }
        }

        reactionTypes = reactionSubTypeList.ToArray();
        List<string> reactionTypeNameList = new List<string>();

        for (int i = 0; i < reactionTypes.Length; i++)
        {
            reactionTypeNameList.Add(reactionTypes[i].Name);
        }

        reactionTypeNames = reactionTypeNameList.ToArray();
    }
}
