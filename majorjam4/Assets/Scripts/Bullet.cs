using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public bool isEnemy;
    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0f, speed);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.transform.tag == "Player")
        {
            PlayerManager player = other.transform.GetComponent<PlayerManager>();

            if(isEnemy && !player.isDead && !player.isInvulnerable)
            {
                player.isDead = true;
                player.deaths++;
                other.transform.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
        else if(other.transform.tag == "Enemy" && !isEnemy)
        {
            Destroy(other.gameObject);
        }

        Destroy(gameObject);
    }
}
