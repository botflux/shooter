using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(PlayerController), typeof(GunController))]
public class Player : LivingEntity
{
    private PlayerController playerController;
    private GunController gunController;

    public float playerSpeed = 1.0f;

    private void Awake ()
    {
        playerController = GetComponent<PlayerController>();
        gunController = GetComponent<GunController>();
    }

    protected override void Start()
    {
        base.Start();
    }

    public void SetDirectonnalInputs (Vector2 inputs)
    {
        playerController.Move(inputs.normalized * playerSpeed);
    }

    public void SetLookPoint (Vector3 lookPoint)
    {
        playerController.LookAt(lookPoint);
    }

    public void Shoot ()
    {
        gunController.Shoot();
    }
}
