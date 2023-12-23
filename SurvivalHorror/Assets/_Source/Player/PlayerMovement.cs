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

	[SerializeField] private int hp = 100;
	[SerializeField] private int Eat = 100;
	private bool chekStartCotutine = false;

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

    private void OnCollisionEnter(Collision collision)
    {
		if (collision.gameObject.tag == "Enemy")
		{
			hp--;
			Debug.Log(hp);
		}
	}

	IEnumerator Hunger()
    {
		Eat--;
		chekStartCotutine = true;
		yield return new WaitForSeconds(1f);
		chekStartCotutine = false;
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

        if (Input.GetKeyDown(KeyCode.LeftShift) && isGrounded == true)
        {
			speed = 5f;
		}
		else if (Input.GetKeyUp(KeyCode.LeftShift) && isGrounded == true)
        {
			speed = 2f;
        }

		if(hp == 0)
        {
			Destroy(gameObject);
        }


		if(chekStartCotutine == false && Eat > 0)
        {
			StartCoroutine(Hunger());
        }

		if(Eat <= 0)
        {
			speed = 0.5f;
		}
	}
}
