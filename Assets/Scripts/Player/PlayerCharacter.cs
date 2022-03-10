using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    private void Start()
    {
        transform.position = Managers.Player.position;
    }
    //����� �������� ������� �������� ������
    public void Hurt(int damage)
    {
        Managers.Player.ChangeHealth(-damage);
    }

}
