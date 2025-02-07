using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ViewInformationComponent : MonoBehaviour
{


    [Header("[Scrolling prefab]")]
    [SerializeField] private float scrollDefault = 100;
    [SerializeField] private float scrollOffset = 100;
    [SerializeField] private Transform scrollContent;
    [SerializeField] private UIItemActor prefabItem;
    [SerializeField] private List<UIItemActor> cachePrefabs;

    [Header("[Actor Skill]")]
    [SerializeField] private Transform tranSkill;
    [SerializeField] private UIItemActorSkill prefabActorSkill;

    [Header("[Text Information]")]
    [SerializeField] private TMP_Text txtName;
    [SerializeField] private TMP_Text txtGem;
    [SerializeField] private TMP_Text txtGold;

    [Header("[Text Stats]")]
    [SerializeField] private TMP_Text txtHealth;
    [SerializeField] private TMP_Text txtPhysicAttack;
    [SerializeField] private TMP_Text txtPhysicDefense;
    [SerializeField] private TMP_Text txtMagicAttack;
    [SerializeField] private TMP_Text txtMagicDefense;
    [SerializeField] private TMP_Text txtAttackSpeed;

    [Header("[Button Tab]")]
    [SerializeField] private List<UIItemTabInformation> tabInformations;


    [Header("[Cache]")]
    [SerializeField] private List<UIItemActorSkill> cacheActorSkills;


    // [private]
    private DataActor _actor;



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
        var actors = GameManager.Instance.Data.GetActors();

        LoadData();
        LoadActor(actors);
        LoadTabInformation();
        SelectDefault();
    }


    private void SelectDefault()
    {
        if (cachePrefabs == null || cachePrefabs.Count <= 0)
            return;

        cachePrefabs[0].OnClickButtonItem();
    }


    private void LoadData()
    {
        var user = GameManager.Instance.Data.UserInfo;
        txtGem.SetText(user.gem.ToString());
        txtGold.SetText(user.gold.ToString());
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
            prefab.Init(item, CallbackCharacter);

            cachePrefabs.Add(prefab);
            ChangeScrollSize(scrollOffset);
        }
    }


    private void CallbackCharacter(string key, bool isLocked)
    {
        _actor = ConfigManager.Instance.GetDataActorByKey(key);
        if (_actor == null)
            return;

        UnSelectPrefab();
        LoadActorData();
        LoadActorSkill();

        // fire event show character 
        this.PostEvent(EventID.ShowCharacter, key);
    }



    private void LoadActorData()
    {
        // load actor data
        txtName.SetText(_actor.name.Replace("_", " "));
        // txtType.SetText(_actor.type);
        // txtDescription.SetText(_actor.description);

        // load actor stats  
        txtHealth.SetText(_actor.stats.hp.ToString());
        txtPhysicAttack.SetText(_actor.stats.p_attack.ToString());
        txtPhysicDefense.SetText(_actor.stats.p_defense.ToString());
        txtMagicAttack.SetText(_actor.stats.m_attack.ToString());
        txtMagicDefense.SetText(_actor.stats.m_defense.ToString());
        txtAttackSpeed.SetText(_actor.stats.attack_speed.ToString());
    }



    private void LoadActorSkill()
    {
        cacheActorSkills.ForEach(x => { if (x) Destroy(x.gameObject); });

        foreach (var item in _actor.skills)
        {
            var ui = Instantiate(prefabActorSkill, tranSkill);
            ui.Init(item, CallbackActorSkill);
            cacheActorSkills.Add(ui);
        }
    }

    private void CallbackActorSkill(string key)
    {
        UIMenuScene.Instance.Model.PlayMainRoratorAnimation(key);
    }



    private void LoadTabInformation()
    {
        tabInformations.ForEach(x => x.Init(CallbackTab));
    }

    private void CallbackTab(string key)
    {
        tabInformations.ForEach(x => x.OnUnSelectItem());
        print("click tab: " + key);
    }



    private void UnSelectPrefab()
    {
        foreach (var ui in cachePrefabs)
            ui.OnItemUnselect();
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




}
