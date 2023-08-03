using System.Collections;
using UnityEngine;

public static class Tools
{
    public static WaitForSeconds seconds = new WaitForSeconds(0.01f);
    public static bool HaveTheType(this ItemType[] itemTypes, ItemType type)
    {
        for (int i = 0; i < itemTypes.Length; i++)
        {
            if (itemTypes[i] == type) return true;
        }
        return false;
    }
    public static Vector3Int LocalToCell(Vector3 localPosition)
    {
        return new Vector3Int(
             Mathf.FloorToInt(localPosition.x),
             Mathf.FloorToInt(localPosition.y),
             0
         );
    }
    public static Vector3Int LocalToCell(SerializableVector3 localPosition)
    {
        return new Vector3Int(
            Mathf.FloorToInt(localPosition.x),
            Mathf.FloorToInt(localPosition.y),
            0
        );
    }
    /// <summary>
    /// UI逐渐显示
    /// </summary>
    /// <param name="canvasGroup"></param>
    /// <param name="transform"></param>
    /// <returns></returns>
    public static IEnumerator PopUI(CanvasGroup canvasGroup, Transform transform)
    {
        canvasGroup.gameObject.SetActive(true);
        while (canvasGroup.alpha < 1)
        {
            transform.localScale += new Vector3(0.04f, 0.04f, 0);
            canvasGroup.alpha = canvasGroup.alpha + 0.04f;
            yield return seconds;
        }
    }
    /// <summary>
    /// UI逐渐消失
    /// </summary>
    /// <param name="canvasGroup"></param>
    /// <param name="transform"></param>
    /// <returns></returns>
    public static IEnumerator RecycleUI(CanvasGroup canvasGroup, Transform transform)
    {
        while (canvasGroup.alpha > 0)
        {
            transform.localScale -= new Vector3(0.04f, 0.04f, 0);
            canvasGroup.alpha = canvasGroup.alpha - 0.04f;
            yield return seconds;
        }
        canvasGroup.gameObject.SetActive(false);
    }
}
