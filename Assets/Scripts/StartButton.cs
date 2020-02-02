using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    public AudioSource click;

    public void StartGame()
    {
        click.Play();
        StartCoroutine(waiter());
    }

    IEnumerator waiter()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("SampleScene");
    }
}
