using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class View : MonoBehaviour
{
    [SerializeField]
    private List<TextMeshProUGUI> textList = new List<TextMeshProUGUI>{};
    [SerializeField]
    private List<Image> imageList = new List<Image>{};

    public void Change(List<string> stringList, List<Sprite> spriteList)
    {
        // バグ防止
        if(stringList.Count > textList.Count || spriteList.Count > imageList.Count)
        {
            Debug.Log("数を間違えている");
            return;
        }

        // 更新
        for(int i = 0; i < stringList.Count; i++)
        {
            textList[i].text = stringList[i];
        }
        for(int i = 0; i < spriteList.Count; i++)
        {
            imageList[i].sprite = spriteList[i];
        }
    }
}
