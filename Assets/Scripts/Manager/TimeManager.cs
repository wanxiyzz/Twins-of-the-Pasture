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
        timeText = UIManager.Instance.text;
        year = 2023;
        huor = 7;
        day = 1;
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
                    //每天更新
                }
                EventHandler.CallHuorUpdate();
            }
        }
        timeTextBuilder.Clear();
        timeTextBuilder.Append(year).Append("年  ").Append(season).Append("   ").Append(day).Append("日  ").Append(huor).Append(":").Append(minute);

        //将构建的字符串设置为UI文本
        UIManager.Instance.text.text = timeTextBuilder.ToString();
    }
}
