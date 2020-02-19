using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatteryIcon : MonoBehaviour
{
    private Camera mainCamera;
    public Car car;
    public Image icon;
    public Vector3 offset;

    void Awake()
    {
        mainCamera = Camera.main;
        offset = transform.localPosition;
    }

    void Update()
    {
        icon.enabled = !car.battery;
        var localToWorld = car.transform.TransformPoint(offset);
        var worldToScreen = mainCamera.WorldToScreenPoint(localToWorld);
        icon.transform.position = worldToScreen;

    }
}
