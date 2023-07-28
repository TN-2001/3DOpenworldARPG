using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class StartManager : MonoBehaviour
{
    public static StartManager I = null;

    [SerializeField]
    private GameObject loadPanel;
    [SerializeField]
    private TextMeshProUGUI startText;

    private void Awake()
    {
        I = this;
    }

    public void OnClickStart()
    {
        loadPanel.SetActive(true);
        startText.text = "ロード";
        SceneManager.LoadScene("Main");
    }
}
