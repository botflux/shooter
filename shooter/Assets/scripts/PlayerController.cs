using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Rigidbody), typeof(Collider))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private Transform tr;
    private Vector3 velocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        tr = transform;
    }

    private void FixedUpdate ()
    {
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }

    public void Move (Vector2 velocity)
    {
        this.velocity = new Vector3(velocity.x, 0f, velocity.y);
    }

    public void LookAt (Vector3 position)
    {
        tr.LookAt(new Vector3(position.x, tr.position.y, position.z));
    }
}
