using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WellManager : MonoBehaviour {

	private Transform[] wellArray;

	void Start()
	{
		wellArray = new Transform[2];
	}

	public void addWell( Transform well, string playerColour )
	{
		int playerIdx = 0;
		if( playerColour == "red" )
		{
			playerIdx = 1;
		}
		if( wellArray[ playerIdx ] )
			Destroy( wellArray[ playerIdx].gameObject );

		wellArray[ playerIdx ] = well;
	}

	public List< Transform > getWellList()
	{
		List< Transform > wellList = new List< Transform >();
		if( wellArray[0] )
			wellList.Add( wellArray[0] );
		if( wellArray[1] )
			wellList.Add( wellArray[1] );
		return wellList;
	}
}
