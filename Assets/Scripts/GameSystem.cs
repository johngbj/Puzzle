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
    
    //Restart�{�^���\�����ɔw��𓮂����Ȃ����߂ɐݒ�
    bool gameOver;
        
    /*
    �J�n�����ł�邱��
    �E�X�R�A���[���Ƀ��Z�b�g
    �E�^�C����ݒ�
    �E�t���[�����[�g��ݒ�
    �E�{�[���𐶐�
    �E�^�C���J�E���g���J�n
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


    //�^�C���J�E���g�_�E���itimeCount�����񂾂񏬂�������j
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

�@//���g���C�{�^���ŃV�[�����ēǍ����čēx�Q�[�������{
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
        //�Q�[�����I����Ă��珈�����甲����
        if(gameOver)
        {
            return;
        }

        //�}�E�X�{�^���𐧌䂷��d�v�ȕ���
        if (Input.GetMouseButtonDown(0))
        {
            // �E�N���b�N���������񂾎�
            OnDragBigin();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            // �E�N���b�N�𗣂�����
            OnDragEnd();
        }
        else if (isDragging)
        {
            //�h���b�O���Ă����
            OnDragging();
        }
    }

    void OnDragBigin()
    {
        // �}�E�X�ɂ��I�u�W�F�N�g�̔���
        // �ERay
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
        if (hit && hit.collider.GetComponent<Ball>())
        {
            Ball ball = hit.collider.GetComponent<Ball>();
            //�{���̔���
            if (ball.IsBomb())
            {
                // �{���Ȃ���͂��܂߂Ĕ��j
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
        // Debug.Log("�h���b�O��");
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
        if (hit && hit.collider.GetComponent<Ball>())
        {
            // �E�������&�������߂�������List�ɒǉ�
            //  �E���ƁH=>���݃h���b�O���Ă���I�u�W�F�N�g��
            Ball ball = hit.collider.GetComponent<Ball>();

            // �������
            if (ball.id == currentDraggingBall.id)
            {
                // �������߂�
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
        // �S�Ă�removeBall�̃T�C�Y��߂�
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

    // bomb�ɂ�锚�j
    void Explosion(Ball bomb)
    {
        List<Ball> explosionList = new List<Ball>();
        // �{���𒆐S�ɔ��j����Ball���W�߂�
        Collider2D[] hitObj = Physics2D.OverlapCircleAll(bomb.transform.position, ParamsSO.Entity.bombRange);
        for (int i = 0; i < hitObj.Length; i++)
        {
            // Ball�������甚�j���X�g�ɒǉ�����
            Ball ball = hitObj[i].GetComponent<Ball>();
            if (ball)
            {
                explosionList.Add(ball);
            }
        }
        // ���j����

        int removeCount = explosionList.Count;
        for (int i = 0; i < removeCount; i++)
        {
            explosionList[i].Explosion();
        }
        StartCoroutine(ballGenerator.Spawns(removeCount));
        int score = removeCount * ParamsSO.Entity.scorePoint;
        AddScore(score);
        //�|�C���g�G�t�F�N�g
        SpawnPointEffect(bomb.transform.position, score);
    }
    
//�|�C���g�G�t�F�N�g��\������N���X
    void SpawnPointEffect(Vector2 position, int score)
    {
        GameObject effectObj = Instantiate(pointEffectPrefab, position, Quaternion.identity);
        PointEffect pointEffect = effectObj.GetComponent<PointEffect>();
        pointEffect.Show(score);
    }

}