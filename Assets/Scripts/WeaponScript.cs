using UnityEngine;
using System.Collections;

public class WeaponScript : MonoBehaviour {

	public Transform shotPrefab;
	public PregameScript pregameScript;
	public float shootingRate = 0.25f;

	private float shootCooldown;

	void Start()
	{
		shootCooldown = 0.0f;
	}

	void Update()
	{
		if( shootCooldown > 0 )
			shootCooldown -= Time.deltaTime;
	}

	public void Attack()
	{
		if( CanAttack )
		{
			shootCooldown = shootingRate;
			var shotTransform = Instantiate ( shotPrefab ) as Transform;
			shotTransform.position = transform.position;

			MoveScript move = shotTransform.gameObject.GetComponent< MoveScript >();
			if( move != null )
				move.direction = this.transform.right;
		}
	}

	public bool CanAttack
	{
		get
		{
			// can fire only if no cooldown and shields are not up
			return ( shootCooldown <= 0.0f && 
			        !GetComponentInParent< PlayerScript >().hasShieldUp &&
			        !pregameScript.isPregame);
		}
	}
}
