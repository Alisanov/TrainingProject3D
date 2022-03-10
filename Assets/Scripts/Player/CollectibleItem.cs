using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    [SerializeField] private string itemName;
    [SerializeField] private AudioClip audioClip;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            Managers.Inventory.AddItem(itemName);
            AudioSource audioSource = other.GetComponent<AudioSource>();
            audioSource?.PlayOneShot(audioClip);
        }
        Destroy(gameObject);
    }
}
