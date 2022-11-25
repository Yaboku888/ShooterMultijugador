using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Variables
    public Camera cam;
    public Transform recoil;

    [SerializeField] private CharacterController controller;
    [SerializeField] private Transform pointOfView;
    [SerializeField] private Transform gunPoint;
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float gravityMod = 2.5f;

    [SerializeField]private float actualSpeed;
    private float horizontalRotateStore;
    private float verticalRotateStore;
    private Vector2 mouseInput;
    private Vector3 direction;
    private Vector3 movement;

    [Header("Ground Detention")]
    [SerializeField] private bool isGrounded;
    [SerializeField] private float radius;
    [SerializeField] private float distance;
    [SerializeField] private Vector3 offset;
    [SerializeField] private LayerMask lm;


    #endregion

    #region Unity Functions
    private void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        cam = Camera.main;
    }
    private void Update()
    {        
        Rotation();
        Movement();
    }

    private void LateUpdate()
    {
        cam.transform.position = recoil.position;
        cam.transform.rotation = recoil.rotation;
        
        gunPoint.transform.position = recoil.position;
        gunPoint.transform.rotation = recoil.rotation;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + offset, radius);
        if (Physics.SphereCast(transform.position + offset, radius, Vector3.down, out RaycastHit hit, distance, lm))
        {
            Gizmos.color = Color.green;
            Vector3 endPoint = ((transform.position + offset) + (Vector3.down * distance));
            Gizmos.DrawWireSphere(endPoint, radius);
            Gizmos.DrawLine(transform.position + offset, endPoint);

            Gizmos.DrawSphere(hit.point, 0.1f);
        }
        else
        {
            Gizmos.color = Color.red;
            Vector3 endPoint = ((transform.position + offset) + (Vector3.down * distance));
            Gizmos.DrawWireSphere(endPoint, radius);
            Gizmos.DrawLine(transform.position + offset, endPoint);
        }
    }
    #endregion

    #region Custom Functions
    private void Rotation()
    {
        mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        horizontalRotateStore += mouseInput.x;
        verticalRotateStore -= mouseInput.y;

        verticalRotateStore = Mathf.Clamp(verticalRotateStore, -60f, 60f);

        transform.rotation = Quaternion.Euler(0f,horizontalRotateStore,0f);
        pointOfView.transform.localRotation = Quaternion.Euler(verticalRotateStore, 0f, 0f);
    }

    private void Movement()
    {
        direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));

        float velY = movement.y;
        movement = ((transform.forward * direction.z) + (transform.right * direction.x)).normalized;
        movement.y = velY;


        if (Input.GetButton("Fire3"))
        {
            actualSpeed = runSpeed;
        }
        else
        {
            actualSpeed = walkSpeed;
        }

        if (IsGrounded())
        {
            movement.y = 0;
        }

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            movement.y = jumpForce * Time.deltaTime;
        }
        movement.y += Physics.gravity.y * Time.deltaTime * gravityMod; 
        controller.Move (movement * (actualSpeed * Time.deltaTime));
    }

    private bool IsGrounded()
    {
        isGrounded = false;
        if (Physics.SphereCast(transform.position + offset,radius,Vector3.down, out RaycastHit hit, distance, lm))
        {
            isGrounded = true;
        }

        return isGrounded;
    }
    #endregion
}
