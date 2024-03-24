using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class CameraTest : MonoBehaviour
{
    public float RotateSpeed;
    public float MoveSpeed;
    public const float FreeMaxPitch = 80;
    public enum CameraStatus {freeCamera=0,player1,player2};
    public CameraStatus _cameraStatus;
    public GameObject _target;//Ŀ������
    public GameObject _player1;
    public GameObject _player2;
    public UnityEngine.Transform initialTransform;
    Vector3 offset;//��������ƫ����
    void Start()
    {
        offset = new Vector3(5, 5, 5);
        initialTransform = transform;
        RotateSpeed = 300f;
        MoveSpeed = 5f;
        _cameraStatus = CameraStatus.player1;
        _player1 = GameObject.Find("T1");
        _player2 = GameObject.Find("T2");
        _target = _player1;
        //��֤���������Ŀ�����壬��z����ת����0
         transform.position = _target.transform.position - offset;
        transform.LookAt(_target.transform.position);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
        //�õ������������֮��ĳ�ʼƫ����

    }

    void LateUpdate()
    {

        if (_cameraStatus == CameraStatus.player1 || _cameraStatus == CameraStatus.player2)
        {
            Rotate();
            Rollup();
            ExchangeStatus();
            Follow();
        }
        else
        {
            Move();
            ExchangeStatus();
    
        }
    }
    void visualAngleReset()
    {
        offset = new Vector3(5, 5, 5);
        transform.position = _target.transform.position - new Vector3(5, 5, 5);
        transform.LookAt(_target.transform.position);
        //transform.eulerAngles = new Vector3(initialTransform.eulerAngles.x, initialTransform.eulerAngles.y, 0);
    }
    //��������桢�������Ź���:
    void ExchangeStatus()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (_cameraStatus == CameraStatus.player2)
            {
                _cameraStatus= CameraStatus.freeCamera;
            }
            else if(_cameraStatus == CameraStatus.player1)
            {
                _cameraStatus = CameraStatus.player2;
                _target = _player2;
                Debug.Log(transform.position);
                Debug.Log($"target{_target.transform.position}");
                visualAngleReset();
                Debug.Log($"after {transform.position}");

            }
            else if(_cameraStatus == CameraStatus.freeCamera)
            {
                _cameraStatus= CameraStatus.player1;
                _target = _player1;
                visualAngleReset();

            }
        }
    }
    public float zoomSpeed = 1f; // ��Ұ�������ٶ�
    float zoom;//���ֹ�����
    void Follow()
    {
        //��Ұ����
        zoom = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed; // ��ȡ���ֹ�����
        if (zoom != 0) // ����й���
        {
            offset -= zoom * offset;
        }
        //��ͷ����
        transform.position = _target.transform.position - offset;
    }

    //������ת��������ת����:

    public float rotationSpeed = 500f;//�������ת�ٶ�
    public bool isRotating, lookup;
    float mousex, mousey;
    void Rotate()
    {
        /*if (Input.GetMouseButtonDown(1))//��������Ҽ�
        {
            isRotating = true;
        }
        if (Input.GetMouseButtonUp(1))
        {
            isRotating = false;
        }*/
        isRotating = true;
        if (isRotating)
        {
            //�õ����x�����ƶ�����
            mousex = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            //��ת���λ����Ŀ�����崦����������������ϵ��y��

            transform.RotateAround(_target.transform.position, Vector3.up, mousex);
            //ÿ����ת�����ƫ����
            offset = _target.transform.position - transform.position;
        }
    }
    void Rollup()
    {
        /*if (Input.GetMouseButtonDown(2))//��������м�
        {
            lookup = true;
        }
        if (Input.GetMouseButtonUp(2))
        {
            lookup = false;
        }*/
        lookup = true;
        if (lookup)
        {
            //�õ����y�����ƶ�����
            mousey = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
            //��ת���λ����Ŀ�����崦���������������x��
            if (Mathf.Abs(transform.rotation.x + mousey-initialTransform.rotation.x) > 90) mousey = 0;
            transform.RotateAround(_target.transform.position, transform.right, mousey);
            //ÿ����ת�����ƫ����
            offset = _target.transform.position - transform.position;
        }

    }
    public float speed = 0.1f;
    void Move()
    {
        CameraRotate();
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Move when "w a s d" is pressed
        if (Mathf.Abs(vertical) > 0.01)
        {
            Vector3 fowardVector = transform.forward;
            fowardVector = new Vector3(fowardVector.x, 0, fowardVector.z).normalized;
            // move forward
            transform.Translate(MoveSpeed * Time.deltaTime * vertical * fowardVector, Space.World);
        }
        if (Mathf.Abs(horizontal) > 0.01)
        {
            Vector3 rightVector = transform.right;
            rightVector = new Vector3(rightVector.x, 0, rightVector.z).normalized;
            // move aside 
            transform.Translate(MoveSpeed * Time.deltaTime * horizontal * rightVector, Space.World);
        }

        // Fly up if space is clicked
        if (Input.GetKey(KeyCode.Space))
        {
            transform.Translate(MoveSpeed * Time.deltaTime * Vector3.up, Space.World);
        }
        // Fly down if left shift is clicked
        if (Input.GetKey(KeyCode.LeftShift))
        {
            transform.Translate(MoveSpeed * Time.deltaTime * Vector3.down, Space.World);
        }

    }
    void CameraRotate()
    {
        float MouseX = Input.GetAxis("Mouse X");
        float MouseY = Input.GetAxis("Mouse Y");
        if ((Mathf.Abs(MouseX) > 0.01 || Mathf.Abs(MouseY) > 0.01))
        {
            transform.Rotate(new Vector3(0, MouseX * RotateSpeed * Time.deltaTime, 0), Space.World);

            float rotatedPitch = transform.eulerAngles.x - MouseY * RotateSpeed * Time.deltaTime * 1f;
            if (Mathf.Abs(rotatedPitch > 180 ? 360 - rotatedPitch : rotatedPitch) < FreeMaxPitch)
            {
                transform.Rotate(new Vector3(-MouseY * RotateSpeed * Time.deltaTime * 1f, 0, 0));
            }
            else
            {
                if (transform.eulerAngles.x < 180)
                    transform.eulerAngles = new Vector3((FreeMaxPitch - 1e-6f), transform.eulerAngles.y, 0);
                else
                    transform.eulerAngles = new Vector3(-(FreeMaxPitch - 1e-6f), transform.eulerAngles.y, 0);
            }
        }
    }
}
