using System.Collections;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    #region vars
    Grid gm;
    #endregion

    private void Start()
    {
        gm = FindObjectOfType<Grid>();
        gm.grid[Mathf.RoundToInt(transform.position.x)][Mathf.RoundToInt(transform.position.z)] = 1;
    }
}