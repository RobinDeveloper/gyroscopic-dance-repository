using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Performance : MonoBehaviour
{
    [Header("Object Refrences")]
    [SerializeField] private SoundMaker m_SoundMaker;
    //[SerializeField] private SampleUserPolling_ReadWrite m_SerialWriter;
    [SerializeField] private Camera[] m_Cameras;
    [SerializeField] private GameObject m_CameraRotateObject;

    
    [Header("Preformance Values")]
    [SerializeField] private float m_PreformanceInitialTime;
    [SerializeField] private Color[] m_Colours = new Color[2];

    [SerializeField] private FlowFieldParticle[] m_Particles;
    
    private bool m_PreformanceStarted;
    private float m_PrefomanceTime;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !m_PreformanceStarted)
            StartPreformance();

        if (m_PreformanceStarted)
            UpdatePreformance();
    }

    private void StartPreformance()
    {
        Debug.Log("Start preformance");
        m_T = 0;
        m_PreformanceStarted = true;
        m_PrefomanceTime = m_PreformanceInitialTime;
        m_Particles = GameObject.FindObjectsOfType<FlowFieldParticle>();

        for (int i = 0; i < m_Particles.Length; i++)
        {
            m_Particles[i].T = 0;
            m_Particles[i].StartColour();
        }
        
        InitializeCamera();
        InitializeSerialWriter();
        InitializeSoundMaker();
    }

    private void InitializeCamera()
    {
        for (int i = 0; i < m_Cameras.Length; i++)
        {
            m_Cameras[i].backgroundColor = m_Colours[0];
        }
    }

    private void InitializeSerialWriter()
    {
        //m_SerialWriter.SendKey('f');
    }

    private void InitializeSoundMaker()
    {
        m_SoundMaker.StartSoundMaker();
        m_SoundMaker.StartSoundMaker();
    }

    private void UpdatePreformance()
    {
        UpdateCamera();
        UpdateParticles();
        UpdateTime();
    }

    private float m_T = 0;
    private void UpdateCamera()
    {
        m_T += Time.deltaTime / 5;

        for (int i = 0; i < m_Cameras.Length; i++)
        {
            m_Cameras[i].backgroundColor = Color.Lerp(m_Colours[0], m_Colours[1], m_T);
            m_Cameras[i].transform.RotateAround(m_CameraRotateObject.transform.position, Vector3.up, 20 * Time.deltaTime);
        }
    }

    private void UpdateParticles()
    {
        for (int i = 0; i < m_Particles.Length; i++)
        {
            m_Particles[i].UpdateColour();
        }
    }

    private void UpdateTime()
    {
        m_PrefomanceTime -= Time.deltaTime;

        if (m_PrefomanceTime <= 0)
            StopPreformance();
    }

    private void StopPreformance()
    {
        Debug.Log("StopPreformance");
        m_PreformanceStarted = false;
        for (int i = 0; i < m_Cameras.Length; i++)
        {
            m_Cameras[i].backgroundColor = m_Colours[0];
        }
        for (int i = 0; i < m_Particles.Length; i++)
        {
            m_Particles[i].StopColours();
        }
        m_SoundMaker.StartSoundMaker();
        m_PrefomanceTime = m_PreformanceInitialTime;
    }
}
