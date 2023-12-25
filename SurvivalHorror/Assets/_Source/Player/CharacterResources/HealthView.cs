using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthView : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private TextMeshProUGUI healthText;

    public Slider HealthBar => healthBar;

    private void Start()
    {
        healthBar.onValueChanged.AddListener(ChangeText);
    }


    private void ChangeText(float value) =>
        healthText.text = $"{healthBar.value}/{healthBar.maxValue}";
}
