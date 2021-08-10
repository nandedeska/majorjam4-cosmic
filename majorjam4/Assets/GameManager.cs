using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject enemyPb;
    public float interval;

    float[][][] pos;
    bool waveGenFinished;
    bool waveFinished;

    public Text waveText;

    private void Awake()
    {
        pos = new float[][][]
        { 
            // WAVES
            new float[][]
            { 
                // ROWS
                new float[] { -2.25f, -1.5f, -0.75f, 0f, 0.75f, 1.5f, 2.25f },
                new float[] {         -1.5f, -0.75f, 0f, 0.75f, 1.5f        },
                new float[] {                -0.75f, 0f, 0.75f,             }
            },
            new float[][]
            { 
                // ROWS
                new float[] {         -1.5f,                    1.5f        },
                new float[] {         -1.5f, -0.75f, 0f, 0.75f, 1.5f        },
                new float[] {                -0.75f, 0f, 0.75f,             }
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
        for (int i = 0; i < pos.Length; i++)
        {
            while (!waveFinished || !waveGenFinished)
                yield return null;

            waveGenFinished = false;
            waveText.text = $"WAVE {i + 1}";
            waveText.gameObject.SetActive(true);

            yield return new WaitForSeconds(delay * 1.5f);
            waveText.gameObject.SetActive(false);

            #region LOAD WAVE
            for (int j = pos[i].Length; j > 0; j--)
            {
                #region LOAD ROW
                int r = Random.Range(0, 3);
                string pathName = (r == 0) ? "EntryPath1" : "EntryPath2";

                for (int k = 0; k < pos[i][j - 1].Length; k++)
                {
                    #region LOAD ENEMY
                    GameObject enemy = Instantiate(enemyPb, new Vector2(pos[i][j - 1][k], -15f), Quaternion.identity);
                    enemy.GetComponent<Enemy>().targetPos = new Vector2(pos[i][j - 1][k], 4 - (j-1));
                    enemy.GetComponent<Enemy>().pathName = pathName;
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
