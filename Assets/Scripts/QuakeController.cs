using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class QuakeController : MonoBehaviour{
	public CharacterController Controller;
	public PlayerInput Input;

	public Vector2 Look{get;set;}
	public Vector2 Move{get;set;}
	public bool Fire{get;set;}
	public bool Jump{get;set;}

	public bool BunnyHop = true;

	public float vSpeed;
	public int Jumps;

	public float gravity = 9.8f;
	public float jumpSpeed = 5;
	public float speed = 15;      // units per second
	public float turnSpeed = 90; // degrees per second
	public int maxJumps = 2;

	// Start is called before the first frame update
	void Start(){
		Controller = Controller ? Controller : GetComponent<CharacterController>();
		Input = Input ? Input : GetComponent<PlayerInput>();
		Jumps = maxJumps;
	}

	// Update is called once per frame
	void Update(){}

	private void FixedUpdate(){
		Transform tf = transform;
		tf.Rotate(0,
				  Look.x * turnSpeed * Time.deltaTime,
				  0);
		Vector3 vel = tf.forward * (Move.y * speed);
		vel += tf.right * (Move.x * speed);
		if(Controller.isGrounded){
			vSpeed = 0; // grounded character has vSpeed = 0...
			Jumps = maxJumps;
		}
		if(Jumps > 0 && Jump){
			vSpeed = jumpSpeed;
			Jumps--;
		}
		else{
			// apply gravity acceleration to vertical speed:
			vSpeed -= gravity * Time.deltaTime;
		}
		Jump = false;
		vel.y = vSpeed; // include vertical speed in vel
		// convert vel to displacement and Move the character:
		Controller.Move(vel * Time.deltaTime);
	}

	public void OnMove(InputValue value){
		Move = value.Get<Vector2>();
	}

	public void OnLook(InputValue value){
		Look = value.Get<Vector2>();
	}

	public void OnFire(InputValue value){
		Fire = value.Get<bool>();
	}

	public void OnJump(InputValue value){
		if(value.isPressed){
			Jump = true;
		}
	}
}
