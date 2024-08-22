using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    Transform tr;
    LineRenderer line;
    //������ �浹�� ���� ������Ʈ�� ������ �޾ƿ� ����
    RaycastHit hit;


    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
        line = GetComponent<LineRenderer>();

        //���� ��ǥ�� �������� ����
        line.useWorldSpace = false;
        //�ʱ⿡ ��Ȱ��ȭ
        line.enabled = false;
        //������ �������� ������ ����
        line.SetWidth(0.3f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        //������ �̸� ����
        Ray ray = new Ray(tr.position + (Vector3.up * 0.02f), tr.forward);

        //������ ���� ���̰� ����(������ ����, ������ ����)
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.blue);

        if(Input.GetButtonDown("Fire1"))
        {
            //Line Renderer�� ù��° ���� ��ġ ���� (������ġ���͸� ���÷� �ٲ۴�)
            line.SetPosition(0, tr.InverseTransformPoint(ray.origin));

            //
            if(Physics.Raycast(ray, out hit, 100f))
            {
                //LineRenderer ���� ��ġ�� ���� SetPosition(int index, Vector3 postion)
                line.SetPosition(1, tr.InverseTransformPoint(hit.point));
            }
            else
            {
                //ray�� ���� distance�� �Ÿ� ������ ��ȯ
                line.SetPosition(1, tr.InverseTransformPoint(ray.GetPoint(100f)));
            }

            //������ ǥ���ϴ� �ڷ�ƾ �Լ� ȣ��
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
