using System.Collections.Generic;
using UnityEngine;

public class GridManager : Singleton<GridManager>
{
    public GridMap gridMap;
    public Season currentSeason;
    public RuleTile[] ruleTiles;
    SceneType sceneType;
    private void OnEnable()
    {
        EventHandler.seasonChange += OnSeasonChange;
    }
    private void OnDisable()
    {
        EventHandler.seasonChange -= OnSeasonChange;
    }

    private void OnSeasonChange(Season season)
    {
        currentSeason = season;
        if (sceneType == SceneType.Field)
        {
            TransitionManager.Instance.GameLoadingAnim(() =>
            {
                ChangeSceneMiddleTiles(sceneType, TileManager.Instance.currentSceneMiddleTiles);
            });
        }
    }

    public void ChangeSceneMiddleTiles(SceneType sceneType, MiddleTileList list)
    {
        currentSeason = TimeManager.Instance.season;
        this.sceneType = sceneType;
        gridMap = FindObjectOfType<GridMap>();
        if (sceneType == SceneType.Field)
            gridMap.ChangeAllTiles(ruleTiles[(int)currentSeason], list);
    }
}

