using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ViewShopComponent : MonoBehaviour
{


    public enum EComponent
    {
        Gold = 0,
        Gem = 1,
        Chest = 2,
    }


    [Header("[Component]")]
    [SerializeField] private EComponent _component;
    [SerializeField] private ComponentShopGold compGold;
    [SerializeField] private ComponentShopGem compGem;
    [SerializeField] private ComponentShopChest compChest;


    [Header("[Text]")]
    [SerializeField] private TMP_Text txtGem;
    [SerializeField] private TMP_Text txtGold;


    [Header("[Button]")]
    [SerializeField] private List<UIButtonClickHover> buttonTabs;


    // [private]
    private ComponentState _stateComponent;




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
        compGold.Load();
        compGem.Load();
        compChest.Load();

        LoadData();

        // setup view - buttons
        SetViewComponent(_component);
        UpdateButtonChoosen();
    }



    private void LoadData()
    {
        var user = GameManager.Instance.Data.UserInfo;
        txtGem.SetText(user.gem.ToString());
        txtGold.SetText(user.gold.ToString());
    }


    public void SetViewComponent(EComponent character)
    {
        _component = character;
        switch (_component)
        {
            case EComponent.Gold:
                SetCurrentComponent(compGold); break;
            case EComponent.Gem:
                SetCurrentComponent(compGem); break;
            case EComponent.Chest:
                SetCurrentComponent(compChest); break;
        }
    }


    private void SetCurrentComponent(ComponentState newComponent)
    {
        if (_stateComponent != null)
            _stateComponent.EndState();

        _stateComponent = newComponent;
        _stateComponent.StartState();
        SetActiveComponent(_component.ToString());
    }


    private void SetActiveComponent(string nameComponent)
    {
        compGold.gameObject.SetActive(compGold.name.Contains(nameComponent));
        compGem.gameObject.SetActive(compGem.name.Contains(nameComponent));
        compChest.gameObject.SetActive(compChest.name.Contains(nameComponent));
    }


    private void ClearButtonSelected()
    {
        buttonTabs.ForEach(x => x.OnChoosenUnselect());
    }


    private void UpdateButtonChoosen()
    {
        buttonTabs.ForEach(x => x.OnChoosenUnselect());
        buttonTabs[(int)_component]?.OnChoosenSelect();
    }



    public void OnClickButtonChest()
    {
        SetViewComponent(EComponent.Chest);
        ClearButtonSelected();
        UpdateButtonChoosen();
    }


    public void OnClickButtonGold()
    {
        SetViewComponent(EComponent.Gold);
        ClearButtonSelected();
        UpdateButtonChoosen();
    }


    public void OnClickButtonGem()
    {
        SetViewComponent(EComponent.Gem);
        ClearButtonSelected();
        UpdateButtonChoosen();
    }



}
