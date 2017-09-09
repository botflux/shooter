using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public LayerMask collisionMask;
    public float damage = 1;

    private Transform tr;
    private float movementSpeed = 10f;

    private void Start ()
    {
        Destroy(gameObject, 3f);
    }

	private void FixedUpdate ()
    {
        if (tr == null)
            tr = transform;

        float moveDistance = movementSpeed * Time.fixedDeltaTime;
        CheckCollisions(moveDistance);

        tr.Translate(Vector3.forward * moveDistance, Space.Self);
    }

    private void CheckCollisions (float moveDistance)
    {
        Ray ray = new Ray(tr.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, moveDistance, collisionMask, QueryTriggerInteraction.Collide))
        {
            OnHitObject(hit);
        }
    }

    void OnHitObject(RaycastHit hit)
    {
        IDamageable damageableObject = hit.collider.GetComponent<IDamageable> ();

        if (damageableObject != null)
        {
            damageableObject.TakeHit(damage, hit);
        }
        
        GameObject.Destroy(gameObject);
    }

    public void SetSpeed (float speed)
    {
        movementSpeed = speed;
    }
}
