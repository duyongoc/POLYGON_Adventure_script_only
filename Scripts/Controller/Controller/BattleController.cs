using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public class BattleController : MonoBehaviour
{


    [Header("[Cheating]")]
    public bool CanSpawnBot = false;
    public bool CanUpdateGame = false;
    public bool IsGameFinished = false;

    [Header("[Config]")]
    [SerializeField] private Transform[] positionEnemies;

    [Header("[Stage]")]
    [SerializeField] private DataStage stage;

    [Header("[Camera - UI]")]
    [SerializeField] private CameraBattle cameraBattle;
    [SerializeField] private UITextDamage uiTextDamage;

    [Header("[Actor]")]
    [SerializeField] private List<ActorBase> players;
    [SerializeField] private List<ActorBase> enemies;


    // [private]
    private ViewManager _viewMgr;
    private GameManager _gameMgr;
    private SoundManager _soundMgr;
    private UIBattle _uiBattle;


    // [properties]
    public CameraBattle CameraBattle { get => cameraBattle; }




    #region Singleton can be destroy
    public static BattleController Instance;


    public void Awake()
    {
        if (Instance != null)
            return;

        Instance = this;
    }

    private void Start()
    {
        _viewMgr = ViewManager.Instance;
        _gameMgr = GameManager.Instance;
        _soundMgr = SoundManager.Instance;
        _uiBattle = UIBattle.Instance;

        Init();
        SetStage();
    }
    #endregion



    private void Init()
    {
        var list = new List<ActorBase>();
        var formation = _gameMgr.Data.GetFormationActors();

        // init list actors 
        foreach (var actor in formation)
        {
            list.Add(SpawnActor(actor.key));
        }

        players = list;
        cameraBattle.Init(list.FirstOrDefault(), list);
        InitUIActor();
    }


    private void SetStage()
    {
        stage = ConfigManager.Instance.StateCurrent;
        var randTran = positionEnemies[UnityEngine.Random.Range(0, positionEnemies.Length)];

        foreach (var bot in stage.bots)
        {
            var actor = ConfigManager.Instance.GetObjectActorByKey(bot);
            var enemy = Instantiate(actor, randTran.position, Quaternion.identity).GetComponent<ActorBase>();
            enemy.SetActorKey(bot);
            enemy.SetActorRole(ERole.BOT);
            AddEnemy(enemy);
        }

        // assign bar health to default enemy
        SetupDefaultEnemy();
    }


    private void SetupDefaultEnemy()
    {
        if (enemies.Count >= 1)
        {
            var enemy = enemies[0];
            _uiBattle.SetBossAvatar(enemy.ActorKey);
            
            enemy.ActorHealth.InjectUIActor(_uiBattle.PanelGame.UI_Boss);
            enemy.transform.localScale = Vector3.one * 1.5f;


            // arrange enemies position array
            var indexPosition = 0;
            var positionList = GetPositionListAround(enemy.transform.position, 1.5f, enemies.Count);
            foreach (var item in enemies)
            {
                item.transform.position = positionList[indexPosition];
                indexPosition = (indexPosition + 1) % positionList.Count;
            }
        }
    }


    private List<Vector3> GetPositionListAround(Vector3 startPosition, float distance, int positionCount)
    {
        var positionList = new List<Vector3>();
        for (int i = 0; i < positionCount; i++)
        {
            var angle = i * (360f / positionCount);
            var dir = ApplyRotationToVector(new Vector3(1, 0, 0), angle);
            var position = startPosition + dir * distance;
            positionList.Add(position);
        }
        return positionList;
    }

    private Vector3 ApplyRotationToVector(Vector3 vec, float angle)
    {
        return Quaternion.Euler(0, angle, 0) * vec;
    }




    private ActorBase SpawnActor(string key)
    {
        var keyObj = Instantiate(ConfigManager.Instance.GetObjectActorByKey(key), transform);
        var actor = keyObj.GetComponent<ActorBase>();
        actor.SetActorKey(key);
        actor.SetActorRole(ERole.PLAYER);
        return actor;
    }


    private void InitUIActor()
    {
        var uiList = _uiBattle.PanelGame.UI_Actor;
        uiList.ForEach(x => x.gameObject.SetActive(false));

        for (int i = 0; i < players.Count; i++)
        {
            var ui = uiList[i];
            var actor = players[i];

            if (actor != null)
            {
                ui.SetAvatar(actor.ActorKey);
                ui.gameObject.SetActive(true);
                actor.ActorHealth.InjectUIActor(ui);
            }
        }
    }




    public void AddEnemy(ActorBase character)
    {
        enemies.Add(character);
    }

    public void RemoveEnemy(ActorBase actor)
    {
        if (enemies.Exists(x => x == actor))
            enemies.Remove(actor);

        // check win condition - no enemies alive
        if (!IsAnyEnemyAlive())
        {
            if (IsGameFinished)
                return;

            IsGameFinished = true;
            ShowBattleWin();
        }
    }


    public void AddPlayer(ActorBase character)
    {
        players.Add(character);
    }

    public void RemovePlayer(ActorBase actor)
    {
        if (players.Exists(x => x == actor))
            players.Remove(actor);

        // check lose condition - no player alive
        if (!IsAnyCharacterAlive())
        {
            if (IsGameFinished)
                return;

            IsGameFinished = true;
            ShowBattleLose();
        }
    }



    public bool IsAnyEnemyAlive()
    {
        foreach (var actor in enemies)
        {
            if (!actor.IsDead)
                return true;
        }
        return false;
    }

    public bool IsAnyCharacterAlive()
    {
        foreach (var actor in players)
        {
            if (!actor.IsDead)
                return true;
        }
        return false;
    }




    private void ShowBattleWin()
    {
        _gameMgr.Data.SaveStage(stage);
        _gameMgr.Data.SaveActorByKey(stage.rewards);

        StartCoroutine(IE_BattleFinished(2, () =>
        {
            _soundMgr.StopMusic();
            _soundMgr.PlaySFX(_soundMgr.SFX_BattleWin);
            _viewMgr.ShowPopupStageResultWin(stage.rewards, _viewMgr.LoadSceneMenu);
        }));
    }


    private void ShowBattleLose()
    {
        StartCoroutine(IE_BattleFinished(2, () =>
        {
            _soundMgr.StopMusic();
            _soundMgr.PlaySFX(_soundMgr.SFX_BattleDefeat);
            _viewMgr.ShowPopupStageResultLose(_viewMgr.LoadSceneMenu);
        }));
    }



    private IEnumerator IE_BattleFinished(float time, Action callback)
    {
        yield return new WaitForSeconds(time);
        callback?.Invoke();
    }



    public void SpawnText(Transform spawnTransform, float damage, Color color, float scale)
    {
        if (damage <= 0)
            return;

        var randX = UnityEngine.Random.Range(-0.25f, 0.25f);
        var position = new Vector3(spawnTransform.position.x + randX, spawnTransform.position.y + 1, spawnTransform.position.z);
        var uiDamage = uiTextDamage.gameObject.SpawnToGarbage(position, Quaternion.identity);
        uiDamage.GetComponent<UITextDamage>().SetText(damage, color, scale);
    }

    public void SpawnText(Transform spawnTransform, string damage, Color color, float scale)
    {
        var randX = UnityEngine.Random.Range(-0.25f, 0.25f);
        var position = new Vector3(spawnTransform.position.x + randX, spawnTransform.position.y + 1, spawnTransform.position.z);
        var uiDamage = uiTextDamage.gameObject.SpawnToGarbage(position, Quaternion.identity);
        uiDamage.GetComponent<UITextDamage>().SetText(damage, color, scale);
    }


}




// public class SaveAndLoadSystem
// {
//     public static void Save(MapController data)
//     {
//         BinaryFormatter formatter = new BinaryFormatter();
//         string path = "Data/data.fun";
//         FileStream stream = new FileStream(path, FileMode.Create);
//         // MapController dt = new MapController(data);
//         // formatter.Serialize(stream, dt);
//         stream.Close();
//     }

//     public static MapController Load()
//     {
//         string path = "Data/data.fun";
//         if (File.Exists(path))
//         {
//             BinaryFormatter formatter = new BinaryFormatter();
//             FileStream stream = new FileStream(path, FileMode.Open);
//             MapController dt = formatter.Deserialize(stream) as MapController;
//             stream.Close();
//             return dt;
//         }
//         else
//         {
//             Debug.Log("Cant find data path");
//             return null;
//         }
//     }
// }