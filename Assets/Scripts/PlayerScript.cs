using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

	public Vector2 speed = new Vector2( 10, 10 );
	public string axis = "R_Vertical";
	public string fireButton = "Fire1";
	public string shieldButton = "R_Shoulder";
	public int topBorder = 4;
	public int bottomBorder = -4;

	public Transform shield;
	public Transform shieldBar;

	public bool player1;

	private Vector2 movement;
	private bool isShielded = false;
	private float maxShield = 5.0f;
	private float currentShield = 5.0f;

	private KeyCode upKey;
	private KeyCode downKey;
	private KeyCode shootKey;
	private KeyCode shieldKey;



	void Start()
	{
		if( player1 )
		{
			upKey = KeyCode.W;
			downKey = KeyCode.S;
			shootKey = KeyCode.LeftShift;
			shieldKey = KeyCode.LeftControl;
		} else {
			upKey = KeyCode.UpArrow;
			downKey = KeyCode.DownArrow;
			shootKey = KeyCode.RightShift;
			shieldKey = KeyCode.RightControl;
		}
	}

	void Update()
	{
		float inputY = Input.GetAxis ( axis );
		if (Input.GetKey( upKey ) )
			inputY = 1.0f;
		if( Input.GetKey ( downKey ) )
			inputY = -1.0f;
		movement = new Vector2( 0, speed.y * inputY );

		if( transform.position.y > topBorder )
			transform.position = new Vector3( transform.position.x, topBorder, transform.position.z );
		if( transform.position.y < bottomBorder )
			transform.position = new Vector3( transform.position.x, bottomBorder, transform.position.z );

		// shooting
		float shoot = Input.GetAxis ( fireButton );
		if( Input.GetKey( shootKey ) )
		   shoot = 1.0f;

		if ( shoot > 0.0f )
		{
			WeaponScript weapon = GetComponentInChildren< WeaponScript >();
			if( weapon != null )
				weapon.Attack();
		}

		// shield
		bool shieldBool = Input.GetButton( shieldButton );
		if( Input.GetKey( shieldKey ) )
		   shieldBool = true;

		if( shieldBool )
		{
			currentShield = currentShield - Time.deltaTime;
		}

		if( shield && currentShield > 0.0f )
		{
			SpriteRenderer sr = shield.GetComponent< SpriteRenderer >();
			sr.enabled = shieldBool;
			CircleCollider2D cc = shield.GetComponent< CircleCollider2D >();
			cc.enabled = shieldBool;
			isShielded = shieldBool;

			ShieldBarScript sbs = shieldBar.GetComponent< ShieldBarScript >();
			sbs.barDisplay = (currentShield / maxShield) * 100.0f;

		} else {
			SpriteRenderer sr = shield.GetComponent< SpriteRenderer >();
			sr.enabled = false;
			CircleCollider2D cc = shield.GetComponent< CircleCollider2D >();
			cc.enabled = false;
			isShielded = false;
		}

	}

	void FixedUpdate()
	{
		rigidbody2D.AddForce( movement );

	}

	public bool hasShieldUp
	{
		get { return isShielded; }
	}

	public void ResetShield()
	{
		currentShield = maxShield;
	}
}
