using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CsWallCtrl : MonoBehaviour
{
    //스파크 파티클 프리팹 연결할 변수
    public GameObject sparkEffect;

    private void Start()
    {
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "BULLET")
        {
            //스파크 파티클을 동적으로 생성
            GameObject spark = Instantiate(sparkEffect, collision.transform.position, Quaternion.identity);
            //시간이 지난 후 삭제 처리
            Destroy(spark, 0.2f);

            //충돌한 게임오브젝트 삭제
            Destroy(collision.gameObject);
        }
    }
}
