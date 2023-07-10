using UnityEngine;

public class PlayerTools : MonoBehaviour
{
    [SerializeField] ToolAnimator[] toolAnimators;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        EventHandler.PickUpTool += SwitchToolAnimation;
    }
    private void OnDisable()
    {
        EventHandler.PickUpTool -= SwitchToolAnimation;
    }
    private void Update()
    {
        //TEST
        if (Input.GetKeyDown(KeyCode.L))
        {
            SwitchToolAnimation(ToolType.Hoe);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            EventHandler.CallUseTool(ToolType.Axe, transform.position);
        }
    }
    public void SwitchToolAnimation(ToolType type)
    {
        for (int i = 0; i < toolAnimators.Length; i++)
        {
            if (toolAnimators[i].type == type)
                animator.runtimeAnimatorController = toolAnimators[i].animator;
        }
    }
}
[System.Serializable]
public class ToolAnimator
{
    public ToolType type;
    public AnimatorOverrideController animator;

}
