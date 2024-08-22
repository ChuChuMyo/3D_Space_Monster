using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//반드시 필요한 컴포넌트를 명시해 해당 컴포넌트가 삭제되는 것을 방지
[RequireComponent(typeof(AudioSource))]
public class CsFireCtrl : MonoBehaviour
{
    //총알 프리팹
    public GameObject bullet;
    //총알 발사 위치
    public Transform firePos;
    //총알 발사 사운드
    public AudioClip fireSfx;
    //AudioSource 컴포넌트를 저장할 변수
    private AudioSource source = null;

    public MeshRenderer muzzleFlash;
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();

        muzzleFlash.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(firePos.position, firePos.forward * 10.0f, Color.green);
        if (Input.GetButtonDown("Fire1"))
        {
            Fire();

            //Ray에 맞은 게임 오브젝트의 정보를 받아올 변수
            RaycastHit hit;
            //RayCast 함수로 Ray를 발사해 맞은 게임오브젝트가 있을 때 true를 반환
            if(Physics.Raycast(firePos.position,firePos.forward,out hit, 10.0f))
            {
                //Ray에 맞은 게임오브젝트의 tag값을 비교해 몬스터 여부 체크
                if(hit.collider.tag == "MONSTER")
                {
                    //SendMassage를 이용해 전달한 인자를 배열에 담음
                    //모든 데이터 형식은 object의 상속을 받기 때문에,
                    //모든 데이터 형식으로 변환이 가능
                    object[] _params = new object[2];
                    _params[0] = hit.point; // Ray에 맞은 정확한 위치값(Vector3)
                    _params[1] = 20; // 몬스터에게 입힐 데미지 값
                    //몬스터에 데미지 입히는 함수 호출
                    hit.collider.gameObject.SendMessage("OnDamage", _params, SendMessageOptions.DontRequireReceiver);
                }
                
                if (hit.collider.tag == "BARREL")
                {
                    //SendMassage를 이용해 전달한 인자를 배열에 담음
                    //모든 데이터 형식은 object의 상속을 받기 때문에,
                    //모든 데이터 형식으로 변환이 가능
                    object[] _params = new object[2];
                    _params[0] = firePos.position; 
                    _params[1] = hit.point; 
                    hit.collider.gameObject.SendMessage("OnDamage", _params, SendMessageOptions.DontRequireReceiver);
                }
            }
        }
    }

    public void Fire()
    {
        //CreateBullet();
        StartCoroutine(ShowMuzzleFlash());
        //사운드 발생 함수(오디오클립, 볼륨)
        source.PlayOneShot(fireSfx, 0.9f);
    }

    void CreateBullet()
    {
        Instantiate(bullet, firePos.position, firePos.rotation);
    }

    IEnumerator ShowMuzzleFlash()
    {
        //스케일을 불규칙하게 변경
        float scale = Random.Range(1.0f, 2.0f);
        muzzleFlash.transform.localScale = Vector3.one * scale;

        //z축으로 불규칙하게 회전
        Quaternion rot = Quaternion.Euler(0, 0, Random.Range(0, 360));
        muzzleFlash.transform.localRotation = rot;
        //활성화
        muzzleFlash.enabled = true;
        //불규칙적인 시간동안 Delay한 다음 MeshRenderer를 비활성화
        yield return new WaitForSeconds(Random.Range(0.05f, 0.3f));
        //비활성화
        muzzleFlash.enabled = false;
    }
}
