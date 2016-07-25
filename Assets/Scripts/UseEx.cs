using UnityEngine;
using System.Collections;

public class UseEx : MonoBehaviour 
{
    public string url = "http://localhost/AssetBundles/assetbundle_0";
    public int version = 0;

    IEnumerator Start()
    {
        // 2초 간의 생명 주기를 가짐
        yield return StartCoroutine(AssetBundleManager.Instance.LoadAssetBundle(url, version, false, 2.0f));
        
        Debug.Log(AssetBundleManager.Instance.IsVersionAdded(url, version));
        Debug.Log(AssetBundleManager.Instance.GetNumberOfABs());
        AssetBundle ab = AssetBundleManager.Instance.GetAssetBundle(url, version);
        GameObject gObj = ab.LoadAsset("Cube 1") as GameObject;
        Instantiate(gObj, Vector3.zero, Quaternion.identity);        
        GameObject gObj1 = ab.LoadAsset("Cube 2") as GameObject;
        Instantiate(gObj1, Vector3.zero, Quaternion.identity);
        
        // 정말 remove 되는지 보기 위하여 에셋 번들 매니저에서 remove 함수를 주석처리
        //AssetBundleManager.Instance.RemoveAllAssetBundles();

        // 1초 대기
        yield return new WaitForSeconds(1.0f);
        Debug.Log(AssetBundleManager.Instance.IsVersionAdded(url, version));

        // 1.5초 대기 총 2.5초 remove
        yield return new WaitForSeconds(1.5f);
        Debug.Log(AssetBundleManager.Instance.IsVersionAdded(url, version));
        
        Caching.CleanCache();
    }
}
