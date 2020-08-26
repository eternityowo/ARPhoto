using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine;

public class Utils
{
    public static Vector2[] ParseStringToSize(string input)
    {
        string[] numbers = Regex.Split(input, @"[^0-9.]+");
        int count = int.Parse(numbers[0]);
        Vector2[] size = new Vector2[count];

        for (int i = 0; i < count; i++)
        {
            size[i] = new Vector2
                (
                    float.Parse(numbers[i + i + 1], CultureInfo.InvariantCulture.NumberFormat),
                    float.Parse(numbers[i + i + 2], CultureInfo.InvariantCulture.NumberFormat)
                );
        }
        return size;
    }
}