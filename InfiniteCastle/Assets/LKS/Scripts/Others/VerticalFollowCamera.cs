using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalFollowCamera : MonoBehaviour
{
    private Transform target;
    private float minY = 0;
    private float maxY = float.PositiveInfinity;

    private void Awake()
    {
        target = GameObject.FindObjectOfType<PlayerController>().transform;
    }

    private void Update()
    {
        if (target != null)
            //transform.position = new Vector3(0, Mathf.Clamp(target.position.y, minY, maxY), -10);
            transform.position = Vector3.Lerp(transform.position,
                new Vector3(0, Mathf.Clamp(target.position.y, minY, maxY), -10), 0.1f);
    }
}
