using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    #region vars
    Grid gm;
    [Tooltip("0-3=Wheel,4=Battery,5-10=Canister")] [SerializeField] int itemType;
    public Material t,b,g;
    Renderer mat;
    #endregion

    private void Start()
    {
        gm = FindObjectOfType<Grid>();
        gm.grid[Mathf.RoundToInt(transform.position.x)][Mathf.RoundToInt(transform.position.z)] = 5 + itemType;
        mat = GetComponent<Renderer>();
    }

    private void Update()
    {
        if (gm.grid[Mathf.RoundToInt(transform.position.x)][Mathf.RoundToInt(transform.position.z)] == 0)
        {
            Destroy(gameObject);
        }

        itemType = gm.grid[Mathf.RoundToInt(transform.position.x)][Mathf.RoundToInt(transform.position.z)] - 5;

        if (itemType >= 0 && itemType <= 3)
        {
            mat.material = t;
        }
        if (itemType == 4)
        {
            mat.material = b;
        }
        if (itemType >= 5 && itemType <= 10)
        {
            mat.material = g;
        }
    }
}