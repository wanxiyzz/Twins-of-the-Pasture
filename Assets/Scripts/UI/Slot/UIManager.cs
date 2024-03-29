using System.Collections;
using UnityEngine;
using UnityEngine.UI;
namespace MyGame.UI
{
    public class UIManager : Singleton<UIManager>
    {
        public Text playerMoney;


        public Text timeText;
        public Text DayText;

        public Sprite[] seasonSprit;
        public Sprite[] Sunny;
        public Sprite[] Cloudy;
        public Sprite[] Overcast;
        public Sprite[] LightRain;
        public Sprite[] HeavyRain;
        public Sprite[] Thunderstorm;

        public Image seasonImage;
        public Image weatherImage;

        private WeatherType currentWeather = WeatherType.Sunny;
        private DayShift currentDayShift = DayShift.Day;
        //提示框
        [SerializeField] GameObject prompt;
        [SerializeField] Text promptText;
        private bool isPrompting;
        private CanvasGroup promptGroup;
        private RectTransform promptTransform;

        protected override void Awake()
        {
            base.Awake();
            promptTransform = prompt.GetComponent<RectTransform>();
            promptGroup = prompt.GetComponent<CanvasGroup>();
        }
        private void OnEnable()
        {
            EventHandler.DayChange += OnDayChange;
            EventHandler.SeasonChange += OnSeasonChange;
            EventHandler.WeatherChange += OnWeatherChange;
        }
        private void OnDisable()
        {
            EventHandler.DayChange -= OnDayChange;
            EventHandler.SeasonChange -= OnSeasonChange;
            EventHandler.WeatherChange -= OnWeatherChange;
        }

        private void OnWeatherChange(WeatherType type)
        {
            currentWeather = type;
            UpdateWeatherSprite();
        }
        public void PromptBox(string statement)
        {
            if (isPrompting) return;
            promptText.text = statement;
            StartCoroutine(RealPromptBox());
        }
        IEnumerator RealPromptBox()
        {
            isPrompting = true;
            var waitTime = new WaitForSeconds(0.005f);
            for (int i = 0; i < 25; i++)
            {
                promptGroup.alpha += 0.04f;
                promptTransform.position = new Vector3(promptTransform.position.x, promptTransform.position.y - 6, 0);
                promptTransform.localScale = new Vector3(promptTransform.localScale.x + 0.04f, promptTransform.localScale.y + 0.04f, 1);
                yield return waitTime;
            }
            yield return new WaitForSeconds(2.5f);
            for (int i = 0; i < 25; i++)
            {
                promptGroup.alpha -= 0.04f;
                promptTransform.position = new Vector3(promptTransform.position.x, promptTransform.position.y + 6, 0);
                promptTransform.localScale = new Vector3(promptTransform.localScale.x - 0.04f, promptTransform.localScale.y - 0.04f, 1);
                yield return waitTime;
            }
            isPrompting = false;
        }
        private void OnSeasonChange(Season season)
        {
            seasonImage.sprite = seasonSprit[(int)season];
        }

        private void OnDayChange(DayShift shift)
        {
            currentDayShift = shift;
            UpdateWeatherSprite();
        }
        /// <summary>
        /// 更新当前天气的图片
        /// </summary>
        private void UpdateWeatherSprite()
        {
            if (currentDayShift == DayShift.Day)
            {
                switch (currentWeather)
                {
                    case WeatherType.Sunny:
                        weatherImage.sprite = Sunny[0];
                        break;
                    case WeatherType.Cloudy:
                        weatherImage.sprite = Cloudy[0];
                        break;
                    case WeatherType.Overcast:
                        weatherImage.sprite = Overcast[0];
                        break;
                    case WeatherType.LightRain:
                        weatherImage.sprite = LightRain[0];
                        break;
                    case WeatherType.HeavyRain:
                        weatherImage.sprite = HeavyRain[0];
                        break;
                    case WeatherType.Thunderstorm:
                        weatherImage.sprite = Thunderstorm[0];
                        break;
                    default: break;
                }
            }
            else
            {
                switch (currentWeather)
                {
                    case WeatherType.Sunny:
                        weatherImage.sprite = Sunny[1];
                        break;
                    case WeatherType.Cloudy:
                        weatherImage.sprite = Cloudy[1];
                        break;
                    case WeatherType.Overcast:
                        weatherImage.sprite = Overcast[1];
                        break;
                    case WeatherType.LightRain:
                        weatherImage.sprite = LightRain[1];
                        break;
                    case WeatherType.HeavyRain:
                        weatherImage.sprite = HeavyRain[1];
                        break;
                    case WeatherType.Thunderstorm:
                        weatherImage.sprite = Thunderstorm[1];
                        break;
                    default: break;
                }
            }
        }

    }
}

