using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class cash2 : NetworkBehaviour
{
    [SyncVar] public int scorevalue = 0;
    Text scores;
    // Start is called before the first frame update
    void Start()
    {
        scores = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        scores.text = "Cash 2 :" + scorevalue + "$";
    }

    [Command]
    public void CmdAddCash(int cash)
    {
        Debug.Log("Server command : add cash " + cash);
        RpcAddCash(cash);
    }

    [ClientRpc]
    public void RpcAddCash(int cash)
    {
        Debug.Log("Client rpc : add cash " + cash);
        scorevalue += cash;
    }
}
