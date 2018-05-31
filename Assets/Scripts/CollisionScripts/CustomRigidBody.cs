﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CustomRigidBody : MonoBehaviour
{
    public bool UseGravity;
    public LayerMask CustomMask;
    Collider[] results;
    Vector3[] contactPoints;
    Collider collider;
    Vector3 newDir, midPoint;
    float dist1, dist2;

    void Start()
    {
        collider = GetComponent<Collider>();
        results = new Collider[10];
        contactPoints = new Vector3[results.Length];
    }

    void Update()
    {
        if (UseGravity)
            ApplyGravity();
    }

    void ApplyGravity()
    {
        Move(Physics.gravity.normalized, Physics.gravity.magnitude, Time.deltaTime, false);
    }

    /// <summary>
    /// Move the transform if no obstacle is detected.
    /// </summary>
    /// <param name="direction">Direction.</param>
    /// <param name="speed">Speed.</param>
    /// <param name="time">Time.</param>
    /// <param name="advanced">if false the transform will not move if a collision is detected. If true the transform will try to slide over the bounding box. Note that the advanced mode is less performant.</param>
    public void Move(Vector3 direction, float speed, float time, bool advanced)
    {
        Vector3 position = transform.position + direction * speed * time;

        if (advanced)
            transform.position = TryMove(position, direction, GetRightExtent(direction));
        else
            transform.position = TryMove(position, GetRightExtent(direction));
    }

    Vector3 TryMove(Vector3 position, float radius)
    {
        if (Physics.OverlapSphereNonAlloc(position, radius, results, CustomMask.value) >= 1)
            return transform.position;
        return position;
    }

    Vector3 TryMove(Vector3 position, Vector3 direction, float radius)
    {
        int num = Physics.OverlapSphereNonAlloc(position, radius, results, CustomMask.value);
        if (num >= 1)
        {
            newDir = Vector3.zero;
            midPoint = Vector3.zero;

            for (int i = 0; i < num; i++)
            {
                contactPoints[i] = results[i].ClosestPointOnBounds(position);
                direction = Vector3.zero;
                if (position != contactPoints[i])
                {
                    dist1 = (transform.position - position).sqrMagnitude;
                    dist2 = (transform.position - contactPoints[i]).sqrMagnitude;
                    direction = dist1 < dist2 ? (position - contactPoints[i]).normalized : (contactPoints[i] - position).normalized;
                }
                else
                    direction = (transform.position - contactPoints[i]).normalized;

                newDir += direction;
                midPoint += contactPoints[i];
            }
            midPoint /= (float)num;

            return TryMove(newDir != Vector3.zero ? midPoint + newDir.normalized * (radius + 0.1f) : transform.position, newDir, radius);
        }
        return position;
    }

    float GetRightExtent(Vector3 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y) && Mathf.Abs(direction.x) > Mathf.Abs(direction.z))
            return collider.bounds.extents.x;
        else if (Mathf.Abs(direction.y) > Mathf.Abs(direction.x) && Mathf.Abs(direction.y) > Mathf.Abs(direction.z))
            return collider.bounds.extents.y;
        else
            return collider.bounds.extents.z;
    }
}
