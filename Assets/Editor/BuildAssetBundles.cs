/*
 * http://docs.unity3d.com/kr/current/Manual/BuildingAssetBundles.html
 */
using UnityEditor;

public class BuildAssetBundles
{
    // MenuItem을 사용하면 위의 메뉴창에 새로운 메뉴를 추가할 수 있습니다.
    // 아래의 코드에서는 Bundles 항목에 하위항목으로 Build AssetBundle항목을 추가하도록 합니다.
    [MenuItem ("Bundles/Build AssetBundles")]
    static void BuildAllAssetBundles ()
    {
        // BuildPipeLine 클래스의 클래스 함수인 BuildAssetBundles 함수입니다. 함수 그대로 에셋 번들을 만들어줍니다.
        // BuildAssetBunldes() 함수 안은 String 매개변수가 필요합니다. 이 매개변수는 빌드된 에셋 번들을 저장할 경로입니다.
        // 예를 들어 Assets폴더의 하위 폴더 AssetBundles 폴더에 저장하려면 "Assets/AssetBundles"라고 입력해야겠지요.
        BuildPipeline.BuildAssetBundles("AssetBundles");
    }
}