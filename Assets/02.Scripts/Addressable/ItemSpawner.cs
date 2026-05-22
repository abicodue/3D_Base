using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private ItemData itemData;

    private GameObject spawned;

    private IEnumerator Start()
    {
        if (itemData == null)
        {
            Debug.LogError("ItemData is null!");
            yield break;
        }

        if (itemData.worldPrefab == null)
        {
            Debug.LogError("worldPrefab is null!");
            yield break;
        }

        var handle = itemData.worldPrefab.InstantiateAsync(
            transform.position, Quaternion.identity);

        yield return handle;

        if (handle.Status == AsyncOperationStatus.Succeeded)
            spawned = handle.Result;
    }

    private void OnDestroy()
    {
        if (spawned != null)
            Addressables.ReleaseInstance(spawned);
    }
}
