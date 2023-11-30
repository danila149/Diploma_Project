using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using GooglyEyesGames.FusionBites;
using UnityEngine.UI;

public class NameEntry : MonoBehaviour
{
    [SerializeField] GameObject canvas;
    [SerializeField] TMP_InputField nameInputFild;
    [SerializeField] Button submitButton;

    public void SubmitName()
    {
        FusionConnector.instance.OnConnectedToRunner(nameInputFild.text);
        canvas.SetActive(false);
    }

    public void ActiveButton()
    {
        submitButton.interactable = true;
    }
}
