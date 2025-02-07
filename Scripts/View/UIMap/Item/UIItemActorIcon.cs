using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIItemActorIcon : MonoBehaviour, IActorHealth
{


    [Header("[Setting]")]
    [SerializeField] private Image imgAvatar;
    [SerializeField] private TMP_Text txtHealth;
    [SerializeField] private Slider sliderHealth;



    public void SetAvatar(string key)
    {
        imgAvatar.sprite = GameController.Instance.LoadSpriteAvartar(key);
    }


    public void UpdateHealth(float currentHealth, float totalHealth)
    {
        if (txtHealth != null)
        {
            txtHealth.SetText($"{(int)currentHealth}/{(int)totalHealth}");
        }

        if (sliderHealth != null)
        {
            if (totalHealth <= 0)
                return;

            // print($"actor:{this.name} {currentHealth} / {totalHealth}");
            sliderHealth.value = (float)currentHealth / totalHealth;
        }
    }


}
