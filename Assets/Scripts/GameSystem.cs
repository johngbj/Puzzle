using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameSystem : MonoBehaviour
{

    [SerializeField] BallGenerator ballGenerator = default;
    bool isDragging;
    [SerializeField] List<Ball> removeBalls = new List<Ball>();
    Ball currentDraggingBall;
    int score;
    [SerializeField] Text scoreText = default;
    [SerializeField] GameObject pointEffectPrefab = default;
    [SerializeField] Text timerText = default;
    int timeCount;
    [SerializeField]GameObject retryPanel = default;
    
    //Restartボタン表示時に背後を動かさないために設定
    bool gameOver;
        
    /*
    開始処理でやること
    ・スコアをゼロにリセット
    ・タイムを設定
    ・フレームレートを設定
    ・ボールを生成
    ・タイムカウントを開始
   */
    void Start()
    {
        score = 0;
        timeCount = ParamsSO.Entity.playPeriod;
        AddScore(0);
        Application.targetFrameRate = 60;
        StartCoroutine(ballGenerator.Spawns(ParamsSO.Entity.initBallCount));
        StartCoroutine(CountDown());
        Debug.Log("game start");
    }


    //タイムカウントダウン（timeCountをだんだん小さくする）
    IEnumerator CountDown()
    {
        while(timeCount > 0)
        {
            yield return new WaitForSeconds(1);
            timeCount--;
            timerText.text = timeCount.ToString();
        }
        Debug.Log("time up");
        gameOver = true;
        retryPanel.SetActive(true);
    }

　//リトライボタンでシーンを再読込して再度ゲームを実施
    public void OnRetryButton()
    {
        SceneManager.LoadScene("Game");
    }

    void AddScore(int point)
    {
        score += point;
        scoreText.text = score.ToString();
    }

    void Update()
    {
        //ゲームが終わってたら処理から抜ける
        if(gameOver)
        {
            return;
        }

        //マウスボタンを制御する重要な部分
        if (Input.GetMouseButtonDown(0))
        {
            // 右クリックを押し込んだ時
            OnDragBigin();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            // 右クリックを離した時
            OnDragEnd();
        }
        else if (isDragging)
        {
            //ドラッグしている間
            OnDragging();
        }
    }

    void OnDragBigin()
    {
        // マウスによるオブジェクトの判定
        // ・Ray
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
        if (hit && hit.collider.GetComponent<Ball>())
        {
            Ball ball = hit.collider.GetComponent<Ball>();
            //ボムの判定
            if (ball.IsBomb())
            {
                // ボムなら周囲を含めて爆破
                Explosion(ball);
            }
            else
            {
                AddRemoveBall(ball);
                isDragging = true;
            }
        }
    }
    void OnDragging()
    {
        // Debug.Log("ドラッグ中");
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
        if (hit && hit.collider.GetComponent<Ball>())
        {
            // ・同じ種類&距離が近かったらListに追加
            //  ・何と？=>現在ドラッグしているオブジェクトと
            Ball ball = hit.collider.GetComponent<Ball>();

            // 同じ種類
            if (ball.id == currentDraggingBall.id)
            {
                // 距離が近い
                float distance = Vector2.Distance(ball.transform.position, currentDraggingBall.transform.position);
                if (distance < ParamsSO.Entity.ballDistance)
                {
                    AddRemoveBall(ball);
                }
            }
        }
    }
    void OnDragEnd()
    {
        int removeCount = removeBalls.Count;
        if (removeCount >= 3)
        {
            for (int i = 0; i < removeCount; i++)
            {
                removeBalls[i].Explosion();
            }
            StartCoroutine(ballGenerator.Spawns(removeCount));
            int score = removeCount * ParamsSO.Entity.scorePoint;
            AddScore(score);
            SpawnPointEffect(removeBalls[removeBalls.Count-1].transform.position, score);

        }
        // 全てのremoveBallのサイズを戻す
        for (int i = 0; i < removeCount; i++)
        {
            removeBalls[i].GetComponent<SpriteRenderer>().color = Color.white;
            removeBalls[i].transform.localScale = Vector3.one*2.0f;
        }
        removeBalls.Clear();
        isDragging = false;
    }

    void AddRemoveBall(Ball ball)
    {
        currentDraggingBall = ball;
        if (removeBalls.Contains(ball) == false)
        {
            ball.transform.localScale = Vector3.one * 2.8f;
            ball.GetComponent<SpriteRenderer>().color = Color.gray;
            removeBalls.Add(ball);
        }
    }

    // bombによる爆破
    void Explosion(Ball bomb)
    {
        List<Ball> explosionList = new List<Ball>();
        // ボムを中心に爆破するBallを集める
        Collider2D[] hitObj = Physics2D.OverlapCircleAll(bomb.transform.position, ParamsSO.Entity.bombRange);
        for (int i = 0; i < hitObj.Length; i++)
        {
            // Ballだったら爆破リストに追加する
            Ball ball = hitObj[i].GetComponent<Ball>();
            if (ball)
            {
                explosionList.Add(ball);
            }
        }
        // 爆破する

        int removeCount = explosionList.Count;
        for (int i = 0; i < removeCount; i++)
        {
            explosionList[i].Explosion();
        }
        StartCoroutine(ballGenerator.Spawns(removeCount));
        int score = removeCount * ParamsSO.Entity.scorePoint;
        AddScore(score);
        //ポイントエフェクト
        SpawnPointEffect(bomb.transform.position, score);
    }
    
//ポイントエフェクトを表示するクラス
    void SpawnPointEffect(Vector2 position, int score)
    {
        GameObject effectObj = Instantiate(pointEffectPrefab, position, Quaternion.identity);
        PointEffect pointEffect = effectObj.GetComponent<PointEffect>();
        pointEffect.Show(score);
    }

}