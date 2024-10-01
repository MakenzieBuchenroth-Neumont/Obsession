using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour {
	CharacterController controller;

	Vector2 movement;
	public float moveSpeed = 2.0f;

	public Transform cam;
	private float turnSmoothTime = 0.1f;
	private float turnSmoothVelocity;

	[SerializeField] Animator animator;

	bool isCrawling = false;
	bool isRunning = false;

	private void Start() {
		controller = GetComponent<CharacterController>();
	}

	private void Update() {
		movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		Vector3 direction = new Vector3(movement.x, 0, movement.y).normalized;
		Vector3 moveDir = Vector3.zero;
		if (direction.magnitude >= 0.1f) {
			float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
			float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
			transform.rotation = Quaternion.Euler(0f, angle, 0f);

			 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
			controller.Move(moveDir.normalized * moveSpeed * Time.deltaTime);

			animator.SetFloat("Vertical", Input.GetAxis("Vertical"));
			animator.SetFloat("Horizontal", Input.GetAxis("Horizontal"));

		}
			animator.SetFloat("Vertical", moveDir.magnitude * 0.5f);

		if (Input.GetKeyDown(KeyCode.C) && !isCrawling) {
			isCrawling = true;
			animator.SetBool("isCrawling", true);
		}

		if (Input.GetKeyDown(KeyCode.C) && isCrawling) {
			isCrawling = false;
			animator.SetBool("isCrawling", false);
		}

		if (Input.GetKey(KeyCode.LeftShift) && !isCrawling) {
			//moveDir.magnitude * 2;
			moveSpeed *= 2;
			animator.SetFloat("Speed", 1);
			isRunning = true;
		}

		//Debugging purposes only
		//Skip the time when the right square bracket is pressed
		if (Input.GetKey(KeyCode.RightBracket)) {
				TimeManager.Instance.Tick();
			}
	}
}
