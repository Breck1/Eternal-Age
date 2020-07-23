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
	float moveVertical;
	float moveHorizontal;
	[ConditionalField(nameof(enableSprint))] public float sprintSpeedMultiplier = 1.5f;

	[Header("Spear")]
	public GameObject SpearUpDown;
	public GameObject SpearLeftRight;
	SpriteRenderer SpearUpDownSprite;
	SpriteRenderer SpearLeftRightSprite;
	public bool tempV;
	public bool tempH;
	public float spearPosition;


	CharacterController controller;
    Animator animator;

	[SerializeField]
	private string Fire1 = "Fire1";

	private string horizontal = "Horizontal";
	private string vertical = "Vertical";
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        currentStamina = maxStamina;
		SpearUpDownSprite = SpearUpDown.GetComponent<SpriteRenderer>();
		SpearLeftRightSprite = SpearLeftRight.GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        UpdateMovement();
		Attacking();
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
		moveHorizontal = Input.GetAxisRaw(horizontal);
		moveVertical = Input.GetAxisRaw(vertical);
		Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0);
        if(movement.x != 0 && movement.y != 0)
            effectiveMovementSpeed = movementSpeed / 1.4f;

		SpearUpDown.SetActive(tempV);
		SpearLeftRight.SetActive(tempH);


		if (enableSprint && Input.GetAxisRaw("Sprint") != 0)
            effectiveMovementSpeed *= (Input.GetAxisRaw("Sprint") * sprintSpeedMultiplier);

        controller.Move(movement * Time.deltaTime * effectiveMovementSpeed);


	}

	void UpdateAnimations()
    {
		
        animator.SetFloat("MoveX", Input.GetAxisRaw(horizontal));
		animator.SetFloat("MoveY", Input.GetAxisRaw(vertical));
		if (enableSprint && Input.GetAxisRaw("Sprint") != 0)
            animator.speed = 1.5f;
        else
            animator.speed = 1;
    }
  //  int Dash(Vector3 dashDirection)
  //  {
  //      Vector3 dashDistance = transform.position;
  //      //float effectiveMovementSpeed = 10.0f;
  //      currentStamina--;
  //      Debug.Log("DashPressed " +dashDirection);
  //      for (int i = 0; i < dashTime; i++)
  //      {
  //          controller.Move(dashDirection * Time.deltaTime * 1);
  //      }
		//currentStamina = maxStamina;
        
  //      return currentStamina;
  //  }
	void Attacking()
	{

		float x = Mathf.Abs(moveHorizontal);
		float y = Mathf.Abs(moveVertical);

		
		if (y > x)
		{
			tempH = false;
			tempV = true;

			if (moveVertical > 0)
			{
				SpearUpDownSprite.flipY = true;
				Debug.Log("flipY");
	
				if (SpearUpDown.transform.localPosition.x != -spearPosition)
				{
					SpearUpDown.transform.localPosition = new Vector3(-spearPosition, SpearUpDown.transform.localPosition.y);
				}
				
			}
			
			if (moveVertical < 0)
			{
				SpearUpDownSprite.flipY = false;
				if (SpearUpDown.transform.localPosition.x != spearPosition)
				{
					SpearUpDown.transform.localPosition = new Vector3(spearPosition, SpearUpDown.transform.localPosition.y);
				}
			}

		}
		if (x > y)
		{
			tempV = false;
			tempH = true;
			if (moveHorizontal > 0)
			{
				SpearLeftRightSprite.flipX = true;
				SpearLeftRightSprite.sortingOrder = 2;

			}
			if(moveHorizontal < 0)
			{
				SpearLeftRightSprite.flipX = false;
				SpearLeftRightSprite.sortingOrder = 0;


			}
		}
		if (Input.GetButtonDown(Fire1))
		{
			//float temp;
			//if (SpearLeftRight.activeInHierarchy == true)
			//{
			//	if (moveHorizontal > 0)
			//	{
			//		SpearLeftRight.transform.localPosition = new Vector3(transform.localPosition.x, 
			//			SpearLeftRightSprite.transform.localPosition.y * 2 * Time.deltaTime);
			//	}
			//	Debug.Log("Left and right spear: ");
			//}
			if (SpearUpDown.activeInHierarchy == true)
			{
				Debug.Log("Up and down spear: ");
			}
			Debug.Log("You hit me");
		}
	}
}
