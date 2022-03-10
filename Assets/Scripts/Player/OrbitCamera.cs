using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
    [SerializeField] private Transform target;

    public float rotSpeed = 1.5f;
    private float _rotY;
    private float _rotX;
    private Vector3 _offset;

    void Start()
    {
        _rotY = transform.eulerAngles.y;
        _offset = target.position - transform.position;
        transform.LookAt(target);
    }

    void LateUpdate()
    {
        if (Input.GetMouseButton(1))
        {
            float horInput = Input.GetAxis("Horizontal");
            float vertInput = Input.GetAxis("Vertical");
            if (horInput != 0)
                _rotY += horInput * rotSpeed;
            else
                _rotY += Input.GetAxis("Mouse X") * rotSpeed * 3;

            if (vertInput != 0)
                _rotX += vertInput * rotSpeed;
            else
                _rotX += Input.GetAxis("Mouse Y") * rotSpeed * 3;
        }
            Quaternion rotation = Quaternion.Euler(_rotX, _rotY, 0);
            transform.position = target.position - (rotation * _offset);
            transform.LookAt(target);
        
        
    }
}
