using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public AudioSource _bgmsource;
    public AudioSource _stepaudio;
    public bool grass;

    private bool walking;
    private int symbols;
    private int sample;

    //audio stuff
    public List<AudioClip> footstepSamples;
    public List<AudioClip> grassFootstepSamples;

    private float tmpTimeBetweenTwoSteps = 0.25f;
    private float _Timer = 0f;

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
        
        if (walking && _grounded)
        {
            sample = 1 - sample;
            PlayFootstep(sample);
        }
        
    }

    void OnTriggerEnter(Collider other)
    { 
        if (other.tag == "Altar")
        {
            TowerController towerController = other.transform.parent.gameObject.GetComponent<TowerController>();
            if (symbols == towerController.symbolNum)
            {
                //TODO: change bgm and terrain and open door
                _bgmsource.clip = towerController.nextBGM;
                _bgmsource.Play();
                towerController.activateNextTerrain();
                towerController.doors.SetActive(false);
                Destroy(other.gameObject);
            }
        }
    }

    private void PlayFootstep(int sample)
    {
        if (!_stepaudio.isPlaying)
        {
            if (_Timer >= tmpTimeBetweenTwoSteps)
            {
                if (!grass)
                {
                    _stepaudio.clip = footstepSamples[sample];
                }
                else
                {
                    _stepaudio.clip = grassFootstepSamples[sample];
                }

                int tmpRandomPitch = UnityEngine.Random.Range(-5, 6);
                _stepaudio.pitch = 1 + (tmpRandomPitch * .0001f);

                int tmpRandomStartPoint = UnityEngine.Random.Range(0, 9);
                _stepaudio.time = tmpRandomStartPoint * .001f;

                _stepaudio.Play();
                _Timer = 0;
            }
            else
            {
                _Timer = Mathf.MoveTowards(_Timer, tmpTimeBetweenTwoSteps, Time.deltaTime);
            }
        }
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
