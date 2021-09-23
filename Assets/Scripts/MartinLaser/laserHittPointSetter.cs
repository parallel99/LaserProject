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
    private bool[] wasHit = new bool[2];
    private GameObject lastHit;

    private LineRenderer[] laserList;
    private laserHittPointSetter[] hitPointList;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        wasHit[0] = false;
        wasHit[1] = false;
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
                if (!hit.collider.CompareTag("ReflectionCube"))
                {
                    if (hit.collider.CompareTag("ResultCube"))
                    {
                        GoalReached();
                    }

                    if (hit.collider.CompareTag("LaserCube") && (lastHit == null || lastHit == hit.collider.gameObject))
                    {
                        wasHit[0] = true;
                        lastHit = hit.collider.gameObject;
                        hit.collider.gameObject.GetComponent<LineRenderer>().enabled = true;
                        hit.collider.gameObject.GetComponent<laserHittPointSetter>().enabled = true;
                    }
                    else if (wasHit[0])
                    {
                        wasHit[0] = false;
                        DisableObject(new List<GameObject>());
                        lastHit = null;
                    }

                    if (hit.collider.CompareTag("LaserSplitter") && (lastHit == null || lastHit == hit.collider.gameObject))
                    {
                        wasHit[1] = true;
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
                    else if (wasHit[1])
                    {
                        wasHit[1] = false;
                        DisableObject(new List<GameObject>());
                        lastHit = null;
                    }
                    break;
                }
            }
            else if (wasHit[1] || wasHit[0])
            {
                wasHit[0] = false;
                wasHit[1] = false;
                DisableObject(new List<GameObject>());
                lastHit = null;
            }
            else
            {
                lineRenderer.positionCount += 1;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, ray.origin + ray.direction * maxLength);
            }
        }
    }

    public void DisableObject(List<GameObject> hits)
    {
        if (lastHit != null && lastHit.CompareTag("LaserCube") && !hits.Contains(gameObject))
        {
            if (!gameObject.CompareTag("StartPoint")) 
            { 
                hits.Add(gameObject);
            }
            lastHit.GetComponent<laserHittPointSetter>().DisableObject(hits);
        }

        if (lastHit != null && lastHit.tag.Equals("LaserSplitter") && !hits.Contains(gameObject))
        {
            hits.Add(gameObject);
            hitPointList = lastHit.GetComponentsInChildren<laserHittPointSetter>();
            for (int i = 0; i < hitPointList.Length; i++)
            {
                hitPointList[i].DisableObject(hits);
            }
        }

        if (lastHit == null && (gameObject.CompareTag("LaserCube") || gameObject.transform.parent.CompareTag("LaserSplitter")))
        {
            hits.Add(gameObject);
        }
        
        if (hits.Contains(gameObject) && (gameObject.CompareTag("LaserCube") || gameObject.CompareTag("LaserSplitter")))
        {
            if (lastHit != null && lastHit.CompareTag("LaserCube"))
            {
                hits.Add(lastHit);
            }
            else if (lastHit != null && lastHit.CompareTag("LaserSplitter"))
            {
                hits.Add(lastHit.transform.GetChild(0).gameObject);
                hits.Add(lastHit.transform.GetChild(1).gameObject);
            }
            
            for (int i = 0; i < hits.Count; i++)
            {
                if (!hits[i].CompareTag("StartPoint"))
                {
                    hits[i].GetComponent<LineRenderer>().enabled = false;
                    hits[i].GetComponent<laserHittPointSetter>().enabled = false;
                }
            }
        }
    }

    public void GoalReached()
    {
        //code to be executed after the result cube was hit
    }
}