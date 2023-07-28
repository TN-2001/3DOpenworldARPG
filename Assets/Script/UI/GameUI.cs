using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    // UI
    [SerializeField]
    private Slider hpSlider = null;
    [SerializeField]
    private Slider staminaSlider = null;
    [SerializeField]
    private GameObject MobileController = null;

    private void Awake()
    {
        // スマホプレイか確認し、MobileControllerのOnOff
        MobileController.SetActive(true);
    }

    // HPSliderの初期化
    public void InitialzeHpSlider(float _maxHp)
    {
        hpSlider.maxValue = _maxHp;
        hpSlider.value = _maxHp;
    }
    // HPSliderの変化
    public void SetHpSlider(float _nextHp)
    {
        hpSlider.value = _nextHp;
    }

    // StaminaSliderの初期化
    public void InitializeStaminaSlider(float _maxStamina)
    {
        staminaSlider.maxValue = _maxStamina;
        staminaSlider.value = _maxStamina;
    }
    public void SetStaminaSlider(float _stamina)
    {
        staminaSlider.value = _stamina;
    }
}
