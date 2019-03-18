using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacles : MonoBehaviour
{
    [SerializeField]
    private float hitPoints;

    public void DamageToObstacles(float damage)
    {
        hitPoints -= damage;
        if (hitPoints <= 0)
        {
            Destroy(transform.gameObject);
        }
    }
}
