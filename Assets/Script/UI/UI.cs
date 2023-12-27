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

    public float damageMultiple = 1.0f;
    float hp = 100.0f;
    public float Hp
    {
        get => hp;
        set
        {
            if (hp != value)
            {
                hp = value * damageMultiple;
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
                    StartCoroutine(Drown());
                    breathe = 0;
                }
                breatheImage.fillAmount = (float)breathe / maxBreathe;
                breatheText.text = $"{breathe}";
            }
        }
    }

    private void Awake()
    {
        breathe = maxBreathe;
        player = FindObjectOfType<Player>();
        depth = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        hungerImage = transform.GetChild(2).GetComponent<Image>();
        hydrationImage = transform.GetChild(3).GetComponent<Image>();
        breatheImage = transform.GetChild(4).GetComponent<Image>();
        hpImage = transform.GetChild(5).GetComponent<Image>();
        hungerText = hungerImage.GetComponentInChildren<TextMeshProUGUI>();
        hydrationText = hydrationImage.GetComponentInChildren<TextMeshProUGUI>();
        breatheText = breatheImage.GetComponentInChildren<TextMeshProUGUI>();
        hpText = hpImage.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        StartCoroutine(HpHunger());
        StartCoroutine(BreathOut());
        StartCoroutine(BreathIn());
    }

    private void Update()
    {
        if (player.transform.position.y > 0) depth.text = "0m";
        else depth.text = $"{(int)-player.transform.position.y}m";
    }

    public int hungerSick = 3;
    public int hydrationSick = 2;
    readonly WaitForSeconds sick = new(5.0f);

    private IEnumerator HpHunger()
    {
        while (true)
        {
            yield return sick;
            if (Hunger >= 90 && Hydration >= 90) Hp += 5;
            Hunger -= hungerSick;
            Hydration -= hydrationSick;
        }
    }

    readonly WaitForSeconds breathOut = new(1.0f);
    readonly WaitForSeconds breathIn = new(0.025f);

    public float safeBreathDepth = 100.0f;

    private IEnumerator BreathOut()
    {
        while (true)
        {
            yield return breathOut;
            if (Water.inWater)
            {
                if (transform.position.y < -safeBreathDepth) Breathe -= 2;
                else Breathe--;
            }
        }
    }
    private IEnumerator BreathIn()
    {
        while (true)
        {
            yield return breathIn;
            if (!Water.inWater) Breathe ++;
        }
    }

    private IEnumerator Drown()
    {
        yield return new WaitForSeconds(2.0f);
        player.Die();
    }
}
