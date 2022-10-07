using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // package stuff
    public float mouseSensitivityX = 1.0f;
    public float mouseSensitivityY = 1.0f;
    public float walkSpeed = 10.0f;

    public float JumpForce = 250.0f;
    public LayerMask GroundedMask;

    Vector3 _moveAmount;
    Vector3 _smoothMoveVelocity;

    Transform _cameraTransform;
    float _verticalLookRotation;

    Rigidbody _rigidbody;
    CapsuleCollider _collider;

    bool _grounded;
    bool _cursorVisible;

    //my stuff
    public bool ended = false;

    private bool walking;

    // Start is called before the first frame update
    void Start()
    {
        _cameraTransform = GetComponentInChildren<Camera>().transform;
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<CapsuleCollider>();
        ToggleMouse();
    }

    // Update is called once per frame
    void Update()
    {
        // rotation
        if (!ended)
        {
            transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * mouseSensitivityX);
            _verticalLookRotation += Input.GetAxis("Mouse Y") * mouseSensitivityY;
            _verticalLookRotation = Mathf.Clamp(_verticalLookRotation, -60, 60);
            _cameraTransform.localEulerAngles = Vector3.left * _verticalLookRotation;

            // movement
            Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
            if (moveDir.magnitude == 0)
            {
                walking = false;
            }
            else
            {
                walking = true;
            }
            Vector3 targetMoveAmount = moveDir * walkSpeed;
            _moveAmount = Vector3.SmoothDamp(_moveAmount, targetMoveAmount, ref _smoothMoveVelocity, .15f);

            // jump
            if (Input.GetButtonDown("Jump"))
            {
                if (_grounded)
                {
                    _rigidbody.AddForce(transform.up * JumpForce);
                }
            }


            Ray ray = new Ray(transform.position, -transform.up);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1 + .1f, GroundedMask))
            {
                _grounded = true;
            }
            else
            {
                _grounded = false;
            }

            /* Lock/unlock mouse on click */
            if (Input.GetMouseButtonUp(2))
            {
                ToggleMouse();
            }
        }
    }

    void FixedUpdate()
    {
        if (!ended)
        {
            _rigidbody.MovePosition(_rigidbody.position + transform.TransformDirection(_moveAmount) * Time.fixedDeltaTime);
        }
        /**
        if (walking && _grounded)
        {
            sample = 1 - sample;
            playFootstep(sample);
        }
        **/
    }

    void ToggleMouse()
    {
        if (_cursorVisible)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _cursorVisible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            _cursorVisible = true;
        }
    }
}
