using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;


public class MainMenu : MonoBehaviourPunCallbacks
{
    [SerializeField] private InputField createInput;
    [SerializeField] private InputField joinInput;
    [SerializeField] private InputField NickNameInput;

    public void ChangeName()
    {
        PhotonNetwork.NickName = NickNameInput.text;
    }
    public void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(createInput.text, roomOptions);
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(joinInput.text);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Game");
    }
}
