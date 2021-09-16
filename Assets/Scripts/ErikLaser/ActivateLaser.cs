using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateLaser : MonoBehaviour
{
    void Start()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
