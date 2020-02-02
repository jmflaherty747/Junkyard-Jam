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
        gm.grid[(int)transform.position.x][(int)transform.position.z] = 1;
    }
}