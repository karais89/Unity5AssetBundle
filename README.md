# README #

## Unity5 AssertBundle Example ##

http://dongin2009.blog.me/

## 유니티 5.0의 에셋 번들에 대해 공부해보자 part 1. 탐구편 ##

1. 에셋 번들은 무엇일까?

Asset + Bundle

Asset = 프리팹, 텍스쳐, 재질, 스크립트, 쉐이더등의 자원들..

Bundle = 묶음

에셋번들은? 에셋의 묶음

2. 에셋 번들의 쓰임과 장점

에셋 번들은 에셋의 묶음이고 원하는 에셋들을 따로 묶어 자장할 수 있습니다.

에셋 번들은 서버에 따로 저장해 놓고 www 통신을 통하여 다운 받을 수 있습니다.

그렇기 때문에 몇 가지 확실한 장점이 있습니다.

**앱스토어에 등록할 최초배포버전의 용량을 줄일 수 있다.**

에셋 번들은 배포시에 포함 시킬 필요가 없습니다. 서버에서 다운받아 사용할 수 있기 때문에 배포버전을 경량화 시킬 수 있습니다.

어느 조사에 의하면 용량이 50MB 이하일때 페이지의 노출 수 대비 다운로드 수가 가장 많다고 합니다.

**게임의 업데이트에 용이합니다.**

에셋번들은 번들별로 CRC, 버전 등의 정보를 가지고 있습니다.

그렇기 때문에 업데이트시에 버전 정보가 다른 에셋 번들들만 업데이트해주면 되니 업데이트에도 유용하게 사용할 수 있습니다.

[유니티 에셋번들 메뉴얼](http://docs.unity3d.com/Manual/AssetBundlesIntro.html)

## 유니티 5.0의 에셋 번들에 대해 공부해보자 part 2. 생성과 빌드편 ##

인스펙터 창 우측 하단의 AssetLabels로 에셋번들을 설정 할 수 있다.

중앙의 긴 리스트 상자에는 에셋번들의 이름을 입력합니다. 가장 중요하고 가장 많이 쓰입니다.

우측의 리스트 상자에는 변형 에셋번들을 만드는 칸입니다. 

꼬리표는 말그래도 이 에셋의 종류를 표시할 수 있는 기능입니다. (캐릭터인지, 이펙트인지, 혹은 터레인인지 하는 꼬리표를 복수로 체크하여 표시)

하지만 결정적으로 번들은 이 꼬리표를 공유하지 않습니다. 번들에는 기록되지 않는 것 같습니다.

에셋번들의 특이점

1. 에셋번들에 등록하는 이름에는 대문자를 포함할 수 없습니다. 대문자를 입력해도 전부 소문자로 자동 변경됩니다.
2. 에셋번들을 등록할때 이름을 폴더이름/에셋이름과 같이 등록하면 리스트 박스에 하위요소로 분류되면 에셋번들 빌드시에도 에셋번들이 해당 폴더로 위치합니다.

에셋번들에 포함할때의 방법 두가지

1. 등록하려는 프리팹들이 포함된 폴더 자체를 assetbundle에 포함합니다. 이 경우엔 폴더 하위 오브젝트들이 전부 등록됩니다.
2. 오브젝트들만 따로 클릭하여 assetbundle에 포함시켜도 됩니다. 다중 선택을 통해 한번에 등록할 수 있습니다.

유니티 4.x와의 차이점과 함수 이용팁

1. 원래 4.x 버전까지는 BuildPipeline.BuildAssetBunlde(메인에섯, 에셋배열[], "경로", BuildAssetBunldeOption, BuildTarget) 등을 요구했습니다.

   5.x에서 메인 에셋은 0번째로 등록된 에셋으로 설정되게끔 바뀌었고 에셋 배열은 더이상 Selection 기능을 이용하여 수집하지 않아도 자동적으로 수집됩니다.

2. 위의 4.x 함수의 매개변수 중 BuildAssetBundleOptions에 중요한 기능 하나가 있었습니다. 

   BuildAssetBundleOptions.CollectDependencies 이라는 기능인데 이것을 옵션으로 넣어주지 않으면 오브젝트가 제대로 저장되지 않았습니다.

   Prefab은 텍스쳐와 메테리얼, 스크립트 등과 유동적으로 상호작용하기 때문에 이 옵션을 넣어줘야만 프리팹에 종속된 자원들을 함께 저장했습니다.

   하지만 Unity 5.x 버전에서는 이 옵션이 Default 값으로 들어가 자동적으로 기능하게 되었습니다.

3. 만일 안드로이드 등 다른 플랫폼에세 사용할 시에 BuildAssetBundles() 함수의 오버로드 함수 중 BuildTarget을 요구하는 함수가 있습니다.

   이 함수에 BuildTarget target = BuildTarget.Android 이런식으로 옵션을 넣어주면 에러를 잡을 수 있습니다. 중요!!

 

### What is this repository for? ###

* Quick summary
* Version
* [Learn Markdown](https://bitbucket.org/tutorials/markdowndemo)

### How do I get set up? ###

* Summary of set up
* Configuration
* Dependencies
* Database configuration
* How to run tests
* Deployment instructions

### Contribution guidelines ###

* Writing tests
* Code review
* Other guidelines

### Who do I talk to? ###

* Repo owner or admin
* Other community or team contact