using UnityEngine;
using Mirror;

public class TankMovement : NetworkBehaviour
{
    public int m_PlayerNumber = 1;              
    public float m_Speed = 12f;              
    public float m_TurnSpeed = 180f;         
    public AudioSource m_MovementAudio;    
    public AudioClip m_EngineIdling;        
    public AudioClip m_EngineDriving;       
    public float m_PitchRange = 0.2f;          


    private string m_MovementAxisName;          
    private string m_TurnAxisName;              
    private Rigidbody m_Rigidbody;              
    private float m_MovementInputValue;         
    private float m_TurnInputValue;             
    private float m_OriginalPitch;

    // for spawning minions
    public GameObject m_LocatorPrefab;
    public GameObject m_ExecutorPrefab;

    public override void OnStopClient()
    {
        base.OnStopClient();
        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (gameManager)
        {
            gameManager.tanksObject.Remove(gameObject);
            gameManager.SetCameraTargets();
            int idx = gameManager.m_Tanks.FindIndex(x => x.m_Instance == gameObject);
            gameManager.m_Tanks.RemoveAt(idx);
            gameManager.DecreasePlayerCount();
        }
    }

    public override void OnStopServer()
    {
        base.OnStopServer();
        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (gameManager)
        {
            gameManager.tanksObject.Remove(gameObject);
            gameManager.SetCameraTargets();
            int idx = gameManager.m_Tanks.FindIndex(x => x.m_Instance == gameObject);
            if (idx >= 0) gameManager.m_Tanks.RemoveAt(idx);
            gameManager.ResetServer();
        }    
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.spawnTank(gameObject);
    }

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.tanksObject.Add(gameObject);
        gameManager.SetCameraTargets();
    }


    private void OnEnable()
    {
        m_Rigidbody.isKinematic = false;
        m_MovementInputValue = 0f;
        m_TurnInputValue = 0f;
    }


    private void OnDisable()
    {
        m_Rigidbody.isKinematic = true;
    }


    private void Start()
    {
        m_MovementAxisName = "Vertical" + m_PlayerNumber;
        m_TurnAxisName = "Horizontal" + m_PlayerNumber;

        m_OriginalPitch = m_MovementAudio.pitch;

    }


    private void Update()
    {
        if (!isLocalPlayer) return;
        m_MovementInputValue = Input.GetAxis(m_MovementAxisName);
        m_TurnInputValue = Input.GetAxis(m_TurnAxisName);

        EngineAudio();
        if (Input.GetKeyDown(KeyCode.E))
        {
            // spawn, spawning a little bit to the left
            Transform spawnMinionPoint = this.transform;
            if (m_LocatorPrefab && m_PlayerNumber == 1)
            {
                CmdSpawnMinions(1, 1, spawnMinionPoint);
            }
        }
        if (Input.GetKeyDown(KeyCode.Slash))
        {
            // spawn, spawning a little bit to the left
            Transform spawnMinionPoint = this.transform;
            if (m_LocatorPrefab && m_PlayerNumber == 2)
            {
                CmdSpawnMinions(1, 0, spawnMinionPoint);
            }
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Transform spawnMinionPoint = this.transform;
            if (m_ExecutorPrefab && m_PlayerNumber == 1)
            {
                CmdSpawnMinions(2, 1, spawnMinionPoint);
            }
        }

        if (Input.GetKeyDown(KeyCode.Period))
        {
            Transform spawnMinionPoint = this.transform;
            if (m_ExecutorPrefab && m_PlayerNumber == 2)
            {
                CmdSpawnMinions(2, 0, spawnMinionPoint);
            }
        }
    }


    private void EngineAudio()
    {
        if (Mathf.Abs(m_MovementInputValue) < 0.1f && Mathf.Abs(m_TurnInputValue) < 0.1f)
        {
            if (m_MovementAudio.clip == m_EngineDriving)
            {
                m_MovementAudio.clip = m_EngineIdling;
                m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                m_MovementAudio.Play();
            }
        }
        else
        {
            if (m_MovementAudio.clip == m_EngineIdling)
            {
                m_MovementAudio.clip = m_EngineDriving;
                m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                m_MovementAudio.Play();
            }
        }
    }


    private void FixedUpdate()
    {
        Move();
        Turn();
    }


    private void Move()
    {
        Vector3 movement = transform.forward * m_MovementInputValue * m_Speed * Time.deltaTime;

        m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
    }


    private void Turn()
    {
        float turn = m_TurnInputValue * m_TurnSpeed * Time.deltaTime;

        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);

        m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turnRotation);
    }

    [Command]
    private void CmdSpawnMinions(int type, int target, Transform transform)
    {
        RpcSpawnMinions(type, target, transform);
    }

    [ClientRpc]
    private void RpcSpawnMinions(int type, int target, Transform transform)
    {
        Debug.Log(transform);
        CameraControl camCon = GameObject.Find("CameraRig").GetComponent<CameraControl>();
        Vector3 spawnPos;
        if (target == 0)
        {
            spawnPos = camCon.m_Targets[1].position;
        }
        else
        {
            spawnPos = camCon.m_Targets[0].position;
        }
        if (type == 1)
        {
            
            GameObject locator = Instantiate(m_LocatorPrefab,
                    spawnPos,
                    Quaternion.identity);
            locator.GetComponent<LocatorMovement>().SetTarget(target);
        } else if (type == 2)
        {

            GameObject executor = Instantiate(m_ExecutorPrefab,
                    spawnPos,
                    Quaternion.identity);
            executor.GetComponent<ExecutorMovement>().SetTarget(target);
        }
    }
}