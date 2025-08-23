using UnityEngine;
using TMPro;
public class WaveTextManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI waveTextCanvas;
    [SerializeField] private TextMeshProUGUI waveText;


    private void OnEnable()
    {
        WaveObserver.OnNewWave += UpdateWaveText;
    }
    private void OnDisable()
    {
        WaveObserver.OnNewWave -= UpdateWaveText;
    }
    private void UpdateWaveText(int waveNumber)
    {
        if (waveText != null)
        {
            waveText.text = "Moriste en la Wave: " + waveNumber.ToString();
            waveTextCanvas.text = "Wave: " + waveNumber.ToString();
        }
    }
}
