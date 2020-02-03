using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject levelName;
    public GameObject levelCompleteText;
    Transform player;
    Vector3 goalCoords;

    void Start()
    {
        StartCoroutine(RemoveIntroText());
        player = GameObject.FindGameObjectWithTag("Player").transform;
        goalCoords = new Vector3(7, 0, 4);
    }
    
    void Update()
    {
        if (player.position == goalCoords)
        {
            levelCompleteText.SetActive(true);
            var playerScript = player.GetComponent<Player>();
            playerScript.enabled = false;
            player.GetComponent<DriveOnClear>().enabled = true;
            playerScript.car.GetComponent<DriveOnClear>().enabled = true;
            StartCoroutine(LevelComplete());
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
    
    IEnumerator LevelComplete()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    IEnumerator RemoveIntroText()
    {
        yield return new WaitForSeconds(5);
        levelName.SetActive(false);
    }
}