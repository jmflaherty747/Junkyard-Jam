using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    public GameObject titleUi;
    public AudioSource click;

    public void StartGame()
    {
        titleUi.SetActive(false);
        click.Play();
    }
}
