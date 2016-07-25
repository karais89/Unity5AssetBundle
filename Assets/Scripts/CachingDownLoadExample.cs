using UnityEngine;
using System.Collections;

public class CachingDownLoadExample : MonoBehaviour 
{
    // 번들을 받을 서버의 URL(주소)
    public string bundleURL;
    // 번들의 Version
    public int version;
    
	void Start () 
    {
        StartCoroutine(DownloadAndCache());	
	}
	
    IEnumerator DownloadAndCache()
    {
        // cache 폴더에 assetbundle을 담아야 하므로 캐싱 시스템이 준비될때까지 기다립니다.
        while(!Caching.ready)
        {
            yield return null;
        }

        // 새 WWW 변수를 만들고 WWW.LoadFromCacheOrDownload(URL, 버전) 함수를 통하여 에셋번들을 다운로드하여 cache 폴더에 씁니다.
        // WWW.LoadFromCacheOrDownload 함수를 사용하면 우선 cache 폴더에 같은 버전의 에셋 번들이 있는지 확인하여
        // 있는 경우 호출하고 없는 경우 url로 부터 다운로드합니다.
        WWW www = WWW.LoadFromCacheOrDownload(bundleURL, version);
        yield return www;

        if(www.error != null)
        {
            Debug.Log("Fail :(");
        }
    }
}
