using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Button))]
public class PRoomEntry : MonoBehaviourPunCallbacks
{
    private RectTransform rectTransform;
    private Button button;
    private string roomName;

    [SerializeField] private Text roomNameText;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        button = GetComponent<Button>();
    }

    private void Start()
    {
        button.onClick.AddListener(() => PhotonNetwork.JoinRoom(roomName));
        button.onClick.AddListener(() => SoundManager.Instance.PlayButtonSound());
    }

    public void JoinRoom(string rn)
    {
        PhotonNetwork.JoinRoom(rn);
    }

    public PRoomEntry SetAsSibling()
    {
        rectTransform.SetAsLastSibling();
        return this;
    }

    public void Activate(string rn)
    {
        this.roomName = rn;
        roomNameText.text = rn;
        this.gameObject.SetActive(true);
    }
}
