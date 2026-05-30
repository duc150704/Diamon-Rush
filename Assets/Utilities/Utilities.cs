using System.Collections;
using System.Collections.Generic;
using System;

public static class Utilities
{
    private static Random random = new Random();

    public static int RandomInt(int inclusive, int exclusive)
    {
        return random.Next(inclusive, exclusive);
    }
}
