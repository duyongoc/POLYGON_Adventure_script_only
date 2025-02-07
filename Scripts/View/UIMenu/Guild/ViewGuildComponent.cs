using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ViewGuildComponent : MonoBehaviour
{
    

    
    [Header("[Text]")]
    [SerializeField] private TMP_Text txtGem;
    [SerializeField] private TMP_Text txtGold;

    

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
        LoadData();
    }


    private void LoadData()
    {
        var user = GameManager.Instance.Data.UserInfo;
        txtGem.SetText(user.gem.ToString());
        txtGold.SetText(user.gold.ToString());
    }


}
