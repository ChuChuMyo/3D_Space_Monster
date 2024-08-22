using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    Transform tr;
    LineRenderer line;
    //광선에 충돌한 게임 오브젝트의 정보를 받아올 변수
    RaycastHit hit;


    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
        line = GetComponent<LineRenderer>();

        //로컬 좌표를 기준으로 변경
        line.useWorldSpace = false;
        //초기에 비활성화
        line.enabled = false;
        //라인의 시작폭과 종료폭 설정
        line.SetWidth(0.3f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        //광선을 미리 생성
        Ray ray = new Ray(tr.position + (Vector3.up * 0.02f), tr.forward);

        //광선을 눈에 보이게 설정(레이의 원점, 레이의 방향)
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.blue);

        if(Input.GetButtonDown("Fire1"))
        {
            //Line Renderer의 첫번째 점의 위치 설정 (월드위치벡터를 로컬로 바꾼다)
            line.SetPosition(0, tr.InverseTransformPoint(ray.origin));

            //
            if(Physics.Raycast(ray, out hit, 100f))
            {
                //LineRenderer 시작 위치를 성정 SetPosition(int index, Vector3 postion)
                line.SetPosition(1, tr.InverseTransformPoint(hit.point));
            }
            else
            {
                //ray를 따라 distance의 거리 지점을 반환
                line.SetPosition(1, tr.InverseTransformPoint(ray.GetPoint(100f)));
            }

            //광선을 표시하는 코루틴 함수 호출
            StartCoroutine(ShowLaserBeam());
        }
    }

    IEnumerator ShowLaserBeam()
    {
        line.enabled = true;
        yield return new WaitForSeconds(Random.Range(0.01f, 0.2f));
        line.enabled = false;
    }
}
