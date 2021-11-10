using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ErrorMessage : MonoBehaviour
{
    public Text errorMessage;

    private float displayTime = 0.0f;
    private const float displayTimeMax = 0.5f;

    public void SetErrorMessage(string msg)
    {
        SoundManager.Instance.PlayErrorSound();
        this.errorMessage.text = msg;
        displayTime = 0.0f;
    }
    private void Update()
    {
        displayTime += Time.deltaTime;
        if (displayTime > displayTimeMax)
        {
            this.gameObject.SetActive(false);
        }
    }

}
