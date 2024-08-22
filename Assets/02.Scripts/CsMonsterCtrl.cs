using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CsMonsterCtrl : MonoBehaviour
{
    public enum MonsterState
    {
        idle,
        trace,
        attack,
        die
    }

    public MonsterState mState;

    public float traceDist = 10f;
    public float attackDist = 2f;
    private bool isDie = false;

    //속도 향상을 위해 각종 컴포넌트 변수 할당
    private Transform monsterTr;
    private Transform playerTr;
    private NavMeshAgent nvAgent;
    private Animator animator;

    public GameObject bloodEffect;
    public GameObject bloodDecal;
    GameUI gameUI;
    public int hp = 100;
    // Start is called before the first frame update
    void Start()
    {
        monsterTr = GetComponent<Transform>();
        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();
        nvAgent = GetComponent<NavMeshAgent>();
        gameUI = GameObject.Find("GameUI").GetComponent<GameUI>();
        animator = GetComponent<Animator>();
        //추적 대상의 위치를 설정하면 바로 추적 시작
        //플레이어의 초기 위치 이후 플레이어의 위치를 따라가려면
        //nvAgent.destination = playerTr.position;

        //일정한 간격으로 몬스터의 행동 상태를 체크하는 코루틴 함수 실행
        StartCoroutine(CheckMonsterState());
        //몬스터의 상태에 따라 동작하는 루틴을 실행하는 코루틴 함수 실행
        StartCoroutine(MonsterAction());

    }

    IEnumerator CheckMonsterState()
    {
        while(!isDie)
        {
            //0.2초 동안 기다렸다가 다음으로 넘어감.
            yield return new WaitForSeconds(0.2f);
            //몬스터와 플레이어 사이의 거리 측정
            float dist = Vector3.Distance(playerTr.position, monsterTr.position);

            //공격거리 범위 이내로 들어왔는지 확인
            if(dist <= attackDist)
            {
                //몬스터의 상태를 공격으로
                mState = MonsterState.attack;
            }
            else if(dist <= traceDist)
            {
                //몬스터의 상태를 추적으로 설정
                mState = MonsterState.trace;
            }
            else
            {
                //몬스터의 상태를 idle로 설정
                mState = MonsterState.idle;
            }
        }
    }

    IEnumerator MonsterAction()
    {
        while(!isDie)
        {
            switch(mState)
            {
                case MonsterState.idle:
                    nvAgent.Stop();
                    animator.SetBool("IsTrace", false);
                    break;
                case MonsterState.trace:
                    nvAgent.destination = playerTr.position;
                    nvAgent.Resume();
                    animator.SetBool("IsAttack", false);
                    animator.SetBool("IsTrace", true);
                    break;
                case MonsterState.attack:
                    animator.SetBool("IsAttack", true);
                    break;
            }
            yield return null;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "BULLET")
        {
            animator.SetTrigger("IsHit");
            Destroy(collision.gameObject);
            CreateBooldEffect(collision.transform.position);

            hp -= collision.gameObject.GetComponent<CsBulletCtrl>().damage;
            if(hp<=0)
            {
                MonsterDie();
            }
            
        }
    }

    void MonsterDie()
    {
        //사망한 몬스터의 태그를 변경
        gameObject.tag = "Untagged";

        StopAllCoroutines();

        isDie = true;
        //스위치 문에 해당하는 루틴은 없음.
        mState = MonsterState.die;
        animator.SetTrigger("IsDie");
        //몬스터에 추가된 Collider 비활성화
        gameObject.GetComponentInChildren<CapsuleCollider>().enabled = false;
        foreach(Collider coll in gameObject.GetComponentsInChildren<Collider>())
        {
            coll.enabled = false;
        }
        gameUI.DispScore(50);
    }

    void CreateBooldEffect(Vector3 pos)
    {
        GameObject blood1 = Instantiate(bloodEffect, pos, Quaternion.identity);
        Destroy(blood1, 2f);
        //데칼 생성 위치 : 바닥에서 조금 위로
        Vector3 decalPos = monsterTr.position + (Vector3.up * 0.05f);
        //데칼의 회전값을 무작위로 설정
        Quaternion decalRot = Quaternion.Euler(90, 0, Random.Range(0, 360));
        //데칼 프리팹 생성
        GameObject blood2 = Instantiate(bloodDecal, decalPos, decalRot);
        //데칼 크기도 불규칙적으로 나타나게 스케일 조정
        float scale = Random.Range(1.5f, 3.5f);
        blood2.transform.localScale = Vector3.one * scale;
        //5초후에 데칼 삭제
        Destroy(blood2, 5f);
    }

    void OnPlayerDie()
    {
        //몬스터의 상태를 체크하는 코루틴을 모두 정지시킴
        StopAllCoroutines();
        //추적을 정지하고 애니메이션을 수행
        nvAgent.Stop();
        animator.SetTrigger("IsPlayerDie");
    }

    private void OnDamage(object[] _params)
    {
        Debug.Log(string.Format("Hit ray {0} : {1}", _params[0], _params[1]));

        //혈흔 효과 함수 호출
        CreateBooldEffect((Vector3)_params[0]);

        //맞은 총알의 Damage를 추출해 몬스터 hp 차감
        hp -= (int)_params[1];
        if(hp <= 0)
        {
            MonsterDie();
        }

        //IsHit Trigger를 발생시키면 Any State에서 gohit로 전이됨
        animator.SetTrigger("IsHit");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
