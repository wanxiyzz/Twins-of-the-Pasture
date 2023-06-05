using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseSprite : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    public Sprite[] openLightSprites;
    public Sprite[] closeLightSprites;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnEnable()
    {
        EventHandler.seasonChange += OnSeasonChange;
        EventHandler.dayChange += OndayChange;
        EventHandler.afterSceneLoadEvent += OnAfterSceneLoadEvent;
    }
    private void OnDisable()
    {
        EventHandler.seasonChange -= OnSeasonChange;
        EventHandler.dayChange -= OndayChange;
        EventHandler.afterSceneLoadEvent -= OnAfterSceneLoadEvent;
    }

    private void OnAfterSceneLoadEvent()
    {
        if (TimeManager.Instance.season != Season.春)
        {
        }
    }

    private void OndayChange(DayShift shift)
    {
        if (shift == DayShift.Day)
            spriteRenderer.sprite = closeLightSprites[(int)TimeManager.Instance.season];
        else
            spriteRenderer.sprite = openLightSprites[(int)TimeManager.Instance.season];
    }
    private void OnSeasonChange(Season season)
    {
        spriteRenderer.sprite = openLightSprites[(int)season];
        TimeManager.Instance.season = season;
    }
}
