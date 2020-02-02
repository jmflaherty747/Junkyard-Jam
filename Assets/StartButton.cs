using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    public GameObject titleUi;

    public void StartGame()
    {
        titleUi.SetActive(false);
    }
}
