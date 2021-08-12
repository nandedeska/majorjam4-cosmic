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
    public float speed;
    public float reachDist = 0.01f;
    public float rotSpeed;
    public string pathName;
    Vector3 lastPos;
    Vector3 curPos;
    public Vector2 targetPos;
    bool inPosition = false;

    public EnemyType enemyType;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        entryPath = GameObject.Find(pathName).GetComponent<PathGenerator>();
        lastPos = transform.position;

        //StartCoroutine(Move(interval))
    }

    private void Update()
    {
        if(entryPathIndex < entryPath.waypoints.Count)
        {
            float dist = Vector2.Distance(entryPath.waypoints[entryPathIndex].position, transform.position);
            transform.position = Vector2.MoveTowards(transform.position, entryPath.waypoints[entryPathIndex].position, Time.deltaTime * speed);

            Quaternion rot = Quaternion.LookRotation(entryPath.waypoints[entryPathIndex].position - transform.position);

            if(rot.eulerAngles.y == 90f)
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

            Quaternion rot = Quaternion.LookRotation(new Vector3(targetPos.x, targetPos.y, 0f) - transform.position);

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

        if (inPosition)
        {
            transform.position = new Vector2(targetPos.x + Mathf.PingPong(Time.time, 2f) - 1f, transform.position.y);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag == "Bullet")
        {
            Destroy(gameObject);
        }
    }

    IEnumerator Move(float delay)
    {
        transform.Translate(new Vector2(0f, -0.25f), Space.Self);

        yield return new WaitForSeconds(delay);

        StartCoroutine(Move(delay));
    }
}
