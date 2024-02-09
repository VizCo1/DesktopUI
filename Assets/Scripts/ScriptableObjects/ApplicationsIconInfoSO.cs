using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ApplicationsIconInfoSO", menuName = "ScriptableObjects/ApplicationsIconInfoSO")]
public class ApplicationsIconInfoSO : ScriptableObject
{
    public ApplicationIcon[] applications;
}

[Serializable]
public class ApplicationIcon
{
    public Sprite icon;
    public string name;
}