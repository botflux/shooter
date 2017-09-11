using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    private Transform tr;

    public SpriteRenderer dot;
    public LayerMask targetMask;
    public Color dotHighlightColor;
    private Color originalColor;

    private void Awake ()
    {
        tr = transform;
    }

    private void Start ()
    {
        Cursor.visible = false;
        originalColor = dot.color;
    }

    private void FixedUpdate()
    {
        tr.Rotate(Vector3.forward * 40 * Time.fixedDeltaTime);
    }

    public void DetectTargets (Ray ray)
    {
        if (Physics.Raycast(ray, 100, targetMask))
        {
            dot.color = dotHighlightColor;
        }
        else
        {
            dot.color = originalColor;
        }
    }
}
