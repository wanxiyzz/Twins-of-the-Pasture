using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridMap : MonoBehaviour
{
    public Tilemap bottom;
    public Tilemap middle;
    public Tilemap top;
    public Tilemap water;
    public Vector2Int bottomLeft;
    public Vector2Int topRight;
    int num;
    bool night;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            EventHandler.CallSeasonChange((Season)num);
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
    public void ChangeAllTiles(RuleTile tile)
    {
        for (int i = bottomLeft.x; i <= topRight.x; i++)
        {
            for (int j = bottomLeft.y; j <= topRight.y; j++)
            {
                Vector3Int pos = new Vector3Int(i, j, 0);
                if (middle.GetTile(pos) != null)
                {
                    middle.SetTile(pos, tile);
                }
            }
        }
    }

}
