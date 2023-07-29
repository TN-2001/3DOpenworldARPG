using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    // 子オブジェクトとそのコンポーネント
    public GameObject loadPanel = null;
    public GameUI gameUI = null;
    public MenuUI menuUI = null;

    protected override void Awake()
    {
        I = this;
    }
}
