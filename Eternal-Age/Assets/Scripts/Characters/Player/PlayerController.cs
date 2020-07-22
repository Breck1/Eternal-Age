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
    public int maxStamina;
    public int currentStamina;

    [Header("Movement")]
    [Separator("Properties")]
    public float movementSpeed = 1.0f;
    public bool enableSprint = true;
    [ConditionalField(nameof(enableSprint))] public float sprintSpeedMultiplier = 1.5f;

    CharacterController controller;
    Animator animator;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        UpdateMovement(); 
        UpdateAnimations();
    }

    void UpdateMovement()
    {
        float effectiveMovementSpeed = movementSpeed;
        Vector3 movement = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);
        if(movement.x != 0 && movement.y != 0)
            effectiveMovementSpeed = movementSpeed / 1.4f;

        if(enableSprint && Input.GetAxisRaw("Sprint") != 0)
            effectiveMovementSpeed *= (Input.GetAxisRaw("Sprint") * sprintSpeedMultiplier);

        controller.Move(movement * Time.deltaTime * effectiveMovementSpeed);    
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
}
