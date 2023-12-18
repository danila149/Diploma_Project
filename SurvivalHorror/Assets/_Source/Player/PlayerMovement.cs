using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public static PlayerMovement Instance { get; private set; }

	public bool PlayerInput { get; set; }

	public float speed = 1.5f;

	public Transform head;

	public float sensitivity = 5f;
	public float headMinY = -40f;
	public float headMaxY = 40f;

	public KeyCode jumpButton = KeyCode.Space;
	public float jumpForce = 10;

	private bool isGrounded;


	private Vector3 direction;
	private float h, v;
	private int layerMask;
	private Rigidbody body;
	private float rotationY;

    private void Awake()
    {
		Instance = this;
	}


    void Start()
	{
		body = GetComponent<Rigidbody>();
		body.freezeRotation = true;
		layerMask = 1 << gameObject.layer | 1 << 2;
		layerMask = ~layerMask;
		PlayerInput = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

	void FixedUpdate()
	{
        if (PlayerInput)
        {
			body.AddForce(direction * speed, ForceMode.VelocityChange);

			if (Mathf.Abs(body.velocity.x) > speed)
			{
				body.velocity = new Vector3(Mathf.Sign(body.velocity.x) * speed, body.velocity.y, body.velocity.z);
			}
			if (Mathf.Abs(body.velocity.z) > speed)
			{
				body.velocity = new Vector3(body.velocity.x, body.velocity.y, Mathf.Sign(body.velocity.z) * speed);
			}
		}
		
		isGrounded = false;
	}

    private void OnTriggerStay(Collider collision)
	{
		if (collision.tag == "Ground")
		{
			isGrounded = true;
		}
	}

	void Update()
	{
        if (PlayerInput)
        {
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
			h = Input.GetAxis("Horizontal");
			v = Input.GetAxis("Vertical");

			float rotationX = head.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivity;
			rotationY += Input.GetAxis("Mouse Y") * sensitivity;
			rotationY = Mathf.Clamp(rotationY, headMinY, headMaxY);
			head.localEulerAngles = new Vector3(-rotationY, rotationX, 0);

			direction = new Vector3(h, 0, v);
			direction = head.TransformDirection(direction);
			direction = new Vector3(direction.x, 0, direction.z);


			if (Input.GetKeyDown(jumpButton) && isGrounded == true)
			{
				body.velocity = new Vector2(0, jumpForce);
			}
        }
        else
        {
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
	}
}
