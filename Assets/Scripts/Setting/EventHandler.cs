using UnityEngine;
using System;

public class EventHandler
{
    public static event Action<Season> SeasonChange;
    public static void CallSeasonChange(Season season)
    {
        SeasonChange?.Invoke(season);
    }
    public static event Action<DayShift> DayChange;
    public static void CallDayChenge(DayShift shift)
    {
        DayChange?.Invoke(shift);
    }
    public static event Action<Vector3> MoveToPosition;
    public static void CallMoveToPosition(Vector3 targetPos)
    {
        MoveToPosition?.Invoke(targetPos);
    }
    public static event Action<string, Vector3> TransitionEvent;
    public static void CallTransitionEvent(string sceneToGO, Vector3 tartgetPos)
    {
        TransitionEvent?.Invoke(sceneToGO, tartgetPos);
    }
    public static event Action<SceneType, string> AfterSceneLoadEvent;
    public static void CallAfterSceneLoadEvent(SceneType sceneType, string sceneName)
    {
        AfterSceneLoadEvent?.Invoke(sceneType, sceneName);
    }
    public static event Action BeforeSceneLoadEvent;
    public static void CallBeforeSceneLoadEvent()
    {
        BeforeSceneLoadEvent?.Invoke();
    }
    public static event Action HourUpdate;
    public static void CallHuorUpdate()
    {
        HourUpdate?.Invoke();
    }
    public static event Action PlanUpdate;
    public static void CallPlanUpdate()
    {
        PlanUpdate?.Invoke();
    }
    public static event Action<Vector3, ItemDetails> UseItemEvent;
    public static void CallUseItemEvent(Vector3 pos, ItemDetails itemDetails)
    {
        UseItemEvent?.Invoke(pos, itemDetails);
    }
    public static event Action<Vector3Int, int> PlantAPlant;
    public static void CallPlantAPlant(Vector3Int pos, int seedID)
    {
        PlantAPlant?.Invoke(pos, seedID);
    }
    public static event Action<ItemDetails, bool> SelectItemEvent;
    public static void CallSelectItemEvent(ItemDetails item, bool isSelected)
    {
        SelectItemEvent?.Invoke(item, isSelected);
    }
    public static event Action<WeatherType> WeatherChange;
    public static void CallWeatherChange(WeatherType weather)
    {
        WeatherChange?.Invoke(weather);
    }
    public static event Action<ToolType> PickUpTool;
    public static void CallPickUpTool(ToolType toolType)
    {
        PickUpTool?.Invoke(toolType);
    }
    public static event Action<Sprite> PickPlaceable;
    public static void CallPickPlaceable(Sprite sprite)
    {
        PickPlaceable?.Invoke(sprite);
    }
    public static event Action<ToolType, Vector3> UseTool;
    public static void CallUseTool(ToolType toolType, Vector3 pos)
    {
        UseTool?.Invoke(toolType, pos);
    }


}
