using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    Player player;
    TextMeshProUGUI depth;

    Image hungerImage;
    Image hydrationImage;
    Image breatheImage;
    Image hpImage;
    TextMeshProUGUI hungerText;
    TextMeshProUGUI hydrationText;
    TextMeshProUGUI breatheText;
    TextMeshProUGUI hpText;

    float hp = 100.0f;
    public float Hp
    {
        get => hp;
        set
        {
            if (hp != value)
            {
                hp = value;
                if (hp > 100) hp = 100.0f;
                else if(hp <= 0)
                {
                    hp = 0.0f;
                    player.Die();
                }
                hpImage.fillAmount = hp * 0.01f;
                hpText.text = $"{(int)hp}";
            }
        }
    }

    int hunger = 100;
    public int Hunger
    {
        get => hunger;
        set
        {
            if(value != hunger)
            {
                hunger = value;
                if(hunger > 125) hunger = 125;
                else if(hunger <= 0)
                {
                    hunger = 0;
                    Hp--;
                }
                hungerImage.fillAmount = (float)hunger * 0.01f;
                hungerText.text = $"{hunger}";
            }
        }
    }

    int hydration = 100;
    public int Hydration
    {
        get=> hydration;
        set
        {
            if(hydration != value)
            {
                hydration = value;
                if(hydration > 100) hydration = 100;
                else if(hydration <= 0)
                {
                    hydration = 0;
                    Hp--;
                }
                hydrationImage.fillAmount = (float)hydration * 0.01f;
                hydrationText.text = $"{hydration}";
            }
        }
    }

    public int maxBreathe = 60;
    int breathe;
    public int Breathe
    {
        get => breathe;
        set
        {
            if(breathe != value)
            {
                breathe = value;
                if(breathe > maxBreathe) breathe = maxBreathe;
                else if (breathe < 0)
                {
                    breathe = 0;
                }
            }
        }
    }

    private void Awake()
    {
        breathe = maxBreathe;
        player = FindObjectOfType<Player>();
        depth = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (player.transform.position.y > 0) depth.text = "0m";
        else depth.text = $"{(int)-player.transform.position.y}m";
    }
}
