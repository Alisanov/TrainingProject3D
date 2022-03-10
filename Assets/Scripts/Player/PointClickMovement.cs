using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]
public class PointClickMovement : MonoBehaviour
{
    //[SerializeField] private Transform target;
    //[SerializeField] private Camera camera;
    [SerializeField] private AudioClip[] walkAudio;
    

    public float rotSpeed = 15.0f;
    public float moveSpeed = 6.0f;

    //public float jumpSpeed = 15.0f;
    public float gravity = -9.8f;
    public float terminalVelocity = -10.0f;
    public float minFall = -1.5f;

    private float _vertSpeed;
    private CharacterController _charController;
    private AudioSource _audioSource;
    private ControllerColliderHit _contact;
    private Animator _animation;

    public float deceleration = 25.0f;
    public float targetBuffer = 1.5f;
    private float _curSpeed = 0f;
    private Vector3 _targetPos = Vector3.one;

    private float pushForce = 3.0f;

    private void Start()
    {
        _charController = GetComponent<CharacterController>();
        _audioSource = GetComponent<AudioSource>();
        _vertSpeed = minFall;
        _animation = GetComponent<Animator>();
    }

    private void Update()
    {
        Vector3 movement = Vector3.zero;

        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit mouseHit;
            if (Physics.Raycast(ray, out mouseHit))
            {
                GameObject hitObject = mouseHit.transform.gameObject;
                if(hitObject.layer == LayerMask.NameToLayer("Ground"))
                _targetPos = mouseHit.point;
                _curSpeed = moveSpeed;
            }
        }

        if (_targetPos != Vector3.one)
        {
            if (_curSpeed > moveSpeed * .5f)
            {
                Vector3 adjusterPos = new Vector3(_targetPos.x, transform.position.y, _targetPos.z);
                Quaternion targetRot = Quaternion.LookRotation(adjusterPos - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotSpeed * Time.deltaTime);
            }
            movement = _curSpeed * Vector3.forward;
            movement = transform.TransformDirection(movement);

            if (Vector3.Distance(_targetPos, transform.position) < targetBuffer)
            {
                _curSpeed -= deceleration * Time.deltaTime;
                if (_curSpeed <= 0)
                    _targetPos = Vector3.one;
            }
        }
        _animation.SetFloat("speed", movement.sqrMagnitude);

        //if (_charController.isGrounded)
        //    _vertSpeed = minFall;
        //else
        //{
        //    _vertSpeed += gravity * 5 * Time.deltaTime;
        //    if (_vertSpeed < terminalVelocity)
        //        _vertSpeed = terminalVelocity;
        //}

        //movement.y = _vertSpeed;

        bool hitGround = false;
        RaycastHit hit;
        if (_vertSpeed < 0 && Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            float check = (_charController.height + _charController.radius) / 1.9f;
            hitGround = hit.distance <= check;  // to be sure check slightly beyond bottom of capsule
        }

        if (hitGround)
        {
            // commented out lines remove jump control
            //if (Input.GetButtonDown("Jump")) {
            //	_vertSpeed = jumpSpeed;
            //} else {
            _vertSpeed = minFall;
            //_animator.SetBool("Jumping", false);
            //}
        }
        else
        {
            _vertSpeed += gravity * 5 * Time.deltaTime;
            if (_vertSpeed < terminalVelocity)
            {
                _vertSpeed = terminalVelocity;
            }

            // workaround for standing on dropoff edge
            if (_charController.isGrounded)
            {
                if (Vector3.Dot(movement, _contact.normal) < 0)
                {
                    movement = _contact.normal * moveSpeed;
                }
                else
                {
                    movement += _contact.normal * moveSpeed;
                }
            }
        }
        movement.y = _vertSpeed;

        movement *= Time.deltaTime;
        _charController.Move(movement);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        _contact = hit;

        Rigidbody body = hit.collider.attachedRigidbody;
        if (body != null && !body.isKinematic)
        {
            body.velocity = hit.moveDirection * pushForce;
        }
    }
    
    public void Run()
    {
        _audioSource.PlayOneShot(walkAudio[Random.Range(0, walkAudio.Length)]);
    }
}

