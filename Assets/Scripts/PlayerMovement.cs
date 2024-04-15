using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody playerRb;
    public Transform cam;
    private bool isJumping;
    public bool isPoweredUp;
    public float powerBounceStrength;
    public float powerupTime = 7f;

    [SerializeField] private float jumpForce;
    [SerializeField] private float speed;
    [SerializeField] private float gravity;
    [SerializeField] private float jumpHeight;
    Vector3 velovity;

    //PlayerInput playerInput;
    //InputAction moveAction;
    //InputAction jumpAction;

    public float sizeInterval;
    public float minSize = 1f;
    public float maxSize = 10f;
    public float currentSize;

    private float raycastLength = 0.6f;

    bool isGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, 0.65f);
    }

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody>();

        //playerInput = GetComponent<PlayerInput>();

        //playerInputActions = new PlayerInputActions();
       // playerInputActions.Movement.Enable();
        //playerInputActions.Movement.Jump.performed += Jump;
    }

    private void Start()
    {
        currentSize = 1f;
        transform.localScale = new Vector3(minSize, minSize, minSize);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        void Grow()
        {

            if (currentSize < maxSize)
            {
                currentSize++;
                raycastLength = raycastLength + 0.6f;
                transform.localScale = new Vector3(currentSize, currentSize, currentSize);
            }
        }

        void Shrink()
        {

            if (currentSize > minSize)
            {
                currentSize--;
                raycastLength = raycastLength - 0.6f;
                transform.localScale = new Vector3(currentSize, currentSize, currentSize);
            }
            //Both of these had an Issue with the word public???
        }
    }

    private void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal") * speed;
        float moveVertical = Input.GetAxis("Vertical") * speed;

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        if(movement.magnitude > 0.1)
        {
            float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg + cam.eulerAngles.y;

            Vector3 moveDir = Quaternion.Euler(0f,targetAngle,0f) * Vector3.forward;
            playerRb.AddForce(moveDir * speed * Time.deltaTime);
        }

        //playerRb.AddForce(movement * speed * Time.deltaTime);
    }

    private void Jump()
    {
        if(isGrounded() == true)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {
            isPoweredUp = true;
            Destroy(other.gameObject);
            StartCoroutine(PowerupCountDownRouutine());
        }

        if (other.CompareTag("Orb"))
        {
            Destroy(other.gameObject);
            //Grow();
        }

    }

    IEnumerator PowerupCountDownRouutine()
    {
        yield return new WaitForSeconds(powerupTime);
        isPoweredUp = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy") && isPoweredUp == true)
        {
            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();

            Vector3 bounceDir = (collision.gameObject.transform.position - transform.position);
            enemyRb.AddForce(bounceDir * powerBounceStrength, ForceMode.Impulse);
        }
    }
}
