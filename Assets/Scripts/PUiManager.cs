using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class PUiManager : MonoBehaviourPunCallbacks
{
    public static PUiManager instance;
    public static PUiManager Instance
    {
        get
        {
            return instance;
        }
    }
    public GameObject lobbyUI;
    public GameObject playerUI;
    public GameObject UIPool;
    public GameObject LogsUI;
    public GameObject ReadyButton;
    public GameObject GameStartButton;
    public GameObject TitleUI;
    public GameObject Background;
    public GameObject errorUI;
    public GameObject deadUI;
    public GameObject resultUI;
    public GameObject EndGameUI;
    public GameObject HowToPlayUI;
    public GameObject GameStartUI;

    public ErrorMessage errMsg;
    public Result result;

    [SerializeField]
    private float gameOverToResultTime;
    [SerializeField]
    private float gameStartTime;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        ReadyButton.SetActive(false);
        GameStartButton.SetActive(false);
        errorUI.SetActive(false);
        resultUI.SetActive(false);
    }

    private void Update()
    {
        if (this.EndGameUI.activeSelf)
        {
            gameOverToResultTime -= Time.deltaTime;
            if (gameOverToResultTime < 0.0f)
            {
                this.EndGameUI.SetActive(false);
                this.resultUI.SetActive(true);
            }
        }

        if (PNetworkManager.Instance.isGameStarted && gameStartTime > 0.0f)
        {
            this.GameStartUI.SetActive(true);
        }
        if (this.GameStartUI.activeSelf)
        {
            this.LogsUI.SetActive(false);
            gameStartTime -= Time.deltaTime;
            if (gameStartTime < 0.0f)
            {
                this.GameStartUI.SetActive(false);
            }
        }
    }

    public void ActivateDeadUI()
    {
        this.deadUI.SetActive(true);
    }

    public void ActivateResultUI()
    {
        this.EndGameUI.SetActive(true);
    }

    public void OnClickHowToPlay()
    {
        this.HowToPlayUI.SetActive(!this.HowToPlayUI.activeSelf);
    }


    public void OnClickReadyButton()
    {
        var hashtable = new ExitGames.Client.Photon.Hashtable();
        hashtable["Ready"] = true;
        PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
        ReadyButton.SetActive(false);
        if (PhotonNetwork.IsMasterClient)
        {
            GameStartButton.SetActive(true);
        }
    }

    public void OnClickTitleButton()
    {
        TitleUI.SetActive(false);
    }

    public void OnClickGameStartButton()
    {
        if (PNetworkManager.Instance.canGameStart == 4)
        {
            SoundManager.Instance.PlayButtonSound();
            PNetworkManager.Instance.isGameStarted = true;
            var hashtable = new ExitGames.Client.Photon.Hashtable();
            hashtable["isGameStarted"] = true;
            PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
            GameStartButton.SetActive(false);
            LogsUI.SetActive(false);
        }
        else
        {
            errorUI.SetActive(true);
            errMsg.SetErrorMessage("not enough people");
        }
    }

    public void OnNameChange(string name)
    {
        PNetworkManager.Instance.SetName(name);
    }

    public override void OnConnectedToMaster()
    {
        lobbyUI.SetActive(true);
        TitleUI.SetActive(true);
    }
    public override void OnJoinedRoom()
    {
        lobbyUI.SetActive(false);
        Background.SetActive(false);
        ReadyButton.SetActive(true);
        LogsUI.SetActive(true);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (changedProps["Rank"] != null)
        {
            int rank = (int)changedProps["Rank"];
            Debug.Log($"{targetPlayer.NickName}: {changedProps["Rank"]}");
            result.resultTexts[rank - 1].text = $"{rank}{result.StNdRdTh[rank - 1]}: {targetPlayer.NickName}";
        }
    }
    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        base.OnRoomPropertiesUpdate(propertiesThatChanged);
        if (propertiesThatChanged["isGameStarted"] != null)
        {
            if ((bool)propertiesThatChanged["isGameStarted"])
            {
                PNetworkManager.Instance.isGameStarted = true;
                this.LogsUI.SetActive(false);
            }
        }
    }
}
