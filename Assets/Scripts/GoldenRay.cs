using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldenRay : MonoBehaviour
{
    LineRenderer rend;
    EdgeCollider2D col;

    public List<Vector2> linePoints = new List<Vector2>();
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<LineRenderer>();
        col = GetComponent<EdgeCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        linePoints[0] = rend.GetPosition(0);
        linePoints[1] = rend.GetPosition(1);
        col.SetPoints(linePoints);
    }
}
