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