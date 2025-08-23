using UnityEngine;

public class DeathCanvas : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private CanvasGroup gameCanvasGroup;
    private void OnEnable()
    {
        PlayerDeathNotifier.OnPlayerDeath += HandlePlayerDeath;
    }

    private void OnDisable()
    {
        PlayerDeathNotifier.OnPlayerDeath -= HandlePlayerDeath;
    }
    private void Awake()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    private void HandlePlayerDeath(bool isDead)
    {
        if (isDead)
        {
            StartCoroutine(HideGameCanvas());
            ShowDeathCanvas();
        }
    }

    private void ShowDeathCanvas()
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
    private System.Collections.IEnumerator HideGameCanvas()
    {
        if (gameCanvasGroup != null)
        {
            LeanTween.alphaCanvas(gameCanvasGroup, 0f, 0.25f)
            .setEase(LeanTweenType.easeOutQuad)
            .setOnComplete(() =>
            {
                gameCanvasGroup.interactable = false;
                gameCanvasGroup.blocksRaycasts = false;
            });
        }
        yield return null;
    }
}
