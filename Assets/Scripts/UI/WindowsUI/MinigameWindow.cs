using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MinigameWindow : Window
{
    [Header("Minigame")]

    [SerializeField] private RawImage _backgroundRawImage;
    [SerializeField] private TMP_Text _titleText;

    private bool _isRenderTexturePrepared;

    private void OnDisable()
    {
        _isRenderTexturePrepared = false;
    }

    public void SetTitle(string title)
    {
        _titleText.SetText(title);
    }

    public void SetApplicationRenderTexture(RenderTexture renderTexture)
    {
        _backgroundRawImage.texture = renderTexture;
        _isRenderTexturePrepared = true;
    }

    public bool IsRenderTexturePrepared()
    {
        return _isRenderTexturePrepared;
    }

    public RectTransform GetRawImageRectTransform()
    {
        return _backgroundRawImage.GetComponent<RectTransform>();
    }
}
