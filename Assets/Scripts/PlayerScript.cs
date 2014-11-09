using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

	public Vector2 speed = new Vector2( 10, 10 );

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
	private KeyCode wellKey;

	private string axis;
	private string fireButton;
	private string shieldButton;
	private string wellButton;
	private string wellTrigger;

	void Start()
	{
		axis = "P1_Vertical";
		fireButton = "P1_R_Trigger";
		shieldButton = "P1_R_Shoulder";
		wellTrigger = "P1_L_Trigger";
		wellButton = "P1_X";
		upKey = KeyCode.W;
		downKey = KeyCode.S;
		shootKey = KeyCode.LeftShift;
		shieldKey = KeyCode.LeftControl;
	}

	void Update()
	{
		if( networkView.isMine )
		{
			float inputY = Input.GetAxis ( axis );
			if (Input.GetKey( upKey ) )
				inputY = 1.0f;
			if( Input.GetKey ( downKey ) )
				inputY = -1.0f;
			movement = new Vector2( 0, speed.y * inputY );

			// translation
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

			// well shot
			if( Input.GetButtonDown( wellButton ) )
			{
				WeaponScript weapon = GetComponentInChildren< WeaponScript >();
				if( weapon != null )
					weapon.ShootWell( Input.GetAxis( wellTrigger ) );
			}

			// shield
			if( Input.GetButtonDown( shieldButton ) && currentShield > 0.0f )
			{
				networkView.RPC( "setShieldOn", RPCMode.AllBuffered );
			}

			if( Input.GetButtonUp( shieldButton ) || ( isShielded && currentShield <= 0.0f ) )
			{
				networkView.RPC ( "setShieldOff", RPCMode.AllBuffered );
			}

		}

		if( isShielded )
		{
			currentShield = currentShield - Time.deltaTime;		
			ShieldBarScript sbs = shieldBar.GetComponent< ShieldBarScript >();
			sbs.barDisplay = (currentShield / maxShield) * 100.0f;
		}
	}

	[RPC]
	void setShieldOn()
	{
		isShielded = true;

		SpriteRenderer sr = shield.GetComponent< SpriteRenderer >();
		sr.enabled = true;
		CircleCollider2D cc = shield.GetComponent< CircleCollider2D >();
		cc.enabled = true;
	}

	[RPC]
	void setShieldOff()
	{
		isShielded = false;

		SpriteRenderer sr = shield.GetComponent< SpriteRenderer >();
		sr.enabled = false;
		CircleCollider2D cc = shield.GetComponent< CircleCollider2D >();
		cc.enabled = false;
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
