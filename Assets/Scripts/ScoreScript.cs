using UnityEngine;
using System.Collections;

public class ScoreScript : MonoBehaviour {
	
	public Vector2 pos = new Vector2( 20, 40 );
	public Vector2 size = new Vector2( 500, 100 );
	public Color colour;
	
	private GUISkin skin;
	private int score = 0;

	void Start()
	{
		skin = Resources.Load ("GUISkin") as GUISkin;
	}
	
	void OnGUI()
	{
		GUI.skin = skin;
		GUI.contentColor = new Color( colour.r, colour.g, colour.b );
		GUI.Label ( new Rect( pos.x, pos.y, size.x, size.y ), score.ToString() );
	}

	public void incrementScore()
	{
		score++;
	}
}
