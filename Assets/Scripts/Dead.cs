using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dead : MonoBehaviour
{
    private float displayTime = 0.0f;
    private const float displayTimeMax = 2.0f;
    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.activeSelf)
        {
            displayTime += Time.deltaTime;
            if (displayTime > displayTimeMax)
            {
                this.gameObject.SetActive(false);
            }
        }
    }
}
