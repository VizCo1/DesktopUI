using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

public class BottomBar : MonoBehaviour
{
    [SerializeField] private Transform _posContainer;
    [SerializeField] private Transform _iconContainer;
    [SerializeField] private GameObject _iconTemplate;

    private IconPosition[] _iconPositions;

    public bool IsMovingIcon { get; set; }

    private static BarIcon _movingIcon;
    public static BottomBar Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        DOVirtual.DelayedCall(0.5f, () =>
        {
            _iconPositions = new IconPosition[_posContainer.childCount];

            int i = 0;
            foreach (Transform posTransform in _posContainer)
            {
                _iconPositions[i++] = posTransform.GetComponent<IconPosition>();
            }

            _iconTemplate.SetActive(false);
        });      
    }

    [Button]
    public void AddIcon()
    {
        GameObject iconGO = Instantiate(_iconTemplate, _iconContainer);
        iconGO.GetComponent<BarIcon>().Init(GetAvailableStartingPosition());
    }

    public void RemoveIcon(GameObject icon)
    {
        // When removing an icon all the other icons on the right side will move one position to the left
        SetIconPositionStatus(icon, false);
        Destroy(icon);
    }

    public int FindProperIndex(Vector2 iconPos)
    {
        float smallestDistance = float.MaxValue;
        int bestIndex = -1;
        for (int i = 0; i <= _iconPositions.Length; i++)
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

    public void SetMovingIcon(BarIcon bottomIcon) => _movingIcon = bottomIcon;
    public BarIcon GetMovingIcon() => _movingIcon;
}
