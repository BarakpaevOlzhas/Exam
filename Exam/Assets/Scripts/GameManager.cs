using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Transform rootWin;

    [SerializeField]
    private Button buttonStart;

    [SerializeField]
    private Button buttonExit;

    [SerializeField]
    private Text textWinOrLose;

    private void Start()
    {
        buttonStart.onClick.AddListener(ButtonClickRestat);
        buttonExit.onClick.AddListener(ButtonClickExit);
    }

    private void ButtonClickExit()
    {
        
    }

    private void ButtonClickRestat()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void Update()
    {
        if (StatPlayer.countEnemy == 10)
        {            
            textWinOrLose.text = "You Win";
            rootWin.gameObject.SetActive(true);
        }

        else if (StatPlayer.hp <= 0)
        {            
            textWinOrLose.text = "You Lose";
            rootWin.gameObject.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        buttonStart.onClick.RemoveAllListeners();
        buttonExit.onClick.RemoveAllListeners();
    }
}
