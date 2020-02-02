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
        new int[] {1, 1, 1, 1, 13, 1, 1, 1}};

    //0=empty,1=obstacle,2=no,3=up/down car,4=left/right car,5-8=wheels,9=battery,7-12=canister,13=exit

    public override string ToString()
    {
        return grid[0][7] + " " + grid[1][7] + " " + grid[2][7] + " " + grid[3][7] + " " + grid[4][7] + " " + grid[5][7] + " " + grid[6][7] + " " + grid[7][7] + "\n" +
            grid[0][6] + " " + grid[1][6] + " " + grid[2][6] + " " + grid[3][6] + " " + grid[4][6] + " " + grid[5][6] + " " + grid[6][6] + " " + grid[7][6] + "\n" +
            grid[0][5] + " " + grid[1][5] + " " + grid[2][5] + " " + grid[3][5] + " " + grid[4][5] + " " + grid[5][5] + " " + grid[6][5] + " " + grid[7][5] + "\n" +
            grid[0][4] + " " + grid[1][4] + " " + grid[2][4] + " " + grid[3][4] + " " + grid[4][4] + " " + grid[5][4] + " " + grid[6][4] + " " + grid[7][4] + "\n" +
            grid[0][3] + " " + grid[1][3] + " " + grid[2][3] + " " + grid[3][3] + " " + grid[4][3] + " " + grid[5][3] + " " + grid[6][3] + " " + grid[7][3] + "\n" +
            grid[0][2] + " " + grid[1][2] + " " + grid[2][2] + " " + grid[3][2] + " " + grid[4][2] + " " + grid[5][2] + " " + grid[6][2] + " " + grid[7][2] + "\n" +
            grid[0][1] + " " + grid[1][1] + " " + grid[2][1] + " " + grid[3][1] + " " + grid[4][1] + " " + grid[5][1] + " " + grid[6][1] + " " + grid[7][1] + "\n" +
            grid[0][0] + " " + grid[1][0] + " " + grid[2][0] + " " + grid[3][0] + " " + grid[4][0] + " " + grid[5][0] + " " + grid[6][0] + " " + grid[7][0];
    }
}