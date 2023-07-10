using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MyGame.WeatherSystem
{
    public class PlayerRainDrop : MonoBehaviour
    {
        private float intervalTime;
        public bool isRain;
        [SerializeField] Transform[] rainDropPos;
        private Coroutine RainCoroutine;

        int index;
        public ObjectPool<RainDrop> rainDropEffect;
        private void Start()
        {
            Initialize();
        }
        private void Initialize()
        {
            rainDropEffect.Initialize(transform);
        }
        private void OnEnable()
        {
            EventHandler.WeatherChange += OnWeatherChange;
        }
        private void OnDisable()
        {
            EventHandler.WeatherChange -= OnWeatherChange;
        }
        private void Update()
        {
            //TEST
            if (Input.GetKeyDown(KeyCode.R))
            {
                Debug.Log((WeatherType)index);
                EventHandler.CallWeatherChange((WeatherType)index);
                index++;
                if (index == 6) index = 0;
            }
        }
        private void OnWeatherChange(WeatherType type)
        {
            if (type == WeatherType.LightRain
            || type == WeatherType.HeavyRain
            || type == WeatherType.Thunderstorm)
            {
                if (isRain)
                {
                    StopCoroutine(RainCoroutine);
                }
                RainCoroutine = StartCoroutine(RainStart(type));
            }
            else
            {
                if (isRain)
                {
                    StopCoroutine(RainCoroutine);
                    isRain = false;
                }
            }
        }
        IEnumerator RainStart(WeatherType type)
        {
            isRain = true;
            int currenIndex = -1;
            WaitForSeconds seconds = type switch
            {
                WeatherType.LightRain => new WaitForSeconds(0.3f),
                WeatherType.HeavyRain => new WaitForSeconds(0.2f),
                WeatherType.Thunderstorm => new WaitForSeconds(0.15f),
                _ => new WaitForSeconds(0.3f)
            };
            while (true)
            {
                int index = Random.Range(0, 6);
                if (currenIndex != index)
                {
                    currenIndex = index;
                    rainDropEffect.PrepareObject(rainDropPos[index].position);
                }
                yield return seconds;
            }
        }
    }
}