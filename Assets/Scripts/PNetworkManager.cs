using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using ExitGames.Client.Photon;

public class PNetworkManager : MonoBehaviourPunCallbacks
{
    public static PNetworkManager instance;
    public static PNetworkManager Instance
    {
        get
        {
            return instance;
        }
    }
    string gameVersion = "1.0";

    public GameObject PlayerPrefab;
    public string playerName = "";
    public GameObject localPlayer;

    public int canGameStart = 0;
    public bool isGameStarted = false;
    public int deadPlayerNum = 0;
    public bool isDead = false;

    Vector3[] initPoss = new Vector3[]
    {
        new Vector3(5.0f, 5.0f, 5.0f),
        new Vector3(-5.0f, 5.0f, 5.0f),
        new Vector3(5.0f, 5.0f, -5.0f),
        new Vector3(-5.0f, 5.0f, -5.0f)
    };


    RoomOptions roomOptions = new RoomOptions
    {
        MaxPlayers = 4,
        IsOpen = true,
        IsVisible = true
    };


    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        instance = this;
    }

    private void Start()
    {
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();
    }

    public void CreatePlayer()
    {
        int id = PhotonNetwork.PlayerList.Length - 1;
        Vector3 dir = Vector3.zero - initPoss[id % 4];
        dir.y = 0;
        Quaternion rot = Quaternion.LookRotation(dir, new Vector3(0f, 1.0f, 0f));
        localPlayer = PhotonNetwork.Instantiate(PlayerPrefab.name, initPoss[id % 4], rot);
        // localPlayer = localPlayer.transform.GetChild(5).gameObject;
        PlayerCamera.Instance.SetUpPlayerCamera();
        var hashtable = new ExitGames.Client.Photon.Hashtable();
        hashtable["Ready"] = false;
        PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
        var rhashtable = new ExitGames.Client.Photon.Hashtable();
        rhashtable["playerNum"] = 4;
        PhotonNetwork.CurrentRoom.SetCustomProperties(rhashtable);
    }

    public void SetName(string name)
    {
        this.playerName = name;
        PhotonNetwork.NickName = this.playerName;
    }

    public void CreateRoom()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.CreateRoom(PhotonNetwork.NickName + "'s Room", roomOptions, TypedLobby.Default);
        }
        else
        {
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public void JoinRoom()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("OnConnectedToMaster");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log("OnJoinedLobby");
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Joined Room");
        CreatePlayer();
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player player)
    {
        Debug.Log(player.NickName + " is joined.");
    }
    public override void OnPlayerLeftRoom(Photon.Realtime.Player player)
    {
        Debug.Log(player.NickName + " is left.");
        canGameStart--;
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
        if (changedProps["Ready"] != null && (bool)changedProps["Ready"])
        {
            canGameStart++;
        }
        if (changedProps["isDead"] != null)
        {
            Debug.Log("dead");
            this.deadPlayerNum++;
        }
    }

    private void Update()
    {
        if (deadPlayerNum == 3)
        {
            PUiManager.Instance.ActivateResultUI();
        }
        if (isDead)
        {
            PUiManager.Instance.ActivateDeadUI();
        }
    }
}
