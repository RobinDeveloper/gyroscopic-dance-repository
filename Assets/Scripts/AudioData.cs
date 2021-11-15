using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioData : MonoBehaviour
{
    private AudioSource m_AudioSource;
    private float[] m_AudioSamples = new float[512]; 
    private float[] m_FrequencyBands = new float[8]; 

    public float[] GetAudioSamples => m_AudioSamples;
    public float[] GetFrequencybands => m_FrequencyBands;
    
    private void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        GetSpectrumData();
        MakeFrequencyBands();
    }

    private void GetSpectrumData()
    {
        m_AudioSource.GetSpectrumData(m_AudioSamples, 0, FFTWindow.Blackman);
    }

    private void MakeFrequencyBands()
    {
        int count = 0;
        for (int i = 0; i < m_FrequencyBands.Length; i++)
        {
            float median = 0;
            int spectrumCount = (int) Math.Pow(2, i) * 2;

            if (i == 7) //make the 512
                spectrumCount += 2;

            for (int j = 0; j < spectrumCount; j++)
            {
                median += m_AudioSamples[count] * (count + 1);
                count++;
            }

            median /= count;
            m_FrequencyBands[i] = median;
        }
    }
}
