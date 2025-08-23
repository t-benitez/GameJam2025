using UnityEngine;

public class DeathCanvas : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
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
            ShowDeathCanvas();
        }
    }

    private void ShowDeathCanvas()
    {
        if (canvasGroup != null)
        {
            LeanTween.alphaCanvas(canvasGroup, 1f, 0.25f)
            .setEase(LeanTweenType.easeOutQuad)
            .setOnComplete(() => {
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            });
        }
    }
}
