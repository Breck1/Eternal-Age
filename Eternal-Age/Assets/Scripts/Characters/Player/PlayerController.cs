using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    // Member Vars
    [Header("Health")]
    [Separator("Stats")]
    public int maxHealth;
    public int currentHealth;

    [Header("Stamina")]
    public int maxStamina = 5;
    public int currentStamina;
    public float dashTime;

    [Header("Movement")]
    [Separator("Properties")]
    public float movementSpeed = 1.0f;
    public bool enableSprint = true;
    [ConditionalField(nameof(enableSprint))] public float sprintSpeedMultiplier = 1.5f;

	[Header("Spear")]
	public GameObject SpearUpDown;
	public GameObject SpearLeftRight;


	CharacterController controller;
    Animator animator;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        currentStamina = maxStamina;
    }
    private void Update()
    {
        UpdateMovement();
        UpdateAnimations();

    }
    //void FixedUpdate()
    //{
    //    UpdateMovement(); 
    //    UpdateAnimations();
    //}

    void UpdateMovement()
    {
        float effectiveMovementSpeed = movementSpeed;
        Vector3 movement = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);
        if(movement.x != 0 && movement.y != 0)
            effectiveMovementSpeed = movementSpeed / 1.4f;
        //if (Input.GetButtonDown("Dash") && currentStamina > 0)
        //{
        //    Dash(movement);
        //}

        if(enableSprint && Input.GetAxisRaw("Sprint") != 0)
            effectiveMovementSpeed *= (Input.GetAxisRaw("Sprint") * sprintSpeedMultiplier);

        controller.Move(movement * Time.deltaTime * effectiveMovementSpeed);
		Attacking(movement);
	}

    void UpdateAnimations()
    {
        animator.SetFloat("MoveX", Input.GetAxisRaw("Horizontal"));
        animator.SetFloat("MoveY", Input.GetAxisRaw("Vertical"));
        if(enableSprint && Input.GetAxisRaw("Sprint") != 0)
            animator.speed = 1.5f;
        else
            animator.speed = 1;
    }
    int Dash(Vector3 dashDirection)
    {
        Vector3 dashDistance = transform.position;
        //float effectiveMovementSpeed = 10.0f;
        currentStamina--;
        Debug.Log("DashPressed " +dashDirection);
        for (int i = 0; i < dashTime; i++)
        {
            controller.Move(dashDirection * Time.deltaTime * 1);
        }
		currentStamina = maxStamina;
        
        return currentStamina;
    }
	void Attacking(Vector3 movement)
	{
		if (movement.y != 0)
		{
			SpearLeftRight.SetActive(false);
			SpearUpDown.SetActive(true);
			Debug.Log(movement);
		}
		if(movement.x != 0)
		{
			SpearLeftRight.SetActive(true);
			SpearUpDown.SetActive(false);
		}
	}
}
