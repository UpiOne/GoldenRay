using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class GameOver : MonoBehaviour
{
    [SerializeField] private Text scoreDisplay;
    [SerializeField] private GameObject restartButton;
    [SerializeField] private GameObject waitingText;
    [SerializeField] private GameObject Spawner;
    PhotonView view;

    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();
        scoreDisplay.text = FindObjectOfType<Score>().score.ToString();

        if (PhotonNetwork.IsMasterClient == false)
        {
            restartButton.SetActive(false);
            restartButton.SetActive(false);
            waitingText.SetActive(true);
        }
    }

    public void OnClickRestart()
    {
        view.RPC("Restart", RpcTarget.All);
    }

    [PunRPC]
    void Restart()
    {
        PhotonNetwork.LoadLevel("Game");
    }

}
