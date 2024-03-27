using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropSplitWindow : MonoBehaviour, IPointerExitHandler
{
    [SerializeField] private Button dropBtn;
    [SerializeField] private Button splitBtn;
    [SerializeField] private Button openSplitMenuBtn;
    [SerializeField] private Slider splitSlider;
    [SerializeField] private TextMeshProUGUI maxAmount;
    [SerializeField] private TextMeshProUGUI currentAmount;
    [SerializeField] private GameObject splitMenu;

    public Button DropBtn => dropBtn;
    public Button SplitBtn => splitBtn;
    public float CurrentValue => splitSlider.value;

    private void Start()
    {
        splitSlider.onValueChanged.AddListener(ShowCurrentChoosenAmount);
    }

    public void TurnOffSplitMenu() =>
        openSplitMenuBtn.interactable = false;

    public void TurnOnSplitMenu() =>
        openSplitMenuBtn.interactable = true;

    public void SetSliderMaxValue(int value)
    {
        splitSlider.maxValue = value;
        maxAmount.text = $"{value}";
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        gameObject.SetActive(false);
        openSplitMenuBtn.interactable = true;
        splitMenu.SetActive(false);
        dropBtn.onClick.RemoveAllListeners();
        splitBtn.onClick.RemoveAllListeners();
    }

    private void ShowCurrentChoosenAmount(float amount) =>
        currentAmount.text = $"{(int)amount}";
}
