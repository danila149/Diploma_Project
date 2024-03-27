using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public bool PlayerInput { get; set; }
	public bool IsHungry { get; set; }

	[SerializeField] private float speed = 2;
	[SerializeField] private float acceleration = 2.5f;
	[SerializeField] private Transform head;

	public float sensitivity = 5f;
	public float headMinY = -40f;
	public float headMaxY = 40f;

	public KeyCode jumpButton = KeyCode.Space;
	public float jumpForce = 10;

	private bool isGrounded;
	private bool isSprinting;


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
		PlayerInput = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

	void FixedUpdate()
	{
        if (PlayerInput)
        {
            if (Cursor.visible)
            {
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
			}
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
        else
        {
			if (!Cursor.visible)
			{
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
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

			float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivity;
			rotationY += Input.GetAxis("Mouse Y") * sensitivity;
			rotationY = Mathf.Clamp(rotationY, headMinY, headMaxY);
			transform.localEulerAngles = new Vector3(0, rotationX, 0);
			head.localEulerAngles = new Vector3(-rotationY, 0, 0);

			direction = new Vector3(h, 0, v);
			direction = head.TransformDirection(direction);
			direction = new Vector3(direction.x, 0, direction.z);


			if (Input.GetKeyDown(jumpButton) && isGrounded == true)
			{
				body.AddForce(transform.up * jumpForce, ForceMode.Impulse);
			}
		}
        else
        {
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}

        if (Input.GetKeyDown(KeyCode.LeftShift) && isGrounded == true)
        {
			isSprinting = true;
			speed *= acceleration;
		}
		else if (Input.GetKeyUp(KeyCode.LeftShift) && isSprinting)
        {
			isSprinting = false;
			speed /= acceleration;
		}

		if(IsHungry)
        {
			speed = 1f;
		}
	}
}
