using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CsFollowCam : MonoBehaviour
{
    //추적할 타깃 게임오브젝트의 Transform 변수
    public Transform targetTr;
    //카메라와의 일정 거리
    public float dist = 10f;
    //카메라 높이 설정
    public float height = 3f;
    //부드러운 추적을 위한 변수
    public float damTrace = 20f;

    //카메라 자신의 Transform 변수
    Transform tr;
    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
        CursorLock();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Update 함수 호출 이후 한번씩 호출되는 함수
    //추적할 타깃의 이동이 종료된 이후에 카메라가 추적하기 위해 사용
    private void LateUpdate()
    {
        //Vector3.Lerp(Vector3 시작위치, Vector3 종료위치, float 시간)
        tr.position = Vector3.Lerp(tr.position, 
            targetTr.position - (targetTr.forward * dist) + (Vector3.up * height), 
            Time.deltaTime * damTrace);

        //카메라가 타깃 게임오브젝트를 바라보게 설정
        tr.LookAt(targetTr.position);
    }

    public void CursorLock()
    {
        // 마우스 커서 화면 중앙에 고정
        Cursor.lockState = CursorLockMode.Locked;
        // 마우스 커서 안보이게
        Cursor.visible = false;
    }
}
