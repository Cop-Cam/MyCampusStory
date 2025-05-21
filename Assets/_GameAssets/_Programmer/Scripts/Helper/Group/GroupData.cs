using UnityEngine;

/// <summary>
/// ScriptableObject for grouping or sorting
/// </summary>
[CreateAssetMenu(fileName = "Name_GroupData", menuName = "ScriptableObjects/GroupData")]
public class GroupData : ScriptableObject
{
    public string GroupName;
}