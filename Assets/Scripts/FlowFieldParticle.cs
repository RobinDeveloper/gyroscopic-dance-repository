using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowFieldParticle : MonoBehaviour
{
    [SerializeField] private float m_MovementSpeed;
    [SerializeField] private int m_AudioBand;

    public int AudioBand
    {
        get => m_AudioBand;
        set => m_AudioBand = value;
    }

    public float MovementSpeed
    {
        get => m_MovementSpeed;
        set => m_MovementSpeed = value; 
    }
    
    private void Update()
    {
        transform.position += transform.forward * m_MovementSpeed * Time.deltaTime;
    }

    public void ApplyRotation(Vector3 _rotation, float _rotateSpeed)
    {
        Quaternion targetRotation = Quaternion.LookRotation(_rotation.normalized);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotateSpeed * Time.deltaTime);
    }
}
