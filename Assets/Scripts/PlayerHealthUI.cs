using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public Slider healthSlider;

    private void Start()
    {
        if (playerHealth != null && healthSlider != null)
        {
            healthSlider.maxValue = playerHealth.maxHealth;
            healthSlider.value = playerHealth.currentHealth;
        }
    }

    private void Update()
    {
        if (playerHealth != null && healthSlider != null)
        {
            float targetHealth = playerHealth.GetActualHealth();
            
            
            if (healthSlider.value != targetHealth)
            {
                LeanTween.value(healthSlider.value, targetHealth, 0.2f)
                    .setOnUpdate((float value) => {
                        healthSlider.value = value;
                    })
                    .setEase(LeanTweenType.easeOutQuad); 
            }
        }
    }
}
