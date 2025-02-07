using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBattlePanelGame : MonoBehaviour
{


    [Header("[Setting]")]
    [SerializeField] private UIItemActorIcon ui_itemBoss;
    [SerializeField] private List<UIItemActorIcon> ui_itemActor;


    // [properties]
    public UIItemActorIcon UI_Boss { get => ui_itemBoss; }
    public List<UIItemActorIcon> UI_Actor { get => ui_itemActor; }

}
