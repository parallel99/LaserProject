using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class laserHittPointSetter : MonoBehaviour
{
    public int reflections;
    public float maxLength;

    public LineRenderer lineRenderer;
    public Ray ray;
    private RaycastHit hit;
    private Vector3 direction;
    private bool wasHit;
    private GameObject lastHit;

    private LineRenderer[] laserList;
    private laserHittPointSetter[] hitPointList;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        wasHit = false;
    }

    // Update is called once per frame
    void Update()
    {
        ray = new Ray(transform.position, transform.forward);

        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, transform.position);

        //float remaingLength = maxLength;
        for (int i = 0; i < reflections; i++)
        {
            if (Physics.Raycast(ray.origin, ray.direction, out hit, maxLength))
            {
                lineRenderer.positionCount += 1;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);
                //remaingLength -= Vector3.Distance(ray.origin, hit.point);
                ray = new Ray(hit.point, Vector3.Reflect(ray.direction, hit.normal));
                if (hit.collider.tag != "ReflectionCube")
                {
                    if (hit.collider.tag == "ResultCube")
                    {
                        //GameObject.FindWithTag("Door").SendMessage("OpenDoor");
                        Debug.Log("Siker");
                    }
                    if (hit.collider.tag.Equals("LaserCube"))
                    {
                        wasHit = true;
                        lastHit = hit.collider.gameObject;
                        hit.collider.gameObject.GetComponent<LineRenderer>().enabled = true;
                        hit.collider.gameObject.GetComponent<laserHittPointSetter>().enabled = true;             
                    }
                    if (hit.collider.tag.Equals("LaserSplitter"))
                    {
                        wasHit = true;
                        lastHit = hit.collider.gameObject;
                        laserList = hit.collider.gameObject.GetComponentsInChildren<LineRenderer>();
                        hitPointList = hit.collider.gameObject.GetComponentsInChildren<laserHittPointSetter>();
                        for (int y = 0; y < laserList.Length; y++)
                        {
                            laserList[y].enabled = true;
                        }
                        for (int y = 0; y < hitPointList.Length; y++)
                        {
                            hitPointList[y].enabled = true;
                        }

                    }
                    else if (wasHit)
                    {
                        DisableObject();
                        wasHit = false;
                    }
                    break;
                }
            }
            else if (wasHit)
            {
                DisableObject();
                wasHit = false;
            }
            else
            {
                lineRenderer.positionCount += 1;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, ray.origin + ray.direction * maxLength);
            }
        }
    }

    public void DisableObject()
    {
        if (lastHit != null && !lastHit.CompareTag("LaserSplitter"))
        {
            lastHit.GetComponent<laserHittPointSetter>().DisableObject();
        }
        if (lastHit != null && lastHit.CompareTag("LaserSplitter"))
        {
            hitPointList = lastHit.GetComponentsInChildren<laserHittPointSetter>();
            for (int i = 0; i < hitPointList.Length; i++)
            {
                hitPointList[i].DisableObject();
            }
        }
        if (gameObject.tag.Equals("LaserCube"))
        {
            gameObject.GetComponent<LineRenderer>().enabled = false;
            gameObject.GetComponent<laserHittPointSetter>().enabled = false;
        }
        if (gameObject.tag.Equals("LaserSplitter"))
        {
            gameObject.GetComponent<LineRenderer>().enabled = false;
            gameObject.GetComponent<laserHittPointSetter>().enabled = false;
        }
    }
}
