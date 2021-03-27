using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LocatorMovement : MonoBehaviour
{
    NavMeshAgent navMeshAgent;
    Transform destination;
    GameManager gameManager;
    Animator animator;

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
            Debug.Log("Target:" + targetPlayer);
            CameraControl camCon = GameObject.Find("CameraRig").GetComponent<CameraControl>();
            destination = camCon.m_Targets[targetPlayer].gameObject.transform;

            SetDestination();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (navMeshAgent.remainingDistance < 10 && lastState == 0)
        {
            animator.SetBool("Chasing", true);
            navMeshAgent.speed *= 2;
            lastState = 1;
        } else if (navMeshAgent.remainingDistance >= 10 && lastState == 1)
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
