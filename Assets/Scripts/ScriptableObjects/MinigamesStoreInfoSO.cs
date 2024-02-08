using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MinigamesStoreInfoSO", menuName = "ScriptableObjects/MinigamesStoreInfoSO")]
public class MinigamesStoreInfoSO : ScriptableObject
{
    [Serializable]
    public class MinigameStore
    {
        public Sprite icon;
        public string name;
        [TextAreaAttribute]
        public string description;
        public int cost;

    }

    public MinigameStore[] minigames;
}
