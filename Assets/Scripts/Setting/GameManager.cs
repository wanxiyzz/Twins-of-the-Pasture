using MyGame.Player;
using MyGame.Buleprint;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public Player player;
    public bool playerCanPick = true;
    public InventoryPlaceable currentPickItem;

    public string currentPickBoxKey;

    public bool isWin;
    public int screenHeight;
    public float toolTipHeight;
    protected override void Awake()
    {
        base.Awake();
        isWin = Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor;
        screenHeight = Screen.height;
        toolTipHeight = screenHeight * 0.138889f;
    }
}
