using UnityEngine;
using System.Collections;

public class RechargePowerupScript : PowerupScript {

	public override void triggerPowerup( Transform player )
	{
		PlayerScript ps = player.GetComponent< PlayerScript >();
		ps.ResetShield();
	}
}
