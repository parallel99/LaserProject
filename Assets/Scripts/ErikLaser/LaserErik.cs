using UnityEngine;

public class LaserErik : MonoBehaviour
{
    [SerializeField]
    private LineRenderer lineRenderer;
    [SerializeField]
    private int laserMaxLength = 5000;

    private bool hitReflectionObj;
    private GameObject lastHit;

    public RaycastHit mirrorHit;

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
        if (!gameObject.transform.parent.tag.Equals("Mirror"))
        {
            lineRenderer.SetPosition(0, transform.position);
        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            if (hit.collider) //TODO: maybe a small fix
            {
                lineRenderer.SetPosition(1, hit.point);
                ReachToFinish(hit);
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
            if (hit.collider.tag.Equals("Mirror"))
            {
                /*var mirrorStartPoint = hit.collider.gameObject.transform.GetChild(0).gameObject;
                mirrorStartPoint.SetActive(true);
                mirrorStartPoint.transform.position = hit.point;
                mirrorStartPoint.GetComponent<LaserErik>().mirrorHit = hit;*/
                var mirrorStartPoint = hit.collider.gameObject.transform.GetChild(0).gameObject;
                mirrorStartPoint.SetActive(true);
                mirrorStartPoint.transform.position = hit.point;
                if (!mirrorStartPoint.TryGetComponent(out LineRenderer renderer))
                {
                    mirrorStartPoint.AddComponent<LineRenderer>();
                } 
                else
                {
                    renderer.SetPosition(0, hit.point);
                    
                    Ray ray = new Ray(hit.point, Vector3.Reflect(transform.forward, hit.normal) * laserMaxLength);
                    RaycastHit hitWithMirror;
                    if (Physics.Raycast(ray.origin, ray.direction, out hitWithMirror))
                    {
                        if (hitWithMirror.collider) //TODO: maybe a small fix
                        {
                            renderer.SetPosition(1, hitWithMirror.point);
                            ReachToFinish(hitWithMirror);
                        }
                        if (hitWithMirror.collider.tag.Equals("ReflectionCube"))
                        {
                            //Needs optimization here and the full script
                            hitReflectionObj = true;
                            lastHit = hitWithMirror.collider.gameObject;
                            hitWithMirror.collider.gameObject.transform.GetChild(0).gameObject.SetActive(true);
                        }
                        else if (hitReflectionObj)
                        {
                            hitReflectionObj = false;
                            lastHit.transform.GetChild(0).gameObject.SetActive(false);
                        }
                        ReachToFinish(hitWithMirror);
                    } 
                    else
                    {
                        renderer.SetPosition(1, Vector3.Reflect(transform.forward, hit.normal) * laserMaxLength);
                    }
                }
                
            }
            /*if (gameObject.transform.parent.tag.Equals("Mirror"))
            {
                if (mirrorHit.point != null)
                {
                    lineRenderer.SetPosition(0, mirrorHit.point);
                    lineRenderer.SetPosition(1, Vector3.Reflect(mirrorHit.transform.forward, mirrorHit.normal) * laserMaxLength);
                    Debug.Log(mirrorHit.point);
                }

            }*/

        } 
        else if (!gameObject.transform.parent.tag.Equals("Mirror"))
        {
            lineRenderer.SetPosition(1, transform.forward * laserMaxLength);
        }
        /*if (gameObject.transform.parent.tag.Equals("Mirror"))
        {
            if (mirrorHit.point != null)
            {
                lineRenderer.SetPosition(0, mirrorHit.point);
                lineRenderer.SetPosition(1, Vector3.Reflect(mirrorHit.transform.forward, mirrorHit.normal) * laserMaxLength);
                Debug.Log(mirrorHit.point);
            }

        }*/
    }

    private void ReachToFinish(RaycastHit hit)
    {
        if (hit.collider.tag.Equals("LaserEndPoint"))
        {
            Debug.Log("Siker");
        }
    }
}