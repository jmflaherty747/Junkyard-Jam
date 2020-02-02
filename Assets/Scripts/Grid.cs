using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public int[][] grid = new int[][] {
        new int[] {1, 1, 1, 1, 1, 1, 1, 1},
        new int[] {1, 0, 0, 0, 0, 0, 0, 1},
        new int[] {1, 0, 0, 0, 0, 0, 0, 1},
        new int[] {1, 0, 0, 0, 0, 0, 0, 1},
        new int[] {1, 0, 0, 0, 0, 0, 0, 1},
        new int[] {1, 0, 0, 0, 0, 0, 0, 1},
        new int[] {1, 0, 0, 0, 0, 0, 0, 1},
        new int[] {1, 1, 1, 1, 1, 1, 1, 1}};

    //0=empty,1=obstacle,2=no,3=up/down car,4=left/right car,5-8=wheels,9=battery,7-12=canister
}