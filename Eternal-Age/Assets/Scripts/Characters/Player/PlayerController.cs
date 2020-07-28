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
	[ConditionalField(nameof(enableSprint))]
	public float sprintSpeedMultiplier = 1.5f;

	[Header("Spear")]
	[Separator("Spear")]
	public GameObject spearUpDownGameObject;
	public GameObject spearLeftRightGameObject;
	SpriteRenderer spearUpDownSprite;
	SpriteRenderer spearLeftRightSprite;
	public bool tempV;
	public bool tempH;

	[Header("Positions")]
	[Separator("Spear Positions")]
	public float spearPositionUpX;
	public float spearPositionUpY;
	public float spearPositionDownX;
	public float spearPositionDownY;
	public float spearPositionLeftX;
	public float spearPositionLeftY;
	public float spearPositionRightX;
	public float spearPositionRightY;

	[Header("Shield")]
	[Separator("Shield")]	
	public GameObject shieldUpDownGameObject;									 
	public GameObject shieldLeftRightGameObject;								 
	private SpriteRenderer shieldUpDownSprite;									 
	private SpriteRenderer shieldLeftRightSprite;
	public float shieldBashTimer = 1;
	[Header("Positions")]
	[Separator("Shield Positions")]
	public float shieldPositionUpX;
	public float shieldPositionUpY;
	public float shieldPositionDownX;
	public float shieldPositionDownY;
	public float shieldPositionLeftX;
	public float shieldPositionLeftY;											 
	public float shieldPositionRightX;																			 
	public float shieldPositionRightY;
	public bool shieldBash = false;
	public float variableXMove;
	public float variableYMove;
	Animator animator;
	CharacterController controller;
	private string Fire1 = "Fire1";
	float tempXvelocity, tempYvelocity;
	private string horizontal = "Horizontal";
	private string vertical = "Vertical";
	RaycastHit raycast;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        currentStamina = maxStamina;
		controller.detectCollisions = true;
		spearUpDownSprite = spearUpDownGameObject.GetComponent<SpriteRenderer>();
		spearLeftRightSprite = spearLeftRightGameObject.GetComponent<SpriteRenderer>();
		shieldUpDownSprite = shieldUpDownGameObject.GetComponent<SpriteRenderer>();
		shieldLeftRightSprite = shieldLeftRightGameObject.GetComponent<SpriteRenderer>();
		gameObject.tag = "Player";
    }

	private void Update()
    {
        UpdateMovement();
		Attacking();
		UpdateAnimations();
		ShieldMove();
	}
    void UpdateMovement()
    {
        float effectiveMovementSpeed = movementSpeed;
		moveHorizontal = Input.GetAxisRaw(horizontal);
		moveVertical = Input.GetAxisRaw(vertical);
		Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0);
        if(movement.x != 0 && movement.y != 0)
			effectiveMovementSpeed = movementSpeed / 1.4f;

		spearUpDownGameObject.SetActive(tempV);
		spearLeftRightGameObject.SetActive(tempH);
		shieldUpDownGameObject.SetActive(tempV);
		shieldLeftRightGameObject.SetActive(tempH);

		if (enableSprint && Input.GetAxisRaw("Sprint") != 0)
			effectiveMovementSpeed *= (Input.GetAxisRaw("Sprint") * sprintSpeedMultiplier);

        controller.Move(movement * Time.deltaTime * effectiveMovementSpeed);
		if (Input.GetButtonDown("Dash"))
		{
			Dash(movement);
		}
		if (Input.GetButtonDown("ShieldBash"))
		{
			shieldBash = true;
			StartCoroutine(ShieldBashDuration(movement));
		}
		if (shieldBash)
		{
			ShieldBash(movement);
		}
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
	int Dash(Vector3 dashDirection)
	{
		Vector3 dashDistance = transform.position;
		float effectiveMovementSpeed = 10.0f;
		currentStamina--;
		for (int i = 0; i < dashTime; i++)
		{
			controller.Move(dashDirection * Time.deltaTime * 1);
		}
		currentStamina = maxStamina;

		return currentStamina;
	}
	void Attacking()
	{

		float x = Mathf.Abs(moveHorizontal);
		float y = Mathf.Abs(moveVertical);


		if (y > x)
		{
			tempV = true;
			tempH = false;
			if (moveVertical > 0)
			{
				spearUpDownSprite.flipY = true;
				spearUpDownSprite.sortingOrder = 0;

				if (spearUpDownGameObject.transform.localPosition.x != spearPositionUpX ||
					spearUpDownGameObject.transform.localPosition.y != spearPositionUpY)
				{
					spearUpDownGameObject.transform.localPosition = new Vector3(spearPositionUpX, spearPositionUpY);
				}
			}

			if (moveVertical < 0)
			{
				spearUpDownSprite.flipY = false;
				spearUpDownSprite.sortingOrder = 2;
				if (spearUpDownGameObject.transform.localPosition.x != spearPositionDownX || 
					spearUpDownGameObject.transform.localPosition.y != spearPositionLeftY)
				{
					spearUpDownGameObject.transform.localPosition = new Vector3(spearPositionDownX, spearPositionDownY);
				}
			}

		}
		if (x > y)
		{
			tempV = false;
			tempH = true;
			if (moveHorizontal > 0)
			{
				spearLeftRightSprite.flipX = true;
				spearLeftRightSprite.sortingOrder = 3;
				if (spearLeftRightGameObject.transform.localPosition.x != spearPositionRightX ||
					spearLeftRightGameObject.transform.localPosition.y != spearPositionRightY)
				{
					spearLeftRightGameObject.transform.localPosition = new Vector3(spearPositionRightX, spearPositionRightY);
				}
			}
			if(moveHorizontal < 0)
			{
				spearLeftRightSprite.flipX = false;
				spearLeftRightSprite.sortingOrder = 0;

				if (spearLeftRightGameObject.transform.localPosition.x != spearPositionLeftX ||
					spearLeftRightGameObject.transform.localPosition.y != spearPositionLeftY)
				{
					spearLeftRightGameObject.transform.localPosition = new Vector3(spearPositionLeftX, spearPositionLeftY);
				}
			}
		}
		if (Input.GetButtonDown(Fire1))
		{
			if (spearUpDownGameObject.activeInHierarchy == true)
			{
			}
		}
	}
	void ShieldMove()
	{
		float x = Mathf.Abs(moveHorizontal);
		float y = Mathf.Abs(moveVertical);


		if (y > x)
		{

			if (moveVertical < 0)
			{
				shieldUpDownSprite.sortingOrder = 2;
				if ((shieldUpDownGameObject.transform.localPosition.x != -shieldPositionDownX && !shieldBash)||
					(shieldUpDownGameObject.transform.localPosition.y != shieldPositionUpY && !shieldBash))
				{
					shieldUpDownGameObject.transform.localPosition = new Vector3(shieldPositionDownX, shieldPositionDownY);
				}
			}

			if (moveVertical > 0)
			{
				shieldUpDownSprite.sortingOrder = 0;
				if ((shieldUpDownGameObject.transform.localPosition.x != shieldPositionUpX && !shieldBash) ||
					(shieldUpDownGameObject.transform.localPosition.y != shieldPositionUpY && !shieldBash))
				{
					shieldUpDownGameObject.transform.localPosition = new Vector3(shieldPositionUpX, shieldPositionUpY);
				}
			}

		}
		if (x > y)
		{
			if (moveHorizontal > 0)
			{
				shieldLeftRightSprite.sortingOrder = 2;
				if (shieldLeftRightGameObject.transform.localPosition.x != shieldPositionRightX ||
					shieldLeftRightGameObject.transform.localPosition.y != shieldPositionRightY)
				{
					shieldLeftRightGameObject.transform.localPosition = new Vector3(shieldPositionRightX, shieldPositionRightY);
				}
			}
			if (moveHorizontal < 0)
			{
				shieldLeftRightSprite.flipX = false;
				shieldLeftRightSprite.sortingOrder = 0;
				if (shieldLeftRightGameObject.transform.localPosition.x != shieldPositionLeftX || 
					shieldLeftRightGameObject.transform.localPosition.y != shieldPositionLeftY)
				{
					shieldLeftRightGameObject.transform.localPosition = new Vector3(shieldPositionLeftX, shieldPositionLeftY);
				}

			}
		}
	}
	void ShieldBash(Vector3 movement)
	{
		//float speed = 1;
		float amount = 1.005f;
		float time = 0.0f;
		if (shieldLeftRightGameObject.activeInHierarchy)
		{
				shieldLeftRightGameObject.transform.localPosition = new Vector3(shieldLeftRightGameObject.transform.localPosition.x * amount,
					shieldLeftRightGameObject.transform.localPosition.y * amount);
				time += Time.deltaTime;
		}
		if (shieldUpDownGameObject.activeInHierarchy)
		{
				shieldUpDownGameObject.transform.localPosition = new Vector3(shieldUpDownGameObject.transform.localPosition.x * amount,
				shieldUpDownGameObject.transform.localPosition.y * amount);
				time += Time.deltaTime;
		}
	}
	IEnumerator ShieldBashDuration(Vector3 movement)
	{
		yield return new WaitForSeconds(shieldBashTimer);
		{
			
			if (movement.x > 0)
			{
				shieldLeftRightGameObject.transform.localPosition = new Vector3(shieldPositionRightX, shieldPositionRightY);
			}
			if (movement.x < 0)
			{
				shieldLeftRightGameObject.transform.localPosition = new Vector3(shieldPositionLeftX, shieldPositionLeftY);
			}
			if (movement.y < 0)
			{
				shieldUpDownGameObject.transform.localPosition = new Vector3(shieldPositionDownX, shieldPositionDownY);

			}
			if (movement.y > 0)
			{
				shieldUpDownGameObject.transform.localPosition = new Vector3(shieldPositionUpX, shieldPositionUpY);
			}
			shieldBash = false;
		}
	}
	private void OnTriggerEnter(Collider collision)
	{
		if (collision.gameObject.tag == "Spikes")
		{
			currentHealth--;
		}
	}
	private void OnControllerColliderHit(ControllerColliderHit hit)
	{

		if (transform.position.x < hit.transform.position.x)
		{

		}

		variableYMove *= -1;
		Rigidbody body = hit.collider.attachedRigidbody;

		Debug.Log("Hit");
	}
	
}
