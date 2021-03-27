using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Mirror;

public class GameManager : NetworkBehaviour
{
    public int m_NumRoundsToWin = 5;
    public float m_StartDelay = 3f;
    public float m_EndDelay = 3f;
    public CameraControl m_CameraControl;
    public Text m_MessageText;
    public GameObject m_TankPrefab;
    public List<TankManager> m_Tanks;
    public List<GameObject> tanksObject;

    private int m_RoundNumber;
    private WaitForSeconds m_StartWait;
    private WaitForSeconds m_EndWait;
    private TankManager m_RoundWinner;
    private TankManager m_GameWinner;

    private int totalPlayer;
    private bool isGameBegin;

    private void Start()
    {
        totalPlayer = 0;
        isGameBegin = false;
        m_Tanks = new List<TankManager>();
        tanksObject = new List<GameObject>();
        m_StartWait = new WaitForSeconds(m_StartDelay);
        m_EndWait = new WaitForSeconds(m_EndDelay);

        //SpawnAllTanks();
        //SetCameraTargets();

        // StartCoroutine(GameLoop());
    }

    private void Update()
    {
        if (!isGameBegin && totalPlayer == 2)
        {
            isGameBegin = true;
            GameObject.Find("NetworkManager").GetComponent<TankNetworkManager>().startGame();
            StartCoroutine(GameLoop());
        }
    }

    public void DecreasePlayerCount()
    {
        totalPlayer--;
        m_MessageText.text = "Player Connected: " + totalPlayer;
    }

    public void ResetServer()
    {
        totalPlayer = 0;
        m_MessageText.text = "Player Connected: " + totalPlayer;
    }

    private void SpawnAllTanks()
    {
        for (int i = 0; i < m_Tanks.Count; i++)
        {
            m_Tanks[i].m_Instance =
                Instantiate(m_TankPrefab, m_Tanks[i].m_SpawnPoint.position, m_Tanks[i].m_SpawnPoint.rotation) as GameObject;
            m_Tanks[i].m_PlayerNumber = i + 1;
            m_Tanks[i].Setup();
        }
    }

    public void spawnTank(GameObject instance)
    {
        TankManager tank = new TankManager();
        tank.m_Instance = instance;
        tank.m_PlayerNumber = totalPlayer + 1;
        tank.Setup();
        totalPlayer++;
        m_Tanks.Add(tank);
        Debug.Log("Tanks count: " + m_Tanks.Count);
        m_MessageText.text = "Player Connected: " + totalPlayer;
    }


    public void SetCameraTargets()
    {
        Transform[] targets = new Transform[tanksObject.Count];

        for (int i = 0; i < targets.Length; i++)
        {
            targets[i] = tanksObject[i].transform;
        }

        m_CameraControl.m_Targets = targets;
    }


    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(RoundStarting());

        yield return StartCoroutine(RoundPlaying());

        yield return StartCoroutine(RoundEnding());

        //if (m_GameWinner != null)
        //{
        //    //Application.LoadLevel(Application.loadedLevel);
        //}
        //else
        //{
        //    StartCoroutine(GameLoop());
        //}
    }


    private IEnumerator RoundStarting()
    {
        // ResetAllTanks();
        DisableTankControl();

        m_CameraControl.SetStartPositionAndSize();

        m_RoundNumber++;
        m_MessageText.text = "ROUND " + m_RoundNumber;

        yield return m_StartWait;
    }


    private IEnumerator RoundPlaying()
    {
        EnableTankControl();

       
        m_MessageText.text = string.Empty;

        while (!OneTankLeft())
        {
           
            yield return null;
        }
    }


    private IEnumerator RoundEnding()
    {
        DisableTankControl();

        m_RoundWinner = null;

        m_RoundWinner = GetRoundWinner();

        if (m_RoundWinner != null)
            m_RoundWinner.m_Wins++;

        m_GameWinner = GetGameWinner();

        string message = EndMessage();
        m_MessageText.text = message;

        yield return m_EndWait;
    }


    private bool OneTankLeft()
    {
        int numTanksLeft = 0;

        for (int i = 0; i < m_Tanks.Count; i++)
        {
            if (m_Tanks[i].m_Instance.activeSelf)
                numTanksLeft++;
        }

        return numTanksLeft <= 1;
    }


    private TankManager GetRoundWinner()
    {
        for (int i = 0; i < m_Tanks.Count; i++)
        {
            if (m_Tanks[i].m_Instance.activeSelf)
                return m_Tanks[i];
        }

        return null;
    }


    private TankManager GetGameWinner()
    {
        for (int i = 0; i < m_Tanks.Count; i++)
        {
            if (m_Tanks[i].m_Wins == m_NumRoundsToWin)
                return m_Tanks[i];
        }

        return null;
    }


    private string EndMessage()
    {
        string message = "DRAW!";

        if (m_RoundWinner != null)
            message = m_RoundWinner.m_ColoredPlayerText + " WINS THE ROUND!";

        message += "\n\n\n\n";

        for (int i = 0; i < m_Tanks.Count; i++)
        {
            message += m_Tanks[i].m_ColoredPlayerText + ": " + m_Tanks[i].m_Wins + " WINS\n";
        }

        if (m_GameWinner != null)
            message = m_GameWinner.m_ColoredPlayerText + " WINS THE GAME!";

        return message;
    }


    private void ResetAllTanks()
    {
        for (int i = 0; i < m_Tanks.Count; i++)
        {
            m_Tanks[i].Reset();
        }
    }


    private void EnableTankControl()
    {
        for (int i = 0; i < m_Tanks.Count; i++)
        {
            m_Tanks[i].EnableControl();
        }
    }


    private void DisableTankControl()
    {
        for (int i = 0; i < m_Tanks.Count; i++)
        {
            m_Tanks[i].DisableControl();
        }
    }
}