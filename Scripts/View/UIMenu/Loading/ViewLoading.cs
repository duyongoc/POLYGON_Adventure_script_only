using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewLoading : ViewBase
{
    

    [Header("[Component]")]
    [SerializeField] private ViewLoadingComponent compLoading;



    #region  UNITY
    // private void Start()
    // {
    // }

    // private void Update()
    // {
    // }
    #endregion




    #region STATE
    public override void StartState() { base.StartState(); }
    public override void UpdateState() { base.UpdateState(); }
    public override void EndState() { base.EndState(); }
    #endregion




    public void Load()
    {
        compLoading.Load();
    }


    public void OnClickButtonReturn()
    {
    }


}
