using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveOnClear : MonoBehaviour
{
    Vector3 speedVector = new Vector3(5, 0, 0);

    private void Start()
    {
        enabled = false;
    }

    private void Update()
    {
        transform.position += (speedVector * Time.deltaTime);
    }
}