using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Health : MonoBehaviour
{

    [SerializeField] private int healph = 10;
  
    PhotonView view;

    [SerializeField] private Text healthDisplay;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private GameObject spawner;

    public void TakeDamage()
    {
        view.RPC("TakeDamageRPC", RpcTarget.All); ;
    }

    [PunRPC]
    void TakeDamageRPC()
    {
        healph--;

        if(healph <= 0)
        {
            gameOver.SetActive(true);
            spawner.SetActive(false);
        }
        healthDisplay.text = healph.ToString();
    }
    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();
    }
}
