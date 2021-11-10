using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class PLog : MonoBehaviourPunCallbacks
{
    public Text logs;
    // Start is called before the first frame update
    void Start()
    {
        UpdateLogs();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player player)
    {
        UpdateLogs();
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
        UpdateLogs();
    }

    public void UpdateLogs()
    {
        Player[] plist = PhotonNetwork.PlayerList;
        string logtxt = "";
        foreach (Player p in plist)
        {
            string ready = "ready?";
            if (p.CustomProperties["Ready"] != null)
            {
                ready = (bool)(p.CustomProperties["Ready"]) ? "ok!" : "ready?";
            }
            logtxt += $"{p.NickName}: {ready}\n";
        }
        logs.text = logtxt;
    }
}
