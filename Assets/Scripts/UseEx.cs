using UnityEngine;
using System.Collections;

public class UseEx : MonoBehaviour 
{
    public string url = "http://localhost/AssetBundles/assetbundle_0";
    public int version = 0;

    IEnumerator Start()
    {
        // LoadAssetBundle(url, version, unload매개변수)를 실행하고 이 함수의 실행이 끝날 때까지 기다립니다.
        yield return StartCoroutine(AssetBundleManager.Instance.LoadAssetBundle(url, version, false));
        
        // 에셋번들이 불러와 졌는지 확인.
        Debug.Log(AssetBundleManager.Instance.IsVersionAdded(url, version));
        
        // 에셋번들이 몇개 저장되어 있는지 확인.
        Debug.Log(AssetBundleManager.Instance.GetNumberOfABs());

        // GetAssetBundle(url, version) 함수를 통해 에셋번들을 리턴받아 참조
        AssetBundle ab = AssetBundleManager.Instance.GetAssetBundle(url, version);

        // 참조한 에셋 번들에서 Cube 1 에셋을 불러왔습니다.
        GameObject gObj = ab.LoadAsset("Cube 1") as GameObject;
        Instantiate(gObj, Vector3.zero, Quaternion.identity);

        // removeAllAssetBundles() 함수를 실행하여 Dic을 비웁니다.
        AssetBundleManager.Instance.RemoveAllAssetBundles();

        // unload 되었는지 확인. 0이 나와야 정상.
        Debug.Log( AssetBundleManager.Instance.GetNumberOfABs() );

        // 컴퓨터의 임의의 Cache 폴더에 용량이 쌓이는 것을 방지하기 위해 테스트시에는 Caching.CleanCache()를 써줍니다.
        Caching.CleanCache();
    }
}
