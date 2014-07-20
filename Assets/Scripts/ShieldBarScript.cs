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

		if( flip )
		{
			GUI.BeginGroup ( new Rect( pos.x, pos.y, size.x, size.y ));
			GUI.Box ( new Rect( size.x * ( (100.0f - barDisplay) / 100.0f ), 0, size.x, size.y ), progressBarFull );
		} else {
			GUI.BeginGroup ( new Rect( pos.x, pos.y, size.x * ( barDisplay / 100.0f ), size.y ));
			GUI.Box ( new Rect( 0, 0, size.x, size.y ), progressBarFull );
		}
		GUI.EndGroup ();

	}

}
