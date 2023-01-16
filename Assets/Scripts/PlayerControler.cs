using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class PlayerControler : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float X, Y;
    [SerializeField] private bool isf = true;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashTime;
    [SerializeField] private float minX, maxX, minY, maxY;

    [SerializeField] private Text nameDisplay;
    [SerializeField] private Joystick joystick;

    PhotonView view;
    Rigidbody2D rb;
    float resetSpeed;
    Health healthScript;
    LineRenderer rend;
 
    // Start is called before the first frame update
    void Start()
    {
        resetSpeed = speed;
        rend = FindObjectOfType<LineRenderer>();
        healthScript = FindObjectOfType<Health>();
        rb = GetComponent<Rigidbody2D>();
        view = GetComponent<PhotonView>();
        joystick = GameObject.FindGameObjectWithTag("Joystick").GetComponent<Joystick>();

        if (view.IsMine)
        {
            nameDisplay.text = PhotonNetwork.NickName;
        }
        else
        {
            nameDisplay.text = view.Owner.NickName;
        }
    }
    // Update is called once per frame
    void Update()
    {
        X = joystick.Horizontal * speed;
        Y = joystick.Vertical * speed;

        if (view.IsMine)
        {
            rb.velocity = new Vector2(X, Y);
            rend.SetPosition(0, transform.position);
            Wrap();
        }
        else
        {
            rend.SetPosition(1, transform.position);
        }
    }
    private void Wrap()
    {
        if(transform.position.x < minX)
        {
            transform.position = new Vector2(maxX, transform.position.y);
        }
        if(transform.position.x > maxX)
        {
            transform.position = new Vector2(minX, transform.position.y);
        }
        if (transform.position.y < minY)
        {
            transform.position = new Vector2(transform.position.x,maxY);
        }
        if (transform.position.y > maxY)
        {
            transform.position = new Vector2(transform.position.x,minY);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (view.IsMine)
        {

            if (collision.tag == "Enemy")
            {
                healthScript.TakeDamage();
            }
        }
       
    }
}
