using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorReflection : MonoBehaviour
{
    [SerializeField]
    private LineRenderer lineRenderer;
    [SerializeField]
    private int laserMaxLength = 5000;

    private bool hitReflectionObj;
    private GameObject lastHit;

    private void Awake()
    {
        hitReflectionObj = false;
    }

    void Start()
    {
        if (lineRenderer is null) lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        //gameObject.transform.rotation = Vector3.Reflect(transform.forward, gameObject.transform.parent.position);
        lineRenderer.SetPosition(0, transform.position);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            if (hit.collider.tag.Equals("Mirror"))
            {
                hit.collider.gameObject.transform.GetChild(0).gameObject.SetActive(true);
                hit.collider.gameObject.transform.GetChild(0).gameObject.transform.position = hit.point;
                
                lineRenderer.SetPosition(0, hit.point);
            }
            if (hit.collider) //TODO: maybe a small fix
            {
                lineRenderer.SetPosition(1, hit.point);
            }
            if (hit.collider.tag.Equals("ReflectionCube"))
            {
                
                //TODO: need to finish this
                hitReflectionObj = true;
                lastHit = hit.collider.gameObject;
                hit.collider.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            }
            else if (hitReflectionObj)
            {
                hitReflectionObj = false;
                lastHit.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
        else
        {
            lineRenderer.SetPosition(1, transform.forward * laserMaxLength);
        }
    }
}
