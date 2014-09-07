using UnityEngine;
using System.Collections;

public class WeaponScript : MonoBehaviour {

	public Transform shotPrefab;
	public PregameScript pregameScript;
	public RespawnScript respawnScript;
	public float shootingRate = 0.25f;
	public float trishotAngle = 10.0f;

	public Transform wellPrefab;
	public WellManager wellManager;
	public string weaponColour;

	private float shootCooldown;
	private bool trishot = false;

	void Start()
	{
		shootCooldown = 0.0f;
	}

	void Update()
	{
		if( shootCooldown > 0 )
			shootCooldown -= Time.deltaTime;
	}

	public void ShootWell()
	{
		if( CanShootWell )
		{
			var wellTransform = Instantiate ( wellPrefab ) as Transform;
			wellTransform.position = transform.position;
			
			PushScript push = wellTransform.GetComponent< PushScript >();
			if( weaponColour == "red" )
				push.force.x *= -1.0f;

			wellManager.addWell ( wellTransform, weaponColour );
		}
	}

	public void Attack()
	{
		if( CanAttack )
		{

			if( trishot )
			{
				var firstShot = Instantiate ( shotPrefab ) as Transform;
				var secondShot = Instantiate ( shotPrefab ) as Transform;
				var thirdShot = Instantiate ( shotPrefab ) as Transform;

				firstShot.position = transform.position;
				secondShot.position = new Vector3( transform.position.x, transform.position.y + 0.1f, transform.position.z );
				thirdShot.position = new Vector3( transform.position.x, transform.position.y - 0.1f, transform.position.z );

				MoveScript move = firstShot.gameObject.GetComponent< MoveScript >();
				if( move != null )
					move.direction = this.transform.right;

				if( this.transform.right.x > 0 )
				{
					float rad = trishotAngle * Mathf.PI / 180.0f;
					float x = Mathf.Cos ( rad );
					float y = Mathf.Sin ( rad );

					move = secondShot.gameObject.GetComponent< MoveScript >();
					move.direction = new Vector2( x, y );
					move = thirdShot.gameObject.GetComponent< MoveScript >();
					move.direction = new Vector2( x, -y );
				}
				else
				{
					float rad = trishotAngle * Mathf.PI / 180.0f;
					float x = Mathf.Cos ( rad );
					float y = Mathf.Sin ( rad );
					
					move = secondShot.gameObject.GetComponent< MoveScript >();
					move.direction = new Vector2( -x, y );
					move = thirdShot.gameObject.GetComponent< MoveScript >();
					move.direction = new Vector2( -x, -y );
				}

			}
			else
			{
				var shotTransform = Instantiate ( shotPrefab ) as Transform;
				shotTransform.position = transform.position;

				MoveScript move = shotTransform.gameObject.GetComponent< MoveScript >();
				if( move != null )
					move.direction = this.transform.right;
			}
			shootCooldown = shootingRate;
			SoundEffectsHelper.Instance.MakeLaserFireSound();
		}
	}

	public bool CanAttack
	{
		get
		{
			// can fire only if no cooldown and shields are not up
			return ( shootCooldown <= 0.0f && 
			        !GetComponentInParent< PlayerScript >().hasShieldUp &&
			        !pregameScript.isPregame &&
			        !respawnScript.respawning);
		}
	}

	public bool CanShootWell
	{
		get
		{
			return true;
		}
	}

	public void setWeaponTrishot( bool onoff )
	{
		trishot = onoff;
	}
}
