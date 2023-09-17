using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using MyGame.GameTime;
public class PlayerLight : MonoBehaviour
{
    [SerializeField] Light2D playerLight;
    private void OnEnable()
    {
        EventHandler.DayChange += OnDayChange;
        EventHandler.AfterSceneLoadEvent += OnAfterSceneLoadEvent;
    }
    private void OnDisable()
    {
        EventHandler.DayChange -= OnDayChange;
        EventHandler.AfterSceneLoadEvent -= OnAfterSceneLoadEvent;
    }
    private void OnDayChange(DayShift dayShift)
    {
        StartCoroutine(IELightChange(dayShift));
    }
    IEnumerator IELightChange(DayShift shift)
    {
        if (shift == DayShift.Day)
        {
            float a = 0;
            while (a < 1)
            {
                playerLight.color = Color.Lerp(Color.white, Color.black, a);
                a += 0.02f;
                yield return Setting.waitSecond;
            }
        }
        else
        {
            float a = 0;
            while (a < 1)
            {
                playerLight.color = Color.Lerp(Color.black, Color.white, a);
                a += 0.02f;
                yield return Setting.waitSecond;
            }
            yield return Setting.waitSecond;
        }
    }

    private void OnAfterSceneLoadEvent(SceneType sceneType, string sceneName)
    {
        if (sceneType == SceneType.Field)
        {
            if (TimeManager.Instance.dayShift == DayShift.Day)
            {
                playerLight.color = Color.black;
            }
            else
            {
                playerLight.color = Color.white;
            }
        }
        else
        {
            playerLight.color = Color.black;
        }
    }
}
