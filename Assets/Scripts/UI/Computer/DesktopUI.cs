using UnityEngine;

public class DesktopUI : IconHolderSpace
{
    private KeyPosition[] _verticalKeyPositions;

    public override void InitializeSpace()
    {
        _iconPositions = new IconPosition[_rows * _columns];
        _verticalKeyPositions = new KeyPosition[_iconPositions.Length / _rows];

        Vector2 initialPos = _initialPosTranform.position /** GameController.Instance.GetMainCanvas().scaleFactor*/;
        float width = _iconWidth + _spacing.x;
        float height = _iconHeight + _spacing.y;

        int verticalKeyPosIndex = 0;
        for (int y = 0; y < _columns; y++)
        {
            for (int x = 0; x < _rows; x++)
            {
                Vector2 pos = initialPos + new Vector2(width * x, height * -y) * ComputerControllerUI.Instance.GetMainCanvas().scaleFactor;
                int index = y * _rows + x;
                _iconPositions[index] = new IconPosition(pos, false);

                if (index % _rows == 0)
                {
                    _verticalKeyPositions[verticalKeyPosIndex] = new KeyPosition(_iconPositions[index].Position.y, index);
                    verticalKeyPosIndex++;
                }
            }
        }

        _iconTemplate.SetActive(false);
    }

    public override GameObject AddIcon(int applicationID)
    {
        GameObject iconGO = Instantiate(_iconTemplate, _iconContainer);
        if (iconGO.TryGetComponent(out DesktopIcon desktopIcon))
        {
            desktopIcon.Init(GetAvailableStartingPosition(), applicationID);
            return iconGO;
        }
        else
        {
            Debug.LogError("Icon template does not have DesktopIcon component");
            return null;
        }
    }

    public override int FindProperIndex(Vector2 iconPos)
    {
        int bestVerticalIndex = -1;
        float smallestDifference = float.MaxValue;
        foreach (KeyPosition keyPos in _verticalKeyPositions)
        {
            float difference = Mathf.Abs((keyPos.Y) - iconPos.y);
            if (difference < smallestDifference)
            {
                smallestDifference = difference;
                bestVerticalIndex = keyPos.Index;
            }
        }

        float smallestDistance = float.MaxValue;
        int bestIndex = -1;
        for (int i = bestVerticalIndex; i < bestVerticalIndex + _rows; i++)
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

    public Vector2 FindProperPosition(Vector2 iconPos)
    {
        int bestVerticalIndex = -1;
        float smallestDifference = float.MaxValue;
        foreach (KeyPosition keyPos in _verticalKeyPositions)
        {
            float difference = Mathf.Abs((keyPos.Y - iconPos.y));
            if (difference < smallestDifference)
            {
                smallestDifference = difference;
                bestVerticalIndex = keyPos.Index;
            }
        }

        float smallestDistance = float.MaxValue;
        int bestIndex = -1;
        for (int i = bestVerticalIndex; i < bestVerticalIndex + _rows; i++)
        {
            float distance = Vector2.Distance(iconPos, _iconPositions[i].Position);
            if (distance < smallestDistance)
            {
                smallestDistance = distance;
                bestIndex = i;
            }
        }

        return _iconPositions[bestIndex].Position;
    }

    // Previous position was occupied, new position was free
    public void SwapIconPositionStatus(GameObject icon, Vector2 pos)
    {
        int index1 = (FindProperIndex(icon.transform.position));
        int index2 = (FindProperIndex(pos));

        if (index1 != index2)
        {
            _iconPositions[index1].IsOccupied = true; // New pos is occupied
            _iconPositions[index2].IsOccupied = false; // Previous pos is NOT occupied
        }
    }

    public bool IsClosestPositionFree(Vector2 position)
    {
        return !_iconPositions[FindProperIndex(position)].IsOccupied;
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
