using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using TMPro;

public class ViewLobbyComponent : MonoBehaviour
{


    [Header("[Component]")]
    [SerializeField] private ComponentLobbyEditActor compChangeActor;


    [Header("[Text]")]
    [SerializeField] private TMP_Text txtUsername;
    [SerializeField] private TMP_Text txtUserLevel;
    [SerializeField] private TMP_Text txtGem;
    [SerializeField] private TMP_Text txtGold;


    [Header("[Setting]")]
    [SerializeField] private List<UIButtonEditActor> ui_BtnEditActors;


    [Header("[DATA]")]
    [SerializeField] private List<DataActor> actors;
    [SerializeField] private List<DataFormation> formations;
    [SerializeField] private List<UIModelRotator> ui_Rotators;


    // [private]
    private int _editIndex;
    private string _editKey;



    #region  UNITY
    // private void Start()
    // {
    // }

    // private void Update()
    // {
    // }
    #endregion




    public void Load()
    {
        actors = GameManager.Instance.Data.GetActors();
        formations = GameManager.Instance.Data.GetFormation();
        ui_Rotators = UIMenuScene.Instance.Model.Ui_Rotators;

        LoadData();
        LoadComponent();
        LoadUIFormation();
        LoadUIButtonEdit();
    }


    private void LoadData()
    {
        var user = GameManager.Instance.Data.UserInfo;
        txtUsername.SetText(user.displayName.ToString());
        txtUserLevel.SetText(user.level.ToString());
        txtGem.SetText(user.gem.ToString());
        txtGold.SetText(user.gold.ToString());
    }


    private void LoadComponent()
    {
        compChangeActor.Load(Callback_ClickItem, Callback_ClickConfirm);
    }


    public void LoadUIFormation()
    {
        if (formations == null || formations.Count < 0)
            return;

        for (int i = 0; i < ui_Rotators.Count; i++)
        {
            var key = Utils.GetDataActorKey(formations[i].actor);

            ui_Rotators[i].Show();
            ui_Rotators[i].LoadModel(key);
        }
    }


    private void LoadUIButtonEdit()
    {
        // init ui edit btn actors
        for (int i = 0; i < ui_BtnEditActors.Count; i++)
        {
            var key = Utils.GetDataActorKey(formations[i].actor);

            ui_BtnEditActors[i].Init(i, ui_Rotators[i], Callback_ClickBtnEdit);
            ui_BtnEditActors[i].LoadModel(key);
        }
    }


    private void UnLoadModelUiEditActor(string key)
    {
        foreach (var ui in ui_BtnEditActors)
        {
            if (string.IsNullOrEmpty(ui.Key))
                continue;

            if (ui.Key.Equals(key))
                ui.UnloadModel();
        }
    }




    private void Callback_ClickBtnEdit(string key, int index)
    {
        _editKey = key;
        _editIndex = index;

        compChangeActor.Show();
        compChangeActor.LoadActorStatus(GameManager.Instance.Data.GetFormation());
        // print($"Callback_ClickButtonEdit: key: {key} | index: {index}");
    }


    private void Callback_ClickItem(string item)
    {
        // print($"Callback_ClickItem: " + item);
    }


    private void Callback_ClickConfirm(string key)
    {
        print($"Callback_ClickConfirm: key: {key} | _editKey: {_editKey} | _editIndex: {_editIndex}");

        if (string.IsNullOrEmpty(key) == false)
        {
            var item = formations.Find(x => x.index == _editIndex);
            print(item);
            if (item != null)
            {
                GameManager.Instance.Data.RemoveFormationByKey(key);
                UnLoadModelUiEditActor(key);

                GameManager.Instance.Data.SaveFormationWithIndex(key, _editIndex);
                ui_BtnEditActors[_editIndex].LoadModel(key);
            }
        }
    }



}
