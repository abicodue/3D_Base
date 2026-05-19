using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressLoader : MonoBehaviour
{
    [SerializeField] private string address;

    private AsyncOperationHandle<GameObject> handle;
    private GameObject instance;

    private IEnumerator Start()
    {
        handle = Addressables.LoadAssetAsync<GameObject>(address);
        yield return handle;

        // 로드가 성공했으면 실행
        if (handle.Status == AsyncOperationStatus.Succeeded)
            Instantiate(handle.Result, transform.position, Quaternion.identity);
    }

    private void OnDestroy()
    {
        if (instance != null)
        {
            Destroy(instance);
        }

        if (handle.IsValid())
        {
            Addressables.Release(handle);
        }
    }

}
