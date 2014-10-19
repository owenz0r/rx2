using UnityEngine;
using System.Collections;

public class GamestateManager : MonoBehaviour {

	private bool _opponentIsAlive = true;

	public bool opponentIsAlive()
	{
		return _opponentIsAlive;
	}

	public void sendOpponentAlive()
	{
		networkView.RPC ( "setOpponentAlive", RPCMode.OthersBuffered );
	}

	[RPC]
	public void setOpponentAlive()
	{
		_opponentIsAlive = true;
	}

	public void sendOpponentDead()
	{
		networkView.RPC ( "setOpponentDead", RPCMode.OthersBuffered );
	}

	[RPC]
	public void setOpponentDead()
	{
		_opponentIsAlive = false;
	}

}
