using UnityEngine;
using System.Collections;

public class Manager : MonoBehaviour 
{
    private static Manager _instance;
    private int number = 0;
    public static Manager Instance
    {
        get
        {
            if(_instance == null)
            {
                GameObject clone = new GameObject("_Manager");
                clone.name = "_Manager";
                _instance = clone.AddComponent<Manager>();
            }
            return _instance;
        }
    }
    public void Hi()
    {
        StartCoroutine(coroutine());
    }
    IEnumerator coroutine()
    {
        int i = number;
        number++;
        Debug.Log(i);
        Debug.Log(i + " before " + Time.realtimeSinceStartup);
        yield return new WaitForSeconds(1.0f);
        Debug.Log(i + " after " + Time.realtimeSinceStartup);
    }
}
