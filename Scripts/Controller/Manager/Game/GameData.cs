using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class GameData : MonoBehaviour
{



    [Header("[Data]")]
    [SerializeField] private bool hasInitUserData;
    [SerializeField] private UserInfo userInfo;


    // [properties]
    public UserInfo UserInfo { get => userInfo; }
    public bool HasInitUserData { get => hasInitUserData; }



    #region UNITY
    // private void Start()
    // {
    // }

    // private void Update()
    // {
    // }
    #endregion



    public void Init()
    {
        SetDefaultUserData();
    }


    private void SetDefaultUserData()
    {
        // init user data
        if (GameManager.Instance.RunGameLocal)
        {
            Utils.LOG("Cheating RunGameLocal [GameManager] user data");
            LoadLocalUserData();
            return;
        }


        // print(PlayerPrefs.HasKey(CONST.KEY_SAVE));
        if (PlayerPrefs.HasKey(CONST.KEY_SAVE))
        {
            Utils.LOG("Load [LOCAL] save user data");
            LoadData();
        }
        else
        {
            Utils.LOG("Init [NEW] user data");
            InitNewUserData();
            SaveData();
        }
    }


    private void LoadData()
    {
        var data = PlayerPrefs.GetString(CONST.KEY_SAVE);
        userInfo = JsonUtility.FromJson<UserInfo>(data);
        hasInitUserData = true;
    }


    private void SaveData()
    {
        var json = JsonUtility.ToJson(userInfo);
        PlayerPrefs.SetString(CONST.KEY_SAVE, json);
        print("save string data: " + json);
    }


    private void LoadLocalUserData()
    {
        userInfo = ConfigManager.Instance.UserInfo;
        hasInitUserData = true;
    }


    private void InitNewUserData()
    {
        userInfo = new UserInfo();
        userInfo.displayName = "guest0";
        userInfo.avatar = "actor";
        userInfo.gold = 0;
        userInfo.level = 1;
        userInfo.expLevel = 1;
        userInfo.maxExpLevel = 100;

        // init user actors
        userInfo.mainActor = new DataActor();
        userInfo.actors = new List<DataActor>();
        userInfo.formation = new List<DataFormation>();
        userInfo.stages = ConfigManager.Instance.GameStages;

        for (int i = 0; i < CONST.DEFAULT_FORMATION; i++)
        {
            userInfo.formation.Add(new DataFormation { index = i, actor = null });
        }
    }


    public List<DataActor> GetActors()
    {
        return userInfo.actors;
    }


    public List<DataActor> GetFormationActors()
    {
        var result = userInfo.formation.Where(x => Utils.IsValidDataActorKey(x.actor))
                                    .Select(x => x.actor)
                                    .ToList();

        // print("GetFormationActors: " + result.Count);
        return result;
    }


    public List<DataFormation> GetFormation()
    {
        return userInfo.formation;
    }


    public List<DataStage> GetStages()
    {
        return userInfo.stages;
    }



    public void SaveInit(DataActor newActor)
    {
        // print($"SaveMainActor: {newActor.key}");
        userInfo.mainActor = newActor;
        userInfo.formation[0].actor = newActor;
        userInfo.actors.AddIfIdNotExists(newActor);

        // add init default actors
        var initActors = ConfigManager.Instance.DefaultInitActors;
        for (int i = 0; i < initActors.Count; i++)
        {
            userInfo.formation[i + 1].actor = initActors[i];
            userInfo.actors.AddIfIdNotExists(initActors[i]);
        }

        SaveData();
    }


    public void SaveStage(DataStage data)
    {
        // print($"SaveStage: {data.id}");
        var stage = userInfo.stages.Find(x => x.id.Equals(data.id));
        stage.status = CONST.STAGE_COMPLETED;
        SaveData();
    }


    public void SaveActorByKey(string[] keys)
    {
        // print($"SaveStage: {data.id}");
        foreach (var key in keys)
        {
            var data = GetDataActorByKey(key);
            userInfo.actors.AddIfIdNotExists(data);
        }
        SaveData();
    }


    public void RemoveFormationByKey(string key)
    {
        foreach (var item in userInfo.formation)
        {
            if (item.actor == null || string.IsNullOrEmpty(item.actor.key))
                continue;

            if (item.actor.key.Equals(key))
                item.actor = null;
        }
        SaveData();
    }


    public void SaveFormationWithIndex(string key, int index)
    {
        var item = userInfo.formation.Find(x => x.index == index);
        if (item != null)
        {
            item.actor = GetDataActorByKey(key);
            SaveData();
        }
    }


    private DataActor GetDataActorByKey(string key)
    {
        return ConfigManager.Instance.GetDataActorByKey(key);
    }



    public void CheatGetAllCharacters()
    {
        userInfo.stages.ForEach(x => x.status = CONST.STAGE_COMPLETED);
        userInfo.actors = ConfigManager.Instance.UserInfo.actors;
        SaveData();
    }


}