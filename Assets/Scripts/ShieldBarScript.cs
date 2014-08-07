using UnityEngine;
using System.Collections;

public class ShieldBarScript : MonoBehaviour {

	public float barDisplay = 100.0f;
	public Texture progressBarFull;

	public Vector2 pos = new Vector2( 20, 40 );
	public Vector2 size = new Vector2( 60, 20 );

	public bool flip = false;

	private GUISkin skin;

	void Start()
	{
		skin = Resources.Load ("GUISkin") as GUISkin;
	}

	void OnGUI()
	{
		GUI.skin = skin;
		float x = pos.x / 100.0f;
		x = x * Screen.width;
		float y = pos.y / 100.0f;
		y = y * Screen.height;

		float sx = size.x / 100.0f;
		sx = sx * Screen.width;
		float sy = size.y / 100.0f;
		sy = sy * Screen.height;

		if( flip )
		{
			GUI.BeginGroup ( new Rect( x, y, sx, sy ));
			GUI.Box ( new Rect( sx * ( (100.0f - barDisplay) / 100.0f ), 0, sx, sy ), progressBarFull );
		} else {
			GUI.BeginGroup ( new Rect( x, y, sx * ( barDisplay / 100.0f ), sy ));
			GUI.Box ( new Rect( 0, 0, sx, sy ), progressBarFull );
		}
		GUI.EndGroup ();
	}

}
