using System.Collections.Generic;
using UnityEngine;

public class EffectChangeMesh : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    private MeshFilter meshFilter;

    private int sortingOrder = 10;

    private void Awake()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        meshFilter = GetComponentInChildren<MeshFilter>();

        if (meshRenderer != null)
            meshRenderer.enabled = false;
    }

    public void ShowFan(Vector2 center, Vector2 direction, float radius, float angle, int segments = 30)
    {
        if (meshRenderer != null) meshRenderer.enabled = true;

        transform.position = center;
        transform.rotation = Quaternion.FromToRotation(Vector2.right, direction.normalized);
        transform.localScale = Vector3.one;

        Mesh mesh = new Mesh();
        List<Vector3> vertices = new List<Vector3> { Vector3.zero };
        List<int> triangles = new List<int>();

        for (int i = 0; i <= segments; i++)
        {
            float t = i / (float)segments;
            float currentAngle = -angle + (angle * 2f) * t;
            float rad = currentAngle * Mathf.Deg2Rad;

            Vector3 point = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f) * radius * 0.5f;
            vertices.Add(point);

            if (i > 0)
            {
                triangles.Add(0);
                triangles.Add(i);
                triangles.Add(i + 1);
            }
        }
        meshRenderer.material.color = new Color(1f, 0f, 0f, 0.5f);
        meshRenderer.sortingOrder = sortingOrder;
        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        if (meshFilter != null)
            meshFilter.mesh = mesh;
    }

    public void Hide()
    {
        meshRenderer.enabled = false;
    }
}
