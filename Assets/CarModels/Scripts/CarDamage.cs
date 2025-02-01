using UnityEngine;

public class CarDamage : MonoBehaviour
{
    public float hits;
    public float maxhits;
    public GameObject CarSmoke;
    public float maxMoveDelta = 0.1f;
    public float maxCollisionStrength = 50.0f;
    public float YforceDamp = 0.1f;
    public float demolutionRange = 0.5f;
    public float impactDirManipulator = 0.0f;
    public MeshFilter[] MeshList;
    public AudioSource Crash;

    private MeshFilter[] meshfilters;
    private float sqrDemRange;

    private struct permaVertsColl
    {
        public Vector3[] permaVerts;
    }
    private permaVertsColl[] originalMeshData;

    void Start()
    {
        meshfilters = MeshList.Length > 0 ? MeshList : GetComponentsInChildren<MeshFilter>();
        sqrDemRange = demolutionRange * demolutionRange;
        LoadOriginalMeshData();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) Repair();
    }

    void LoadOriginalMeshData()
    {
        originalMeshData = new permaVertsColl[meshfilters.Length];
        for (int i = 0; i < meshfilters.Length; i++)
        {
            originalMeshData[i].permaVerts = meshfilters[i].mesh.vertices;
        }
    }

    void Repair()
    {
        for (int i = 0; i < meshfilters.Length; i++)
        {
            meshfilters[i].mesh.vertices = originalMeshData[i].permaVerts;
            meshfilters[i].mesh.RecalculateNormals();
            meshfilters[i].mesh.RecalculateBounds();
        }
        CarSmoke.SetActive(false);
        hits = 0;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude < maxCollisionStrength * 0.1f)
            return;

        Vector3 colRelVel = collision.relativeVelocity;
        colRelVel.y *= YforceDamp;

        Vector3 colPointToMe = transform.position - collision.contacts[0].point;

        float colStrength = colRelVel.magnitude * Vector3.Dot(collision.contacts[0].normal, colPointToMe.normalized);

        if (colStrength > 0.1f)
        {
            OnMeshForce(collision.contacts[0].point, Mathf.Clamp01(colStrength / maxCollisionStrength));
        }
    }

    public void OnMeshForce(Vector3 originPos, float force)
    {
        if (force <= 0f) return;

        Crash.Play();
        hits++;
        if (hits > maxhits)
        {
            CarSmoke.SetActive(true);
        }

        force = Mathf.Clamp01(force);

        foreach (var meshFilter in meshfilters)
        {
            Vector3[] verts = meshFilter.mesh.vertices;

            for (int i = 0; i < verts.Length; i++)
            {
                Vector3 scaledVert = Vector3.Scale(verts[i], transform.localScale);
                Vector3 vertWorldPos = meshFilter.transform.position + (meshFilter.transform.rotation * scaledVert);
                Vector3 originToMeDir = vertWorldPos - originPos;
                Vector3 flatVertToCenterDir = transform.position - vertWorldPos;
                flatVertToCenterDir.y = 0.0f;

                if (originToMeDir.sqrMagnitude < sqrDemRange)
                {
                    float dist = Mathf.Clamp01(originToMeDir.sqrMagnitude / sqrDemRange);
                    float moveDelta = force * (1.0f - dist) * maxMoveDelta;

                    Vector3 moveDir = Vector3.Slerp(originToMeDir, flatVertToCenterDir, impactDirManipulator).normalized * moveDelta;

                    verts[i] += Quaternion.Inverse(transform.rotation) * moveDir;
                }
            }

            meshFilter.mesh.vertices = verts;
            meshFilter.mesh.RecalculateBounds();
        }
    }
}