using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClockTimeButton : MonoBehaviour
{
    public static ClockTimeButton instance;

    Button button;

    void Start()
    {
        button = GetComponent<Button>();
    }

    public void DisableButton()
    {
        button.interactable = false;
    }
}
