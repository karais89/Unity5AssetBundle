using UnityEngine;
using System.Collections;

public class BundleLoad : MonoBehaviour 
{
    // 번들을 받을 서버의 url(주소)
    public string bundleURL;
    // 번들의 version
    public int version;

	// Use this for initialization
	void Start () {
        StartCoroutine(LoadAssetBundle());
    }

    IEnumerator LoadAssetBundle()
    {
        /*
         * cache 폴더에 AssetBundle을 담아야 하므로 캐싱 시스템이 준비될때까지 기다립니다.
         * 저희는 이 BundleLoad 함수를 이용하여 다운로드후에 바로 데이터를 로딩하여 값을 확인하기에 캐싱 함수를 corution으로 돌립니다.
         * 실질적으로 앱의 시작시에 서버와 연동하여 AssetBundle을 비교할때나 Cacheing이 필요할 뿐
         * 게임 안에서는 이미 시작시에 서버와 연동하여 에셋번들을 전부 최신버전으로 업데이트 하였을 것이기에
         * caching에 대한 부분은 따로 작성하실 필요는 없습니다.
         */
        while(!Caching.ready)
        {
            yield return null;
        }

        // 역시나 불러와줍니다.
        WWW www = WWW.LoadFromCacheOrDownload(bundleURL, version);
        yield return www;

        // 다운로드된 www에 물려있는 assetbundle을 Assetbundle 자료형으로 참조합니다.
        AssetBundle bundle = www.assetBundle;
        for(int i = 0; i < 3; i ++)
        {
            // 참조한 assetbundle에 비동기 형식으로 에셋을 불러와줍니다.
            AssetBundleRequest request = bundle.LoadAssetAsync("Cube " + (i + 1), typeof(GameObject));
            yield return request;

            // 밑의 코드로도 불러올 수 있습니다. 차이는 위의 Async가 붙는 코드의 경우 request에 불러와야 하며 비동기 형식
            // 이므로 메인 스레드가 멈추는 것을 방지할 수 있습니다. 대신 코드가 조금 더 무겁겠지요.
            // GameObject obj = bundle.LoadAsset("Cube " + (i + 1)) as GameObject;

            GameObject obj = request.asset as GameObject;
            GameObject temp = Instantiate(obj) as GameObject;
            temp.transform.position = new Vector3(-5 + (i * 5), 0, 0);
        }

        // 꼭 번들을 unload 해주셔야만 합니다. 메모리 소모 복수 인스턴스 방지 등등을 위해.
        bundle.Unload(false);
        
        // 웹서버 연결 끊기
        www.Dispose();
    }	
}
