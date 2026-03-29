using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("References")]
    public GameObject housePrefab;
    public Transform roadPlane;

    public Transform houseContainer;

    [Header("Spacing")]
    public float spacing = 20f;

    [Header("Configuracion")]
    [Tooltip("Separacion minima entre casas accionables (1-3)")]
    [Range(1, 3)]
    public int minGapBetweenActive = 1;
    public int maxActiveAtOnce = 2;

    // Nombres de los hijos del prefab
    const string ADM_NAME = "ADMIRATION_PNG_0";
    const string INTR_NAME = "INTERRROGATION_PNG_0";
    const string PARK_NAME = "ParkingZone";

    private enum HouseType { Admiration, Interrogation }

    private class HouseRow
    {
        public GameObject left;
        public GameObject right;
        public HouseType leftType;
        public HouseType rightType;
    }

    private List<HouseRow> rows = new();
    private List<int> activeRows = new(); // indices de filas accionables activas
    private int nextRowToActivate = 0;

    void Start()
    {
        transform.SetParent(transform);

        Spawn();
        // Activar las primeras filas accionables
        ActivateNext();
        ActivateNext();
    }

    void Spawn()
    {
        if (housePrefab == null) return;

        float roadZMin = GetRoadZMin();
        float roadCenterX = roadPlane != null ? roadPlane.position.x : 0f;
        float startZ = housePrefab.transform.position.z;
        float posY = housePrefab.transform.position.y;
        float posX = housePrefab.transform.position.x;
        float mirrorX = roadCenterX + (roadCenterX - posX);
        Quaternion rotA = housePrefab.transform.rotation;
        Quaternion rotB = Quaternion.Euler(0f, 0f, 0f);

        int i = 1;
        while (true)
        {
            float z = startZ - spacing * i;
            if (z < roadZMin) break;

            var row = new HouseRow
            {
                left = Instantiate(housePrefab, new Vector3(posX, posY, z), rotA, transform),
                right = Instantiate(housePrefab, new Vector3(mirrorX, posY, z), rotB, transform),
                leftType = Random.value > 0.5f ? HouseType.Admiration : HouseType.Interrogation,
                rightType = Random.value > 0.5f ? HouseType.Admiration : HouseType.Interrogation
            };

            // Inicializar todas apagadas
            ApplyHouseState(row.left, false, row.leftType);
            ApplyHouseState(row.right, false, row.rightType);

            rows.Add(row);
            i++;
        }

        Debug.Log($"[HouseSpawner] Filas generadas: {rows.Count}");
    }

    // Activa la siguiente fila disponible respetando el gap
    void ActivateNext()
    {
        if (nextRowToActivate >= rows.Count) return;

        int index = nextRowToActivate;  // guardar antes de modificar
        var row = rows[index];

        ApplyHouseState(row.left, true, row.leftType, index);  // pasar index
        ApplyHouseState(row.right, true, row.rightType, index);  // pasar index

        activeRows.Add(index);
        nextRowToActivate += Random.Range(minGapBetweenActive, 4);
    }

    void DeactivateRow(int index)
    {
        if (index < 0 || index >= rows.Count) return;
        var row = rows[index];
        ApplyHouseState(row.left, false, row.leftType);
        ApplyHouseState(row.right, false, row.rightType);
    }

    void ApplyHouseState(GameObject house, bool on, HouseType type, int rowIndex = -1)
    {
        Transform adm = house.transform.Find(ADM_NAME);
        Transform intr = house.transform.Find(INTR_NAME);
        Transform park = house.transform.Find(PARK_NAME);

        bool isAdmiration = type == HouseType.Admiration;

        if (adm != null) adm.GetComponent<SpriteRenderer>().enabled = on && isAdmiration;
        if (intr != null) intr.GetComponent<SpriteRenderer>().enabled = on && !isAdmiration;

        if (park != null)
        {
            var parkSprite = park.GetComponent<SpriteRenderer>();
            var parkCol = park.GetComponent<BoxCollider>();

            if (parkSprite != null) parkSprite.enabled = on && !isAdmiration;
            if (parkCol != null) parkCol.enabled = on && !isAdmiration;

            if (on && !isAdmiration && rowIndex >= 0)
            {
                var t = park.GetComponent<ParkingTrigger>()
                     ?? park.gameObject.AddComponent<ParkingTrigger>();
                t.Init(this, rowIndex);
            }
        }
    }

    // Llamado por ParkingTrigger cuando el camion estaciona
    public void OnTruckParked(int rowIndex)
    {
        DeactivateRow(rowIndex);
        activeRows.Remove(rowIndex);
        ActivateNext();
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
}