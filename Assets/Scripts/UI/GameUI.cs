using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField]
    private Slider hpSlider;
    [SerializeField]
    private RectTransform hpRedSliderTra;
    private float maxHp;
    private float currentHp = 1;
    private float nextHp = 1;
    private Coroutine setHpRedSlider;
    [SerializeField]
    private Slider staminaSlider;
    [SerializeField]
    private Image staminaSliderImage;
    [SerializeField]
    private Color staminaNomal;
    [SerializeField]
    private Color staminaSmall;
    private float maxStamina;
    [SerializeField]
    private Button settingBtn;

    public void InitialzeHpSlider(float _maxHp)
    {
        maxHp = _maxHp;
    }

    public void SetHpSlider(float _nextHp)
    {
        if (setHpRedSlider != null)
        {
            StopCoroutine(setHpRedSlider);
        }

        currentHp = nextHp;
        hpRedSliderTra.localScale = new Vector3(currentHp, 1, 1);

        nextHp = _nextHp / maxHp;
        hpSlider.value = nextHp;

        setHpRedSlider = StartCoroutine(SetHpRedSlider());
    }

    private IEnumerator SetHpRedSlider()
    {
        float changeHp = (currentHp - nextHp) / 50;

        while (true)
        {
            if (currentHp <= nextHp)
            {
                currentHp = nextHp;
                yield break;
            }

            currentHp -= changeHp;
            hpRedSliderTra.localScale = new Vector3(currentHp, 1, 1);

            yield return new WaitForSeconds(0.1f);
        }
    }

    public void InitializeStaminaSlider(float _maxStamina)
    {
        maxStamina = _maxStamina;
    }

    public void SetStaminaSlider(float _stamina)
    {
        staminaSlider.value = _stamina / maxStamina;

        if (staminaSlider.value > 0.2f)
        {
            staminaSliderImage.color = staminaNomal;
        }
        else
        {
            staminaSliderImage.color = staminaSmall;
        }
    }
}
