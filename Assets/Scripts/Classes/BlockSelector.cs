using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSelector
{
    // I used FloodFill algorithm to detect adjacent same values efficiently

    // Below arrays details all 4 possible movements
    private static int[] row = {-1, 0, 0, 1};
    private static int[] col = { 0, -1, 1, 0 };
    private const int MATRIXLENGHT = 8;

    // Checks if the current movement is safe
    private static bool IsSafe(int[,]M, int x, int y, int target)
    {
        return x >= 0 && x < MATRIXLENGHT && y >= 0 && y < MATRIXLENGHT
               && M[x,y] == target;
    }

    // Flood fill using DFS
    public static void FloodFill(int[,] M, int x, int y)
    {
        int target = M[x,y];

        // replace current value with -1 to indicate that it is selected
        M[x,y] = -1;

        // process all 4 adjacent movements of current movement
        for (int k = 0; k < row.Length; k++)
        {
            if (IsSafe(M, x + row[k], y + col[k], target))
                FloodFill(M, x + row[k], y + col[k]);
        }

    }
}
