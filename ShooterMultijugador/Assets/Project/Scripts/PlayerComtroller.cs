using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComtroller : MonoBehaviour
{
    #region Variables
    
    
    [SerializeField] private Camera cam;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Transform pointOfView;
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 8f;
    [SerializeField] private float jumpForce = 12;
    [SerializeField] private float Gravitymod = 2.5f;
   

    private float actualSpeed;
    private float horizoltalRotationStore;
    private float verticalRotationsStore;

    private Vector2 mouseInput;
    private Vector3 direcion;
    private Vector3 movement;
    
    [Header("Ground Detection")]
    [SerializeField] private bool isGround;
    [SerializeField] private float radio;
    [SerializeField] private float distance;
    [SerializeField] private Vector3 offset;
    [SerializeField] private LayerMask lm;

    #endregion

    #region Unity Funciones 

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
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
        cam.transform.position = pointOfView.position;
        cam.transform.rotation = pointOfView.rotation;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + offset, radio);
        if (Physics.SphereCast(transform.position + offset, radio, Vector3.down, out RaycastHit hit, distance, lm))
        {
            Gizmos.color = Color.red;
            Vector3 endPoint = (transform.position + offset) + (Vector3.down * distance);
            Gizmos.DrawWireSphere(endPoint, radio);
            Gizmos.DrawLine(transform.position + offset, endPoint);

            Gizmos.DrawSphere(hit.point, 0.1f);
        }
        else
        {
            Gizmos.color = Color.red;
            Vector3 endPoint = (transform.position + offset) + (Vector3.down * distance);
            Gizmos.DrawWireSphere(endPoint, radio);
            Gizmos.DrawLine(transform.position + offset, endPoint);
        }
    }
    #endregion

    #region Custom Functions
    private void Rotation()
    {
        mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        horizoltalRotationStore += mouseInput.x;
        verticalRotationsStore += mouseInput.y;

        verticalRotationsStore = Mathf.Clamp(verticalRotationsStore, -60, 60);

        transform.rotation = Quaternion.Euler(0f, horizoltalRotationStore, 0f);
        pointOfView.transform.localRotation= Quaternion.Euler(verticalRotationsStore, 0f, 0f);   
    }

    private void Movement()
    {
        direcion = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        float velY = movement.y;
        movement = ((transform.forward * direcion.z) + (transform.right * direcion.x)).normalized;
        movement.y = velY;
   

        if (Input.GetButton("Fire3"))
        {
            actualSpeed = runSpeed;
        }
        else
        {
            actualSpeed = walkSpeed;
        }

        if(characterController.isGrounded)
        {
            movement.y = 0;
        }
        if (Input.GetButtonDown("Jump") && characterController.isGrounded) 
        {
            movement.y = jumpForce;
        }

        movement.y += Physics.gravity.y * Time.deltaTime * Gravitymod;
        characterController.Move(movement * (actualSpeed * Time.deltaTime));
    }

    public bool IsGrounded()
    {
        if (Physics.SphereCast(transform.position + offset, radio, Vector3.down, out RaycastHit hit, distance, lm ))
        {

        }
        return true;

    }
   
    #endregion
}
