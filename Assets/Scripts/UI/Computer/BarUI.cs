using System.Collections.Generic;
using UnityEngine;

public class BarUI : IconHolderSpace
{
    private static BarIcon _movingIcon;

    private Pool _iconPool;
    private List<BarIcon> _barIconsList;

    public bool IsMovingIcon { get; set; }


    private void Awake()
    {
        _iconPool = GetComponent<Pool>();
        _barIconsList = new List<BarIcon>();
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
                Vector2 pos = initialPos + new Vector2(width * x, height * -y) * ComputerControllerUI.Instance.GetMainCanvas().scaleFactor;
                _iconPositions[y * (int)width + x] = new IconPosition(pos, false);
            }
        }

        _iconTemplate.SetActive(false);
    }

    public override GameObject AddIcon()
    {
        // Reuse or create new icon
        if (!_iconPool.TryDequeue(out GameObject iconGO))
        {
            iconGO = _iconPool.CreateGameObject();
        }

        if (iconGO.TryGetComponent(out BarIcon barIcon))
        {
            _barIconsList.Add(barIcon);
            iconGO.SetActive(true);
            //barIcon.Init(GetAvailableStartingPosition());
            barIcon.Init(GetAvailableStartingIndex());
            return iconGO;
        }
        else
        {
            Debug.LogError("Icon template does not have BarIcon component");
            return null;
        }
    }

    public void FixPositionsAndRemoveIcon(BarIcon icon)
    {
        for (int i = 0; i < _barIconsList.Count; i++)
        {
            if (_barIconsList[i].Index > icon.Index)
            {
                // Move icon to a free position
                _barIconsList[i].FixIconPosition(GetPosition(--_barIconsList[i].Index));
            }
        }

        // Now the last position is free
        _iconPositions[_barIconsList.Count - 1].IsOccupied = false;

        // Remove icon
        _iconPool.Enqueue(icon.gameObject);
        _barIconsList.Remove(icon);
    }

    public override int FindProperIndex(Vector2 iconPos)
    {
        float smallestDistance = float.MaxValue;
        int bestIndex = -1;
        for (int i = 0; i < _iconPositions.Length; i++)
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
