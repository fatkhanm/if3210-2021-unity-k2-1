using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SpawnCoint : NetworkBehaviour
{
    public GameObject m_coinpreb;
    public TankNetworkManager networkManager;
    public float nextPawnTime;
    public float delay = 10;
    public bool gameStart;
    

    // Start is called before the first frame update
    void Start()
    {
        gameStart = false;
        if (gameStart)
        {
            GameObject obj = Instantiate(m_coinpreb, transform.position, transform.rotation);
            NetworkServer.Spawn(obj);

        }
    }

    // Update is called once per frame
    void Update()
    {
        gameStart = networkManager.isGameStart;
        if (SPawnTime())
            Spawn();
    }

    void Spawn()
    {
        nextPawnTime = Time.time + delay;
        if (gameStart)
        {
            GameObject obj = Instantiate(m_coinpreb, transform.position, transform.rotation);
            NetworkServer.Spawn(obj);

        }

    }

    bool SPawnTime()
    {
        return Time.time >= nextPawnTime;
    }
}
