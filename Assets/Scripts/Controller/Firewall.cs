using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firewall : MonoBehaviour
{
    [SerializeField] private float speed = 10.0f;
    [SerializeField] private int damage = 1;
    [SerializeField] private AudioClip[] fireball;
    
    void Update()
    {
        transform.Translate(0, 0, speed * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        PlayerCharacter player = other.GetComponent<PlayerCharacter>();
        if (player != null)
        {
            player.Hurt(damage);
            AudioSource audioSource = player.GetComponent<AudioSource>();
            audioSource.GetComponent<AudioSource>().PlayOneShot(fireball[Random.Range(0, fireball.Length)]);
        }

        Destroy(gameObject);
    }
}
