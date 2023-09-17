using UnityEngine;
using UnityEngine.Tilemaps;
using MyGame.GameTime;
public class GridMap : MonoBehaviour
{
    public Tilemap bottom;
    public Tilemap middle;
    public Tilemap top;
    public Tilemap water;

    public Tilemap wall;

    public MiddleTileList middleTileList;
    public TileDetailsDataList dataList;

    public Vector2Int bottomLeft;
    public Vector2Int topRight;
    int num;
    bool night = true;
    private void Update()
    {
        //TEST
        if (Input.GetKeyDown(KeyCode.Q))
        {
            EventHandler.CallSeasonChange((Season)num);
            TimeManager.Instance.season = (Season)num;
            num++;
            if (num > 3)
            {
                num = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (night)
                EventHandler.CallDayChenge(DayShift.Night);
            else
                EventHandler.CallDayChenge(DayShift.Day);
            night = !night;
        }
    }
    /// <summary>
    /// 切换季节草坪
    /// </summary>
    /// <param name="tile"></param>
    public void ChangeAllTiles(RuleTile tile, TileDetailsDataList list)
    {
        middle.ClearAllTiles();
        foreach (var item in list.gameTileDataList)
        {
            foreach (var tileDetails in item.tiles)
            {
                if (tileDetails.haveTop)
                    middle.SetTile(tileDetails.position.ToVector3Int(), tile);
            }
        }
    }

}
