using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�ݵ�� �ʿ��� ������Ʈ�� ����� �ش� ������Ʈ�� �����Ǵ� ���� ����
[RequireComponent(typeof(AudioSource))]
public class CsFireCtrl : MonoBehaviour
{
    //�Ѿ� ������
    public GameObject bullet;
    //�Ѿ� �߻� ��ġ
    public Transform firePos;
    //�Ѿ� �߻� ����
    public AudioClip fireSfx;
    //AudioSource ������Ʈ�� ������ ����
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

            //Ray�� ���� ���� ������Ʈ�� ������ �޾ƿ� ����
            RaycastHit hit;
            //RayCast �Լ��� Ray�� �߻��� ���� ���ӿ�����Ʈ�� ���� �� true�� ��ȯ
            if(Physics.Raycast(firePos.position,firePos.forward,out hit, 10.0f))
            {
                //Ray�� ���� ���ӿ�����Ʈ�� tag���� ���� ���� ���� üũ
                if(hit.collider.tag == "MONSTER")
                {
                    //SendMassage�� �̿��� ������ ���ڸ� �迭�� ����
                    //��� ������ ������ object�� ����� �ޱ� ������,
                    //��� ������ �������� ��ȯ�� ����
                    object[] _params = new object[2];
                    _params[0] = hit.point; // Ray�� ���� ��Ȯ�� ��ġ��(Vector3)
                    _params[1] = 20; // ���Ϳ��� ���� ������ ��
                    //���Ϳ� ������ ������ �Լ� ȣ��
                    hit.collider.gameObject.SendMessage("OnDamage", _params, SendMessageOptions.DontRequireReceiver);
                }
                
                if (hit.collider.tag == "BARREL")
                {
                    //SendMassage�� �̿��� ������ ���ڸ� �迭�� ����
                    //��� ������ ������ object�� ����� �ޱ� ������,
                    //��� ������ �������� ��ȯ�� ����
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
        //���� �߻� �Լ�(�����Ŭ��, ����)
        source.PlayOneShot(fireSfx, 0.9f);
    }

    void CreateBullet()
    {
        Instantiate(bullet, firePos.position, firePos.rotation);
    }

    IEnumerator ShowMuzzleFlash()
    {
        //�������� �ұ�Ģ�ϰ� ����
        float scale = Random.Range(1.0f, 2.0f);
        muzzleFlash.transform.localScale = Vector3.one * scale;

        //z������ �ұ�Ģ�ϰ� ȸ��
        Quaternion rot = Quaternion.Euler(0, 0, Random.Range(0, 360));
        muzzleFlash.transform.localRotation = rot;
        //Ȱ��ȭ
        muzzleFlash.enabled = true;
        //�ұ�Ģ���� �ð����� Delay�� ���� MeshRenderer�� ��Ȱ��ȭ
        yield return new WaitForSeconds(Random.Range(0.05f, 0.3f));
        //��Ȱ��ȭ
        muzzleFlash.enabled = false;
    }
}
