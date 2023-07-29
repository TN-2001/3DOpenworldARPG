using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuUI : StateMachine<MenuUI>
{
    // UI
    [SerializeField]
    private Button retryBtn;
    [SerializeField]
    private Button endBtn;

    private void OnEnable()
    {
        GameManager.I.Input.actions.FindActionMap("Menu").Enable();
        Time.timeScale = 0;
    }

    private void Start()
    {
        retryBtn.onClick.AddListener(delegate{ 
            UIManager.I.loadPanel.SetActive(true);
            Time.timeScale = 1;
            SceneManager.LoadSceneAsync("Main");
        });

        endBtn.onClick.AddListener(delegate{ 
            UIManager.I.loadPanel.SetActive(true);
            Time.timeScale = 1;
            SceneManager.LoadSceneAsync("Start");
        });
    }

    private void Update()
    {
        if(GameManager.I.Input.actions["Close"].WasPerformedThisFrame())
        {
            // GameUIに遷移
            gameObject.SetActive(false);
            UIManager.I.gameUI.gameObject.SetActive(true);          
        }
    }

    private void OnDisable()
    {
        GameManager.I.Input.actions.FindActionMap("Menu").Disable();
        Time.timeScale = 1;
    }
}
