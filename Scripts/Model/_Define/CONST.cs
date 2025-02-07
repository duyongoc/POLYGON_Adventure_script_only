using UnityEngine;

public class CONST
{


    // key save user data save local
    public const string KEY_SAVE = "DATA";
    public const string KEY_SAVE_LOCAL = "DATA_LOCAL";
    public const string KEY_TUTORIAL = "TUTORIAL";


    // due to use unity nav mesh - 3.5 value for default
    // we need to convert value navmesh to that value from character's speed 
    // =>> exp: 140 / 40 = 3.5 
    public const float FACTOR_MOVE_SPEED = 40;

    // convert a second, time delay attack to the number
    // =>> exp: 1 / (200 / 300) = 1.5
    public const float FACTOR_ATTACK_SPEED = 300;


    // [default damage]
    public const int DEFAULT_DAMAGE = 10;
    public const int DEFAULT_FORMATION = 4;

    // [target priority]
    public const int PRIORITY_DEFAULT = 100;


    public const string ACTOR_FIGHTER = "fighter";
    public const string ACTOR_RANGER = "ranger";
    public const string ACTOR_TANKER = "tanker";
    public const string ACTOR_SUPPORT = "support";
    public const string ACTOR_MAGE = "mage";


    // public const string STAGE_DEFAULT = "default";
    public const string STAGE_COMPLETED = "completed";
    public const string STAGE_INCOMPLETED = "incompleted";


    // token of my database
    public const string TOKEN = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjY3N2UxN2Y1MTE2N2EwYTU3NmY2Y2ZmMiIsImlhdCI6MTczNjMxNjkxNywiZXhwIjoxNzY3ODUyOTE3fQ.OxhnUo-vtwbDC0kBiK-D-m60N66_kcN-n2xw76YK12k";



}



// [view game]
public enum EViewGame
{
    None,
    Loading,
    Selection,
    Information,
    Lobby,
    Stage,
    Map,
    Shop,
    Guild,
    Boss,
    BattlePass,
    Roulette,
}
