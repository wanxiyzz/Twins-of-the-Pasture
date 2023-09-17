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
        Color changeColor = new Color(0.02f, 0.02f, 0.02f);
        if (shift == DayShift.Day)
        {
            while (playerLight.color.r > 0)
            {
                playerLight.color -= changeColor;
                yield return new WaitForFixedUpdate();
            }
        }
        else
        {
            while (playerLight.color.r < 1)
            {
                playerLight.color += changeColor;
                yield return new WaitForFixedUpdate();
            }
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
