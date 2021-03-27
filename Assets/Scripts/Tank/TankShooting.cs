using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class TankShooting : NetworkBehaviour
{
    public int m_PlayerNumber = 1;
    public Rigidbody m_Shell1;
    public Rigidbody m_Shell2;
    public Rigidbody m_Shell3;

    public Transform m_FireTransform;
    public Slider m_AimSlider;
    public AudioSource m_ShootingAudio;
    public AudioClip m_ChargingClip;
    public AudioClip m_FireClip;
    public float m_MinLaunchForce = 15f;
    public float m_MaxLaunchForce = 30f;
    public float m_MaxChargeTime = 0.75f;

    private string m_FireButton;
    [SyncVar] private float m_CurrentLaunchForce;
    [SyncVar] private Vector3 firingPosition;
    private float m_ChargeSpeed;
    private bool m_Fired;

    public static int shopvalue1 = 1;
    public static int shopvalue2 = 1;


    private void OnEnable()
    {
        m_CurrentLaunchForce = m_MinLaunchForce;
        m_AimSlider.value = m_MinLaunchForce;
    }


    private void Start()
    {
        m_FireButton = "Fire" + m_PlayerNumber;
        m_ChargeSpeed = (m_MaxLaunchForce - m_MinLaunchForce) / m_MaxChargeTime;
    }


    private void Update()
    {
        if (!isLocalPlayer) return;
        m_AimSlider.value = m_MinLaunchForce;
        if (m_CurrentLaunchForce >= m_MaxLaunchForce && !m_Fired)
        {
            m_CurrentLaunchForce = m_MaxLaunchForce;
            CmdTankShooting();
        }
        else if (Input.GetButtonDown(m_FireButton))
        {
            m_Fired = false;
            m_CurrentLaunchForce = m_MinLaunchForce;
            m_ShootingAudio.clip = m_ChargingClip;
            m_ShootingAudio.Play();
        }
        else if (Input.GetButton(m_FireButton) && !m_Fired)
        {
            m_CurrentLaunchForce += m_ChargeSpeed * Time.deltaTime;
            m_AimSlider.value = m_CurrentLaunchForce;
        }
        else if (Input.GetButtonUp(m_FireButton) && !m_Fired && m_PlayerNumber == 1)
        {
            CmdTankShooting();
        }
        else if (Input.GetButtonUp(m_FireButton) && !m_Fired && m_PlayerNumber == 2)
        {
            CmdTankShooting2();
        }
    }

    [Command]
    private void CmdTankShooting()
    {
        Fire();
    }
    [Command]
    private void CmdTankShooting2()
    {
        Fire2();
    }

    [ClientRpc]
    private void Fire()
    {
        m_Fired = true;
        Rigidbody shell;
        shell = m_Shell1;
        if (shopvalue1 == 2)
        {
            shell = m_Shell2;
        } else if (shopvalue1 >= 3)
        {
            shell = m_Shell3;
        }
        Rigidbody shellInstance = Instantiate(shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;
        shellInstance.velocity = m_CurrentLaunchForce * m_FireTransform.forward; ;
        m_ShootingAudio.clip = m_FireClip;
        m_ShootingAudio.Play();
        m_CurrentLaunchForce = m_MinLaunchForce;
    }

    [ClientRpc]
    private void Fire2()
    {
        m_Fired = true;
        Rigidbody shell;
        shell = m_Shell1;
        if (shopvalue2 == 2)
        {
            shell = m_Shell2;
        }
        else if (shopvalue2 >= 3)
        {
            shell = m_Shell3;
        }
        Rigidbody shellInstance = Instantiate(shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;
        shellInstance.velocity = m_CurrentLaunchForce * m_FireTransform.forward; ;
        m_ShootingAudio.clip = m_FireClip;
        m_ShootingAudio.Play();
        m_CurrentLaunchForce = m_MinLaunchForce;
    }
}