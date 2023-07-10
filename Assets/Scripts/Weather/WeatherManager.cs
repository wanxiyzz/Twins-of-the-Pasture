using MyGame.GameTime;
namespace MyGame.WeatherSystem
{
    public class WeatherManager : Singleton<WeatherManager>
    {
        public WeatherType currentWeather;
        private void OnEnable()
        {
            EventHandler.DayChange += OnDayChenge;
        }
        private void OnDisable()
        {
            EventHandler.DayChange += OnDayChenge;
        }

        private void OnDayChenge(DayShift shift)
        {
            switch (TimeManager.Instance.season)
            {
                case Season.春:
                case Season.秋:
                    //WORKFLOW:季节天气反应
                    break;
                case Season.冬:
                    break;
                case Season.夏:
                    break;

                default: return;
            }
        }

        void Update()
        {

        }
    }
}