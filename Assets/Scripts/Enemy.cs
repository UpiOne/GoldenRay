using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private GameObject deathFX;
    PlayerControler[] players;
    PlayerControler nearestPlayer;
    PhotonView view;
    Score score;
    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();
        players = FindObjectsOfType<PlayerControler>();
        score = FindObjectOfType<Score>();
    }
    // Update is called once per frame
    void Update()
    {
        float distanceOne = Vector2.Distance(transform.position, players[0].transform.position);
        float distanceTwo = Vector2.Distance(transform.position, players[1].transform.position);
      

        if (distanceOne < distanceTwo)
        {
            nearestPlayer = players[0];
        }
        else
        {
            nearestPlayer = players[1];
        }
      
        if (nearestPlayer != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, nearestPlayer.transform.position,speed*Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (collision.tag == "GoldenRay")
            {
                score.AddScore();
                view.RPC("SpawnParticle", RpcTarget.All);
                PhotonNetwork.Destroy(this.gameObject);
            }
        }
    }

    [PunRPC]
    void SpawnParticle()
    {
        Instantiate(deathFX, transform.position, Quaternion.identity);
    }
}
