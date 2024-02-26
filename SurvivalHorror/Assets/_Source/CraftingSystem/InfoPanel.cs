using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour
{
    [SerializeField] private Button btn;
    [SerializeField] private Image img;
    [SerializeField] private TextMeshProUGUI equipmentName;
    [SerializeField] private TextMeshProUGUI description;

    public Button Button => btn;

    public void SetInfo(Sprite icon, string equipmentName, string description)
    {
        img.sprite = icon;
        this.equipmentName.text = equipmentName;
        this.description.text = description;
    }


    public void Activate(bool active)
    {
        Image btnImg = btn.GetComponent<Image>();
        if (active)
        {
            btnImg.color = Color.white;
            btn.interactable = true;
        }
        else
        {
            btnImg.color = Color.gray;
            btn.interactable = false;
        }
    }
}
