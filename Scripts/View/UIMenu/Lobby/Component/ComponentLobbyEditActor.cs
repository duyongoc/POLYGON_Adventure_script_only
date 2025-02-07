using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ComponentLobbyEditActor : MonoBehaviour
{


    [Header("[Setting]")]
    [SerializeField] private float scrollDefault = 100;
    [SerializeField] private float scrollOffset = 100;
    [SerializeField] private Transform scrollContent;
    [SerializeField] private UIItemActor prefabItem;
    [SerializeField] private List<UIItemActor> cachePrefabs;


    // [private]
    private string _key;
    private bool _itemLocked;
    private Action<string> _cbConfirmed;
    private Action<string> _cbItemClicked;



    public void Load(Action<string> cbItemClick, Action<string> cbConfirm)
    {
        _cbConfirmed = cbConfirm;
        _cbItemClicked = cbItemClick;

        LoadActor(GameManager.Instance.Data.GetActors());
        LoadActorStatus(GameManager.Instance.Data.GetFormation());
    }


    private void LoadActor(List<DataActor> actors)
    {
        // clear prefab cache
        ClearCache();
        ResetScrollSize(scrollDefault);

        // create prefab
        foreach (var item in actors)
        {
            var prefab = Instantiate(prefabItem, scrollContent.transform, false);
            prefab.Init(item, Callback_ItemClick);

            cachePrefabs.Add(prefab);
            ChangeScrollSize(scrollOffset);
        }
    }


    public void LoadActorStatus(List<DataFormation> formations)
    {
        var result = cachePrefabs.Where(x => formations.Where(value => Utils.IsValidDataActorKey(value.actor))
                                                        .Any(value => value.actor.key.Equals(x.Data.key)))
                                    .ToList();

        // var results = cachePrefabs.Where(x => formations.Any(value =>  (value.actor?.key ?? "")  ?? "" == x.Data.key)).ToList();
        // print("LoadActorStatus count: " + result.Count);
        result.ForEach(x =>
        {
            x.OnHoverSelect();
            x.OnItemUnselect();
        });
    }



    private void Callback_ItemClick(string key, bool isLocked)
    {
        _key = key;
        _itemLocked = isLocked;
        _cbItemClicked?.Invoke(_key);

        UnSelectItems();
    }



    private void UnSelectItems()
    {
        foreach (var ui in cachePrefabs)
            ui.OnItemUnselect();
    }


    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }


    private void ClearCache()
    {
        cachePrefabs.ForEach(x => { if (x != null) Destroy(x.gameObject); });
        cachePrefabs.Clear();
    }


    private void ResetScrollSize(float value)
    {
        var scroll = scrollContent.AsRectTransform();
        scroll.sizeDelta = new Vector2(scroll.sizeDelta.x, value);
    }


    private void ChangeScrollSize(float value)
    {
        var scroll = scrollContent.AsRectTransform();
        scroll.sizeDelta = new Vector2(scroll.sizeDelta.x, scroll.sizeDelta.y + value);
    }


    public void OnClickButtonCancel()
    {
        Hide();
    }


    public void OnClickButtonConfirm()
    {
        Hide();
        _cbConfirmed?.Invoke(_key);
    }




}
