using UnityEngine;

public class GenericCanvas : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    public void OnShowCanvasClick()
    {
        ShowCanvas();
    }

    public void OnGoBackClick()
    {
        HideCanvas();
    }
    private void Awake()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    

    private void ShowCanvas()
    {
        if (canvasGroup != null)
        {
            LeanTween.alphaCanvas(canvasGroup, 1f, 0.25f)
            .setEase(LeanTweenType.easeOutQuad)
            .setOnComplete(() =>
            {
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            });
        }
    }
    private void HideCanvas()
    {
        if (canvasGroup != null)
        {
            LeanTween.alphaCanvas(canvasGroup, 0f, 0.25f)
            .setEase(LeanTweenType.easeOutQuad)
            .setOnComplete(() =>
            {
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
            });
        }
    }
}
