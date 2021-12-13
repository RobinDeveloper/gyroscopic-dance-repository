using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowFieldParticle : MonoBehaviour
{
    [SerializeField] private float m_MovementSpeed;
    [SerializeField] private int m_AudioBand;

    private Color m_Color;
    private MeshRenderer m_MeshRenderer;
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

    private void Start()
    {
        m_MeshRenderer = GetComponent<MeshRenderer>();
        m_Color = m_MeshRenderer.material.color;
    }

    public void StartColour()
    {
        m_MeshRenderer.material.color = Color.black;
    }

    public void StopColours()
    {
        m_MeshRenderer.material.color = Color.black;
        ;
    }
    
    private float m_T = 0;
    
    public float T { get => m_T;
        set => m_T = value;
    }
    public void UpdateColour()
    {
        m_T += Time.deltaTime / 5;
        m_MeshRenderer.material.color = Color.Lerp(Color.black, m_Color, m_T);;
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
