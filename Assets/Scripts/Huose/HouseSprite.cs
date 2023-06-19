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

    private void OnAfterSceneLoadEvent(SceneType sceneType, string sceneName)
    {
        StartCoroutine(ChangeImage(TimeManager.Instance.season, TimeManager.Instance.dayShift));
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
        Debug.Log((int)season);
        StartCoroutine(ChangeImage(season, TimeManager.Instance.dayShift));
    }
    IEnumerator ChangeImage(Season season, DayShift dayShift)
    {
        yield return new WaitForSeconds(0.3f);
        if (dayShift == DayShift.Day)
            spriteRenderer.sprite = openLightSprites[(int)season];
        else
            spriteRenderer.sprite = closeLightSprites[(int)season];
    }
}
