using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public int id;
    [SerializeField] GameObject explosionPrefab = default;
    public void Explosion()
    {
        Destroy(gameObject);
        // 爆破エフェクトを生成して破壊
        GameObject explosion = Instantiate(explosionPrefab, transform.position, transform.rotation);
        Destroy(explosion, 0.2f);

    }
    public bool IsBomb()
    {
        //if (id == -1)
        //{
        //    return true;
        //}
        //return false;

        return id == -1;
    }
}