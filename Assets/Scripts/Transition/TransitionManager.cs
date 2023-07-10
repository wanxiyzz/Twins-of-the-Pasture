using System.Collections;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using MyGame.Cursor;

public class TransitionManager : Singleton<TransitionManager>
{
    public string currentSceneName;
    public CanvasGroup canvasGroup;
    public SceneType sceneType;
    Color fadeIn = new Color(0.1f, 0.1f, 0.1f, 1);
    Color fadeOut = new Color(0.1f, 0.1f, 0.1f, 0);
    private bool inFade;
    protected override void Awake()
    {
        base.Awake();
        StartCoroutine(LoadSceneSetActive("01.Field"));

    }
    private void OnEnable()
    {
        EventHandler.TransitionEvent += OnTransitionEvent;
    }

    private void OnDisable()
    {
        EventHandler.TransitionEvent -= OnTransitionEvent;
    }

    private void OnTransitionEvent(string sceneToGo, Vector3 pos)
    {
        StartCoroutine(Transition(sceneToGo, pos));
    }
    /// <summary>
    /// 游戏开始时使用
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    IEnumerator LoadSceneSetActive(string sceneName)
    {
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        sceneType = sceneName switch
        {
            "01.Field" => SceneType.Field,
            "02.Home" => SceneType.MyHuose,
            _ => SceneType.Field,

        };
        Scene newScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
        SceneManager.SetActiveScene(newScene);
        currentSceneName = newScene.name;
        EventHandler.CallAfterSceneLoadEvent(sceneType, sceneName);
    }
    /// <summary>
    /// 场景切换
    /// </summary>
    /// <param name="sceneName">场景名称</param>
    /// <param name="targetPosition">目标位置</param>
    /// <returns></returns>
    IEnumerator Transition(string sceneName, Vector3 pos)
    {
        yield return FadeIn();
        EventHandler.CallBeforeSceneLoadEvent();
        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        Debug.Log("加载了" + sceneName);
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
        currentSceneName = sceneName;
        //WORKFLOW:添加场景时添加
        sceneType = sceneName switch
        {
            "01.Field" => SceneType.Field,
            "02.Home" => SceneType.MyHuose,
            _ => SceneType.Field,

        };
        EventHandler.CallAfterSceneLoadEvent(sceneType, sceneName);
        EventHandler.CallMoveToPosition(pos);
        yield return new WaitForSeconds(0.3f);

        yield return FadeOut();
    }
    /// <summary>
    /// 逐渐变黑
    /// </summary>
    /// <returns></returns>
    IEnumerator FadeIn()
    {
        while (inFade)
        {
            yield return new WaitForEndOfFrame();
        }
        CursorManager.Instance.cursorEnable = false;
        inFade = true;
        CursorManager.Instance.checkImage.SetActive(false);
        GameManager.Instance.player.playerInput = false;
        while (true)
        {
            if (canvasGroup.alpha >= 1)
                break;
            canvasGroup.alpha = canvasGroup.alpha + 0.05f;
            yield return new WaitForFixedUpdate();
        }
        GC.Collect();
    }
    /// <summary>
    /// 逐渐变透明
    /// </summary>
    /// <returns></returns>
    IEnumerator FadeOut()
    {
        while (true)
        {
            if (canvasGroup.alpha <= 0)
                break;
            canvasGroup.alpha = canvasGroup.alpha - 0.05f;
            yield return new WaitForFixedUpdate();
        }
        GameManager.Instance.player.playerInput = true;
        inFade = false;
    }
    public void GameLoadingAnim(Action a)
    {
        StartCoroutine(GameLoading(a));
    }
    /// <summary>
    /// 启用加载动画
    /// </summary>
    private IEnumerator GameLoading(Action a)
    {
        yield return FadeIn();
        a();
        yield return new WaitForSeconds(0.3f);
        yield return FadeOut();
    }

}
