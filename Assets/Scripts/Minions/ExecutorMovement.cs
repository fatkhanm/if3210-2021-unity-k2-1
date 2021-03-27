using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ExecutorMovement : MonoBehaviour
{
    NavMeshAgent navMeshAgent;
    Transform destination;
    GameManager gameManager;
    Animator animator;

    public ParticleSystem m_ExplosionParticles;
    public AudioSource m_ExplosionAudio;


    int targetPlayer = -1;
    int lastState = 0;

    // Start is called before the first frame update
    void Start()
    {
        
        navMeshAgent = this.GetComponent<NavMeshAgent>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        animator = this.GetComponent<Animator>();
        if (navMeshAgent != null && gameManager != null && targetPlayer != -1)
        {
            CameraControl camCon = GameObject.Find("CameraRig").GetComponent<CameraControl>();
            if (targetPlayer == 0)
            {
                gameObject.transform.position = camCon.m_Targets[1].gameObject.transform.position;
            }
            destination = camCon.m_Targets[targetPlayer].gameObject.transform;
            //SetDestination();
        }
    }

    // Update is called once per frame
    void Update()
    {
        navMeshAgent.destination = destination.position;
        SetDestination();
        if (Vector3.Distance(gameObject.transform.position, destination.position) < 1)
        {
            m_ExplosionAudio.Play();
            if (navMeshAgent.remainingDistance < 1)
            {
                m_ExplosionParticles.transform.parent = null;
                m_ExplosionParticles.Play();
                Destroy(gameObject);
                destination.GetComponent<TankHealth>().TakeDamage(50);
                return;
            }
        }
        
        if (navMeshAgent.remainingDistance < 5 && lastState == 0)
        {
            animator.SetBool("Chasing", true);
            navMeshAgent.speed *= 2;
            lastState = 1;
        }
        else if (navMeshAgent.remainingDistance >= 5 && lastState == 1)
        {
            animator.SetBool("Chasing", false);
            navMeshAgent.speed /= 2f;
            lastState = 0;
        }
        SetDestination();
    }

    void SetDestination()
    {
        if (destination)
        {
            Debug.Log("Chasing player" + this.targetPlayer);
            Vector3 targetPosition = destination.transform.position;
            navMeshAgent.SetDestination(targetPosition);
        }
    }

    public void SetTarget(int targetId)
    {
        this.targetPlayer = targetId;
    }
}
