using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//CREIT: https://forum.unity.com/threads/problem-with-my-ocean-waves-mesh-deformation.293309/
public class WaveScript : MonoBehaviour
{
    public float waveHeight = 10.0f;
    public float speed = 1.0f;
    public float waveLength = 1.0f;
    public float noiseStrength = 4.0f;
    public float noiseWalk = 1.0f;
    public bool diagonalWaves = false;

    private Vector3[] baseHeight;
    private Vector3[] vertices;
    private Mesh mesh;

    void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        if (baseHeight == null)
        {
            baseHeight = mesh.vertices;
        }
    }

    void Update()
    {
        if (vertices == null)
        {
            vertices = new Vector3[baseHeight.Length];
        }

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 vertex = baseHeight[i];
            if (diagonalWaves)
            {
                vertex.y += Mathf.Sin(Time.time * speed + baseHeight[i].x * waveLength + baseHeight[i].y * waveLength + baseHeight[i].z * waveLength) * waveHeight;
            }
            else
            {
                vertex.y += Mathf.Sin(Time.time * speed + baseHeight[i].x * waveLength + baseHeight[i].y * waveLength) * waveHeight;
            }
            vertex.y += Mathf.PerlinNoise(baseHeight[i].x + noiseWalk, baseHeight[i].y + Mathf.Sin(Time.time * 0.1f)) * noiseStrength;
            vertices[i] = vertex;
        }
        mesh.vertices = vertices;
        mesh.RecalculateNormals();
    }
}
