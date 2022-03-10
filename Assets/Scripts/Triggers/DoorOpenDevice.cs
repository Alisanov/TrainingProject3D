using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpenDevice : MonoBehaviour
{
    [SerializeField] private Vector3 open;
    [SerializeField] private Vector3 close;
    [SerializeField] private float speed = 3f;

    private bool _open;

    public void Operate()
    {
        if (_open)
            iTween.RotateTo(transform.gameObject, open, speed);
        else
            iTween.RotateTo(transform.gameObject, close, speed);

        _open = !_open;
    }

    public void Activate()
    {
        if (!_open)
        {
            iTween.RotateTo(transform.gameObject, open, speed);
            _open = true;
        }
    }
    public void Deactivate()
    {
        if (_open)
        {
            iTween.RotateTo(transform.gameObject, close, speed);
            _open = false;
        }
    }
}
