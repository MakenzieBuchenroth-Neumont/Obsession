using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
	[SerializeField] private Animator animator;
	[SerializeField] private Rigidbody rb;
	public float walkingSpeed, walkBackwardsSpeed, olwSpeed, runSpeed, roSpeed;
	public bool walking;
	public Transform playerTrans;

	public static bool isCrawling = false;

	private void FixedUpdate() {
		if (Input.GetKey(KeyCode.W)) {
			rb.velocity = transform.forward * walkingSpeed * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.S)) {
			rb.velocity = -transform.forward * walkBackwardsSpeed * Time.deltaTime;
		}

	}

	// Update is called once per frame
	void Update() {
		if (Input.GetKeyDown(KeyCode.W)) {
			animator.SetTrigger("walk");
			animator.ResetTrigger("idle");
			walking = true;
		}
		if (Input.GetKeyUp(KeyCode.W)) {
			animator.ResetTrigger("walk");
			animator.SetTrigger("idle");
			rb.velocity = Vector3.zero;
			walking = false;
		}
		if (Input.GetKeyDown(KeyCode.S)) {
			animator.SetTrigger("walkBack");
			animator.ResetTrigger("idle");
		}
		if (Input.GetKeyUp(KeyCode.S)) {
			animator.ResetTrigger("walkBack");
			animator.SetTrigger("idle");
			rb.velocity = Vector3.zero;
		}
		if (Input.GetKey(KeyCode.A)) {
			playerTrans.Rotate(0, -roSpeed * Time.deltaTime, 0);
		}
		if (Input.GetKey(KeyCode.D)) {
			playerTrans.Rotate(0, roSpeed * Time.deltaTime, 0);
		}
		if (walking == true && isCrawling == false) {
			if (Input.GetKeyDown(KeyCode.LeftShift)) {
				walkingSpeed = walkingSpeed + runSpeed;
				animator.SetTrigger("run");
				animator.ResetTrigger("walk");
			}
			if (Input.GetKeyUp(KeyCode.LeftShift)) {
				walkingSpeed = olwSpeed;
				animator.ResetTrigger("run");
				animator.SetTrigger("walk");
			}
		}

		if (walking == false && Input.GetKeyUp(KeyCode.LeftControl)) {
			if (isCrawling == false) {
				animator.SetBool("crawl", true);
				isCrawling = true;
			}
			else if (isCrawling == true) {
				animator.SetBool("crawl", false);
				isCrawling = false;
			}
		}
	}
}
