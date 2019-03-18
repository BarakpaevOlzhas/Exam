using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpUi : MonoBehaviour
{
    [SerializeField]
    private Text textHp;

    [SerializeField]
    private Text textEnemy;

    private void Update()
    {
        textHp.text = StatPlayer.hp.ToString();

        textEnemy.text = StatPlayer.countEnemy.ToString() + "/10";
    }
}
