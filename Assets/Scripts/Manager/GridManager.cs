using System.Collections.Generic;
using UnityEngine;

public class GridManager : Singleton<GridManager>
{
    public GridMap gridMap;
    public Season currentSeason;
    public RuleTile[] ruleTiles;
    private void OnEnable()
    {
        EventHandler.seasonChange += OnSeasonChange;
        EventHandler.afterSceneLoadEvent += OnAfterSceneLoadEvent;
    }
    private void OnDisable()
    {
        EventHandler.seasonChange -= OnSeasonChange;
        EventHandler.afterSceneLoadEvent -= OnAfterSceneLoadEvent;
    }

    private void OnSeasonChange(Season season)
    {
        currentSeason = season;
        gridMap.ChangeAllTiles(ruleTiles[(int)season]);
    }

    private void OnAfterSceneLoadEvent()
    {
        gridMap = FindObjectOfType<GridMap>();
        if (currentSeason != Season.春)
        {
            gridMap.ChangeAllTiles(ruleTiles[(int)currentSeason]);
        }

    }
    public bool CheckMiddleTile(Vector3Int pos)
    {
        if (gridMap.middle != null && gridMap.middle.GetTile(pos) != null)
            return false;
        return true;
    }
}

