using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventHandler
{
    public static event Action<Season> seasonChange;
    public static void CallSeasonChange(Season season)
    {
        seasonChange?.Invoke(season);
    }
    public static event Action<DayShift> dayChange;
    public static void CallDayChenge(DayShift shift)
    {
        dayChange?.Invoke(shift);
    }
    public static event Action<Vector3> moveToPosition;
    public static void CallMoveToPosition(Vector3 targetPos)
    {
        moveToPosition?.Invoke(targetPos);
    }
    public static event Action<string, Vector3> transitionEvent;
    public static void CallTransitionEvent(string sceneToGO, Vector3 tartgetPos)
    {
        transitionEvent?.Invoke(sceneToGO, tartgetPos);
    }
    public static event Action<SceneType, string> afterSceneLoadEvent;
    public static void CallAfterSceneLoadEvent(SceneType sceneType, string sceneName)
    {
        afterSceneLoadEvent?.Invoke(sceneType, sceneName);
    }
    public static event Action beforeSceneLoadEvent;
    public static void CallBeforeSceneLoadEvent()
    {
        beforeSceneLoadEvent?.Invoke();
    }
    public static event Action hourUpdate;
    public static void CallHuorUpdate()
    {
        hourUpdate?.Invoke();
    }
    public static event Action planUpdate;
    public static void CallPlanUpdate()
    {
        planUpdate?.Invoke();
    }
    public static event Action<Vector3, ItemDetails> useItemEvent;
    public static void CallUseItemEvent(Vector3 pos, ItemDetails itemDetails)
    {
        useItemEvent?.Invoke(pos, itemDetails);
    }
    public static event Action<Vector3Int, int> plantAPlant;
    public static void CallPlantAPlant(Vector3Int pos, int seedID)
    {
        plantAPlant?.Invoke(pos, seedID);
    }
    public static event Action<ItemDetails, bool> SelectItemEvent;
    public static void CallSelectItemEvent(ItemDetails item, bool isSelected)
    {
        SelectItemEvent?.Invoke(item, isSelected);
    }

}
