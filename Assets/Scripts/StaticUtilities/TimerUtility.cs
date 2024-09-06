using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TimerUtility
{

    public static bool TimerCountDown(ref float timer, float decrement)
    {
        timer -= decrement;

        if (timer <= 0)
        {
            timer = 0;
            return true;
        }

        return false;
    }
}
