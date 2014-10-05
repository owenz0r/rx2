using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {

	private float startServerButtonX;
	private float startServerButtonY;
	private float startServerButtonW;
	private float startServerButtonH;

	private string gameName = "OMcN_RX2";
	private bool refreshing = false;
	private HostData[] hostData;

	void Start()
	{
		startServerButtonW = 0.1f * Screen.width;
		startServerButtonH = 0.05f * Screen.height;
		startServerButtonX = (0.3f * Screen.width);
		startServerButtonY = (0.3f * Screen.height) - (startServerButtonH / 2);
	}

	void Update()
	{
		if( refreshing )
		{
			print ( "..." );
			if( MasterServer.PollHostList().Length > 0 )
			{
				refreshing = false;
				print ( MasterServer.PollHostList().Length );
				hostData = MasterServer.PollHostList();
			}
		}
	}

	void refreshHostList()
	{
		MasterServer.RequestHostList( gameName );
		refreshing = true;
	}
	
	
	void OnMasterServerEvent( MasterServerEvent msEvent )
	{
		if( msEvent == MasterServerEvent.RegistrationSucceeded )
			print( "Server registered!" );
	}
	
	void OnServerInitialized()
	{
		print( "Server Initialized!" );
	}
	
	void startServer()
	{
		Network.InitializeServer( 2, 25001, !Network.HavePublicAddress() );
		MasterServer.RegisterHost( gameName, "Owen's Awesome Game", "Isn't is awesome?" );
	}

	void OnGUI()
	{
		if( !Network.isClient && !Network.isServer)
		{
			if( GUI.Button( new Rect( startServerButtonX, 
			                         startServerButtonY, 
			                         startServerButtonW, 
			                         startServerButtonH ), "Start Server" ) )
			{
				print( "Start server" );
				startServer();
			}
			if( GUI.Button( new Rect( startServerButtonX, 
			                         startServerButtonY + startServerButtonH + (0.02f * Screen.height), 
			                         startServerButtonW, 
			                         startServerButtonH ), "Refresh" ) )
			{
				print( "Refreshing..." );
				refreshHostList();
			}
			
			if( hostData != null )
			{
				for( int i=0; i < hostData.Length; i++ )
				{
					if( GUI.Button( new Rect( startServerButtonX * 1.5f,
					                         startServerButtonY + (i * (startServerButtonH  + (0.02f * Screen.height) ) ),
					                         startServerButtonW,
					                         startServerButtonH ),
					               hostData[i].gameName ) )
					{
						Network.Connect( hostData[i] );
					}
				}
			}
		}
	}
}
