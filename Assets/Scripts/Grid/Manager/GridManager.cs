using UnityEngine;
using MyGame.GrassSystem;
using UnityEngine.UI;

namespace MyGame.Tile
{
    public class GridManager : Singleton<GridManager>
    {
        public GridMap gridMap;
        public Season currentSeason;
        public RuleTile[] ruleTiles;
        public RuleTile digTile;
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
                    OnAfterSceneLoad(sceneType, TileManager.Instance.currentSceneButtomTiles);
                });
            }
        }
        /// <summary>
        /// 场景切换时调用
        /// </summary>
        public void OnAfterSceneLoad(SceneType sceneType, TileDetailsDataList list)
        {
            this.sceneType = sceneType;
            gridMap = FindObjectOfType<GridMap>();
            GrassManager.Instance.OnAfterSceneLoad(sceneType, TransitionManager.Instance.currentSceneName, gridMap.top);
            if (sceneType == SceneType.Field)
                gridMap.ChangeAllTiles(ruleTiles[(int)currentSeason], list);
        }
        public void DigPostion(Vector3Int pos)
        {
            gridMap.middle.SetTile(pos, digTile);
        }
    }

}