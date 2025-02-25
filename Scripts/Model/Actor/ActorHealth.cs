using UnityEngine;
using UnityEngine.UI;

public class ActorHealth : MonoBehaviour
{

    [Header("Character's health")]
    [SerializeField] private float totalHealth;
    [SerializeField] private float currentHealth;
    [SerializeField] private ActorUICanvas actorUI;



    // [private]
    private ActorBase actor;
    private BattleController battleController;
    private IActorHealth uiActorHealth;


    // [properties]
    public float TotalHealth { get => totalHealth; set => totalHealth = value; }
    public float CurrentHealth { get => currentHealth; set => currentHealth = value; }




    #region UNITY
    // private void Start()
    // {
    // }

    // private void LateUpdate()
    // {
    // }
    #endregion



    public void Init(float health)
    {
        totalHealth = health;
        currentHealth = health;
        battleController = BattleController.Instance;
        actor = GetComponent<ActorBase>();

        actorUI.Init(actor.Role);
        uiActorHealth?.UpdateHealth(currentHealth, totalHealth);
    }


    public void InjectUIActor(IActorHealth iActor)
    {
        uiActorHealth = iActor;
        uiActorHealth?.UpdateHealth(currentHealth, totalHealth);
    }


    public void DecreaseHP(float hp, bool playAimHit = false)
    {
        currentHealth -= hp;
        actor.CurrentStats.hp = currentHealth;
        actorUI.SetSliderValue(currentHealth / totalHealth);
        uiActorHealth?.UpdateHealth(currentHealth, totalHealth);

        // spawn text ui 
        battleController.SpawnText(transform, hp, Color.red, playAimHit ? 2 : 1);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            actorUI.ShowUICanvas(false);

            // only trigger dead one time
            if (actor.IsDead == false)
            {
                actor.OnDead();
            }
        }
    }


    public void IncreaseHP(float hp)
    {
        if (currentHealth >= totalHealth)
            currentHealth = totalHealth;

        currentHealth += hp;
        actor.CurrentStats.hp = currentHealth;
        actorUI.SetSliderValue(currentHealth / totalHealth);
        battleController.SpawnText(transform, hp, Color.green, 1.5f);
    }
}
