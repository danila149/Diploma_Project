using UnityEngine;
using System.Collections;
using Unity.Netcode;

[RequireComponent(typeof(Rigidbody))]

public class MovePerson : NetworkBehaviour
{

	public float speed = 1.5f;

	public Transform head;

	public float sensitivity = 5f; // чувствительность мыши
	public float headMinY = -40f; // ограничение угла для головы
	public float headMaxY = 40f;

	public KeyCode jumpButton = KeyCode.Space; // клавиша для прыжка
	public float jumpForce = 10; // сила прыжка

	private bool isGrounded;


	private Vector3 direction;
	private float h, v;
	private int layerMask;
	private Rigidbody body;
	private float rotationY;


	void Start()
	{
		body = GetComponent<Rigidbody>();
		body.freezeRotation = true;
		layerMask = 1 << gameObject.layer | 1 << 2;
		layerMask = ~layerMask;
	}

    public override void OnNetworkSpawn()
    {


		
		base.OnNetworkSpawn();
    }

    void FixedUpdate()
	{
		if (!IsOwner)
			return;
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

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Ground")
		{
			isGrounded = true;
		}
	}
	private void OnCollisionExit(Collision collision)
	{
		if (collision.gameObject.tag == "Ground")
		{
			isGrounded = false;
		}
	}

	void Update()
	{
		if (IsOwner)
        {
			if(head == null)
            {
				head = Camera.main.gameObject.transform;
			}

			head.position = transform.position;
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
	}

}