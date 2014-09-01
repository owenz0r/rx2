using UnityEngine;
using System.Collections;

public class PowerupScript : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void triggerPowerup( Transform player )
	{
		WeaponScript weap = player.GetComponentInChildren< WeaponScript >();
		weap.setWeaponTrishot( true );
	}
}
