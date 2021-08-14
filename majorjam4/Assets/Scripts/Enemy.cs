using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    BITER, FIGHTER
}

public class Enemy : MonoBehaviour
{
    Rigidbody2D rb;
    public float interval;

    public PathGenerator entryPath;
    public int entryPathIndex;
    public string pathName;
    public PathGenerator attackPath;
    public int atkPathIndex;

    public float speed;
    public float reachDist = 0.01f;
    public float rotSpeed;

    public Vector2 targetPos;

    bool inPosition = false;
    public float chanceInterval;
    bool isAttacking;
    bool isShooting;
    public GameObject bullet;

    public EnemyType enemyType;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        entryPath = GameObject.Find(pathName).GetComponent<PathGenerator>();
        
        StartCoroutine(Attack(chanceInterval));

        //StartCoroutine(Move(interval))
    }

    private void Update()
    {
        #region ENTRY
        if (entryPathIndex < entryPath.waypoints.Count)
        {
            float dist = Vector2.Distance(entryPath.waypoints[entryPathIndex].position, transform.position);
            transform.position = Vector2.MoveTowards(transform.position, entryPath.waypoints[entryPathIndex].position, Time.deltaTime * speed);

            Quaternion rot = Quaternion.identity;

            if (entryPath.waypoints[entryPathIndex].position - transform.position != Vector3.zero)
            {
                rot = Quaternion.LookRotation(entryPath.waypoints[entryPathIndex].position - transform.position);
            }

            if (rot.eulerAngles.y == 90f)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(new Vector3(0f, 0f, rot.eulerAngles.y - rot.eulerAngles.x)), Time.deltaTime * rotSpeed);
            else if(rot.eulerAngles.y == 270f)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(new Vector3(0f, 0f, rot.eulerAngles.y + rot.eulerAngles.x)), Time.deltaTime * rotSpeed);

            if (dist <= reachDist)
            {
                entryPathIndex++;
            }
        } 
        else if(entryPathIndex == entryPath.waypoints.Count)
        {
            float dist = Vector2.Distance(targetPos, transform.position);
            transform.position = Vector2.MoveTowards(transform.position, targetPos, Time.deltaTime * speed);

            Quaternion rot = Quaternion.identity;

            if (new Vector3(targetPos.x, targetPos.y, 0f) - transform.position != Vector3.zero)
            {
                rot = Quaternion.LookRotation(new Vector3(targetPos.x, targetPos.y, 0f) - transform.position);
            }

            if (rot.eulerAngles.y == 90f)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(new Vector3(0f, 0f, rot.eulerAngles.y - rot.eulerAngles.x)), Time.deltaTime * rotSpeed);
            else if (rot.eulerAngles.y == 270f)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(new Vector3(0f, 0f, rot.eulerAngles.y + rot.eulerAngles.x)), Time.deltaTime * rotSpeed);

            if (dist <= reachDist)
            {
                transform.rotation = Quaternion.Euler(Vector3.zero);
                inPosition = true;
                entryPathIndex++;
            }
        }
        #endregion

        if (inPosition && !isAttacking)
        {
            transform.position = new Vector2(targetPos.x + Mathf.PingPong(Time.time, 2f) - 1f, transform.position.y);
        }

        #region BITE ATTACK
        if (isAttacking && enemyType == EnemyType.BITER)
        {
            if (atkPathIndex < attackPath.waypoints.Count)
            {
                float dist = Vector2.Distance(attackPath.waypoints[atkPathIndex].position, transform.position);
                transform.position = Vector2.MoveTowards(transform.position, attackPath.waypoints[atkPathIndex].position, Time.deltaTime * speed);

                Quaternion rot = Quaternion.identity;

                if(attackPath.waypoints[atkPathIndex].position - transform.position != Vector3.zero)
                {
                    rot = Quaternion.LookRotation(attackPath.waypoints[atkPathIndex].position - transform.position);
                }

                if (rot.eulerAngles.y == 90f)
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(new Vector3(0f, 0f, rot.eulerAngles.y - rot.eulerAngles.x)), Time.deltaTime * rotSpeed);
                else if (rot.eulerAngles.y == 270f)
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(new Vector3(0f, 0f, rot.eulerAngles.y + rot.eulerAngles.x)), Time.deltaTime * rotSpeed);

                if (dist <= reachDist)
                {
                    atkPathIndex++;
                }
            }
            else if (atkPathIndex == attackPath.waypoints.Count)
            {

                float dist = Vector2.Distance(targetPos, transform.position);
                transform.position = Vector2.MoveTowards(transform.position, targetPos, Time.deltaTime * speed);

                Quaternion rot = Quaternion.identity;

                if (new Vector3(targetPos.x, targetPos.y, 0f) - transform.position != Vector3.zero)
                {
                    rot = Quaternion.LookRotation(new Vector3(targetPos.x, targetPos.y, 0f) - transform.position);
                }

                if (rot.eulerAngles.y == 90f)
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(new Vector3(0f, 0f, rot.eulerAngles.y - rot.eulerAngles.x)), Time.deltaTime * rotSpeed);
                else if (rot.eulerAngles.y == 270f)
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(new Vector3(0f, 0f, rot.eulerAngles.y + rot.eulerAngles.x)), Time.deltaTime * rotSpeed);

                if (dist <= reachDist)
                {
                    transform.rotation = Quaternion.Euler(Vector3.zero);
                    inPosition = true;
                    atkPathIndex++;
                    isAttacking = false;
                }
            }
        }
        #endregion

        #region FIGHTER ATTACK
        if (isAttacking && enemyType == EnemyType.FIGHTER)
        {
            if (atkPathIndex == -1)
            {
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, 0f), Time.deltaTime * speed);

                if (!isShooting)
                    StartCoroutine(Shoot());
            }
            else if (atkPathIndex < attackPath.waypoints.Count && atkPathIndex > -1)
            {
                float dist = Vector2.Distance(attackPath.waypoints[atkPathIndex].position, transform.position);
                transform.position = Vector2.MoveTowards(transform.position, attackPath.waypoints[atkPathIndex].position, Time.deltaTime * speed);

                Quaternion rot = Quaternion.identity;

                if (attackPath.waypoints[atkPathIndex].position - transform.position != Vector3.zero)
                {
                    rot = Quaternion.LookRotation(attackPath.waypoints[atkPathIndex].position - transform.position);
                }

                if (rot.eulerAngles.y == 90f)
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(new Vector3(0f, 0f, rot.eulerAngles.y - rot.eulerAngles.x)), Time.deltaTime * rotSpeed);
                else if (rot.eulerAngles.y == 270f)
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(new Vector3(0f, 0f, rot.eulerAngles.y + rot.eulerAngles.x)), Time.deltaTime * rotSpeed);

                if (dist <= reachDist)
                {
                    atkPathIndex++;
                }
            }
            else if (atkPathIndex == attackPath.waypoints.Count)
            {

                float dist = Vector2.Distance(targetPos, transform.position);
                transform.position = Vector2.MoveTowards(transform.position, targetPos, Time.deltaTime * speed);

                Quaternion rot = Quaternion.identity;

                if (new Vector3(targetPos.x, targetPos.y, 0f) - transform.position != Vector3.zero)
                {
                    rot = Quaternion.LookRotation(new Vector3(targetPos.x, targetPos.y, 0f) - transform.position);
                }

                if (rot.eulerAngles.y == 90f)
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(new Vector3(0f, 0f, rot.eulerAngles.y - rot.eulerAngles.x)), Time.deltaTime * rotSpeed);
                else if (rot.eulerAngles.y == 270f)
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(new Vector3(0f, 0f, rot.eulerAngles.y + rot.eulerAngles.x)), Time.deltaTime * rotSpeed);

                if (dist <= reachDist)
                {
                    transform.rotation = Quaternion.Euler(Vector3.zero);
                    inPosition = true;
                    atkPathIndex++;
                    isAttacking = false;
                }
            }
        }
        #endregion
    }

    IEnumerator Move(float delay)
    {
        transform.Translate(new Vector2(0f, -0.25f), Space.Self);

        yield return new WaitForSeconds(delay);

        StartCoroutine(Move(delay));
    }

    IEnumerator Attack(float delay)
    {
        if (inPosition && !isAttacking)
        {
            if (enemyType == EnemyType.BITER)
            {
                int r = Random.Range(0, 100);
                int randPath = Random.Range(0, 2);
                atkPathIndex = 0;
                int n = FindObjectsOfType<Enemy>().Length;

                if (r < Mathf.Round(20f * (3.5f / n)))
                {
                    switch (randPath)
                    {
                        case 0:
                            attackPath = GameObject.Find("BitePath1").GetComponent<PathGenerator>();
                            break;

                        case 1:
                            attackPath = GameObject.Find("BitePath2").GetComponent<PathGenerator>();
                            break;
                    }

                    isAttacking = true;
                }
            }
            else if (enemyType == EnemyType.FIGHTER)
            {
                int r = Random.Range(0, 100);
                int randPath = Random.Range(0, 2);
                atkPathIndex = -1;
                int n = FindObjectsOfType<Enemy>().Length;

                if (r < Mathf.Round(20f * (3.5f / n)))
                {
                    switch (randPath)
                    {
                        case 0:
                            attackPath = GameObject.Find("FighterPath1").GetComponent<PathGenerator>();
                            break;

                        case 1:
                            attackPath = GameObject.Find("FighterPath2").GetComponent<PathGenerator>();
                            break;
                    }

                    isAttacking = true;
                }
            }
        }

        yield return new WaitForSeconds(delay);

        StartCoroutine(Attack(delay));
    }

    IEnumerator Shoot()
    {
        isShooting = true;
        float r1 = Random.Range(0.15f, 0.25f);
        float r2 = Random.Range(-0.15f, -0.25f);
        Instantiate(bullet, new Vector2(transform.position.x + r1, transform.position.y - 1f), Quaternion.identity);
        Instantiate(bullet, new Vector2(transform.position.x + r2, transform.position.y - 1f), Quaternion.identity);

        yield return new WaitForSeconds(0.75f);

        atkPathIndex++;
        isShooting = false;
    }
}
