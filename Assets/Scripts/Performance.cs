using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Performance : MonoBehaviour
{
    [Header("Object Refrences")]
    [SerializeField] private SoundMaker m_SoundMaker;
    [SerializeField] private SampleUserPolling_ReadWrite m_SerialWriter;
    [SerializeField] private Camera m_Camera;
    
    [Header("Preformance Values")]
    [SerializeField] private float m_PreformanceInitialTime;
    [SerializeField] private Color[] m_Colours = new Color[2];
    
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
        m_PrefomanceTime = m_PreformanceInitialTime;
        InitializeCamera();
        InitializeSerialWriter();
        InitializeSoundMaker();
    }

    private void InitializeCamera()
    {
        m_Camera.backgroundColor = m_Colours[0];
    }

    private void InitializeSerialWriter()
    {
        m_SerialWriter.SendKey('f');
    }

    private void InitializeSoundMaker()
    {
        m_SoundMaker.StartSoundMaker();
    }

    private void UpdatePreformance()
    {
        UpdateCamera();
        UpdateTime();
    }

    private void UpdateCamera()
    {
        m_Camera.backgroundColor = Color.Lerp(m_Colours[0], m_Colours[2], Time.deltaTime);
    }

    private void UpdateTime()
    {
        m_PrefomanceTime -= Time.deltaTime;

        if (m_PrefomanceTime <= 0)
            StopPreformance();
    }

    private void StopPreformance()
    {
        m_PreformanceStarted = false;
        m_PrefomanceTime = m_PreformanceInitialTime;
    }
}
