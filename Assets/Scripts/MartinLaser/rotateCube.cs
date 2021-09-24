using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateCube : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseOver()
    {
        if(Input.GetMouseButtonDown(1)){
            transform.Rotate(0.0f, 5.0f, 0.0f, Space.Self);
        }
        else if(Input.GetMouseButtonDown(0))
        {
            transform.Rotate(0.0f, -5.0f, 0.0f, Space.Self);
        }
    }
}
