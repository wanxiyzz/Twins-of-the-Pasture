using MyGame.Player;
using MyGame.Buleprint;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public Player player;
    public CheckBox playerCheck;
    public bool playerCanPick = true;
    public InventoryPlaceable currentPickItem;

    public bool isWin;
    public int screenHeight;
    public float toolTipHeight;

    //动物和植物房间出去的位置
    public string outSceneName;
    public Vector3 backPosition;
    protected override void Awake()
    {
        base.Awake();
        isWin = Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor;
        screenHeight = Screen.height;
        toolTipHeight = screenHeight * 0.138889f;
    }
}
