using System.Collections.Generic;
using UnityEngine;

public class InfiniteRoadSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject roadSegmentPrefab;
    public GameObject[] housePrefabs;
    public GameObject parkingPrefab;
    public GameObject topePrefab;
    public GameObject npcPrefab;

    [Header("Referencia")]
    public Transform player;

    [Header("Configuracion de Generacion")]
    public float rowLength = 40f;
    public int visibleRows = 8;
    public int bufferRows = 3;

    [Header("Casas")]
    public float houseMargin = 8f;
    public float parkingMargin = 2f;
    public float houseSpacingZ = 5f;
    [Range(0f, 1f)] public float interactiveChance = 0.4f;

    [Header("NPC")]
    public Vector3 npcOffset = new Vector3(-1f, 1f, 0f);

    [Header("Topes")]
    [Range(0f, 1f)] public float topeChance = 0.2f;

    private LinkedList<GameObject> activeRows = new LinkedList<GameObject>();
    private int nextRowIndex;
    private int lastPlayerRow;
    private HouseEvents lastInteractiveHouse;
    private float roadHalfWidth;
    private float houseLengthZ;

    void Start()
    {
        CalculateRoadWidth();
        CalculateHouseLength();

        // rowLength minimo = tamaño de la casa + houseSpacingZ
        if (rowLength < houseLengthZ + houseSpacingZ)
        {
            rowLength = houseLengthZ + houseSpacingZ;
            Debug.Log($"[InfiniteRoad] rowLength ajustado a {rowLength:F1} (casa={houseLengthZ:F1} + gap={houseSpacingZ:F1})");
        }

        float playerZ = player.position.z;
        lastPlayerRow = GetRowIndex(playerZ);
        nextRowIndex = lastPlayerRow;

        for (int i = 0; i < visibleRows; i++)
        {
            SpawnRow();
        }
    }

    void Update()
    {
        int currentPlayerRow = GetRowIndex(player.position.z);

        if (currentPlayerRow != lastPlayerRow)
        {
            int rowsAdvanced = currentPlayerRow - lastPlayerRow;
            lastPlayerRow = currentPlayerRow;

            for (int i = 0; i < rowsAdvanced; i++)
            {
                SpawnRow();
            }

            CleanupRows();
        }
    }

    void CalculateRoadWidth()
    {
        if (roadSegmentPrefab == null) { roadHalfWidth = 5f; return; }

        MeshFilter mf = roadSegmentPrefab.GetComponent<MeshFilter>();
        if (mf != null && mf.sharedMesh != null)
        {
            Bounds b = mf.sharedMesh.bounds;
            roadHalfWidth = b.extents.x * roadSegmentPrefab.transform.localScale.x;
        }
        else
        {
            Renderer r = roadSegmentPrefab.GetComponent<Renderer>();
            if (r != null)
                roadHalfWidth = r.bounds.extents.x;
            else
                roadHalfWidth = 5f;
        }

        Debug.Log($"[InfiniteRoad] roadHalfWidth calculado = {roadHalfWidth:F2}");
    }

    void CalculateHouseLength()
    {
        if (housePrefabs == null || housePrefabs.Length == 0) { houseLengthZ = 10f; return; }

        GameObject prefab = housePrefabs[0];
        MeshFilter mf = prefab.GetComponentInChildren<MeshFilter>();
        if (mf != null && mf.sharedMesh != null)
        {
            houseLengthZ = mf.sharedMesh.bounds.size.z * prefab.transform.localScale.z;
        }
        else
        {
            Renderer r = prefab.GetComponentInChildren<Renderer>();
            if (r != null)
                houseLengthZ = r.bounds.size.z;
            else
                houseLengthZ = 10f;
        }

        Debug.Log($"[InfiniteRoad] houseLengthZ calculado = {houseLengthZ:F2}");
    }

    int GetRowIndex(float z)
    {
        // El camion avanza en Z negativo, asi que convertimos a indice positivo creciente
        return Mathf.FloorToInt(-z / rowLength);
    }

    void SpawnRow()
    {
        float zPos = -nextRowIndex * rowLength;

        GameObject rowParent = new GameObject("Row_" + nextRowIndex);
        rowParent.transform.position = new Vector3(0f, 0f, zPos);

        // Segmento de calle
        if (roadSegmentPrefab != null)
        {
            GameObject road = Instantiate(roadSegmentPrefab, new Vector3(0f, 0f, zPos), Quaternion.identity, rowParent.transform);
            road.name = "RoadSegment";
        }

        // Casa izquierda
        SpawnHouseSide(rowParent.transform, zPos, -1f);

        // Casa derecha
        SpawnHouseSide(rowParent.transform, zPos, 1f);

        // Tope
        if (topePrefab != null && Random.value < topeChance)
        {
            float topeZ = zPos + Random.Range(-rowLength * 0.3f, rowLength * 0.3f);
            Instantiate(topePrefab, new Vector3(0f, topePrefab.transform.position.y, topeZ), topePrefab.transform.rotation, rowParent.transform);
        }

        activeRows.AddLast(rowParent);
        nextRowIndex++;
    }

    void SpawnHouseSide(Transform parent, float zPos, float side)
    {
        // Elegir prefab de casa aleatorio
        if (housePrefabs == null || housePrefabs.Length == 0) return;

        GameObject housePrefab = housePrefabs[Random.Range(0, housePrefabs.Length)];
        float houseX = side * (roadHalfWidth + houseMargin);

        // Rotar la casa para que mire hacia la calle
        Quaternion houseRot = side > 0
            ? Quaternion.Euler(0f, 180f, 0f)
            : Quaternion.Euler(0f, 0f, 0f);

        // Offset en Z para separar casas del mismo lado
        float houseZ = zPos - (side > 0 ? 0f : houseSpacingZ);

        GameObject house = Instantiate(housePrefab, new Vector3(houseX, 0f, houseZ), houseRot, parent);
        house.name = side > 0 ? "House_Right" : "House_Left";

        Debug.Log($"[InfiniteRoad] {house.name} pos=({houseX:F1}, 0, {houseZ:F1}) | roadHalfWidth={roadHalfWidth:F1} | rowLength={rowLength}");

        bool isInteractive = Random.value < interactiveChance;

        if (isInteractive)
        {
            // Estacionamiento: en el centro de la casa, entre la calle y la casa
            if (parkingPrefab != null)
            {
                Renderer houseRenderer = house.GetComponentInChildren<Renderer>();
                float parkZ = houseRenderer != null ? houseRenderer.bounds.center.z : houseZ;
                float parkX = side * (roadHalfWidth + parkingMargin);
                Quaternion parkRot = Quaternion.Euler(90f, 0f, 0f);
                GameObject parking = Instantiate(parkingPrefab, new Vector3(parkX, 0.01f, parkZ), parkRot, parent);
                Debug.Log($"[InfiniteRoad] Parking pos=({parkX:F1}, 0.01, {parkZ:F1}) | houseCenter={parkZ:F1}");
                parking.name = "Parking";

                // NPC en la misma posicion que el estacionamiento
                if (npcPrefab != null)
                {
                    float npcX = side * (roadHalfWidth + parkingMargin) + (side * npcOffset.x);
                    GameObject npc = Instantiate(npcPrefab, new Vector3(npcX, npcOffset.y, parkZ + npcOffset.z), Quaternion.identity, parent);
                    npc.name = "NPC";
                }
            }

            // Configurar HouseEvents si el prefab lo tiene
            HouseEvents houseEvents = house.GetComponent<HouseEvents>();
            if (houseEvents != null)
            {
                houseEvents.SetActive(true);

                // Conectar con la casa interactiva anterior
                if (lastInteractiveHouse != null)
                {
                    lastInteractiveHouse.nextHouse = houseEvents;
                }
                lastInteractiveHouse = houseEvents;
            }
        }
        else
        {
            // Casa decorativa: desactivar HouseEvents si existe
            HouseEvents houseEvents = house.GetComponent<HouseEvents>();
            if (houseEvents != null)
            {
                houseEvents.SetActive(false);
                houseEvents.enabled = false;
            }
        }
    }

    void CleanupRows()
    {
        int playerRow = GetRowIndex(player.position.z);

        while (activeRows.Count > 0)
        {
            GameObject oldest = activeRows.First.Value;
            int oldestRow = GetRowIndex(oldest.transform.position.z);

            // Si la fila esta demasiado atras del jugador, destruirla
            if (playerRow - oldestRow > bufferRows)
            {
                activeRows.RemoveFirst();
                Destroy(oldest);
            }
            else
            {
                break;
            }
        }
    }
}
