using UnityEngine;
using System.Collections.Generic; // Necesario para usar List

public class TopeSpawner : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject topePrefab;
    public MeshRenderer calleRenderer;

    [Header("Configuración")]
    [Range(1, 100)] public int cantidadTopes = 10;
    public float margenInicial = 20f;

    [Tooltip("Distancia mínima para que no se junten 3 o más")]
    public float distanciaMinima = 5f;

    void Start()
    {
        if (topePrefab == null || calleRenderer == null) return;
        Spawn();
    }

    void Spawn()
    {
        float inicioZ = calleRenderer.bounds.max.z - margenInicial;
        float finZ = calleRenderer.bounds.min.z;

        List<float> posicionesZ = new List<float>();

        int intentos = 0; // Para evitar bucles infinitos
        int creados = 0;

        while (creados < cantidadTopes && intentos < 200)
        {
            float randomZ = Random.Range(finZ, inicioZ);

            // Validar si hay demasiados topes cerca de esta posición
            if (EsPosicionValida(randomZ, posicionesZ))
            {
                posicionesZ.Add(randomZ);

                Vector3 posicion = new Vector3(
                    topePrefab.transform.position.x,
                    topePrefab.transform.position.y,
                    randomZ
                );

                GameObject copia = Instantiate(topePrefab, posicion, topePrefab.transform.rotation);
                copia.transform.SetParent(this.transform);
                creados++;
            }
            intentos++;
        }
    }

    bool EsPosicionValida(float nuevaZ, List<float> existentes)
    {
        int cercanos = 0;
        foreach (float z in existentes)
        {
            // Si la distancia es menor a la mínima, contamos cuántos hay
            if (Mathf.Abs(nuevaZ - z) < distanciaMinima)
            {
                cercanos++;
            }
        }
        // Retorna verdadero solo si hay menos de 2 topes cerca (permite parejas, no tríos)
        return cercanos < 2;
    }
}