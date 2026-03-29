using UnityEngine;

public class ParkingTrigger : MonoBehaviour
{
    private ObstacleSpawner spawner;
    private int rowIndex;

    public void Init(ObstacleSpawner s, int index)
    {
        spawner = s;
        rowIndex = index;
    }
}