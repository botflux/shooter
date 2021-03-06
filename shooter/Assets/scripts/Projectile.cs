﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public LayerMask collisionMask;
    public Color trailColor;
    public float damage = 1;

    private Transform tr;
    private float movementSpeed = 10f;

    private float skinWidth = .1f;

    private void Awake ()
    {
        tr = transform;
    }

    private void Start ()
    {
        Destroy(gameObject, 3f);
        Collider[] intialCollisions = Physics.OverlapSphere(tr.position, .1f, collisionMask);

        if (intialCollisions != null && intialCollisions.Length > 0)
        {
            OnHitObject(intialCollisions[0], tr.position);
        }

        GetComponent<TrailRenderer>().material.SetColor("_TintColor", trailColor);
    }

    private void FixedUpdate ()
    {
        float moveDistance = movementSpeed * Time.fixedDeltaTime;
        CheckCollisions(moveDistance);

        tr.Translate(Vector3.forward * moveDistance, Space.Self);
    }

    private void CheckCollisions (float moveDistance)
    {
        Ray ray = new Ray(tr.position, tr.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, moveDistance + skinWidth, collisionMask, QueryTriggerInteraction.Collide))
        {
            OnHitObject(hit.collider, hit.point);
        }
    }

    void OnHitObject (Collider coll, Vector3 hitPoint)
    {
        IDamageable damageableObject = coll.GetComponent<IDamageable>();

        if (damageableObject != null)
        {
            damageableObject.TakeHit(damage, hitPoint, transform.forward);
        }

        GameObject.Destroy(gameObject);
    }

    public void SetSpeed (float speed)
    {
        movementSpeed = speed;
    }
}
