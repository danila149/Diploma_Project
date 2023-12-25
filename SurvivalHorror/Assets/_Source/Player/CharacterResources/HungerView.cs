using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HungerView : MonoBehaviour
{
    [SerializeField] private Slider foodBar;
    [SerializeField] private TextMeshProUGUI hungerText;

    public Slider FoodBar => foodBar;

    private void Start()
    {
        foodBar.onValueChanged.AddListener(ChangeText);
    }


    private void ChangeText(float value) =>
        hungerText.text = $"{foodBar.value}/{foodBar.maxValue}";
}
