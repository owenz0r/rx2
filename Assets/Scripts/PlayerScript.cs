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
	private float wellForce = 0.0f;
	private float wellForceDelta = 0.0f;

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

	public enum ControlStyle { KeyboardMouse, Controller };
	public ControlStyle currentControlStyle;

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

	public void setControls( ControlStyle style )
	{
		if( style == ControlStyle.KeyboardMouse )
		{
			currentControlStyle = ControlStyle.KeyboardMouse;
			setKeyboardMouseControls();
		} else {
			currentControlStyle = ControlStyle.Controller;
			setXBoxControls();
		}
	}

	void setKeyboardMouseControls()
	{
		upKey = KeyCode.W;
		downKey = KeyCode.S;
		wellKey = KeyCode.E;
		//shootKey = KeyCode.Mouse0;
		//shieldKey = KeyCode.Mouse1;
	}

	void setXBoxControls()
	{
		axis = "P1_Vertical";
		fireButton = "P1_R_Trigger";
		shieldButton = "P1_R_Shoulder";
		wellTrigger = "P1_L_Trigger";
		wellButton = "P1_X";
	}

	void Update()
	{
		if( networkView.isMine )
		{
			float inputY = 0.0f;
			bool shoot = false;
			bool shootWell = false;
			bool shieldButtonDown = false;
			bool shieldButtonUp = false;

			if( currentControlStyle == ControlStyle.KeyboardMouse )
			{
				if (Input.GetKey( upKey ) )
					inputY = 1.0f;
				if( Input.GetKey ( downKey ) )
					inputY = -1.0f;
				movement = new Vector2( 0, speed.y * inputY );

				if( Input.GetMouseButton( 0 ) )
					shoot = true;

				if( Input.GetKey( wellKey ) )
				{
					wellForceDelta += Time.deltaTime;
					if( wellForceDelta > 3.0f )
						wellForceDelta = 3.0f;
				}

				if( Input.GetKeyUp( wellKey ) )
				{
					wellForce = wellForceDelta / 3.0f;
					shootWell = true;
					wellForceDelta = 0.0f;
				}

				if( Input.GetMouseButtonDown( 1 ) )
					shieldButtonDown = true;
				if( Input.GetMouseButtonUp( 1 ) )
					shieldButtonUp = true;
			}
			else
			{
				inputY = Input.GetAxis ( axis );
				movement = new Vector2( 0, speed.y * inputY );

				if( Input.GetAxis ( fireButton ) > 0 )
					shoot = true;

				shootWell = Input.GetButton( wellButton );
				wellForce = Input.GetAxis( wellTrigger );
				shieldButtonDown = Input.GetButtonDown( shieldButton );
				shieldButtonUp = Input.GetButtonUp( shieldButton );
			}

			// translation
			if( transform.position.y > topBorder )
				transform.position = new Vector3( transform.position.x, topBorder, transform.position.z );
			if( transform.position.y < bottomBorder )
				transform.position = new Vector3( transform.position.x, bottomBorder, transform.position.z );

			if ( shoot )
			{
				WeaponScript weapon = GetComponentInChildren< WeaponScript >();
				if( weapon != null )
					weapon.Attack();
			}

			// well shot
			if( shootWell )
			{
				WeaponScript weapon = GetComponentInChildren< WeaponScript >();
				if( weapon != null )
					weapon.ShootWell( wellForce );
			}

			// shield
			if( shieldButtonDown && currentShield > 0.0f )
			{
				networkView.RPC( "setShieldOn", RPCMode.AllBuffered );
			}

			if( shieldButtonUp || ( isShielded && currentShield <= 0.0f ) )
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
