using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using Unity.Notifications.Android;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private InputField roomInputField;
    [SerializeField] private GameObject lobbyPanel;
    [SerializeField] private GameObject roomPanel;
    [SerializeField] private Text roomName;

    [SerializeField] private RoomItem roomItemPrefab;
    List<RoomItem> roomItemsList = new List<RoomItem>();
    [SerializeField] private Transform contentObject;

    [SerializeField] private float timeBetweenUpdates = 1.5f;
    float nextUpdateTime;

    List<PlayerItem> playerItemsList = new List<PlayerItem>();
    [SerializeField] private PlayerItem playerItemPrefab;
    [SerializeField] private Transform playerItemParent;

    [SerializeField] private GameObject playButton;
    private void Awake()
    {
        AndroidNotificationChannel channel= new AndroidNotificationChannel()
        {
            Name = "Кто-то вошел",
            Description = "Уведомление что кто-то вошел",
            Id = "Online",
            Importance = Importance.Default
        };

        AndroidNotificationCenter.RegisterNotificationChannel(channel);
    }
    private void Start()
    {
        PhotonNetwork.JoinLobby();

    }
    public void OnClickCreate()
    {
        if (roomInputField.text.Length >= 1)
        {
            PhotonNetwork.CreateRoom(roomInputField.text, new RoomOptions() { MaxPlayers = 2,BroadcastPropsChangeToAll=true});
        }
       
    }
    public override void OnJoinedRoom()
    {
        lobbyPanel.SetActive(false);
        roomPanel.SetActive(true);
        roomName.text = "Название комнаты: " + PhotonNetwork.CurrentRoom.Name;
        UpdatePlayerList();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if (Time.time >= nextUpdateTime)
        {
            UpdateRoomList(roomList);
            nextUpdateTime = Time.time + timeBetweenUpdates;
        }
    }

    void UpdateRoomList(List<RoomInfo> list)
    {
        foreach (RoomItem item in roomItemsList)
        {
            Destroy(item.gameObject);
        }

        roomItemsList.Clear();

        foreach (RoomInfo room in list)
        {
            RoomItem newRoom = Instantiate(roomItemPrefab, contentObject);
            newRoom.SetRoomName(room.Name);
            roomItemsList.Add(newRoom);
        }
    }

    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    public void OnClickLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom() 
    {
        roomPanel.SetActive(false);
        lobbyPanel.SetActive(true);
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();

    }
  
    private void UpdatePlayerList()
    {
        foreach(PlayerItem item in playerItemsList)
        {
            Destroy(item.gameObject);
        }
        playerItemsList.Clear();

        if (PhotonNetwork.CurrentRoom == null)
        {
            return;
        }

        foreach (KeyValuePair<int,Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            PlayerItem newPlayerItem = Instantiate(playerItemPrefab, playerItemParent);
            newPlayerItem.SetPlayerInfo(player.Value);
            if(player.Value == PhotonNetwork.LocalPlayer)
            {
                newPlayerItem.ApplyLocalChanges();
            }
            playerItemsList.Add(newPlayerItem);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerList();
  
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerList();
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount >=2)
        {
            playButton.SetActive(true);
        }
        else
        {
            playButton.SetActive(false);
        }

        AndroidNotification notification = new AndroidNotification()
        {
            Title = "Создана новая комната: " + PhotonNetwork.CurrentRoom.Name,
            Text = PhotonNetwork.LocalPlayer + " Ожидает тебя в игре - скорее заходи",
            FireTime = System.DateTime.Now.AddSeconds(5)
        };
        AndroidNotificationCenter.SendNotification(notification, channelId: "Online");
    }
    
    public void OnClickPlayButton()
    {
        PhotonNetwork.LoadLevel("Game");
    }
}