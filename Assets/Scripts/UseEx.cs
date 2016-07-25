using UnityEngine;
using System.Collections;

public class UseEx : MonoBehaviour 
{
    public string url = "http://localhost/AssetBundles/assetbundle_0";
    public int version = 0;

    IEnumerator Start()
    {
        // 일단 에셋 번들을 로드합시다.
        yield return StartCoroutine(AssetBundleManager.Instance.LoadAssetBundle(url, version, false, 2.0f));

        // 중복로드 테스트용으로 같은걸 한번 더 로드해봤는데 걸러냅니다 ~
        yield return StartCoroutine(AssetBundleManager.Instance.LoadAssetBundle(url, version, false, 2.0f));
        
        // 로드 확인
        Debug.Log(AssetBundleManager.Instance.IsVersionAdded(url, version));
        
        // 중복 로드 테스트 확인용 1이 나와야 정상
        Debug.Log(AssetBundleManager.Instance.GetNumberOfABs());
        
        // 일반 로딩으로 cube 1 불러옵니다.
        AssetBundleManager.Instance.LoadAssetFromAB(url, version, "Cube 1");

        // 비동기로 Cube 2를 불러옵니다.
        yield return StartCoroutine(AssetBundleManager.Instance.LoadAssetFromABAsync(url, version, "Cube 2"));

        // Cube 1을 리턴받아 한개 생성
        GameObject gObj1 = AssetBundleManager.Instance.GetLoadedAsset(url, version, "Cube 1") as GameObject;
        Instantiate(gObj1);

        // Cube 2를 리턴받아 한개 생성
        GameObject gObj2 = AssetBundleManager.Instance.GetLoadedAsset(url, version, "Cube 2") as GameObject;
        Instantiate(gObj2);
        
        // 1초 대기
        yield return new WaitForSeconds(1.0f);
        Debug.Log(AssetBundleManager.Instance.IsVersionAdded(url, version));

        // 1.5초 대기 총 2.5초 remove
        yield return new WaitForSeconds(1.5f);
        Debug.Log(AssetBundleManager.Instance.IsVersionAdded(url, version));
        
        Caching.CleanCache();
    }
}
