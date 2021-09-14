using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserErik : MonoBehaviour
{
    [SerializeField]
    private LineRenderer lineRenderer;
    [SerializeField]
    private int laserMaxLength = 5000;

    void Start()
    {
        if(lineRenderer is null) lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        lineRenderer.SetPosition(0, transform.position);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            if (hit.collider) //TODO: maybe a small fix
            {
                lineRenderer.SetPosition(1, hit.point);
            }
            if (hit.collider.tag.Equals("ReflectionCube"))
            {
                //TODO: need to finish this
                hit.collider.gameObject.transform.GetChild(0).gameObject.transform.GetComponent<LineRenderer>().enabled = true;
            }
        }
        else
        {
            lineRenderer.SetPosition(1, transform.forward * laserMaxLength);
        }
    }
}
