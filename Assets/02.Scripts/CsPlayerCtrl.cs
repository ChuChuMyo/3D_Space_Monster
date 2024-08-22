using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
//클래스에 [Sysytem.Serializable]를 명시해 인스펙터 뷰에 노출
[System.Serializable]
public class Anim
{
    public AnimationClip idle;
    public AnimationClip runForward;
    public AnimationClip runBackward;
    public AnimationClip runRight;
    public AnimationClip runLeft;
}


public class CsPlayerCtrl : MonoBehaviour
{
    float h = 0f;
    float v = 0f;

    Transform tr;
    //이동 속도
    public float moveSpeed = 10f;
    //회전 속도
    public float rotSpeed = 100f;

    //인스펙터뷰에 표시할 애니메이션 클래스 변수
    public Anim anim;
    //자식으로있는 3D모델의 Animation 컴포넌트에 접근하기 위한 변수
    public Animation _animation;

    public int hp = 100;

    public Image imgHpbar;
    //Player 생명 초기값
    private int initHp;

    void Start()
    {
        initHp = hp;
        tr = GetComponent<Transform>();
        //자신의 자식으로 있는 Animation 컴포넌트를 찾아와 변수에 할당
        _animation = GetComponentInChildren<Animation>();

        //Animation 컴포넌트의 애니메이션 클립을 저장
        _animation.clip = anim.idle;
        //애니메이션 실행
        _animation.Play();
    }

    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        //전후좌우 이동 방향 벡터 계산
        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);
        //(기준 좌표계 Space.World : 월드좌표계 / Space.Slef : 로컬좌표계)
        tr.Translate(moveDir * Time.deltaTime * moveSpeed, Space.Self);

        //Vector3.up 축을 기준으로 rotSpeed만큼의 속도로 회전
        //마우스 좌우 이동 값을 감지 처리
        tr.Rotate(Vector3.up * Time.deltaTime * rotSpeed * Input.GetAxis("Mouse X"));

        //키보드 입력값을 기준으로 동작할 애니메이션 수행
        if (v >= 0.1f)
        {
            //전진 애니메이션
            //CrossFade : 애니메이션의 변가를 부드럽게 해주는 블렌딩 함수
            //(애니메이션 클립 명칭, 페이드아웃되는 시간)
            _animation.CrossFade(anim.runForward.name, 0.4f);
        }
        else if (v <= -0.1f)
        {
            //후진 이동 애니메이션
            _animation.CrossFade(anim.runBackward.name, 0.3f);
        }
        else if (h >= 0.1f)
        {
            //오른쪽 애니메이션
            _animation.CrossFade(anim.runRight.name, 0.3f);
        }
        else if (h <= -0.1f)
        {
            //왼쪽 애니메이션
            _animation.CrossFade(anim.runLeft.name, 0.3f);
        }
        else
        {
            //대기 애니메이션
            _animation.CrossFade(anim.idle.name, 0.3f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "PUNCH")
        {
            hp -= 10;
            Debug.Log("Player HP = " + hp.ToString());

            imgHpbar.fillAmount = (float)hp / (float)initHp;

            if(hp <= 0)
            {
                PlayerDie();
            }
        }
    }

    void PlayerDie()
    {
        Debug.Log("플레이어 사망");
        //MONSTER라는 태그를 가진 모든 게임오브젝트를 찾아옴
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("MONSTER");
        //모든 몬스터의 OnPlayerDie 함수를 순차적으로 호출
        foreach(GameObject monster in monsters)
        {
            //private 함수 호출 방법
            monster.SendMessage("OnPlayerDie", SendMessageOptions.DontRequireReceiver);
        }
    }
}
