using UnityEngine;

public class DesktopUI : IconHolderSpace
{
    private KeyPosition[] _verticalKeyPositions;
    private RectTransform _rectTransform;

    private int _initialColumns;
    private float _initialSpacinY;
    private float _initialIconHeight;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _initialColumns = _columns;
        _initialSpacinY = _spacing.y;
        _initialIconHeight = _iconHeight;
    }

    public override void InitializeSpace()
    {
        AdjustVerticalSpace();

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
                Vector2 pos = initialPos + new Vector2(width * x, height * -y) * ComputerController.Instance.GetMainCanvas().scaleFactor;
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

    private void AdjustVerticalSpace()
    {
        float ySpacing = _initialSpacinY;
        int columns = _initialColumns;
        float iconHeight = _initialIconHeight;

        for (int i = 0; i < 10; i++)
        {
            float nextHeight = (columns + 1) * (ySpacing + _iconHeight);

            if (nextHeight < _rectTransform.rect.height)
            {
                columns++;
            }
            else
            {
                break;
            }
        }

        _columns = columns;
        
        for (int i = 0; i < 20; i++)
        {
            float nextHeight = (columns) * (ySpacing + 1 + iconHeight + 1);

            if (nextHeight < _rectTransform.rect.height)
            {
                ySpacing++;
                iconHeight++;
            }
            else
            {
                break;
            }
        }

        _spacing.y = ySpacing;
        _iconHeight = iconHeight;
    }

    protected override void FixAllIcons()
    {
        for (int i = 0; i < _iconContainer.childCount; i++)
        {
            if (_iconContainer.GetChild(i).gameObject != _iconTemplate)
            {
                DesktopIcon desktopIcon = _iconContainer.GetChild(i).GetComponent<DesktopIcon>();

                Vector2 pos = FindProperPosition(desktopIcon.transform.position);
           
                int index = FindProperIndex(pos);

                if (_iconPositions[index].IsOccupied)
                {
                    int verticalIndex = -1;
                    float smallestDifference = float.MaxValue;
                    foreach (KeyPosition keyPos in _verticalKeyPositions)
                    {
                        float difference = Mathf.Abs(keyPos.Y - pos.y);
                        if (difference < smallestDifference)
                        {
                            smallestDifference = difference;
                            verticalIndex = keyPos.Index;
                        }
                    }

                    float smallestDistance = float.MaxValue;
                    int bestIndex = -1;
                    for (int vIndex = verticalIndex; vIndex >= 0; vIndex--)
                    {
                        for (int j = verticalIndex; j < verticalIndex + _rows; j++)
                        {
                            float distance = Vector2.Distance(pos, _iconPositions[j].Position);
                            if (!_iconPositions[j].IsOccupied && distance < smallestDistance)
                            {
                                smallestDistance = distance;
                                bestIndex = j;
                            }
                        }

                        if (bestIndex != -1)
                        {
                            desktopIcon.FixIconPosition(_iconPositions[bestIndex].Position);
                            _iconPositions[bestIndex].IsOccupied = true;
                            break;
                        }
                    }
                }
                else
                {
                    desktopIcon.FixIconPosition(pos);
                    _iconPositions[index].IsOccupied = true;
                }
            }
        }
    }

    public override GameObject TryAddIcon(int applicationID, ApplicationIcon minigameIcon)
    {
        GameObject iconGO = Instantiate(_iconTemplate, _iconContainer);
        if (iconGO.TryGetComponent(out DesktopIcon desktopIcon))
        {
            desktopIcon.Init(GetAvailableStartingPosition(), applicationID, minigameIcon);
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
        float smallestDistance = float.MaxValue;
        foreach (KeyPosition keyPos in _verticalKeyPositions)
        {
            float difference = Mathf.Abs(keyPos.Y - iconPos.y);
            if (difference < smallestDistance)
            {
                smallestDistance = difference;
                bestVerticalIndex = keyPos.Index;
            }
        }

        smallestDistance = float.MaxValue;
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
        float smallestDistance = float.MaxValue;
        foreach (KeyPosition keyPos in _verticalKeyPositions)
        {
            float difference = Mathf.Abs(keyPos.Y - iconPos.y);
            if (difference < smallestDistance)
            {
                smallestDistance = difference;
                bestVerticalIndex = keyPos.Index;
            }
        }

        smallestDistance = float.MaxValue;
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

    public void SwapIconPositionStatus(GameObject icon, Vector2 pos)
    {
        int index1 = FindProperIndex(icon.transform.position);
        int index2 = FindProperIndex(pos);

        if (index1 != index2)
        {
            _iconPositions[index1].IsOccupied = true; // New pos is now OCCUPIED
            _iconPositions[index2].IsOccupied = false; // Previous pos is now FREE
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
