using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{

    public float dampTime = 0.15f;
    public Vector2 m_MinBounds;
    public Vector2 m_MaxBounds;
    public Transform FollowTarget;
    public Vector3 TargetOffset;
    public float MoveSpeed = 2f;

    private Transform _myTransform;

    private void Start()
    {
        // Cache camera transform
        _myTransform = transform;
    }

    public void SetTarget(Transform aTransform)
    {
        FollowTarget = aTransform;
    }

    private void LateUpdate()
    {
        if (FollowTarget != null)
            _myTransform.position = Vector3.Lerp(_myTransform.position, FollowTarget.position + TargetOffset, MoveSpeed * Time.deltaTime);
    }


    private void LockPositionBounds()
    {
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, m_MinBounds.x, m_MaxBounds.x), transform.position.y, Mathf.Clamp(transform.position.x, m_MinBounds.y, m_MaxBounds.y));
    }
}