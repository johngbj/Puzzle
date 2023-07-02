using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallGenerator : MonoBehaviour
{
    // Ball��Prefab
    [SerializeField] GameObject ballPrefab = default;
    // Ball�̉摜�𐧌䂷��z��
    [SerializeField] Sprite[] ballSprites = default;
    // Bomb�̉摜
    [SerializeField] Sprite bombSprite = default;

    //�{�[���𐶐�����N���X
    public IEnumerator Spawns(int count)
    {
        for(int i=0; i < count; i++) {
            Vector2 pos = new Vector2(Random.Range(-0.2f, 0.2f), 8f);
            GameObject ball = Instantiate(ballPrefab, pos, Quaternion.identity);
            // �摜��ݒ肷��
            int ballID = Random.Range(0, ballSprites.Length); 

            // �����{���Ȃ� ballID = -1
            if (Random.Range(0, 100) < ParamsSO.Entity.bombRate) 
            {
                ballID = -1;
                ball.GetComponent<SpriteRenderer>().sprite = bombSprite;
            }
            // ����ȊO�Ȃ� ���܂łƓ���
            else
            {
                ball.GetComponent<SpriteRenderer>().sprite = ballSprites[ballID];
            }
            ball.GetComponent<Ball>().id = ballID;
            yield return new WaitForSeconds(0.04f);
        }
    }




}
