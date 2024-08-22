using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : MonoBehaviour
{
    public Transform[] points;
    public GameObject monsterPrefab;

    public float createTime = 2f;
    public int maxMonster = 2;
    public bool isGameOver = false;
    // Start is called before the first frame update
    void Start()
    {
        points = GameObject.Find("SpawnPoint").GetComponentsInChildren<Transform>(); // 부모 포함

        if(points.Length >0)
        {
            StartCoroutine(CreateMonster());
        }
    }

    IEnumerator CreateMonster()
    {
        while(!isGameOver)
        {
            int monsterCount = (int)GameObject.FindGameObjectsWithTag("MONSTER").Length;
            if(monsterCount < maxMonster)
            {
                yield return new WaitForSeconds(createTime);

                int idx = Random.Range(1, points.Length); // 부모 제외
                Instantiate(monsterPrefab, points[idx].position, points[idx].rotation);
            }
            else
            {
                yield return null;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
