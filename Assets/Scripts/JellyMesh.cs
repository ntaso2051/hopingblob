using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyMesh : MonoBehaviour
{
    public float Intensity;
    public float Mass;
    public float stiffness;
    public float damping;

    private Mesh OriginalMesh, MeshClone;
    private MeshRenderer renderer;
    private JellyVertex[] jv;
    private Vector3[] vertexArray;
    // Start is called before the first frame update
    void Start()
    {
        OriginalMesh = this.GetComponent<MeshFilter>().sharedMesh;
        MeshClone = Instantiate(OriginalMesh);
        this.GetComponent<MeshFilter>().sharedMesh = MeshClone;
        renderer = this.GetComponent<MeshRenderer>();

        jv = new JellyVertex[MeshClone.vertices.Length];
        for (int i = 0; i < MeshClone.vertices.Length; i++)
        {
            jv[i] = new JellyVertex(i, transform.TransformPoint(MeshClone.vertices[i]));
        }
    }

    private void FixedUpdate()
    {
        vertexArray = OriginalMesh.vertices;
        for (int i = 0; i < jv.Length; i++)
        {
            Vector3 target = transform.TransformPoint(vertexArray[jv[i].id]);
            float intensity = (1 - (renderer.bounds.max.y - target.y) / renderer.bounds.size.y) * Intensity;
            jv[i].Shake(target, Mass, stiffness, damping);
            target = transform.InverseTransformPoint(jv[i].position);
            vertexArray[jv[i].id] = Vector3.Lerp(vertexArray[jv[i].id], target, intensity);
        }
        MeshClone.vertices = vertexArray;
    }

    // Update is called once per frame
    void Update()
    {

    }



    public class JellyVertex
    {
        public int id;
        public Vector3 position, velocity, force;
        public JellyVertex(int _id, Vector3 _pos)
        {
            this.id = _id;
            this.position = _pos;
        }
        public void Shake(Vector3 target, float m, float s, float d)
        {
            force = (target - position) * s;
            velocity = (velocity + force / m) * d;
            position += velocity;
            if ((velocity + force + force / m).magnitude < 0.001f)
            {
                position = target;
            }
        }
    }
}
