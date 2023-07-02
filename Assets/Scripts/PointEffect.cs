using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class PointEffect : MonoBehaviour
{
    //スコアに応じて表示を変更

    [SerializeField] Text text = default;
    public void Show(int score)
    {
        text.text = score.ToString();
        StartCoroutine(MoveUp());
    }
    // 上にあげる
    IEnumerator MoveUp()
    {
        for (int i = 0; i < 20; i++)
        {
            yield return null;
            transform.Translate(0, 0.1f, 0);
        }
        Destroy(gameObject, 0.2f);
    }



}
