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

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        ray = new Ray(transform.position, transform.forward);

        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, transform.position);
        float remaingLength = maxLength;
        for(int i = 0; i < reflections; i++)
        {
            if(Physics.Raycast(ray.origin, ray.direction, out hit, remaingLength))
            {
                lineRenderer.positionCount += 1;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);
                remaingLength -= Vector3.Distance(ray.origin, hit.point);
                ray = new Ray(hit.point, Vector3.Reflect(ray.direction, hit.normal));
                if(hit.collider.tag != "ReflectionCube")
                {
                    if(hit.collider.tag == "ResultCube")
                    {
                        GameObject.FindWithTag("Door").SendMessage("OpenDoor");
                        break;
                    }
                    break;
                }
                else
                {
                    hit.collider.gameObject.SendMessage("LaserHit");
                    lineRenderer.positionCount += 1;
                    lineRenderer.SetPosition(lineRenderer.positionCount - 1, ray.origin + ray.direction * remaingLength);
                }
            }
        }
    }
}
