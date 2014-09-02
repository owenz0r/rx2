using UnityEngine;
using System.Collections;

public class TrishotPowerupScript : PowerupScript {

	public override void triggerPowerup( Transform player )
	{
		WeaponScript weap = player.GetComponentInChildren< WeaponScript >();
		weap.setWeaponTrishot( true );
	}
}
