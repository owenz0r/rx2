using UnityEngine;
using System.Collections;

public class RotateScript : MonoBehaviour {
	
	public float angularVelocity = 12.0f;
	public float radialDeadZone = 0.25f;

	// Update is called once per frame
	void Update () {
		if( networkView.isMine )
		{
			var rightStick = new Vector2( Input.GetAxis( "P1_R_Horizontal" ), Input.GetAxis( "P1_R_Vertical" ) );
			var direction = new Vector3( rightStick.y * -1.0f, rightStick.x, 0 );
			//print ( direction );
			if( direction.magnitude > radialDeadZone )
			{
				var currentRotation = Quaternion.LookRotation( Vector3.forward, direction );
				transform.rotation = Quaternion.Lerp( transform.rotation, currentRotation, Time.deltaTime * angularVelocity );
			} else {
				var currentRotation = Quaternion.LookRotation( Vector3.forward, Vector3.up );
				transform.rotation = Quaternion.Lerp( transform.rotation, currentRotation, Time.deltaTime * angularVelocity );
			}
		}
	}
}
