using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    private Transform tr;
    private float movementSpeed = 10f;

	void FixedUpdate ()
    {
        if (tr == null)
            tr = transform;

        tr.Translate(Vector3.forward * Time.fixedDeltaTime * movementSpeed, Space.Self);
    }

    public void SetSpeed (float speed)
    {
        movementSpeed = speed;
    }
}
