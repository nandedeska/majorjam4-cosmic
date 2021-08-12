using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public GameObject[] enemyAnimBase;
    public float interval;

    float[][][][] enemy;
    bool waveGenFinished;
    bool waveFinished;

    public Text waveText;

    private void Awake()
    {
        enemy = new float[][][][]
        {
            // WAVES
            new float[][][]
            {
                // BATCHES
                new float[][]
                {
                    new float[3] { 0.3f, 2f, 0f },
                    new float[3] { 0.3f, 2.6f, 0f },
                    new float[3] { -0.3f, 2f, 0f },
                    new float[3] { -0.3f, 2.6f, 0f },
                    new float[3] { 0.3f, 3.2f, 1f },
                    new float[3] { 0.3f, 3.8f, 1f },
                    new float[3] { -0.3f, 3.2f, 1f },
                    new float[3] { -0.3f, 3.8f, 1f }
                },
                new float[][]
                {
                    new float[3] { 0.9f, 2f, 0f },
                    new float[3] { 0.9f, 2.6f, 0f },
                    new float[3] { 1.5f, 2f, 0f },
                    new float[3] { 1.5f, 2.6f, 0f },
                    new float[3] { -0.9f, 2f, 0f },
                    new float[3] { -0.9f, 2.6f, 0f },
                    new float[3] { -1.5f, 2f, 0f },
                    new float[3] { -1.5f, 2.6f, 0f }
                },
                new float[][]
                {
                    new float[3] { 2.1f, 2f, 0f },
                    new float[3] { 2.1f, 2.6f, 0f },
                    new float[3] { 2.7f, 2f, 0f },
                    new float[3] { 2.7f, 2.6f, 0f },
                    new float[3] { -2.1f, 2f, 0f },
                    new float[3] { -2.1f, 2.6f, 0f },
                    new float[3] { -2.7f, 2f, 0f },
                    new float[3] { -2.7f, 2.6f, 0f }
                },
                new float[][]
                {
                    new float[3] { 0.9f, 3.2f, 1f },
                    new float[3] { 1.5f, 3.2f, 1f },
                    new float[3] { 0.9f, 3.8f, 1f },
                    new float[3] { 1.5f, 3.8f, 1f },
                    new float[3] { -0.9f, 3.2f, 1f },
                    new float[3] { -1.5f, 3.2f, 1f },
                    new float[3] { -0.9f, 3.8f, 1f },
                    new float[3] { -1.5f, 3.8f, 1f }
                },
            }
        };
    }

    private void Start()
    {
        #region OLD SPAWN SYSTEM
        /*float y = 4;

        for (int i = 0; i < waves; i++)
        {
            float x = -2.5f;

            for (int j = 0; j < 5; j++)
            {
                Instantiate(enemyPb, new Vector2(x, y), Quaternion.identity);
                x += 1.25f;
            }

            y += 1f;
        }*/
        #endregion

        /*for (int i = 0; i < pos.Length; i++)
        {
            for (int j = 0; j < pos[i].Length; j++)
            {
                string str = $"Wave {i++}: ";
                for (int k = 0; k < pos[i][j].Length; k++)
                {
                    str += pos[i][j][k] + ", ";
                }
                Debug.Log(str);
            }
        }*/

        waveGenFinished = true;

        StartCoroutine(Spawn(interval));
    }

    private void Update()
    {
        if (FindObjectsOfType<Enemy>().Length != 0)
            waveFinished = false;
        else
            waveFinished = true;
    }

    IEnumerator Spawn(float delay)
    {
        for (int i = 0; i < enemy.Length; i++)
        {
            while (!waveFinished || !waveGenFinished)
                yield return null;

            waveGenFinished = false;
            waveText.text = $"WAVE {i + 1}";
            waveText.gameObject.SetActive(true);

            yield return new WaitForSeconds(delay * 1.5f);
            waveText.gameObject.SetActive(false);

            #region LOAD WAVE
            for (int j = 0 ; j < enemy[i].Length; j++)
            {
                #region LOAD ROW
                int r = Random.Range(0, 3);
                string pathName = string.Empty;

                switch (r)
                {
                    case 0:
                        pathName = "EntryPath1";
                        break;

                    case 1:
                        pathName = "EntryPath2";
                        break;

                    case 2:
                        pathName = "EntryPath3";
                        break;
                }

                for (int k = 0; k < enemy[i][j].Length; k++)
                {
                    #region LOAD ENEMY
                    Enemy spawned = Instantiate(enemyPrefabs[(int)enemy[i][j][k][2]], new Vector2(0f, 15f), Quaternion.identity).GetComponent<Enemy>();
                    spawned.targetPos = new Vector2(enemy[i][j][k][0], enemy[i][j][k][1]);
                    spawned.pathName = pathName;
                    spawned.GetComponentInChildren<Animator>().Play("Base Layer.Default", 0, 
                        enemyAnimBase[(int)enemy[i][j][k][2]].GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime);
                    yield return new WaitForSeconds(spawned.speed / (spawned.speed * 10));
                    #endregion
                }

                waveFinished = false;

                yield return new WaitForSeconds(delay);
                #endregion
            }

            waveGenFinished = true;
            #endregion
        }
    }
}
