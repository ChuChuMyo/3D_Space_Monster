using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CsSpawn : MonoBehaviour
{
    public GameObject barrel;
    //박스 콜라이더의 사이즈를 가져오기 위한 변수
    private BoxCollider area;

    public int count;

    void Start()
    {
        area = GetComponent<BoxCollider>();
        for (int i = 0; i < count; i++)
        {
            //생성 + 스폰위치를 포함하는 함수
            Spawn();
        }
        //박스콜라이더 컴퍼넌트 비활성화
        area.enabled = false;
    }

    private Vector3 GetRandomPosition()
    {
        Vector3 basePosition = transform.position; // 0,0,0
        Vector3 size = area.size;

        float posX = basePosition.x + Random.Range(-size.x / 2f, size.x / 2f);
        float posZ = basePosition.z + Random.Range(-size.z / 2f, size.z / 2f);

        Vector3 spawnPos = new Vector3(posX, 0, posZ);
        return spawnPos;
    }

    private void Spawn()
    {
        //int selection = Random.Range(0, prefabs.Length);

        Vector3 spawnPos = GetRandomPosition(); // 랜덤위치함수

        GameObject instance = Instantiate(barrel, spawnPos, Quaternion.identity);


    }
}
