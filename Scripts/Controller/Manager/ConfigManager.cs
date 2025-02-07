using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ConfigManager : Singleton<ConfigManager>
{


    [Header("[Game]")]
    [SerializeField] private List<DataStage> gameStages;
    [SerializeField] private List<DataActor> gameActors;
    [SerializeField] private List<DataActor> gameActorSelection;
    [SerializeField] private List<GameObject> gamePrefabActors;
    [SerializeField] private List<GameObject> gamePrefabModels;

    [Header("[Default]")]
    [SerializeField] private List<DataActor> defaultInitActors;

    [Header("[Stage]")]
    [SerializeField] private DataStage stateCurrent;
    [SerializeField] private DataStage stateChallenge;

    [Header("[Data]")]
    [SerializeField] private UserInfo userInfo;



    // [properties]
    public List<DataStage> GameStages { get => gameStages; }
    public List<DataActor> GameActors { get => gameActors; }
    public List<DataActor> GameActorSelection { get => gameActorSelection; }
    public List<DataActor> DefaultInitActors { get => defaultInitActors; }

    public List<GameObject> GamePrefabActors { get => gamePrefabActors; }
    public List<GameObject> GamePrefabModels { get => gamePrefabModels; }

    public UserInfo UserInfo { get => userInfo; }
    public DataStage StateCurrent { get => stateCurrent; set => stateCurrent = value; }





    #region UNITY
    // private void Start()
    // {
    // }

    // private void Update()
    // {
    // }
    #endregion




    public DataActor GetDataActorByKey(string key)
    {
        var actor = gameActors.Where(x => x.key.Equals(key)).FirstOrDefault();
        if (actor == null)
        {
            Debug.LogError($"GetActorInfoByKey is null: [{key}]");
        }

        // print("actor: " + actor.name);
        return actor;
    }


    public GameObject GetObjectActorByKey(string key)
    {
        // print($"TestSQL | key: {key} | level: {level} ");
        // var stat = from stats in listStats.Where(x => x.key.Equals(key))
        //            from result in stats.stats.Where(y => y.level.Equals(level))
        //            select result;

        // var stat = listStats.Where(x => x.key.Equals(key))
        //                     .SelectMany(x => x.stats)
        //                     .Where(stats => stats.level.Equals(level));


        var model = gamePrefabActors.Where(x => x.name.Equals(key)).FirstOrDefault();
        if (model == null)
        {
            Debug.LogError($"GetActorByKey is null: [{key}]");
        }

        // print("model: " + model.name);
        return model;
    }


    public GameObject GetModelByKey(string key)
    {
        var model = gamePrefabModels.Where(x => x.name.Equals(key)).FirstOrDefault();
        if (model == null)
        {
            Debug.LogError($"GetModelByKey is null: [{key}]");
        }

        // print("model: " + model.name);
        return model;
    }


    public void SetChallengeMode()
    {
        stateCurrent = stateChallenge;
    }





    // private void LoadStats()
    // {
    //     configCharacterStats.Clear();
    //     foreach (var sheet in guardianWarsConfig.sheets)
    //     {
    //         Debug.Log("=>>>>>>>>>>>>>>sheet.name: " + sheet.name);
    //         var listStat = new List<CharacterStats>();
    //         for (int i = 0; i < sheet.list.Count; i++)
    //         {
    //             var row = sheet.list[i];
    //             var fieldInfos = typeof(GuardianWarsConfig.Param).GetFields();
    //             var stats = new CharacterStats();
    //             for (int j = 0; j < fieldInfos.Length; j++)
    //             {
    //                 var fieldInfo = fieldInfos[j];
    //                 var name = fieldInfo.Name;
    //                 var value = fieldInfo.GetValue(row).ToString();
    //                 if (name.Equals("level")) stats.level = int.Parse(value);
    //                 else if (name.Equals("hp")) stats.hp = float.Parse(value);
    //                 else if (name.Equals("p_attack")) stats.p_attack = float.Parse(value);
    //                 else if (name.Equals("p_defense")) stats.p_defense = float.Parse(value);
    //                 else if (name.Equals("m_attack")) stats.m_attack = float.Parse(value);
    //                 else if (name.Equals("m_defense")) stats.m_defense = float.Parse(value);
    //                 else if (name.Equals("attack_speed")) stats.attack_speed = float.Parse(value);
    //                 else if (name.Equals("speed")) stats.speed = float.Parse(value);
    //                 // Debug.Log($"sheet.name: {sheet.name} | row:  | name: {name} | value: {value}");
    //             }
    //             Debug.Log($"sheet.name: {sheet.name} | Add: {JsonUtility.ToJson(stats)}");
    //             listStat.Add(stats);
    //         }
    //         configCharacterStats.Add(sheet.name, listStat);
    //     }
    // }



#if UNITY_EDITOR
    [ContextMenu("Load")]
    private void Load()
    {
        // LoadStats();
        gameActors.ForEach(x =>
        {
            x.stats.hp = Random.Range(1500, 2200);
            x.stats.p_attack = Random.Range(450, 550);
            x.stats.p_defense = Random.Range(300, 400);
            x.stats.m_attack = Random.Range(400, 500);
            x.stats.m_defense = Random.Range(300, 500);
            x.stats.attack_speed = Random.Range(200, 400);
            x.stats.speed = Random.Range(130, 160);
        });

        // foreach(var actor in gameActors)
        // {
        //     foreach(var skill in actor.skills)
        //     {
        //         // skill.id = skill.id.Replace("hero_blade", actor.key);
        //         // skill.key = skill.key.Replace("hero_blade", actor.key);
        //         skill.description = $"Description of {skill.key.Replace("_", " ")}";
        //     }
        // }
    }

    [ContextMenu("ReorderStages")]
    private void ReorderStages()
    {
        for (int i = 0; i < gameStages.Count; i++)
        {
            gameStages[i].id = "stage_" + (i + 1);
            gameStages[i].key = "stage_" + (i + 1);
        }
    }
#endif




}
