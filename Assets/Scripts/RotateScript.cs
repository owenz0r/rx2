using UnityEngine;
using System.Collections;

public class RotateScript : MonoBehaviour {
	
	public float angularVelocity = 12.0f;
	public float radialDeadZone = 0.25f;

	// Update is called once per frame
	void Update () {
		if( networkView.isMine )
		{
			if( GetComponentInParent<PlayerScript>().currentControlStyle == PlayerScript.ControlStyle.Controller )
			{
				var rightStick = new Vector2( Input.GetAxis( "P1_R_Horizontal" ), Input.GetAxis( "P1_R_Vertical" ) );
				Vector3 direction;
				if( transform.parent.tag == "blue" )
				{
					direction = new Vector3( rightStick.y * -1.0f, rightStick.x, 0 );
				} else {
					direction = new Vector3( rightStick.y, rightStick.x * -1.0f, 0 );
				}
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
			else
			{
				Vector3 mousePos = Camera.main.ScreenToWorldPoint( Input.mousePosition );
				Vector3 relativePos = mousePos - transform.position;
				Vector3 direction;
				if( transform.parent.tag == "blue" )
				{
					direction = new Vector3( relativePos.y * -1.0f, relativePos.x, 0.0f );
				} else {
					direction = new Vector3( relativePos.y, relativePos.x * -1.0f, 0.0f );
				}
				var currentRotation = Quaternion.LookRotation( Vector3.forward, direction );
				transform.rotation = Quaternion.Lerp ( transform.rotation, currentRotation, Time.deltaTime * angularVelocity );

			}
		}
	}
}
