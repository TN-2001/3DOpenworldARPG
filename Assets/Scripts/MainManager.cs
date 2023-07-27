using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainManager : MonoBehaviour
{
    public static MainManager I = null;

    [SerializeField]
    private int enemyCount;
    private float timeCount;
    public GameUI gameUI;
    [SerializeField]
    private GameObject playerUI;
    [SerializeField]
    private Button settingBtn;
    [SerializeField]
    private Button closeBtn;
    [SerializeField]
    private TextMeshProUGUI gameText;
    [SerializeField]
    private Button retryBtn;
    [SerializeField]
    private Button endBtn;
    [SerializeField]
    private GameObject gameEndUI;
    [SerializeField]
    private TextMeshProUGUI gameEndText;
    [SerializeField]
    private GameObject loadPanel;

    private void Awake()
    {
        I = this;
    }

    private void Start()
    {
        settingBtn.onClick.AddListener(delegate{
            gameUI.gameObject.SetActive(false);
            playerUI.SetActive(true);
            gameText.text = $"Time:{timeCount.ToString("f2")}";
            Time.timeScale = 0;
        });

        closeBtn.onClick.AddListener(delegate{
            gameUI.gameObject.SetActive(true);
            playerUI.SetActive(false);
            gameText.text = $"Time:{timeCount.ToString("f2")}";
            Time.timeScale = 1;
        });

        retryBtn.onClick.AddListener(delegate{ 
            StartCoroutine(LoadScene(0, 1));
        });

        endBtn.onClick.AddListener(delegate{ 
            StartCoroutine(LoadScene(0, 0));
        });
    }

    private void Update()
    {
        timeCount += Time.deltaTime;
    }

    public void DiePlayer()
    {
        gameEndText.text = $"Game Over\nTime:{timeCount.ToString("f2")}";
        gameEndUI.SetActive(true);
        StartCoroutine(LoadScene(3, 0));
    }

    public void DieEnemy()
    {
        enemyCount -= 1;

        if (enemyCount <= 0)
        {
            gameEndText.text = $"Game Clear\nTime:{timeCount.ToString("f2")}";
            gameEndUI.SetActive(true);
            StartCoroutine(LoadScene(3, 0));
        }
    }

    private IEnumerator LoadScene(float waitTime, int number)
    {
        yield return new WaitForSeconds(waitTime);

        loadPanel.SetActive(true);
        Time.timeScale = 1;

        if (number == 0)
        {
            SceneManager.LoadScene("Start");
        }
        else if (number == 1)
        {
            SceneManager.LoadScene("Main");
        }
    }
}
