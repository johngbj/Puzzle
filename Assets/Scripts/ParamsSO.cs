using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ParamsSO : ScriptableObject
{

    [Header("�v���C�b��")]
    public int playPeriod;

    [Header("�����̃{�[���̐�")]
    public int initBallCount;

    [Header("�{�[�������������̓��_")]
    public int scorePoint;

    [Header("�{�[���̔��苗��")]
    public float ballDistance;

    [Header("Bomb�̔����m����")]
    [Range(1, 100)]
    public float bombRate;

    [Header("Bomb�͈̔�")]
    [Range(1, 10)]
    public float bombRange;

    //MyScriptableObject���ۑ����Ă���ꏊ�̃p�X
    public const string PATH = "ParamsSO";

    //MyScriptableObject�̎���
    private static ParamsSO _entity;
    public static ParamsSO Entity
    {
        get
        {
            //���A�N�Z�X���Ƀ��[�h����
            if (_entity == null)
            {
                _entity = Resources.Load<ParamsSO>(PATH);

                //���[�h�o���Ȃ������ꍇ�̓G���[���O��\��
                if (_entity == null)
                {
                    Debug.LogError(PATH + " not found");
                }
            }

            return _entity;
        }
    }
}