using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderingAI : MonoBehaviour
{
    public float speed = 0.5f;
    public float obstacleRange = 5.0f;
    private bool _alive;
    [SerializeField] private GameObject fireballPrefab;
    private GameObject _fireball;
    public const float baseSpeed = 0.5f;
    public float speed—hange;
    private AudioSource _audioSource;
    [SerializeField] private AudioClip fireballSound;
    
    private void OnSpeedChanged(float value)
    {
        speed—hange = value;

    }
    private void Start()
    {
        _alive = true;
        _audioSource = GetComponent<AudioSource>();
        // Debug.Log("Speed enemy: " + speed—hange);
    }
    void Update()
    {
        if (_alive)
        {
            
            speed = baseSpeed + speed—hange;
            //Debug.Log("Speed enemy: " + speed);
            transform.Translate(0, 0, speed * Time.deltaTime);

            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;
            if (Physics.SphereCast(ray, 0.1f, out hit))
            {
                GameObject hitObject = hit.transform.gameObject;
                if (hitObject.GetComponent<PlayerCharacter>())
                {
                    if(_fireball == null)
                    {
                        _audioSource.PlayOneShot(fireballSound);
                        _fireball = Instantiate(fireballPrefab) as GameObject;
                        _fireball.transform.position = transform.TransformPoint(Vector3.forward * 1.5f);
                        _fireball.transform.rotation = transform.rotation;
                    }
                }
                if (hit.distance < obstacleRange)
                {
                    float angle = Random.Range(-110, 110);
                    transform.Rotate(0, angle, 0);
                }
            }
        }
    }
    public void SetAlive(bool alive)
    {
        _alive = alive;
    }
}
