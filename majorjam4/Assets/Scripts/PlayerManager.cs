using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public float speed;
    Rigidbody2D rb;

    public GameObject bullet;
    public float bulletOffset;

    public float shootDelay;
    bool canShoot = true;

    public bool isDead;
    public int deaths;

    public bool isInvulnerable;
    public Text invulnerableText;

    PlayerControls controls;

    Vector2 move;


    private void Awake()
    {
        controls = new PlayerControls();
        controls.Player.Movement.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Player.Movement.canceled += ctx => move = Vector2.zero;

        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (isDead) return;

        float x = Input.GetAxisRaw("Horizontal");

        if(x != 0)
            rb.velocity = new Vector2(x * speed, 0f);
        else
            rb.velocity = new Vector2(move.x * speed, 0f);

        if (Input.GetKeyDown(KeyCode.Space) || controls.Player.Shoot.triggered)
        {
            StartCoroutine(Shoot(shootDelay));
        }
    }

    IEnumerator Shoot(float delay)
    {
        if (canShoot)
        {
            Instantiate(bullet, new Vector2(transform.position.x, transform.position.y + bulletOffset), Quaternion.identity);

            canShoot = false;

            yield return new WaitForSeconds(delay);

            canShoot = true;
        }
    }

    public IEnumerator Invulnerable(float duration)
    {
        isInvulnerable = true;
        invulnerableText.gameObject.SetActive(true);

        yield return new WaitForSeconds(duration);

        isInvulnerable = false;
        invulnerableText.gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag == "Enemy" && !isDead && !isInvulnerable)
        {
            isDead = true;
            deaths++;
            GetComponentInChildren<SpriteRenderer>().enabled = false;
        }
    }

    private void OnEnable()
    {
        controls.Player.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }
}
