using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "New Potion", menuName = "Items/Potion")]
public class PotionData : ScriptableObject {

    // Holds potion type
    public enum PotionType { Health, Mana, Both, None }         //type of restoration

    public enum Conditions {None, Poison, Confusion, Multi}     //Conditions

    public enum TargetType {Enemy, Player, Both, None}          // target of effect

    public enum ItemCategory { Usable, Story, Equipment, Key}   //type of item

    [Header("Potion Details")]
    public string potionName;       //Name of potion

    [TextArea]
    public string potionDescription;    //text blurb about potion
    public PotionType potionType;       //Type of potion
    public Conditions conditionsType;   //Does it effct condition
    public TargetType targetType;       //Who is it used on
    public ItemCategory itemType;       // type of item
    public int HP;               // value restored for HP
    public int MP;               // Value restored for MP


    //public useItem() {

    //}
}
/*
[CustomEditor(typeof(Potion))]
public class PotionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Get the target Potion script
        Potion potion = (Potion)target;

        // Draw default fields
        potion.potionName = EditorGUILayout.TextField("Potion Name", potion.potionName);
        potion.potionDescription = EditorGUILayout.TextArea(potion.potionDescription, GUILayout.Height(60));

        potion.potionType = (Potion.PotionType)EditorGUILayout.EnumPopup("Potion Type", potion.potionType);

        // Show only relevant fields
        if (potion.potionType == Potion.PotionType.Health || potion.potionType == Potion.PotionType.Both)
        {
            potion.restoreHP = EditorGUILayout.IntField("Restore HP", potion.restoreHP);
        }
        if (potion.potionType == Potion.PotionType.Mana || potion.potionType == Potion.PotionType.Both)
        {
            potion.restoreMP = EditorGUILayout.IntField("Restore MP", potion.restoreMP);
        }

        // Apply changes
        if (GUI.changed)
        {
            EditorUtility.SetDirty(potion);
        }
    }
}*/