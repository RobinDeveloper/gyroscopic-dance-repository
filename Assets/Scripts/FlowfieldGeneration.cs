using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class FlowfieldGeneration : MonoBehaviour
{
    [SerializeField] private Vector3Int m_Grid;
    [SerializeField] private float m_Increment;
    [SerializeField] private Vector3 m_Offset;
    [SerializeField] private Vector3 m_OffsetSpeed;
    [SerializeField] private float m_CellSize;

    [SerializeField] private GameObject m_ParticleGameObject;
    [FormerlySerializedAs("m_AmountOfParticles")] public int AmountOfParticles;
    
    [FormerlySerializedAs("m_ParticleScale")] public float ParticleScale;
    [FormerlySerializedAs("m_ParticleMoventSpeed")] public float ParticleMoventSpeed;
    [FormerlySerializedAs("m_ParticleRotationSpeed")] public float ParticleRotationSpeed;
    [SerializeField] private float m_SpawnRadius;
    
    [FormerlySerializedAs("m_Particles")] [HideInInspector] public List<FlowFieldParticle> Particles;

    private Vector3[,,] m_FlowFieldDirection;
    
    private FastNoise m_FastNoise;

    private void Awake()
    {
        Particles = new List<FlowFieldParticle>();
        m_FlowFieldDirection = new Vector3[m_Grid.x,m_Grid.y,m_Grid.z];
        m_FastNoise = new FastNoise();

        for (int i = 0; i < AmountOfParticles; i++)
        {
            int attemptPlacingParticle = 0;

            while (attemptPlacingParticle < 100)
            {
                Vector3 position = transform.position;
                Vector3 pos = new Vector3(
                    Random.Range(position.x, position.x + m_Grid.x * m_CellSize),
                    Random.Range(position.y, position.y + m_Grid.y * m_CellSize),
                    Random.Range(position.z, position.z + m_Grid.z * m_CellSize));

                bool isInside = IsParticleInsideotherParticle(pos);

                if (!isInside)
                {
                    GameObject go = Instantiate(m_ParticleGameObject).gameObject;
                    go.transform.position = pos;
                    go.transform.parent = this.transform;
                    go.transform.localScale = new Vector3(ParticleScale, ParticleScale, ParticleScale);
                    Particles.Add(go.GetComponent<FlowFieldParticle>());
                    break;
                }
                else
                {
                    attemptPlacingParticle++;
                }
            }
        }
    }

    private void Update()
    {
        CalculateFlowField();
        ParticleBehaviour();
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position + new Vector3((m_Grid.x * m_CellSize) * 0.5f,(m_Grid.y * m_CellSize) * 0.5f,(m_Grid.z * m_CellSize) * 0.5f),
            new Vector3(m_Grid.x * m_CellSize, m_Grid.y * m_CellSize, m_Grid.z * m_CellSize));
    }

    private void CalculateFlowField()
    {
        m_Offset = m_Offset + (m_OffsetSpeed * Time.deltaTime);
        
        float xOffset = 0;
        for (int x = 0; x < m_Grid.x; x++)
        {
            float yOffset = 0;
            for (int y = 0; y < m_Grid.y; y++)
            {
                float zOffset = 0;
                for (int z = 0; z < m_Grid.z; z++)
                {
                    float noise = m_FastNoise.GetSimplex(xOffset + m_Offset.x, yOffset + m_Offset.y, zOffset + m_Offset.z) + 1;
                    Vector3 noiseDirection = new Vector3(Mathf.Cos(noise * Mathf.PI),Mathf.Sin(noise * Mathf.PI), Mathf.Cos(noise * Mathf.PI));
                    m_FlowFieldDirection[x, y, z] = Vector3.Normalize(noiseDirection);
                    
                    zOffset += m_Increment;
                }
                yOffset += m_Increment;
            }
            xOffset += m_Increment;
        }
    }

    private void ParticleBehaviour()
    {
        foreach (FlowFieldParticle particle in Particles)
        {
            if (particle.transform.position.x > transform.position.x + (m_Grid.x * m_CellSize))
                particle.transform.position = new Vector3(transform.position.x, particle.transform.position.y, particle.transform.position.z);
            else if(particle.transform.position.x < transform.position.x)
                particle.transform.position = new Vector3(transform.position.x + (m_Grid.x * m_CellSize), particle.transform.position.y, particle.transform.position.z);
            
            if (particle.transform.position.y > transform.position.y + (m_Grid.y * m_CellSize))
                particle.transform.position = new Vector3(particle.transform.position.x, transform.position.y, particle.transform.position.z);
            else if(particle.transform.position.y < transform.position.y)
                particle.transform.position = new Vector3(particle.transform.position.y, transform.position.y + (m_Grid.y * m_CellSize), particle.transform.position.z);
            
            if (particle.transform.position.z > transform.position.z + (m_Grid.z * m_CellSize))
                particle.transform.position = new Vector3(particle.transform.position.x, particle.transform.position.y, transform.position.z);
            else if(particle.transform.position.z < transform.position.z)
                particle.transform.position = new Vector3(particle.transform.position.y, particle.transform.position.y , transform.position.z + (m_Grid.z * m_CellSize));


            Vector3Int pos = new Vector3Int(Mathf.FloorToInt(Mathf.Clamp((particle.transform.position.x - transform.position.x) / m_CellSize, 0 , m_Grid.x - 1)),
                                            Mathf.FloorToInt(Mathf.Clamp((particle.transform.position.y - transform.position.y) / m_CellSize, 0 , m_Grid.y - 1)),
                                            Mathf.FloorToInt(Mathf.Clamp((particle.transform.position.z - transform.position.z) / m_CellSize, 0 , m_Grid.z - 1)));
            
            particle.ApplyRotation(m_FlowFieldDirection[pos.x,pos.y,pos.z], ParticleRotationSpeed);
            particle.MovementSpeed = ParticleMoventSpeed;
            particle.transform.localScale = new Vector3(ParticleScale, ParticleScale, ParticleScale);
        }
    }

    private bool IsParticleInsideotherParticle(Vector3 _position)
    {
        return Particles.Any(t => Vector3.Distance(_position, t.transform.position) < m_SpawnRadius);
    }
}
