using UnityEngine;

public class TopeSpawner : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject topePrefab;
    public MeshRenderer calleRenderer;

    [Header("Configuración")]
    [Range(1, 100)] public int cantidadTopes = 10;
    public float margenInicial = 20f;

    void Start()
    {
        if (topePrefab == null || calleRenderer == null) return;
        Spawn();
    }

    void Spawn()
    {
        // 1. Calculamos el área de la calle en Z
        float inicioZ = calleRenderer.bounds.max.z - margenInicial;
        float finZ = calleRenderer.bounds.min.z;

        for (int i = 0; i < cantidadTopes; i++)
        {
            // 2. Posición aleatoria SOLO en Z
            float randomZ = Random.Range(finZ, inicioZ);

            // 3. Mantener X e Y exactas del prefab original
            Vector3 posicion = new Vector3(
                topePrefab.transform.position.x,
                topePrefab.transform.position.y,
                randomZ
            );

            // 4. Instanciar con rotación original y meter en el contenedor
            GameObject copia = Instantiate(topePrefab, posicion, topePrefab.transform.rotation);
            copia.transform.SetParent(this.transform);
        }
    }
}