using UnityEngine;
using System.Collections.Generic;

public class HouseSpawner : MonoBehaviour
{
    [Header("References")]
    public GameObject housePrefab;
    public Transform roadPlane;

    [Header("Spacing")]
    public float spacing = 20f;

    [Header("Event Settings")]
    [Tooltip("Cada cuantas filas se prende un evento. 1 = todas, 3 = cada 3 filas")]
    public int rowInterval = 3;

    private List<(GameObject left, GameObject right)> rows = new();
    private int currentRow = -1;

    void Start()
    {
        Spawn();
        // Empezar en la primera fila que corresponde al intervalo
        currentRow = rowInterval - 1;
        ActivateRow(currentRow);
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

            var left = Instantiate(housePrefab, new Vector3(posX, posY, z), rotA, transform);
            var right = Instantiate(housePrefab, new Vector3(mirrorX, posY, z), rotB, transform);

            SetHouse(left, false, false); // izquierda = interrogation
            SetHouse(right, false, true);  // derecha   = admiration
            rows.Add((left, right));
            i++;
        }
    }

    void ActivateRow(int index)
    {
        if (index < 0 || index >= rows.Count) return;
        SetHouse(rows[index].left, true, false); // izquierda: interrogation
        SetHouse(rows[index].right, true, true);  // derecha:   admiration
    }

    void DeactivateRow(int index)
    {
        if (index < 0 || index >= rows.Count) return;
        SetHouse(rows[index].left, false, false);
        SetHouse(rows[index].right, false, true);
    }

    // isAdmiration: true = mostrar admiration, false = mostrar interrogation
    void SetHouse(GameObject house, bool on, bool isAdmiration)
    {
        Transform adm = house.transform.Find("ADMIRATION_PNG_0");
        Transform intr = house.transform.Find("INTERRROGATION_PNG_0");
        Transform park = house.transform.Find("ParkingZone");

        if (adm != null) adm.GetComponent<SpriteRenderer>().enabled = on && isAdmiration;
        if (intr != null) intr.GetComponent<SpriteRenderer>().enabled = on && !isAdmiration;

        if (park != null)
        {
            park.GetComponent<SpriteRenderer>().enabled = on;
            if (on)
            {
                var t = park.GetComponent<ParkingTrigger>() ?? park.gameObject.AddComponent<ParkingTrigger>();
                t.Init(this);
            }
        }
    }

    public void OnTruckEntered()
    {
        DeactivateRow(currentRow);
        currentRow += rowInterval;
        ActivateRow(currentRow);
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