using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AssetLoadManager : MonoBehaviour 
{
    private static AssetBundleManager abManager;
    private static AssetBundleTimeManager timeManager;
    private static Dictionary<string, Dictionary<string, object>> dicAsset;

    void Awake()
    {
        dicAsset = new Dictionary<string, Dictionary<string, object>>();
        abManager = transform.parent.GetComponent<AssetBundleManager>();
        timeManager = transform.parent.FindChild("_AssetBundleTimeManager").GetComponent<AssetBundleTimeManager>();
    }

    // 에셋번들로부터 에셋을 로드
    public void LoadAssetFromAB(string url, int version, string assetName, bool resetLife)
    {
        string keyName = abManager.MakeKeyName(url, version);
        // 에셋 번들 매니저에 에셋번들이 로딩되어 있지 않거나 에셋이 이미 로드되어 있다면 작업을 수행하지 않습니다.
        if (!abManager.IsVersionAdded(url, version) || IsAssetLoaded(url, version, assetName))
        {
            //yield return null;
        }
        else
        {
            // 에셋번들 로딩, 에셋 아직 안불러온 상태
            AssetBundle ab = abManager.GetAssetBundle(url, version);
            // 생명주기 리셋을 원할시 타임매니저에서 리셋
            if(resetLife && timeManager.IsHaveLife(keyName))
            {
                timeManager.ResetLifeTime(keyName);
            }
            LoadAsset(url, version, ab, assetName);
        }
    }

    // 에셋 번들 로딩
    private void LoadAsset(string url, int version, AssetBundle AB, string assetName)
    {
        string keyName = abManager.MakeKeyName(url, version);
        if(!dicAsset.ContainsKey(keyName))
        {
            Dictionary<string, object> ab = new Dictionary<string, object>();
            dicAsset.Add(keyName, ab);
        }
        if(!dicAsset[keyName].ContainsKey(assetName))
        {
            object obj = AB.LoadAsset(assetName);
            dicAsset[keyName].Add(assetName, obj);
        }
    }
    
    // 위의 함수와 비슷하나 비동기 로딩입니다.
    public IEnumerator LoadAssetFromABAsync(string url, int version, string assetName, bool resetLife)
    {
        string keyName = abManager.MakeKeyName(url, version);
        if(!abManager.IsVersionAdded(url, version) || IsAssetLoaded(url, version, assetName))
        {
            //yield return null;
        }
        else
        {
            AssetBundle ab = abManager.GetAssetBundle(url, version);
            if (resetLife && timeManager.IsHaveLife(keyName))
            {
                timeManager.ResetLifeTime(keyName);
            }
            // 코루틴 함수를 실행하며 이 때 이 함수가 끝날때까지 기다립니다.
            yield return StartCoroutine(LoadAssetAsync(url, version, ab, assetName));
        }
    }

    private IEnumerator LoadAssetAsync(string url, int version, AssetBundle AB, string assetName)
    {
        string keyName = abManager.MakeKeyName(url, version);
        if(!dicAsset.ContainsKey(keyName))
        {
            Dictionary<string, object> ab = new Dictionary<string, object>();
            dicAsset.Add(keyName, ab);
        }
        if(!dicAsset[keyName].ContainsKey(assetName))
        {
            // 전에 로딩부에서 살펴보았던 Request를 이용한 함수입니다. 비동기로 실행
            AssetBundleRequest abReq = AB.LoadAssetAsync(assetName);
            yield return abReq;
            if(abReq.asset != null)
            {
                dicAsset[keyName].Add(assetName, abReq.asset);
            }
        }
    }
    
    // 불러와진 에셋을 외부로 리턴. 마지막 bool 값은 리턴후 에셋을 지우는지 여부
    public object GetAsset(string url, int version, string assetName, bool remove = true)
    {
        string keyName = abManager.MakeKeyName(url, version);
        object obj = null;
        if(abManager.IsVersionAdded(url, version) && IsAssetLoaded(url, version, assetName))
        {
            obj = dicAsset[keyName][assetName];
            if(remove)
            {
                dicAsset[keyName].Remove(assetName);
            }
        }
        return obj;
    }

    // 에셋이 로딩되었는지 확인
    private bool IsAssetLoaded(string url, int version, string assetName)
    {
        string keyName = abManager.MakeKeyName(url, version);
        if(dicAsset.ContainsKey(keyName))
        {
            // 있다면 그 키의 value에서 다시 asset이 로딩되었는지 확인
            return dicAsset[keyName].ContainsKey(assetName);
        }
        return false;
    }

    // 에셋을 Dic에서 삭제하는 함수
    public bool RemoveAsset(string url, int version, string assetName)
    {
        string keyName = abManager.MakeKeyName(url, version);
        if(IsAssetLoaded(url, version, assetName))
        {
            dicAsset[keyName].Remove(assetName);
            return true;
        }
        return false;
    }

    // 포함된 에셋들을 삭제하는 함수
    // 이 함수는 Dic의 키로 검사하여 그 에셋 번들에 해당하는 노드 전체를 날리는 함수입니다.
    // 에셋번들 unload(bool)시에 괄호 안의 bool 값이 true이면 에셋번들에서 불러왔던 에셋까지도 전부 삭제하기 때문에
    // 이 경우 참조하고 있는 dic의 저장값들이 null참조나 key error에 빠질 수가 있어 이 함수를 통하여 삭제해줍니다.
    public void RemoveIncludedAssets(string keyName)
    {
        if(dicAsset.ContainsKey(keyName))
        {
            dicAsset.Remove(keyName);
        }
    }

    // dic을 통째로 날리는 함수
    public void RemoveAllAssets()
    {
        dicAsset.Clear();
    }
}
