using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    //�^�C�g���A�{�^������������Game�֑J��

    public void OnStartButton()
    {
        Debug.Log("go to game");
        SceneManager.LoadScene("Game");
    }


}
