using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ScoreboardDataSO", menuName = "ScriptableObjects/ScoreboardDataSO")]
public class ScoreboardDataSO : ScriptableObject
{
    [Serializable]
    public class ScoreboardField
    {
        public string name = "Empty";
        public float score = 0;
    }

    private const int MAX_SCOREBOARD_FIELDS = 5;

    [SerializeField] private ScoreboardField[] _scoreboardFields;

    public ScoreboardField GetScoreboardFieldAt(int index)
    {
        if (index < _scoreboardFields.Length)
        {
            return _scoreboardFields[index];
        }
        else
        {
            // Defaut ScoreboardField
            return new ScoreboardField();
        }
    }

    private void OnValidate()
    {
        if (_scoreboardFields.Length > MAX_SCOREBOARD_FIELDS)
        {
            Debug.LogError("There can only be a maximum of 5 scoreboard fields in " + name);
        }
    }
}
