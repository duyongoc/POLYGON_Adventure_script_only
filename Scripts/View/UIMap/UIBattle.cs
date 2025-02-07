using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBattle : MonoBehaviour
{


    [Header("[Setting]")]
    [SerializeField] private UIBattlePanelGame panelGame;


    // [properties]
    public UIBattlePanelGame PanelGame { get => panelGame; set => panelGame = value; }



    #region Singleton can be destroy
    public static UIBattle Instance;
    public void Awake()
    {
        if (Instance != null)
            return;

        Instance = this;
        Init();
    }
    #endregion



    private void Init()
    {
    }


    public void SetBossAvatar(string key)
    {
        panelGame.UI_Boss.SetAvatar(key);
    }


    public void OnClickButtonMoveToCharacter()
    {
        BattleController.Instance.CameraBattle.MoveToMainCharacter();
    }


}
