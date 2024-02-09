using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MinigameWindow : Window
{
    [Header("Minigame")]

    [SerializeField] private RawImage _backgroundRawImage;
    [SerializeField] private TMP_Text _titleText;

    public void SetTitle(string title)
    {
        _titleText.SetText(title);
    }

    public void SetApplicationRenderTexture(RenderTexture renderTexture)
    {
        _backgroundRawImage.texture = renderTexture;
    }
}
