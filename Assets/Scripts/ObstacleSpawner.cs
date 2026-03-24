using UnityEngine;

public class HouseSpawner : MonoBehaviour
{
    [Header("References")]
    public GameObject housePrefab;
    public Transform roadPlane;

    [Header("Spacing")]
    public float spacing = 20f;

    private float roadZMin;
    private float roadCenterX;

    void Start()
    {
        roadZMin = GetRoadZMin();
        roadCenterX = roadPlane != null ? roadPlane.position.x : 0f;
        Spawn();
    }

    float GetRoadZMin()
    {
        if (roadPlane == null) return -500f;

        MeshFilter mf = roadPlane.GetComponent<MeshFilter>();
        if (mf != null && mf.sharedMesh != null)
        {
            Vector3 wMin = roadPlane.TransformPoint(mf.sharedMesh.bounds.min);
            Vector3 wMax = roadPlane.TransformPoint(mf.sharedMesh.bounds.max);
            return Mathf.Min(wMin.z, wMax.z);
        }

        return roadPlane.position.z - roadPlane.localScale.z * 10f / 2f;
    }

    void Spawn()
    {
        if (housePrefab == null) { Debug.LogWarning("Asigna el prefab."); return; }

        float startZ = housePrefab.transform.position.z;
        float posY = housePrefab.transform.position.y;
        float posX = housePrefab.transform.position.x;

        // El lado opuesto es el reflejo en X respecto al centro del plano
        float mirrorX = roadCenterX + (roadCenterX - posX);

        Quaternion rotOriginal = housePrefab.transform.rotation;
        Quaternion rotMirror = Quaternion.Euler(0f, 0f, 0f);

        int i = 1;
        while (true)
        {
            float z = startZ - spacing * i;
            if (z < roadZMin) break;

            // Lado original
            Instantiate(housePrefab, new Vector3(posX, posY, z), rotOriginal);

            // Lado espejo con rotacion en 0
            Instantiate(housePrefab, new Vector3(mirrorX, posY, z), rotMirror);

            i++;
        }
    }
}