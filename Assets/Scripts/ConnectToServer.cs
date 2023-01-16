using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    [SerializeField] private InputField usernameInput;
    [SerializeField] private Text buttonText;
   

    public void OnClickConnect()
    {
        if(usernameInput.text.Length >= 1)
        {
            PhotonNetwork.NickName = usernameInput.text;
            buttonText.text = "Подключение...";
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.ConnectUsingSettings();
        }
    }
/*    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
*/
    public override void OnConnectedToMaster()
    {
        SceneManager.LoadScene("Lobby");
    }
}
