using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UserInfo
{

    public string userId;
    public string displayName;
    public string email;
    public string avatar;
    public float gem = 0;
    public float gold = 0;

    [Header("[Level]")]
    public float level = 1;
    public float expLevel = 0;
    public float maxExpLevel = 100;

    [Header("[Data]")]
    public DataActor mainActor;

    [Header("[Data List]")]
    public List<DataFormation> formation;
    public List<DataActor> actors;
    public List<DataStage> stages;

}