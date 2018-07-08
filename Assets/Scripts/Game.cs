﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Game : NetworkBehaviour {

    
    public int rounds = 3;
    public int lives = 2;
    public int MaxPlayers = 2;
    private bool host = false;

    private NetworkHandler network;
    private GameMode gameMode;
    private static Game instance;
    private static Player localPlayer;

    public class SyncListPlayerRecord : SyncListStruct<PlayerRecord>{}
    [SyncVar]
    public SyncListPlayerRecord networkPlayers = new SyncListPlayerRecord();
    private State state = State.STARTING;
    

    public void Awake()
    {
        //Network Test Object
        GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 1);

        gameMode = GameMode.LOCALMULTIPLAYER;
        instance = this;
    }
    /*************************************************************************/

    /// <summary>
    /// Notifies game of a player death - If not hosting Notifies host
    /// </summary>
    /// <param name="player"></param>
    public void TriggerDeath(Player player)
    {
        Debug.Log("Player " + player.name + " has died");
    }

    public void RegisterPlayer(NetworkIdentity id)
    {
       //if (!isServer) return;
        Debug.Log("Registering Player with game id = " + id.netId.ToString());
        if (networkPlayers.Count < MaxPlayers)
        {
            networkPlayers.Add(new PlayerRecord(id));
        }
    }

    public void UnregisterPlayer(NetworkIdentity id)
    {
        if (!isServer) return;
        networkPlayers.Remove(new PlayerRecord(id));

    }

    public int GetLives()
    {
        return lives;
    }
    
    public int GetRounds()
    {
        return rounds;
    }

    public bool IsHost()
    {
        return host;
    }

    public NetworkHandler GetNetworkHandler()
    {
        return network;
    }

    public State GetState()
    {
        return state;
    }

    public GameMode GetGameMode()
    {
        return gameMode;
    }




    /*************************************************************************/
    /// <summary>
    /// Retrieves registered Players from the game. Provide a player number. Players
    /// are registered in order as they come. null if that player is unavailable
    /// </summary>
    /// <param name="playerNumber"></param>
    /// <returns></returns>
    public Player GetPlayer(int playerNumber)
    {
        if (playerNumber >= networkPlayers.Count) return null;
        GameObject player = ClientScene.FindLocalObject(networkPlayers[playerNumber].id.netId);
        if (!player) return null;
        Debug.Log(networkPlayers[playerNumber].id.netId.ToString());
        Player result = player.GetComponent<Player>();
        return result;
    }

    public int GetNumberOfPlayers()
    {
        return networkPlayers.Count;
    }
    /// <summary>
    /// Used to get the local instance of a player
    /// </summary>
    /// <returns> Local Player -> 1 Per Client </returns>
    public static Player GetLocalPlayer()
    {
        return localPlayer;
    }

    /// <summary>
    /// Get's Game Singleton
    /// </summary>
    /// <returns> Single instance of the client game</returns>
    public static Game GetInstance()
    {
        if (!instance)
        {
            Debug.Log("Game was not properly instantiated");
        }
        Debug.Log("Offering Game");
        return instance;
    }

    void Update()
    {
        Debug.Log("Players - " + networkPlayers.Count );
        foreach (PlayerRecord record in networkPlayers)
        {
            Debug.Log("I have a player - " + record.id.netId.ToString());
        }
        //TODO Updates based on inputs and notifications - Biggest being death notfication
        switch (state)
        {
            case State.FIGHTING:
                break;
            case State.PAUSED:
                break;
            case State.STARTING:
                break;
            case State.ROUND_BEGINING:
                break;
            case State.ROUND_ENDING:
                break;
            case State.COMPLETED:
                break;
            case State.SUMMARIZING:
                break;
        }
    }
    /*************************************************************************/
    public enum State
    {
        PAUSED,
        STARTING,
        ROUND_BEGINING,
        FIGHTING,
        ROUND_ENDING,
        SUMMARIZING,
        COMPLETED
    }

    public enum GameMode
    {
        SINGLEPLAYER,
        LOCALMULTIPLAYER,
        NETWORKMULTIPLAYER
    }

    public struct PlayerRecord
    {
        public NetworkIdentity id;
        public PlayerRecord(NetworkIdentity id)
        {
            this.id = id;
        }
    }


}
