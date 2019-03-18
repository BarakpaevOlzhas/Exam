using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
   [SerializeField]
   private float damage;

   public float Damage()
    {
        return damage;
    }
}
