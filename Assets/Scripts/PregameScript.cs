﻿using UnityEngine;
using System.Collections;

public class PregameScript : MonoBehaviour {

	private GUISkin skin;
	private int current = 0;
	private float counter = 0.0f;
	private string[] countdown;
	private bool initial = true;

	public bool isPregame = false;
	public bool beginCountdown;

	void Start()
	{
		beginCountdown = false;
		skin = Resources.Load ("GUISkin") as GUISkin;
		countdown = new string[4];
		countdown[0] = "3";
		countdown[1] = "2";
		countdown[2] = "1";
		countdown[3] = "GO!";

	}

	void Update()
	{
		if( isPregame )
		{
			if( beginCountdown )
			{
				if( initial )
				{
					SoundEffectsHelper.Instance.MakeCountdownTickSound();
					initial = false;
				}

				counter += Time.deltaTime;
				if( counter >= 1.0f )
				{
					current++;
					counter = 0.0f;

					if( current < 3 )
					{
						SoundEffectsHelper.Instance.MakeCountdownTickSound();
					} else if( current == 3 ) {
						SoundEffectsHelper.Instance.MakeCountdownGoSound();
					}

					if( current > 3 )
					{
						isPregame = false;
						current = 0;
					}
				}
			}
		}
	}

	void OnGUI()
	{
		if( isPregame )
		{
			if( beginCountdown )
			{
				GUI.skin = skin;
				GUI.contentColor = Color.yellow;
				GUIStyle labelStyle = skin.GetStyle( "Label" );
				labelStyle.alignment = TextAnchor.MiddleCenter;
				GUI.Label ( new Rect( (Screen.width/2.0f) - 100, (Screen.height/2.0f) - 50, 200, 100 ), countdown[ current ], labelStyle );
			}
		}
	}

	public void startPregame()
	{
		if( !isPregame )
		{
			initial = true;
			isPregame = true;
		}
	}

	public void startCountdown()
	{
		beginCountdown = true;
	}
}
