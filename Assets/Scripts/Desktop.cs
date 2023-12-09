using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using VInspector;
using static UnityEditor.PlayerSettings;

public class Desktop : MonoBehaviour
{
    private const int ROW_COUNT = 15;

    [SerializeField] private Transform _posContainer;
    [SerializeField] private Transform _iconContainer;
    [SerializeField] private GameObject _iconTemplate;

    private KeyPosition[] _verticalKeyPositions;
    private IconPosition[] _iconPositions;


    private static DesktopIcon _movingIcon;
    public static Desktop Instance { get; private set; }
    public bool IsMovingIcon { get; internal set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        DOVirtual.DelayedCall(0.5f, () =>
        {
            _verticalKeyPositions = new KeyPosition[(_posContainer.childCount / ROW_COUNT)];
            _iconPositions = new IconPosition[_posContainer.childCount];

            int iconIndex = 0;
            int verticalKeyPosIndex = 0;
            foreach (Transform posTransform in _posContainer)
            {
                _iconPositions[iconIndex] = posTransform.GetComponent<IconPosition>();
                if (iconIndex % ROW_COUNT == 0 && iconIndex != _posContainer.childCount)
                {
                    _verticalKeyPositions[verticalKeyPosIndex] = new KeyPosition(posTransform.position.y, iconIndex);
                    verticalKeyPosIndex++;
                }
                iconIndex++;
            }

            _iconTemplate.SetActive(false);
        });
    }

    [Button]
    public void AddIcon()
    {
        GameObject iconGO = Instantiate(_iconTemplate, _iconContainer);
        if (iconGO.TryGetComponent(out DesktopIcon component))
        {
            component.Init(GetAvailableStartingPosition());
        }
        else
        {
            Debug.LogError("Icon template does not have DesktopIcon component");
        }
    }

    public void RemoveIcon(GameObject icon)
    {
        SetIconPositionStatus(icon, false);
        Destroy(icon);
    }

    private Vector2 GetAvailableStartingPosition()
    {
        foreach (IconPosition iconPos in _iconPositions)
        {
            if (!iconPos.IsOccupied)
            {
                iconPos.IsOccupied = true;
                return iconPos.transform.position;
            }
        }

        return Vector2.zero;
    }

    public int FindProperIndex(Vector2 iconPos)
    {
        int bestVerticalIndex = -1;
        float smallestDifference = float.MaxValue;
        foreach (KeyPosition keyPos in _verticalKeyPositions)
        {
            float difference = Mathf.Abs(keyPos.Y - iconPos.y);
            if (difference < smallestDifference)
            {
                smallestDifference = difference;
                bestVerticalIndex = keyPos.Index;
            }
        }

        float smallestDistance = float.MaxValue;
        int bestIndex = -1;
        for (int i = bestVerticalIndex; i <= bestVerticalIndex + ROW_COUNT - 1; i++)
        {
            float distance = Vector2.Distance(iconPos, _posContainer.GetChild(i).position);
            if (distance < smallestDistance)
            {
                smallestDistance = distance;
                bestIndex = i;
            }
        }

        return bestIndex;
    }

    public void SetIconPositionStatus(GameObject icon, bool setter) => _iconPositions[(FindProperIndex(icon.transform.position))].IsOccupied = setter;
    public void SetIconPositionStatus(Vector2 pos, bool setter) => _iconPositions[(FindProperIndex(pos))].IsOccupied = setter;

    public Vector2 FindProperPosition(Vector2 iconPos)
    {
        int bestVerticalIndex = -1;
        float smallestDifference = float.MaxValue;
        foreach (KeyPosition keyPos in _verticalKeyPositions)
        {
            float difference = Mathf.Abs(keyPos.Y - iconPos.y);
            if (difference < smallestDifference)
            {
                smallestDifference = difference;
                bestVerticalIndex = keyPos.Index;
            }
        }

        float smallestDistance = float.MaxValue;
        int bestIndex = -1;
        for (int i = bestVerticalIndex; i <= bestVerticalIndex + ROW_COUNT - 1; i++)
        {
            float distance = Vector2.Distance(iconPos, _posContainer.GetChild(i).position);
            if (distance < smallestDistance)
            {
                smallestDistance = distance;
                bestIndex = i;
            }
        }

        return _posContainer.GetChild(bestIndex).position;
    }

    public DesktopIcon GetMovingIcon() => _movingIcon;

    public void SetMovingIcon(DesktopIcon icon) => _movingIcon = icon;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        foreach (Transform posTransform in _posContainer)
        {
            Gizmos.DrawCube(posTransform.position, new Vector2(_iconTemplate.GetComponent<RectTransform>().rect.width / 2, _iconTemplate.GetComponent<RectTransform>().rect.height / 2));
        }
    }

    private struct KeyPosition
    {
        public float Y {  get; set; }
        public int Index { get; set; }

        public KeyPosition(float y, int index)
        {
            Y = y;
            Index = index;
        }    
    }
}
