using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public float speed;
    Rigidbody2D rb;

    public GameObject bullet;
    public float bulletOffset;

    public float shootDelay;
    bool canShoot = true;

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

    private void OnEnable()
    {
        controls.Player.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }
}
