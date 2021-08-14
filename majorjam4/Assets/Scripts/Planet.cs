using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlanetState
{
    Normal, Damaged, Destroyed
}

public class Planet : MonoBehaviour
{
    public Sprite damaged;
    public Sprite destroyed;

    public PlanetState state;
    public PlanetState oldState;

    public int maxHp;
    public int hp;

    private void Start()
    {
        hp = maxHp;
        state = PlanetState.Normal;
        oldState = state;
    }

    private void Update()
    {
        if (hp <= maxHp / 2 && hp > 0)
            state = PlanetState.Damaged;
        else if (hp <= 0)
            state = PlanetState.Destroyed;

        if(oldState != state)
        {
            oldState = state;

            if (state == PlanetState.Damaged)
                GetComponent<SpriteRenderer>().sprite = damaged;
            else if (state == PlanetState.Destroyed)
                GetComponent<SpriteRenderer>().sprite = destroyed;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Enemy" || other.tag == "Bullet")
        {
            hp -= 5;
        }
    }
}
