using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScale : MonoBehaviour
{

    float distance = 0.0f;
    //����ϵ��
    float scaleFactor = 1f;


    float maxDistance = 10f;
    float minDistance = 0.0f;


    //��¼��һ���ֻ�����λ���ж��û�������Ŵ�����С����
    private Vector2 oldPosition1;
    private Vector2 oldPosition2;


    private Vector2 lastSingleTouchPosition;

    private Vector3 m_CameraOffset;
    private Camera m_Camera;

    public bool useMouse = true;

    //������������Ի�ķ�Χ
    float xMin = -10;
    float xMax = 10;
    float zMin = -5;
    float zMax = 20;

    //�������������¼��ָ˫ָ�ı任
    private bool m_IsSingleFinger;

    //��ʼ����Ϸ��Ϣ����
    void Start()
    {
        m_Camera = this.GetComponent<Camera>();
        m_CameraOffset = m_Camera.transform.position;
    }

    void Update()
    {
        //�жϴ�������Ϊ���㴥��
        if (Input.touchCount == 1)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began || !m_IsSingleFinger)
            {
                //�ڿ�ʼ�������ߴ�������ָ�ſ�������ʱ���¼һ�´�����λ��
                lastSingleTouchPosition = Input.GetTouch(0).position;
            }
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                MoveCamera(Input.GetTouch(0).position);
            }
            m_IsSingleFinger = true;

        }
        else if (Input.touchCount > 1)
        {
            //���ӵ�ָ���������ָ������ʱ��,��¼һ�´�����λ��
            //��֤�������Ŷ��Ǵ���ָ��ָ������ʼ��
            if (m_IsSingleFinger)
            {
                oldPosition1 = Input.GetTouch(0).position;
                oldPosition2 = Input.GetTouch(1).position;
            }

            if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved)
            {
                ScaleCamera();
            }

            m_IsSingleFinger = false;
        }


        //������
        if (useMouse)
        {
            distance -= Input.GetAxis("Mouse ScrollWheel") * scaleFactor;
            distance = Mathf.Clamp(distance, minDistance, maxDistance);
            if (Input.GetMouseButtonDown(0))
            {
                lastSingleTouchPosition = Input.mousePosition;
                Debug.Log("GetMouseButtonDown:" + lastSingleTouchPosition);
            }
            if (Input.GetMouseButton(0))
            {
                MoveCamera(Input.mousePosition);
            }
        }


    }

    /// <summary>
    /// ������������ͷ
    /// </summary>
    private void ScaleCamera()
    {
        //�������ǰ���㴥�����λ��
        var tempPosition1 = Input.GetTouch(0).position;
        var tempPosition2 = Input.GetTouch(1).position;


        float currentTouchDistance = Vector3.Distance(tempPosition1, tempPosition2);
        float lastTouchDistance = Vector3.Distance(oldPosition1, oldPosition2);

        //�����ϴκ����˫ָ����֮��ľ�����
        //Ȼ��ȥ����������ľ���
        distance -= (currentTouchDistance - lastTouchDistance) * scaleFactor * Time.deltaTime;


        //�Ѿ�������ס��min��max֮��
        distance = Mathf.Clamp(distance, minDistance, maxDistance);


        //������һ�δ������λ�ã����ڶԱ�
        oldPosition1 = tempPosition1;
        oldPosition2 = tempPosition2;
    }


    //Update����һ�����ý����Ժ����������������������λ��
    private void LateUpdate()
    {
        var position = m_CameraOffset + m_Camera.transform.forward * -distance;
        m_Camera.transform.position = position;
    }


    private void MoveCamera(Vector3 scenePos)
    {
        Vector3 lastTouchPostion = m_Camera.ScreenToWorldPoint(new Vector3(lastSingleTouchPosition.x, lastSingleTouchPosition.y, -1));
        Vector3 currentTouchPosition = m_Camera.ScreenToWorldPoint(new Vector3(scenePos.x, scenePos.y, -1));

        Vector3 v = currentTouchPosition - lastTouchPostion;
        m_CameraOffset += new Vector3(v.x, 0, v.z) * m_Camera.transform.position.y;

        //���������λ�ÿ����ڷ�Χ��
        m_CameraOffset = new Vector3(Mathf.Clamp(m_CameraOffset.x, xMin, xMax), m_CameraOffset.y, Mathf.Clamp(m_CameraOffset.z, zMin, zMax));
        //Debug.Log(lastTouchPostion + "|" + currentTouchPosition + "|" + v);
        lastSingleTouchPosition = scenePos;
    }


}
