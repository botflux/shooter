using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Player))]
public class PlayerInput : MonoBehaviour
{
    private Player player;
    private Camera mainCamera;

    private void Awake ()
    {
        player = GetComponent<Player>();
        mainCamera = Camera.main;
    }

    private void Update ()
    {
        // Movement input
        Vector2 inputs = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        player.SetDirectonnalInputs(inputs);

        // Look input
        Plane ground = new Plane(Vector3.up, Vector3.zero);
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        float enter;
        if (ground.Raycast(ray, out enter))
        {
            Vector3 p = ray.GetPoint(enter);
            player.SetLookPoint(p);
        }
        
        // Weapon input
        if (Input.GetMouseButton(0))
        {
            player.Shoot();
        }   
    }
}
