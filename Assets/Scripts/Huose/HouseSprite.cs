using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyGame.GameTime;
namespace MyGame.Huose
{
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
            EventHandler.SeasonChange += OnSeasonChange;
            EventHandler.DayChange += OndayChange;
            EventHandler.AfterSceneLoadEvent += OnAfterSceneLoadEvent;
        }
        private void OnDisable()
        {
            EventHandler.SeasonChange -= OnSeasonChange;
            EventHandler.DayChange -= OndayChange;
            EventHandler.AfterSceneLoadEvent -= OnAfterSceneLoadEvent;
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
            StartCoroutine(ChangeImage(season, TimeManager.Instance.dayShift));
        }
        IEnumerator ChangeImage(Season season, DayShift dayShift)
        {
            yield return new WaitForSeconds(0.35f);
            if (dayShift == DayShift.Day)
                spriteRenderer.sprite = closeLightSprites[(int)season];
            else
                spriteRenderer.sprite = openLightSprites[(int)season];
        }
    }
}