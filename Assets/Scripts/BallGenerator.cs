using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallGenerator : MonoBehaviour
{
    // BallのPrefab
    [SerializeField] GameObject ballPrefab = default;
    // Ballの画像を制御する配列
    [SerializeField] Sprite[] ballSprites = default;
    // Bombの画像
    [SerializeField] Sprite bombSprite = default;

    //ボールを生成するクラス
    public IEnumerator Spawns(int count)
    {
        for(int i=0; i < count; i++) {
            Vector2 pos = new Vector2(Random.Range(-0.2f, 0.2f), 8f);
            GameObject ball = Instantiate(ballPrefab, pos, Quaternion.identity);
            // 画像を設定する
            int ballID = Random.Range(0, ballSprites.Length); 

            // もしボムなら ballID = -1
            if (Random.Range(0, 100) < ParamsSO.Entity.bombRate) 
            {
                ballID = -1;
                ball.GetComponent<SpriteRenderer>().sprite = bombSprite;
            }
            // それ以外なら 今までと同じ
            else
            {
                ball.GetComponent<SpriteRenderer>().sprite = ballSprites[ballID];
            }
            ball.GetComponent<Ball>().id = ballID;
            yield return new WaitForSeconds(0.04f);
        }
    }




}
