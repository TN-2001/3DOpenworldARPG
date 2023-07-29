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
    private GameObject mobileController = null;

    private void OnEnable()
    {
        GameManager.I.Input.actions.FindActionMap("Game").Enable();
    }

    private void Start()
    {
        // スマホプレイか確認し、MobileControllerのOnOff
        if(UnityEngine.Device.Application.isMobilePlatform)
        {
            mobileController.SetActive(true);
        }
        else
        {
            mobileController.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        if(GameManager.I.Input.actions["Menu"].WasPerformedThisFrame())
        {
            // MenuUIに遷移
            gameObject.SetActive(false);
            UIManager.I.menuUI.gameObject.SetActive(true);
        }
    }

    private void OnDisable()
    {
        GameManager.I.Input.actions.FindActionMap("Game").Disable();
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
    // StaminaSliderの変化
    public void SetStaminaSlider(float _stamina)
    {
        staminaSlider.value = _stamina;
    }
}
