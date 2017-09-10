using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof (NavMeshAgent))]
public class Enemy : LivingEntity
{
    public enum State
    {
        Idle,
        Chasing,
        Attacking
    };

    private State currentState;

    public ParticleSystem deathEffect;

    private NavMeshAgent agent;
    private Transform target;
    private LivingEntity targetEntity;
    private Material skinMaterial;

    private Color originalColor;

    private float attackDistance = .5f;
    private float timeBtwAttacks = 1f;
    private float damage = 1f;

    private float nextAttackTime;
    private float myCollisionRaduis;
    private float targetCollisionRaduis;

    private bool hasTarget;

    private void Awake ()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    protected override void Start()
    {
        base.Start();
        currentState = State.Chasing;

        skinMaterial = GetComponent<Renderer>().material;
        originalColor = skinMaterial.color;

        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
            hasTarget = true;

            myCollisionRaduis = GetComponent<CapsuleCollider>().radius;
            targetCollisionRaduis = target.GetComponent<CapsuleCollider>().radius;

            targetEntity = target.GetComponent<LivingEntity>();
            targetEntity.OnDeath += OnTargetDeath;

            StartCoroutine(UpdatePath());
        }

    }

    public override void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection)
    {
        if (damage>=health)
        {
            Destroy(Instantiate<ParticleSystem>(deathEffect, hitPoint, Quaternion.FromToRotation(Vector3.forward, hitDirection)).gameObject, deathEffect.main.startLifetimeMultiplier);
        }

        base.TakeHit(damage, hitPoint, hitDirection);
    }

    private void OnTargetDeath ()
    {
        hasTarget = false;
        currentState = State.Idle;
    }

    private void Update()
    {
        if (hasTarget)
        {
            if (Time.time > nextAttackTime)
            {
                float sqrDistanceToTarget = (target.position - transform.position).sqrMagnitude;

                if (sqrDistanceToTarget < Mathf.Pow(attackDistance + myCollisionRaduis + targetCollisionRaduis, 2))
                {
                    nextAttackTime = Time.time + timeBtwAttacks;
                    StartCoroutine(Attack());
                }
            }
        }
    }

    IEnumerator Attack()
    {
        currentState = State.Attacking;
        agent.enabled = false;

        Vector3 originalPosition = transform.position;
        Vector3 dirToTarget = (target.position - transform.position).normalized;
        Vector3 attackPosition = target.position - dirToTarget * myCollisionRaduis;

        float attackSpeed = 3;
        float percent = 0;

        skinMaterial.color = Color.red;
        bool hasAppliedDamage = false;

        while (percent <= 1)
        {
            if (percent >= .5f && !hasAppliedDamage)
            {
                hasAppliedDamage = true;
                targetEntity.TakeDamage(damage);
            }

            percent += Time.deltaTime * attackSpeed;
            float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
            transform.position = Vector3.Lerp(originalPosition, attackPosition, interpolation);
            
            yield return null;
        }

        skinMaterial.color = originalColor;

        currentState = State.Chasing;
        agent.enabled = true;
    }


    IEnumerator UpdatePath ()
    {
        float refreshRate = 0.4f;
        while (hasTarget)
        {
            if (currentState == State.Chasing)
            {
                Vector3 dirToTarget = (target.position - transform.position).normalized;
                Vector3 targetPosition = target.position - dirToTarget * (myCollisionRaduis + targetCollisionRaduis + attackDistance / 2f);

                if (!dead)
                {
                    SetDestination(targetPosition);
                }
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
