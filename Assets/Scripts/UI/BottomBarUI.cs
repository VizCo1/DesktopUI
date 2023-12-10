using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;
using static UnityEditor.PlayerSettings;

public class BottomBarUI : IconHolderSpace
{
    private static BarIcon _movingIcon;
    public static BottomBarUI Instance { get; private set; }

    public bool IsMovingIcon { get; set; }

    private void Awake()
    {
        Instance = this;
    }

    protected override void InitializeSpace()
    {
        _iconPositions = new IconPosition[_rows * _columns];

        Vector2 initialPos = _initialPosTranform.position;
        float width = _iconWidth + _spacing.x;
        float height = _iconHeight + _spacing.y;

        for (int y = 0; y < _columns; y++)
        {
            for (int x = 0; x < _rows; x++)
            {
                Vector2 pos = initialPos + new Vector2(width * x, height * -y) * GameController.Instance.GetMainCanvas().scaleFactor;
                _iconPositions[y * (int)width + x] = new IconPosition(pos, false);
            }
        }

        _iconTemplate.SetActive(false);
    }

    [Button]
    public override void AddIcon()
    {
        GameObject iconGO = Instantiate(_iconTemplate, _iconContainer);
        if (iconGO.TryGetComponent(out BarIcon component))
        {
            component.Init(GetAvailableStartingPosition());
        }
        else
        {
            Debug.LogError("Icon template does not have BarIcon component");
        }
    }

    public override void RemoveIcon(GameObject icon)
    {
        // When removing an icon all the other icons on the right side will move one position to the left
        SetIconPositionStatus(icon, false);
        Destroy(icon);
    }

    public override int FindProperIndex(Vector2 iconPos)
    {
        float smallestDistance = float.MaxValue;
        int bestIndex = -1;
        for (int i = 0; i <= _iconPositions.Length; i++)
        {
            float distance = Vector2.Distance(iconPos, _iconPositions[i].Position);
            if (distance < smallestDistance)
            {
                smallestDistance = distance;
                bestIndex = i;
            }
        }

        return bestIndex;
    }

    public BarIcon GetMovingIcon() => _movingIcon;
    public void SetMovingIcon(BarIcon icon) => _movingIcon = icon;
}
