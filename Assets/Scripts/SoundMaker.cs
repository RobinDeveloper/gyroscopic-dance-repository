using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMaker : MonoBehaviour
{
    //[Range(150,250)]  //Creates a slider in the inspector
    [SerializeField] private float m_FrequencyPitch = 150;

    //[Range(100,300)]  //Creates a slider in the inspector
    [SerializeField] private float m_FrequencyYaw = 100;

    //[Range(200,400)]  //Creates a slider in the inspector
    [SerializeField] private float m_FrequencyRoll = 200;

    //[Range(150,250)]  //Creates a slider in the inspector
    [SerializeField] private float m_FrequencyX = 150;

    //[Range(100,300)]  //Creates a slider in the inspector
    [SerializeField] private float m_FrequencyY = 100;

    //[Range(200,400)]  //Creates a slider in the inspector
    [SerializeField] private float m_FrequencyZ = 400;


    [SerializeField] private float m_SampleRate = 44100;
    [SerializeField] private float m_WaveLengthInSeconds = 2.0f;

    private AudioSource m_AudioSource;
    private int m_TimeIndex = 0;

    private void Awake()
    {
        m_AudioSource = gameObject.AddComponent<AudioSource>();
        m_AudioSource.playOnAwake = false;
        m_AudioSource.spatialBlend = 0; //force 2D sound
        m_AudioSource.Stop(); //avoids audiosource from starting to play automatically
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StartSoundMaker();
        }
    }

    public void StopSoundMaker()
    {
        m_AudioSource.Stop();
    }
    
    public void StartSoundMaker()
    {
        Debug.Log("StartSoundMaker");
        if(!m_AudioSource.isPlaying)
        {
            m_TimeIndex = 0;  //resets timer before playing sound
            m_AudioSource.Play();
        }
        else
        {
            m_AudioSource.Stop();
        }
    }

    private void OnAudioFilterRead(float[] _data, int _channels)
    {
        for (int i = 0; i < _data.Length; i += _channels)
        {
            _data[i] = CreateSine(m_TimeIndex, m_FrequencyPitch, m_SampleRate);
            _data[i] += CreateSine(m_TimeIndex, m_FrequencyYaw, m_SampleRate);
            _data[i] += CreateSine(m_TimeIndex, m_FrequencyRoll, m_SampleRate);

            if (_channels == 2)
            {
                _data[i + 1] = CreateSine(m_TimeIndex, m_FrequencyX, m_SampleRate);
                _data[i + 1] += CreateSine(m_TimeIndex, m_FrequencyY, m_SampleRate);
                _data[i + 1] += CreateSine(m_TimeIndex, m_FrequencyZ, m_SampleRate);
            }

            m_TimeIndex++;

            //if timeIndex gets too big, reset it to 0
            if (m_TimeIndex >= (m_SampleRate * m_WaveLengthInSeconds))
            {
                m_TimeIndex = 0;
            }
        }
    }

    //Creates a sinewave
    private float CreateSine(int _timeIndex, float _frequency, float _sampleRate)
    {
        return Mathf.Sin(2 * Mathf.PI * _timeIndex * _frequency / _sampleRate);
    }

    public void UpdateYPRXYZ(Vector3 _yawPitchRoll, Vector3 _accelerationXYZ)
    {
        //normalizeValues between the correct values and apply them

        //m_FrequencyPitch = _yawPitchRoll.y;
        //m_FrequencyRoll = _yawPitchRoll.z;
        //m_FrequencyYaw = _yawPitchRoll.x;
//
        //m_FrequencyX = _accelerationXYZ.x;
        //m_FrequencyY = _accelerationXYZ.y;
        //m_FrequencyZ = _accelerationXYZ.z;

        m_FrequencyPitch = NormalizeValue(-2f,2f,150f,250f,_yawPitchRoll.y);
        m_FrequencyRoll = NormalizeValue(-2f,2f,200f,400f,_yawPitchRoll.z);
        m_FrequencyYaw = NormalizeValue(-2f,2f,100f,300f,_yawPitchRoll.x);

        m_FrequencyX = NormalizeValue(-4000f,4000f,150f, 250f, _accelerationXYZ.x);
        m_FrequencyY = NormalizeValue(-4000f,4000f,100f, 300f, _accelerationXYZ.y);
        m_FrequencyZ = NormalizeValue(-4000f,4000f,200f, 400f, _accelerationXYZ.z);
    }

    private float NormalizeValue(float _rmin, float _rmax, float _tmin, float _tmax, float _value)
    {
        //float x = (_max - _min) / (_max - _min) * (_value - _max) + _max;
        float x = (((_value - _rmin) * (_tmax - _tmin)) / (_rmax - _rmin)) + _tmin;
        //Debug.Log(x);
        return x;
    }
}