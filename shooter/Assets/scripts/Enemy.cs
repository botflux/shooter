using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof (NavMeshAgent))]
public class Enemy : LivingEntity
{
    private NavMeshAgent agent;
    private Transform target;

    private void Awake ()
    {
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected override void Start()
    {
        base.Start();
        StartCoroutine(UpdatePath());
    }
    
    IEnumerator UpdatePath ()
    {
        float refreshRate = 0.4f;
        while (target != null)
        {
            Vector3 targetPosition = new Vector3(target.position.x, 0, target.position.z);

            if (!dead)
            {
                SetDestination(targetPosition);
            }
            yield return new WaitForSeconds(refreshRate);
        }
    }

    public void SetDestination (Vector3 newDestination)
    {
        agent.destination = newDestination;
        agent.isStopped = false;
    }
}
