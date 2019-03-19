using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ConditionCollection))]
public class ConditionCollectionEditor : EditorWithSubEditors<ConditionEditor, Condition>
{
    public SerializedProperty collectionsProperty;
    private ConditionCollection conditionCollection;

    private SerializedProperty descriptionProperty;
    private SerializedProperty conditionsProperty;
    private SerializedProperty reactionCollectionProperty;

    private const float conditionButtonWidth = 30f;
    private const float collectionButtonWidth = 125f;

    private const string conditionCollectionPropDescriptionName = "description";
    private const string conditionCollectionPropRequiredConditionsName = "requiredConditions";
    private const string conditionCollectionPropReactionCollectionName = "reactionCollection";

    private void OnEnable()
    {
        conditionCollection = (ConditionCollection)target;

        if (target == null)
        {
            DestroyImmediate(this);
            return;
        }

        descriptionProperty = serializedObject.FindProperty(conditionCollectionPropDescriptionName);
        conditionsProperty = serializedObject.FindProperty(conditionCollectionPropRequiredConditionsName);
        reactionCollectionProperty = serializedObject.FindProperty(conditionCollectionPropReactionCollectionName);

        CheckAndCreateSubEditors(conditionCollection.requiredConditions);
    }

    private void OnDisable()
    {
        CleanupEditors();
    }

    protected override void SubEditorSetup(ConditionEditor editor)
    {
        editor.editorType = ConditionEditor.EditorType.ConditionCollection;
        // 新建的 condition 需要知道自己属于哪个 condition collection
        editor.conditionsProperty = conditionsProperty;
    }

    #region InspectorGUI

    public override void OnInspectorGUI()
    {
        // Pull the information from the target into the serializedObject.
        serializedObject.Update();

        // Check if the Editors for the Conditions need creating and optionally create them.
        CheckAndCreateSubEditors(conditionCollection.requiredConditions);

        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUI.indentLevel++;

        EditorGUILayout.BeginHorizontal();

        // Use the isExpanded bool for the descriptionProperty to store whether the foldout is open or closed.
        descriptionProperty.isExpanded = EditorGUILayout.Foldout(descriptionProperty.isExpanded, descriptionProperty.stringValue);

        // Display a button showing 'Remove Collection' which removes the target from the Interactable when clicked.
        if (GUILayout.Button("Remove Collection", GUILayout.Width(collectionButtonWidth)))
        {
            collectionsProperty.RemoveFromObjectArray(conditionCollection);
        }

        EditorGUILayout.EndHorizontal();

        // If the foldout is open show the expanded GUI.
        if (descriptionProperty.isExpanded)
        {
            ExpandedGUI();
        }

        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();

        // Push all changes made on the serializedObject back to the target.
        serializedObject.ApplyModifiedProperties();
    }


    private void ExpandedGUI()
    {
        EditorGUILayout.Space();

        // Display the description for editing.
        EditorGUILayout.PropertyField(descriptionProperty);

        EditorGUILayout.Space();

        // Display the Labels for the Conditions evenly split over the width of the inspector.
        float space = EditorGUIUtility.currentViewWidth / 3f;

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Condition", GUILayout.Width(space));
        EditorGUILayout.LabelField("Satisfied?", GUILayout.Width(space));
        EditorGUILayout.LabelField("Add/Remove", GUILayout.Width(space));
        EditorGUILayout.EndHorizontal();

        // Display each of the Conditions.
        EditorGUILayout.BeginVertical(GUI.skin.box);
        for (int i = 0; i < subEditors.Length; i++)
        {
            subEditors[i].OnInspectorGUI();
        }
        EditorGUILayout.EndHorizontal();

        // Display a right aligned button which when clicked adds a Condition to the array.
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("+", GUILayout.Width(conditionButtonWidth)))
        {
            Condition newCondition = ConditionEditor.CreateCondition();
            conditionsProperty.AddToObjectArray(newCondition);
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        // Display the reference to the ReactionCollection for editing.
        EditorGUILayout.PropertyField(reactionCollectionProperty);
    }

    #endregion

    public static ConditionCollection CreateConditionCollection()
    {
        ConditionCollection newConditionCollection = CreateInstance<ConditionCollection>();

        newConditionCollection.description = "New condition collection";
        newConditionCollection.requiredConditions = new Condition[1];
        newConditionCollection.requiredConditions[0] = ConditionEditor.CreateCondition();

        return newConditionCollection;
    }
}
