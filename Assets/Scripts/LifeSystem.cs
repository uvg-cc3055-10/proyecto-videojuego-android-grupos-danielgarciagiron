// Universidad del Valle de Guatemala
// Daniel Garcia, 14152
// Programacion de plataformas moviles y juegos

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeSystem : MonoBehaviour {

    private int startingLife = 100;
    private int startingMagica = 100;
    private bool isAlive=true;
    private int messageCounter = 0;
    private bool isBlocking = false;

    public int life = 0;
    public int magica = 0;
    public int damage = 10;
    public float speed = 6.0f;

	void Start () {
        life = startingLife;
        magica = startingMagica;
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "EnemyWeapon")
        {
            if(isBlocking == true)
            {
                life -= (damage/2);
            }
            else
            {
                life -= damage;
            }

            if (life <= 0)
            {
                life = 0;
                isAlive = false;
            }
        }
        if(other.gameObject.tag == "MagicaPickup")
        {
            if(magica <= 80)
            {
                magica += 20;
            }
        }
        if (other.gameObject.tag == "HealthPickup")
        {
            if (life <= 80)
            {
                life += 20;
            }
        }
    }

    void OnEnable()
    {
        EventManager.startListening("Spell1", Spell1);
        EventManager.startListening("Spell2", Spell2);
        EventManager.startListening("Spell3", Spell3);
        EventManager.startListening("BlockModifier", BlockModifier);
    }

    void OnDisable()
    {
        EventManager.stopListening("Spell1", Spell1);
        EventManager.stopListening("Spell2", Spell2);
        EventManager.stopListening("Spell3", Spell3);
        EventManager.stopListening("BlockModifier", BlockModifier);
    }

    void BlockModifier(int info)
    {
        if(info == 1)
        {
            isBlocking = true;
        }
        else
        {
            isBlocking = false;
        }
    }

    void Spell1(int info)
    {
        if (magica >= 25)
        {
            magica = magica - info;
            EventManager.triggerEvent("SpellAnimation", 1);
            life += 20;
            if(life >= 100)
            {
                life = 100;
            }
        }
    }

    void Spell2(int info)
    {
        if (magica >= 5)
        {
            magica = magica - info;
            EventManager.triggerEvent("SpellAnimation", 2);

        }
    }

    void Spell3(int info)
    {
        if (magica >= 10)
        {
            magica = magica - info;
            EventManager.triggerEvent("SpellAnimation", 3);

        }
    }

    public int currentLife
    {
        get { return life;}
        set { life = value;}
    }

    public int currentMagica
    {
        get { return magica;}
        set { magica = value;}
    }

    public bool currentStatus
    {
        get { return isAlive;}
        set { isAlive = value;}
    }

    public float currentSpeed
    {
        get { return speed;}
        set { speed = value;}
    }

}
