using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveOnClear : MonoBehaviour
{
    private void Start()
    {
        enabled = false;
    }

    private void Update()
    {
        var speedVector = new Vector3(5, 0, 0);
        transform.position += (speedVector * Time.deltaTime);
    }
}
