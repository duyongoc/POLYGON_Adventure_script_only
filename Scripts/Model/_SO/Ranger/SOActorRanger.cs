using UnityEngine;

[CreateAssetMenu(fileName = "Ranger", menuName = "CONFIG/Actor/Ranger/[Ranger]", order = 400)]
public class SOActorRanger : ScriptableObject
{

    [Header("[CONFIG]")]
    public DataActorStat levelStats;
    public DataActorStat defaultStats;


    [Header("[Skill]")]
    public SOSkillRanger attackNormal;
    public SOSkillRanger attackSkill_1;
    public SOSkillRanger attackSkill_2;
    public SOSkillRanger attackSkill_3;


    [Header("[Attack]")]
    public int priorityFocus = CONST.PRIORITY_DEFAULT;
    public float combatPoint = 20;
    public float attackRange = 2.5f;
    public float attackDetection = 10f;



#if UNITY_EDITOR
    [ContextMenu("RandomAttribute")]
    public void RandomAttribute()
    {
        levelStats.hp = Random.Range(1500, 2200);
        levelStats.p_attack = Random.Range(450, 550);
        levelStats.p_defense = Random.Range(300, 400);
        levelStats.m_attack = Random.Range(400, 500);
        levelStats.m_defense = Random.Range(300, 500);
        levelStats.attack_speed = Random.Range(200, 400);
        levelStats.speed = Random.Range(130, 160);

        defaultStats = levelStats;
        UnityEditor.EditorUtility.SetDirty(this);
    }
#endif


}
