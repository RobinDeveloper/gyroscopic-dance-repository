using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioFlowField : MonoBehaviour
{
    [SerializeField] private AudioData m_AudioData;
    
    private FlowfieldGeneration m_FlowField;
    
    [SerializeField] private Vector2 m_MoveSpeedMinMax;
    [SerializeField] private Vector2 m_RotateSpeedMinMax;

    [SerializeField] private Color[] m_Colors;
    

    private void Awake()
    {
        m_FlowField = GetComponent<FlowfieldGeneration>();

        int freqBands = 0;
        for (int i = 0; i < m_FlowField.Particles.Count; i++)
        {
            int limitedBand = freqBands % 8;
            m_FlowField.Particles[i].AudioBand = limitedBand;
            freqBands++;
            
            Material mat = m_FlowField.Particles[i].GetComponent<Renderer>().material;
            mat.color = m_Colors[m_FlowField.Particles[i].AudioBand];
            mat.SetColor("_EmissionColor", m_Colors[m_FlowField.Particles[i].AudioBand]);
        }
    }

    private void Update()
    {
        for (int i = 0; i < m_FlowField.Particles.Count; i++)
        {
            m_FlowField.ParticleMoventSpeed =
                Mathf.Lerp(m_MoveSpeedMinMax.x, m_MoveSpeedMinMax.y, m_AudioData.GetFrequencybands[m_FlowField.Particles[i].AudioBand] * 750);
            m_FlowField.ParticleRotationSpeed =
                Mathf.Lerp(m_RotateSpeedMinMax.x, m_RotateSpeedMinMax.y, m_AudioData.GetFrequencybands[m_FlowField.Particles[i].AudioBand] * 750);
            
            //float scale = ((m_AudioData.GetFrequencybands[m_FlowField.Particles[i].AudioBand] * 20) + 0.1f);

            //m_FlowField.ParticleScale= scale;
        }
    }
}
