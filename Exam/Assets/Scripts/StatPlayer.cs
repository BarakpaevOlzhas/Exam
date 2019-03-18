using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatPlayer : MonoBehaviour
{
    public static float hp;

    public static int countEnemy;

    private void Start()
    {
        hp = 100;
        countEnemy = 0;
    }
}
