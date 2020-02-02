using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    #region vars
    Grid gm;
    [Tooltip("0-3=Wheel,4=Battery,5-10=Canister")] [SerializeField] int itemType;
    #endregion

    private void Start()
    {
        gm = FindObjectOfType<Grid>();
        gm.grid[(int)transform.position.x][(int)transform.position.z] = 5 + itemType;
    }
}