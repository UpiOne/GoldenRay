using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public int score = 0;
    [SerializeField] private Text scoreDisplay;

    PhotonView view;

    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();
    }
    public void AddScore()
    {
        view.RPC("AddScoreRPC", RpcTarget.All); ;
    }

    [PunRPC]
    void AddScoreRPC()
    {
        score++;
        scoreDisplay.text = score.ToString();
    }

}
