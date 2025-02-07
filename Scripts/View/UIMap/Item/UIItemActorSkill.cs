using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItemActorSkill : MonoBehaviour
{


    [Header("[Setting]")]
    [SerializeField] private Image imgAvatar;


    // [private]
    private DataActorSkill _skill;
    private Action<string> _cbClicked;



    public void Init(DataActorSkill skill, Action<string> callback)
    {
        _skill = skill;
        _cbClicked = callback;

        LoadData();
    }


    private void LoadData()
    {
        imgAvatar.sprite = GameController.Instance.LoadSpriteActorSkill(_skill.key);
        imgAvatar.SetActive(true);
    }


    public void OnClickButtonItem()
    {
        _cbClicked.CheckInvoke(_skill.key);
    }


}
