using UnityEngine;

public class LaserErik : MonoBehaviour
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