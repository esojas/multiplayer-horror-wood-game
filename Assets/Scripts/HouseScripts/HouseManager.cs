using UnityEngine;

public class HouseManager : MonoBehaviour
{
    private int completedHouses;

    private void OnEnable() => HouseProgress.OnHouseCompleted += HandleHouseCompleted;
    private void OnDisable() => HouseProgress.OnHouseCompleted -= HandleHouseCompleted;

    private void HandleHouseCompleted(HouseProgress house)
    {
        completedHouses++;
        Debug.Log($"{house.name} done. Total: {completedHouses}");
    }
}
