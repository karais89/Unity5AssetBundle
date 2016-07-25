using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AssetBundleTimeManager : MonoBehaviour 
{
    // maxTime(생명주기)와 currentTime(진행시간)으로 이루어져 있습니다.
    public class LifeTime
    {
        private float _maxTime;
        public float currentTime;

        public LifeTime(float maxTimeIn)
        {
            _maxTime = maxTimeIn;
            SetCurrentToMax();
        }
        
        // 현재 진행시간을 생명주기 수치로 되돌립니다.
        public void SetCurrentToMax()
        {
            currentTime = _maxTime;
        }

        // 현재 진행시간에서 frame 주기만큼을 빼줍니다.
        public void SubtractElapsedTime()
        {
            currentTime -= Time.deltaTime;
        }
    }

    private static AssetBundleManager abManager;
    private static Dictionary<string, LifeTime> dicLifeTime;
    private static List<string> lstKeyName;

    // 여기서는 Instance을 따로 만들지 않습니다. 에셋 번들 매니저의 Instance을 처음 불러오면
    // 알아서 이 AssetBundleTimeManager까지 만들어 주기 때문에 Awake()를 이용하여
    // 에셋 번들 매니저에서 AddComponent<AssetBundleTimeManager>()를 실행한 다음 여기로 들어와 초기화합니다.
    void Awake()
    {
        dicLifeTime = new Dictionary<string, LifeTime>();
        lstKeyName = new List<string>();
        abManager = transform.parent.GetComponent<AssetBundleManager>();
    }
    	
	void Update () 
    {
        SubtractLifeTimes();	
	}

    // 생명주기를 감소시키는 함수입니다.
    private void SubtractLifeTimes()
    {
        // 키 이름과 생명주기 클래스로 이뤄진 Dic의 요소 갯수만큼 반복하여 각 요소들의 생명주기를 뺍니다.
        for(int i =0; i < dicLifeTime.Count; i++)
        {
            // 혹시 --i가 된다면 dictionary의 Count값이 1 작아진 상태이기 때문에 key error을 볼 수 있습니다. 방지용.
            if(dicLifeTime.ContainsKey(lstKeyName[i]))
            {
                LifeTime lifeTime = dicLifeTime[lstKeyName[i]];
                lifeTime.SubtractElapsedTime();
                if (lifeTime.currentTime <= 0.0f)
                {
                    RemoveAssetBundle(lstKeyName[i]);
                    --i;
                }
            }
        }
    }
    
    // 특정 에셋 번들이 생명주기를 가지고 있는지를 리턴
    public bool IsHaveLife(string keyName)
    {
        return lstKeyName.Contains(keyName);
    }

    // 에셋 번들 매니저에서 실행하는 함수. keyName과 생명주기를 받아와 Dic에 저장.
    public void SetLifeTime(string keyName, float lifeTime)
    {
        LifeTime time = new LifeTime(lifeTime);
        dicLifeTime.Add(keyName, time);
        lstKeyName.Add(keyName);
    }

    // 생명주기를 초기화합니다.
    public void ResetLifeTime(string keyName)
    {
        dicLifeTime[keyName].SetCurrentToMax();
    }

    // 생명주기가 있는 에셋번들의 생명주기를 remove 합니다.
    public void RemoveLifeTime(string keyName)
    {
        if(lstKeyName.Contains(keyName))
        {
            dicLifeTime.Remove(keyName);
            lstKeyName.Remove(keyName);
        }
    }
    
    // 모든 생명주기 remove
    public void RemoveAllLifeTime()
    {
        dicLifeTime.Clear();
        lstKeyName.Clear();
    }

    // 에셋 번들 unload 에셋 번들 매니저를 통하여 리셋시키고 dic정보도 remove
    private void RemoveAssetBundle(string keyName)
    {
        abManager.RemoveAssetBundle(keyName);
        dicLifeTime.Remove(keyName);
        lstKeyName.Remove(keyName);
    }
}
