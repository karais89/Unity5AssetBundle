using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AssetBundleManager : MonoBehaviour 
{
    private static AssetBundleManager _instance = null;

    // 매니저를 만듬에 있어서 Class 자체를 static으로 지정하여 사용할 수도 있습니다.
    // 저의 경우엔 static으로 자신을 하나 물고 있고, AssetBundleManager.Instance.Anythine()의 형식으로 접근하는 방식이 편해서
    // 이렇게 작성을 하였습니다. 클래스를 static 선언하시고 생성자에서 초기화해도 상관 없습니다.
    public static AssetBundleManager Instance
    {
        get
        {
            // 처음 호출시엔 인스턴스가 물려있을리 없으니 게임오브젝트에 에셋번들매니저 클래스를 붙이고 물려줍니다.
            if(_instance == null)
            {
                GameObject go = new GameObject("_AssetBundleManager");
                go.isStatic = true;
                _instance = go.AddComponent<AssetBundleManager>();

                GameObject go2 = new GameObject("_AssetBundleTimeManager");
                go2.transform.parent = go.transform;
                go2.transform.localPosition = Vector3.zero;
                timeManager = go2.AddComponent<AssetBundleTimeManager>();

                GameObject go3 = new GameObject("_AssetBundleLoadManager");
                go3.transform.parent = go.transform;
                go3.transform.localPosition = Vector3.zero;
                loadManager = go3.AddComponent<AssetLoadManager>();

                dicAssetBundle = new Dictionary<string, AssetBundleNode>();
                lstKeyName = new List<string>();
            }
            return _instance;             
        }
    }
    
    // 에셋 번들을 저장할 노드입니다. 에셋번들, url, 버전, unload시 bool 조건값을 저장합니다.
    public class AssetBundleNode
    {
        public AssetBundle assetBundle;
        public string url;
        public int version;
        public bool removeAll;

        public AssetBundleNode(string url, int version, bool removeAll, AssetBundle assetBundle)
        {
            this.url = url;
            this.version = version;
            this.removeAll = removeAll;
            this.assetBundle = assetBundle;
        }

        // 물려있는 에셋번들의 unload() 함수를 실행해줍니다.
        public void UnloadAssetBundle()
        {
            assetBundle.Unload(removeAll);
        }
    }

    private static AssetLoadManager loadManager;
    private static AssetBundleTimeManager timeManager;
    private static Dictionary<string, AssetBundleNode> dicAssetBundle;
    private static List<string> lstKeyName; // save keyName for remove All ABs    

    // 에셋 번들을 로드하여 Dic에 저장
    public IEnumerator LoadAssetBundle(string url, int version, bool removeAll, float lifeTime = 0.0f)
    {
        string keyName = MakeKeyName(url, version);
        // 이 부분 주석처리한 이유는 역시나 캐싱은 다운로드시에나 필요한 것이기 때문에..
        // 게임중에 캐싱해야될 일이 생긴다는 건 크래시나 프레임드랍 등에 노출된다는 상황입니다.
        // 캐싱가능할때까지 대기
        /*
        while(!Caching.ready)
        {
            yield return null;
        }*/

        // 중복 로드되는 상황을 막습니다.
        if(IsVersionAdded(url, version))
        {
            //yield return null;
        }
        else
        {
            // 로드 되지 않은 경우, 노드를 만들고 Dic에 저장.
            using(WWW www = WWW.LoadFromCacheOrDownload(url, version))
            {
                yield return www;
                if(www.error == null)
                {
                    AssetBundleNode node = new AssetBundleNode(url, version, removeAll, www.assetBundle);
                    dicAssetBundle.Add(keyName, node);
                    lstKeyName.Add(keyName);

                    // 로드한 에셋 번들에 생명주기 설정을 원하면 타임 매니저를 통하여 생명주기 설정을 합니다.
                    if (lifeTime > 0)
                    {
                        timeManager.SetLifeTime(keyName, lifeTime);
                    }
                }
            }
        }
    }

    // 에셋 로드매니저에게 에셋을 비동기 로드하라는 명령을 내리는 함수.
    public IEnumerator LoadAssetFromABAsync(string url, int version, string assetName, bool resetLife = true)
    {
        yield return StartCoroutine(loadManager.LoadAssetFromABAsync(url, version, assetName, resetLife));
    }

    // 일반 로드 명령. 위의 비동기 명령을 쓰는 걸 추천. 에셋 번들, 에셋 로드시에 크기가 커지면 크래쉬 위험성이 생긴다고 합니다.
    public void LoadAssetFromAB(string url, int version, string assetName, bool resetLife = true)
    {
        loadManager.LoadAssetFromAB(url, version, assetName, resetLife);
    }

    // 로딩된 에셋을 가져옵니다.
    public object GetLoadedAsset(string url, int version, string assetName, bool removeAfter = true)
    {
        object obj = loadManager.GetAsset(url, version, assetName, removeAfter);
        return obj;
    }

    // 위의 LoadAssetBundle() 함수를 통하여 불러온 에셋 번들을 리턴 시키는 함수.
    public AssetBundle GetAssetBundle(string url, int version)
    {
        string keyName = MakeKeyName(url, version);
        return dicAssetBundle[keyName].assetBundle;
    }

    // 받아온 url, version 정보로 저장된 에셋번들이 있는지 확인
    public bool IsVersionAdded(string url, int version)
    {
        string keyName = MakeKeyName(url, version);
        return dicAssetBundle.ContainsKey(keyName);       
    }

    // url, version 정보를 받아와 keyName을 만듭니다.
    public string MakeKeyName(string url, int version)
    {
        return url + version;
    }

    // 에셋 번들을 unload 시키는 함수입니다. 반드시 Dic에서 Remove 시키기 전에 에셋 번들을 unload 시킵시다.
    public void RemoveAssetBundle(string url, int version)
    {
        string keyName = MakeKeyName(url, version);
        RemoveAssetBundle(keyName);
    }

    // 위의 함수와 같은 함수입니다. 
    public void RemoveAssetBundle(string keyName)
    {
        if (dicAssetBundle.ContainsKey(keyName))
        {
            // 중요한 부분입니다. AssetBundle.Unload(true)일시에 불러왔던 기존 에셋들이 삭제되기에
            // 로드매니저에 물려있던 부분들도 날려줍니다
            if(dicAssetBundle[keyName].removeAll == true)
            {
                loadManager.RemoveIncludedAssets(keyName);
            }
            // 생명주기를 가지고 있다면 생명주기도 삭제해줘야 합니다.
            // 그렇지 않다면 이후에 타임매니저 쪽에서 데이터를 참조했을 때 key error, null 참조 등에 노출됩니다.
            if(timeManager.IsHaveLife(keyName))
            {
                timeManager.RemoveLifeTime(keyName);
            }
            dicAssetBundle[keyName].UnloadAssetBundle();
            dicAssetBundle.Remove(keyName);
            lstKeyName.Remove(keyName);
        }
    }

    // 현재 저장되어 있는 에셋번들의 갯수를 리턴
    public int GetNumberOfABs()
    {
        return dicAssetBundle.Count;
    }

    // 저장되어 있는 모든 에셋 번들을 Unload 시킵니다.
    public void RemoveAllAssetBundles()
    {
        for(int i = 0; i < dicAssetBundle.Count; i++)
        {
            dicAssetBundle[lstKeyName[i]].UnloadAssetBundle();
            // 이 함수를 실행하면 로드매니저에는 unloaad 옵션이 false인 에셋 번들만이 남습니다.
            if(dicAssetBundle[lstKeyName[i]].removeAll == true)
            {
                loadManager.RemoveIncludedAssets(lstKeyName[i]);
            }
        }
        dicAssetBundle.Clear();
        lstKeyName.Clear();
        timeManager.RemoveAllLifeTime();
    }

    // 에셋 로드 매니저를 다 날려버리는 함수입니다.
    public void RemoveAllAssets()
    {
        loadManager.RemoveAllAssets();
    }
}
