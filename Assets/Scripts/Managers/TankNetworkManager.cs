using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public struct CreateTankMessage : NetworkMessage
{
    public int id;
}

public class TankNetworkManager : NetworkManager
{
    // Start is called before the first frame update
    public NetworkConnection playerOneConn;
    public NetworkConnection playerTwoConn;
    public int connectedPlayers;
    public bool isGameStart;

    public override void Start()
    {
        base.Start();
        connectedPlayers = 0;
        isGameStart = false;
        NetworkServer.RegisterHandler<CreateTankMessage>(OnTankConnected);
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        if (connectedPlayers == 0)
        {
            playerOneConn = conn;
            CreateTankMessage msg = new CreateTankMessage
            {
                id = connectedPlayers
            };
            connectedPlayers++;
            conn.Send(msg);
        } else if (connectedPlayers == 1)
        {
            playerTwoConn = conn;
            CreateTankMessage msg = new CreateTankMessage
            {
                id = connectedPlayers
            };
            connectedPlayers++;
            conn.Send(msg);
        }
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);
    }

    public void OnTankConnected(NetworkConnection conn, CreateTankMessage msg)
    {
        Debug.Log("CONNECTED");
        GameObject obj = Instantiate(playerPrefab);
        obj.GetComponent<TankMovement>().m_PlayerNumber = msg.id + 1;
        NetworkServer.AddPlayerForConnection(conn, obj);
    }

    public void RespawnTank(int tankId)
    {
        if (tankId == 1)
        {
            // aaaa
        } else if (tankId == 2)
        {
            // aaaaaaaaaaaa
        }
    }

    public void startGame()
    {
        isGameStart = true;
    }
}
