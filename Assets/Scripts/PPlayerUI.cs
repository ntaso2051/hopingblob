using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PPlayerUI : MonoBehaviourPunCallbacks
{
    public Transform playerTransform;
    public Text playerName;

    private void Start()
    {
        playerName.text = $"{photonView.Owner.NickName}";
    }

    private float GetDistance()
    {
        return (transform.position - Camera.main.transform.position).magnitude;
    }

    private void LateUpdate()
    {
        if (playerTransform != null)
        {
            this.transform.rotation = Camera.main.transform.rotation;
            this.transform.localScale = Vector3.one * 0.1f * GetDistance();
        }
    }
}
