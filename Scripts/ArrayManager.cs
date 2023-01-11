using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayManager : MonoBehaviour
{
    public static int[,] placements = new int[10, 10];
    public static int[,] placementsAI = new int[10, 10];

    void Start()
    {

        /*for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                placements[j, i] = -1;
                placementsAI[j, i] = -1;
            }
        }*/
    }

    public static void Initiate()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                placements[j, i] = -1;
                placementsAI[j, i] = -1;
            }
        }
    }
}
