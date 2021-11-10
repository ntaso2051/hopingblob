using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class PRoomsView : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private PRoomEntry roomEntryPrefab = default;

    private ScrollRect scrollRect;
    private Dictionary<string, PRoomEntry> activeEntries = new Dictionary<string, PRoomEntry>();
    private Stack<PRoomEntry> inactiveEntries = new Stack<PRoomEntry>();

    public string roomListStr = "";

    private void Awake()
    {
        scrollRect = GetComponent<ScrollRect>();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("OnRoomListUpdate");
        foreach (var info in roomList)
        {
            PRoomEntry entry;
            if (activeEntries.TryGetValue(info.Name, out entry))
            {
                if (!info.RemovedFromList)
                {
                    entry.gameObject.SetActive(true);
                }
                else
                {
                    activeEntries.Remove(info.Name);
                    entry.gameObject.SetActive(false);
                    inactiveEntries.Push(entry);
                }
            }
            else if (!info.RemovedFromList)
            {
                entry = (inactiveEntries.Count > 0) ? inactiveEntries.Pop().SetAsSibling() : Instantiate(roomEntryPrefab, scrollRect.content);
                entry.Activate(info.Name);
                activeEntries.Add(info.Name, entry);
            }
        }
    }
}
