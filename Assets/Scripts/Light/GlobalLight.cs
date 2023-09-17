using System.Collections;
using System.Collections.Generic;
using MyGame.GameTime;
using UnityEngine;
using UnityEngine.Rendering.Universal;
public class GlobalLight : MonoBehaviour
{
    [SerializeField] Light2D globalLight;
    public Color nightColor;
    public Color springDayColor;
    public Color summerDayColor;
    public Color autumnDayColor;
    public Color winterDayColor;
    public Color changeColor;
    private Color currentColor;
    private bool isChange;
    private void Awake()
    {
        currentColor = TimeManager.Instance.season switch
        {
            Season.春 => springDayColor,
            Season.夏 => summerDayColor,
            Season.秋 => autumnDayColor,
            Season.冬 => winterDayColor,
            _ => new Color(0, 0, 0),
        };
        changeColor = (currentColor - nightColor) / 50;
        globalLight.color = TimeManager.Instance.dayShift switch
        {
            DayShift.Day => currentColor,
            DayShift.Night => nightColor,
            _ => Color.black,
        };
    }
    private void OnEnable()
    {
        EventHandler.DayChange += OnDayChange;
        EventHandler.SeasonChange += OnSeasonChange;
    }
    private void OnDisable()
    {
        EventHandler.DayChange -= OnDayChange;
        EventHandler.SeasonChange -= OnSeasonChange;
    }
    private void OnDayChange(DayShift dayShift)
    {
        if (dayShift == DayShift.Day)
        {
            StartCoroutine(IELightChange(nightColor, currentColor));
        }
        else
        {
            StartCoroutine(IELightChange(currentColor, nightColor));
        }
    }
    private void OnSeasonChange(Season season)
    {
        Debug.Log(season);
        currentColor = season switch
        {
            Season.春 => springDayColor,
            Season.夏 => summerDayColor,
            Season.秋 => autumnDayColor,
            Season.冬 => winterDayColor,
            _ => new Color(0, 0, 0),
        };
        if (TimeManager.Instance.dayShift == DayShift.Day)
        {
            Debug.Log(globalLight.color);
            Debug.Log(currentColor);
            StartCoroutine(IELightChange(globalLight.color, currentColor));
        }
    }
    IEnumerator IELightChange(Color currentColor, Color targetColor)
    {
        if (isChange) yield return Setting.waitSecond;
        float a = 0;
        isChange = true;
        while (a < 1)
        {
            globalLight.color = Color.Lerp(currentColor, targetColor, a);
            a += 0.02f;
            yield return Setting.waitSecond;
        }
        isChange = false;

    }


}
