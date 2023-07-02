using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    //タイトル、ボタンを押したらGameへ遷移

    public void OnStartButton()
    {
        Debug.Log("go to game");
        SceneManager.LoadScene("Game");
    }


}
