using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using MyGame.GameTime;

public class HouseLight : MonoBehaviour
{
    [SerializeField] Light2D houseLight;

    public Color targetColor;
    private bool isChange;
    private void Awake()
    {
        if (TimeManager.Instance.dayShift == DayShift.Day)
        {
            houseLight.color = Color.black;
        }
        else
        {
            houseLight.color = targetColor;
        }
    }
    private void OnEnable()
    {
        EventHandler.DayChange += OnDayChange;
    }
    private void OnDisable()
    {
        EventHandler.DayChange -= OnDayChange;
    }
    private void OnDayChange(DayShift dayShift)
    {
        if (dayShift == DayShift.Day)
        {
            StartCoroutine(IELightChange(targetColor, Color.black));
        }
        else
        {
            StartCoroutine(IELightChange(Color.black, targetColor));
        }
    }
    IEnumerator IELightChange(Color currentColor, Color targetColor)
    {
        if (isChange) yield return Setting.waitSecond;
        float a = 0;
        isChange = true;
        while (a < 1)
        {
            houseLight.color = Color.Lerp(currentColor, targetColor, a);
            a += 0.02f;
            yield return Setting.waitSecond;
        }
        isChange = false;

    }

}
