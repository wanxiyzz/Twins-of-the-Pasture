using System.Security.Cryptography.X509Certificates;
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
        EventHandler.SeasonChange += OnSeasonChange;
    }
    private void OnDisable()
    {
        EventHandler.SeasonChange -= OnSeasonChange;
    }

    private void OnSeasonChange(Season season)
    {
        currentSeason = season;
        if (sceneType == SceneType.Field)
        {
            TransitionManager.Instance.GameLoadingAnim(() =>
            {
                OnAfterSceneLoad(sceneType, TileManager.Instance.currentSceneMiddleTiles);
            });
        }
    }
    /// <summary>
    /// 场景切换时调用
    /// </summary>
    public void OnAfterSceneLoad(SceneType sceneType, MiddleTileList list)
    {
        currentSeason = TimeManager.Instance.season;
        this.sceneType = sceneType;
        gridMap = FindObjectOfType<GridMap>();
        GrassManager.Instance.OnAfterSceneLoad(sceneType, TransitionManager.Instance.currentSceneName, gridMap.top);
        if (sceneType == SceneType.Field)
            gridMap.ChangeAllTiles(ruleTiles[(int)currentSeason], list);
    }
}

