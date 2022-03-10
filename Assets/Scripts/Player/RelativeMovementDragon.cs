using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class RelativeMovementDragon : MonoBehaviour
{
    [SerializeField] private Transform target;

    public float rotSpeed = 15.0f;
    public float moveSpeed = 6.0f;

    public float flySpeed = 15.0f;
    public float gravity = -9.8f;
    public float terminalVelocity = -10.0f;
    public float minFall = -1.5f;

    private float _vertSpeed;
    private CharacterController _charController;
    private ControllerColliderHit _contract;
    private bool _isPressJump;
    private bool _isPressLeftShift;
    private Animator _animation;

    private void Start()
    {
        _charController = GetComponent<CharacterController>();
        _vertSpeed = minFall;
        _animation = GetComponent<Animator>();
    }

    private void Update()
    {
        Vector3 movement = Vector3.zero;

        float horInput = Input.GetAxis("Horizontal");
        float verInput = Input.GetAxis("Vertical");

        if (horInput != 0 || verInput != 0)
        {
            movement.x = horInput * moveSpeed;
            movement.z = verInput * moveSpeed;
            movement = Vector3.ClampMagnitude(movement, moveSpeed);

            Quaternion tmp = target.rotation;
            target.eulerAngles = new Vector3(0, target.eulerAngles.y, 0);
            movement = target.TransformDirection(movement);
            target.rotation = tmp;

            Quaternion direction = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Lerp(transform.rotation, direction, rotSpeed * Time.deltaTime);

        }

        //bool hitGround = false;
        //RaycastHit hit;
        //if (_vertSpeed < 0 && Physics.Raycast(transform.position, Vector3.down, out hit))
        //{
        //    float check = (_charController.height + _charController.radius) / 1.9f;
        //    hitGround = hit.distance <= check;

        //}
        ////Debug.Log(hitGround);
        //if (hitGround) 
        //{
        //    if (Input.GetButtonDown("Jump"))
        //        _vertSpeed = jumpSpeed;
        //    else
        //        _vertSpeed = minFall;
        //}
        //else
        //{
        //    _vertSpeed += gravity * 5 * Time.deltaTime;
        //    if (_vertSpeed < terminalVelocity)
        //        _vertSpeed = terminalVelocity;
        //    if(_charController.isGrounded)
        //    {
        //        if (Vector3.Dot(movement, _contract.normal) < 0)
        //            movement = _contract.normal * moveSpeed;
        //        else
        //            movement += _contract.normal * moveSpeed;
        //    }
        //}

        _animation.SetFloat("speed", movement.sqrMagnitude);

        _animation.SetBool("fly", !_charController.isGrounded);
        Debug.Log(!_charController.isGrounded);

        if (Input.GetButtonDown("Jump"))
            _isPressJump = true;
        else if (Input.GetButtonUp("Jump"))
            _isPressJump = false;

        if (Input.GetKeyDown(KeyCode.LeftShift))
            _isPressLeftShift = true;
        else if (Input.GetKeyUp(KeyCode.LeftShift))
            _isPressLeftShift = false;

        if (_isPressJump)
            _vertSpeed += flySpeed;
        else if (_isPressLeftShift)
            _vertSpeed -= flySpeed;


        if (!_isPressJump)
        {
            if (_charController.isGrounded)
            {
                _vertSpeed = minFall;
            }
            else
            {
                _vertSpeed += gravity * 5 * Time.deltaTime;
                if (_vertSpeed < terminalVelocity)
                    _vertSpeed = terminalVelocity;
            }
        }




        movement.y = _vertSpeed;

        movement *= Time.deltaTime;
        _charController.Move(movement);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        _contract = hit;
    }
}
