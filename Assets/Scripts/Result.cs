using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class Result : MonoBehaviourPunCallbacks
{
    public Text[] resultTexts = new Text[4];
    public string[] StNdRdTh = new string[4]
    {
        "st",
        "nd",
        "rd",
        "th"
    };
}
