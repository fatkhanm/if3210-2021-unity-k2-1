using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class score : NetworkBehaviour
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
        scores.text = "Cash 1 :" + scorevalue +"$";
    }

    [Command]
    public void CmdAddCash(int cash)
    {
        RpcAddCash(cash);
    }

    [ClientRpc]
    public void RpcAddCash(int cash)
    {
        scorevalue += cash;
    }
}
