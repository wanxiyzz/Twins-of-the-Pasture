using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyGame.GameTime;
namespace MyGame.HouseSystem
{
    public class HouseSprite : MonoBehaviour
    {
        SpriteRenderer spriteRenderer;
        public Sprite[] openLightSprites;
        public Sprite[] closeLightSprites;
        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            StartCoroutine(IEChangeImage(TimeManager.Instance.season, TimeManager.Instance.dayShift));
        }
        private void OnEnable()
        {
            EventHandler.SeasonChange += OnSeasonChange;
            EventHandler.DayChange += OndayChange;
        }
        private void OnDisable()
        {
            EventHandler.SeasonChange -= OnSeasonChange;
            EventHandler.DayChange -= OndayChange;
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
            StartCoroutine(IEChangeImage(season, TimeManager.Instance.dayShift));
        }
        IEnumerator IEChangeImage(Season season, DayShift dayShift)
        {
            yield return new WaitForSeconds(0.35f);
            ChangeImage(season, dayShift);
        }
        public void ChangeImage(Season season, DayShift dayShift)
        {
            if (dayShift == DayShift.Day)
                spriteRenderer.sprite = closeLightSprites[(int)season];
            else
                spriteRenderer.sprite = openLightSprites[(int)season];
        }
    }
}