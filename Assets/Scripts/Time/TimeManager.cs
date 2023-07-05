using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : Singleton<TimeManager>
{
    public DayShift dayShift;
    public int minute;
    private int huor;
    private int day;
    public Season season;
    private int seasonNumber;
    private int year;
    public bool clockPause;
    private float tikTime;

    private StringBuilder timeTextBuilder = new StringBuilder();
    private Text timeText;
    private void Start()
    {
        timeText = UIManager.Instance.timeText;
        year = 2023;
        huor = 7;
        day = 1;
        dayShift = DayShift.Day;
    }
    private void Update()
    {
        UpdateTime();
        if (Input.GetKey(KeyCode.T))
        {
            for (int i = 0; i < 200; i++)
            {
                UpdateTime();
            }
        }
    }
    private void UpdateTime()
    {
        tikTime += Time.deltaTime;
        if (tikTime > Setting.secondThreshold)
        {
            minute++;
            tikTime -= Setting.secondThreshold;
            if (minute > Setting.minuteThreshold)
            {
                huor++;
                if (season == Season.春 || season == Season.秋)
                {
                    if (huor == Setting.springNightTime)
                    {
                        dayShift = DayShift.Night;
                        EventHandler.CallDayChenge(dayShift);
                    }
                    else if (huor == Setting.springDayTime)
                    {
                        dayShift = DayShift.Day;
                        EventHandler.CallDayChenge(dayShift);
                    }
                }
                else if (season == Season.夏)
                {
                    if (huor == Setting.summerNightTime)
                    {
                        dayShift = DayShift.Night;
                        EventHandler.CallDayChenge(dayShift);
                    }
                    else if (huor == Setting.summerDayTime)
                    {
                        dayShift = DayShift.Day;
                        EventHandler.CallDayChenge(dayShift);
                    }
                }
                else
                {
                    if (huor == Setting.winterNightTime)
                    {
                        dayShift = DayShift.Night;
                        EventHandler.CallDayChenge(dayShift);
                    }
                    else if (huor == Setting.winterDayTime)
                    {
                        dayShift = DayShift.Day;
                        EventHandler.CallDayChenge(dayShift);
                    }
                }

                minute = 0;
                if (huor > Setting.huorThreshold)
                {
                    day++;
                    huor = 0;
                    if (day > Setting.dayThreshold)
                    {
                        seasonNumber++;
                        if (seasonNumber >= 4)
                        {
                            seasonNumber = 0;
                            year++;
                        }
                        season = (Season)seasonNumber;
                        EventHandler.CallSeasonChange(season);
                    }
                    timeTextBuilder.Clear();
                    timeTextBuilder.Append(year).Append("年").Append(day).Append("日");

                    //将构建的字符串设置为UI文本
                    UIManager.Instance.DayText.text = timeTextBuilder.ToString();
                }
                EventHandler.CallHuorUpdate();
            }
            timeTextBuilder.Clear();
            timeTextBuilder.Append(huor).Append(":").Append(minute);

            //将构建的字符串设置为UI文本
            UIManager.Instance.timeText.text = timeTextBuilder.ToString();
        }
    }
}
