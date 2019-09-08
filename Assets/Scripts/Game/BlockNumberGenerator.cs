using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockNumberGenerator
{
    // Number Color be shown in Level Manager
    public static Color GetColor(int number)
    {
        float colorChooserB = number * 0.05f;
        float colorChooserG = -number * 0.03f;
        float colorChooserR = number * 0.08f;
        if (number % 2 == 0)
        {
            colorChooserB = number * 0.08f;
            colorChooserG = number * 0.05f;
            colorChooserR = -number * 0.03f;
        }

        return new Color(0.5f + colorChooserR, 0.5f + colorChooserG, 0.5f + colorChooserB);
    }

    // Number probabilities 
    public static void SetNumberProbabilities(int stage)
    {
        float[] percentage = new float[stage+3];
        float fullPercentage = 1.0f;

        for (int i = 0; i < percentage.Length-1; i++)
        {
            percentage[i] = UnityEngine.Random.Range(i, fullPercentage);
        }
    }
}
