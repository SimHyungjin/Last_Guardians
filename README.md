# &emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;[LastGuardians]

### 목차
[게임개요](#게임-개요)<br>
[프로젝트개요](#프로젝트-개요)<br>
[팀원소개](#팀원소개)<br>
[프레임워크](#프레임워크)<br>
[플레이방법](#플레이-방법)<br>
[주요기술](#주요-기술)<br>
[주요기능](#주요-기능)<br>
[주요기술](#주요-기술)<br>
[트러블슈팅](#트러블-슈팅)<br>

<br>

## 게임개요
- 게임 제목 : 라스트 가디언스
- 장르 : 액션 타워 디펜스
- 플렛폼 : 모바일(안드로이드)

<br><br>
[목차로](#목차)<br>

## 프로젝트 개요
- 개발 환경 : Unity, C# , visual studio
- 개발 기간 : 2025.04.04~

<br><br>
[목차로](#목차)<br>

## 팀원소개
| 이름 | 직책 | 담당 |
|:---:|:---:|:---|
| 이근희 | 기획자 | - |
| 심형진 | 리드개발 | 플레이어, 인벤토리 시스템, 상점 시스템, 환경 장애물, 저장 시스템, 분석 툴 |
| 김영재 | 개발자 | 인게임 시스템, 멀리건 시스템, 몬스터 시스템, 환경 시스템, 튜토리얼 |
| 김동석 | 개발자 | 타워 시스템, 카드 시스템, 타워업그레이드 시스템, 데이터로드 시스템, 각종 자동화 툴 |
| 이건우 | 개발자 | 사운드 시스템, UI 시스템, 씬 이동 시스템, 보상 시스템, 데이터로드 시스템 |

<br><br>
[목차로](#목차)<br>

## 프레임워크

### 초기 구상안
<img src="https://velog.velcdn.com/images/kds5063/post/eadb6717-18b2-4f90-8602-25d1be7c535c/image.png" width="500" height="550"/>

### 최종 완성본
<img src="https://velog.velcdn.com/images/kds5063/post/ac47ba73-3e02-4010-845f-148d6cc26126/image.png" width="700" height="550"/>


기획서 : [기획서](https://veil-birthday-79f.notion.site/1c8b530245da80428dd3dd3645000f33)<br>
협업사이트 : [Figma](https://www.figma.com/board/SnORbIMoVQ7wOcAxD1JMgd/Untitled?node-id=1219-1160&t=1FFcQmC3UedSeaiZ-0)

<br><br>
[목차로](#목차)<br>

## 플레이 방법

- 조작법 : 모든 조작은 터치 조작입니다.

- 타이틀화면 -> 마을 -> 타워디펜스 게임 순으로 진행됩니다.

- 전초기지를 습격하는 마족들을 무찔러 중앙 게이트를 수호하라!

### 마을

마을에서 각종 강화(플레이어아이템, 타워업그레이드) 또는 보상수령을 할 수 있다.

### 타워 디팬스
- 몬스터들이 중앙목표물의 체력을 0으로 만들면 게임오버.
- 게임 시작시 초기카드들을 선택한다(속성타워 6개중 3개, 일반타워 4개중 2개)
- 초기 카드들 중 레벨업마다 나오는 선택지에서 카드를 골라 카드를 드래그하여 타워를 설치한다.
- 타워를 이용해 몬스터를 처치하고 몬스터가 드랍하는 경험치를 먹어 레벨업을 한다.

- 현상금 시스템을 이용하여 추가적인 자원을 획득한다.

- 플레이어의 행동
>1. 플레이어 이동
>    플레이어터치후 스와이프 하여 원하는 장소로 플레이어에게 이동명령을 내릴 수 있다.
>2. 타워 설치
>    타워 카드 선택 이후 드래그를 통해 원하는 위치에 타워를 설치 할 수 있다.
>3. 현상금시스템
>    현상금몬스터를 소환하여 추가 경험치를 획득해 성장을 가속한다.

<br><br>
[목차로](#목차)<br>


## 주요 기술

- Scriptable Object
- Singleton
- CustomizeEditor
- UnityWebRequest
- Newtonsoft.Json
- FSM
- InputSystem
- Factory Pattern
- Interface 기반 다형성
- DIP (Dependency Inversion Principle)
- RuleTile

<br><br>
[목차로](#목차)<br>

## 주요 기능

### &emsp;마을

#### &emsp;&emsp;아이템인벤토리
<img src="https://github.com/user-attachments/assets/a19d1ab1-9bbf-419e-a044-9f03db64f02e" width="800" height="400"/><br>

- 장비 강화와 판매/분해를 할수 있는 아이템 인벤토리

#### &emsp;&emsp;타워업그레이드
<img src="https://github.com/user-attachments/assets/1776f366-de0a-4ea5-a257-5f49c15f2356" width="800" height="400"/><br>

- 마스터리포인트를 사용하여 타워에 강력한 업그레이드를 줄 수 있는 업그레이드 시스템

#### &emsp;&emsp;방치보상

<img src="https://github.com/user-attachments/assets/2d859c73-4bb5-43e0-8bda-188ef21de5b3" width="800" height="400"/><br>

- 접속중, 미접속중 상관없이 쌓여가는 방치보상

<br>

### &emsp;타워 디팬스


#### &emsp;&emsp;멀리건(초기카드선택)


<img src="https://github.com/user-attachments/assets/83d16d1f-874b-4a20-9ddd-14b63ceb6ea5" width="800" height="400"/><br>

- 게임 중 사용할 타워들을 골라 시작하는 멀리건 시스템

#### &emsp;&emsp;타워설치

<img src="https://github.com/user-attachments/assets/919ea827-803d-4a0d-83fd-d28c6f45b1be" width="800" height="400"/><br>

- 손에 보유한 카드를 사용해 타워설치, 타워 조합

#### &emsp;&emsp;레벨업

<img src="https://github.com/user-attachments/assets/c1b4b830-b05d-4766-a89b-8c3a155ec380" width="800" height="400"/><br>

- 몬스터를 처치해 레벨업, 카드획득


#### &emsp;&emsp;계절/날씨 시스템

<br>
- 시간과 웨이브에 따른 다양한 효과를 가진 날씨 시스템

#### &emsp;&emsp;현상금소환


<img src="https://github.com/user-attachments/assets/91d5bb2a-bd0e-42a3-8b85-1d26de7c29e1" width="800" height="400"/><br>

- 추가보상을 제공하는 소환가능한 현상금몬스터

#### &emsp;&emsp;게임종료
 
 
<img src="https://github.com/user-attachments/assets/268c2e94-5a8c-46e1-8f9f-e52276977be6" width="800" height="400"/><br>

- 게임 종료시 클리어한 웨이브에따라 보상을 받을수있다.



<br><br> 
[목차로](#목차)<br>


## 기능 구현
<details><summary> 타워설치</summary>

<img src="https://github.com/user-attachments/assets/4531016c-300e-46d1-a6bc-d27a9c5f786b" width="400" height="500"><br>

InputSystem에서 현재 클릭위치 좌표를 받아온이후 wolrd좌표로 변환이후 스냅하여 해당 타일에 설치가능여부 판단(설치가능여부는 시작지점부터 끝지점까지 navmesh가 이어지는지)<br>
검사 이후 설치 가능이면 해당 좌표에 타워설치<br>

<img src="https://github.com/user-attachments/assets/41240dfa-c3ab-487f-9591-6c1abdc95041" width="500" height="370"><br>


타워는 TowerPrefab 하나로 구성<br>
BaseTower에 자식들인 타워를 SO를 받아서 동적 생성
</details>

<details><summary> 타워/트랩/투사체 스킬</summary>

<img src="https://github.com/user-attachments/assets/b597ad67-eceb-415c-a3a0-d6fb356b780f" width="500" height="430"><br>

타워에서 발사시 Factory를 통해 프로젝타일 생성후 발사<br>
투사체의 외형과 효과는 각각 투사체에서 생성시 Init함수를 통해 설정<br>
투사체에 관련된 효과(슬로우,화염,스턴 등)은 Effects라는 Interface를 통해 관리<br>
하나의 프로젝타일에 여러 효과를 중첩/확장 가능<br>

</details>

<details><summary> 타워상호작용</summary>
<img src="https://github.com/user-attachments/assets/69b3ac48-660f-4459-a538-483e89be57ea" width="450" height="450"><br>

타워(오브젝트 트랩)설치 시 주변과 상호작용<br>
상호작용은 버프타워와 장애물 중 일부(물,불,플랫폼)과 발생한다.<br>
버프타워 설치 시 ```OverlapCircleAll``` 을통해 주변 타워와 상호작용<br>
버프타워 해제 시 주변타워를 ```OverlapCircleAll```로 스캔한 이후 버프를 받던 타워들이 버프를 해제하고 주변버프타워를 재 스캔하여 재적용(버프는 인덱스로 저장)<br>
장애물 주변에 설치 시 장애물 주변에 미리 설치된 탐지체에게 탐지되어 버프적용<br>
장애물 파괴시 주변타워에게 버프타워 해제과정과 유사한 방식으로 적용
</details>

<details><summary> 데이터SO와 데이터다운로더</summary>
<img src="https://github.com/user-attachments/assets/27871b70-d1b9-45f8-9f80-c5c7995de0d8" width="300" height="400"><br>
게임 내에서 사용되는 데이터들을 GoogleSheet로 정리한 후 에디터를 통해 받아와 SO파일로 자동 생성 후 관리
</details>

<details><summary> 몬스터 상태이상 적용</summary>
<img src="https://github.com/user-attachments/assets/ee37d0b7-c2ed-4718-8956-b78f204430f5" width="400" height="500"><br>
각각의 상태이상 클래스는 StatusEffect를 상속받고 AffectEffect로 효과적용 RemoveEffect로 효과제거를 한다.
각각의 몬스터에 EffectHandler를 붙인뒤 EffectHandler가 현재 몬스터의 StatusEffect의 List를 가지고 각각의 Effect를 타이머로 돌려서 관리한다
</details>

<details><summary> 계절 / 날씨 시스템</summary>
<img src="https://github.com/user-attachments/assets/5e531514-8c4d-4133-a3d2-00becd186b5b" width="400" height="500"><br>
계절은 게임 진입시 시간에 따라 결정된다
 날씨는 상태패턴으로 구현했으며, 해당하는 날씨에 따라 각각의 효과가 부여된다.
 날씨는 5웨이브마다 가중치기반 랜덤으로 변경되며 가중치 랜덤은 누적확률값 방식을 사용했다.
</details>

<br><br>
[목차로](#목차)<br>

## 트러블 슈팅
<details><summary>타워설치 검사 길막힘</summary>

- 상황 :
타워 설치 시, 피봇 타워를 사용해 NavMesh를 통해 경로 검사를 하여, 경로를 막는 공간에 타워 설치를 하지 못하도록 함.<br>

- 문제점 :
경로 검사가 끝날 때까지 몬스터들이 해당 위치에 건물이 있는 것으로 판단하여 (실제로는 존재하지 않음) 건물이 설치되지 않았음에도 경로를 돌아가는 문제가 발생함.<br>.

- 해결방안 :
경로 검사가 끝난 타일을 캐싱하여 저장하고, 이미 검사된 타일에 대해서는 재검사를 하지 않도록 함.
```
    public IEnumerator CanConstructCoroutine(Vector2 curPos, Action<bool> callback)
    {
        Vector2 constructPos = PostionArray(curPos);

        if (constructCache.TryGetValue(constructPos, out bool cachedResult))
        {
            callback?.Invoke(cachedResult);
            yield break;
        }
        if (IsAnyObjectOnTile(constructPos))
        {
            callback?.Invoke(false);
            yield break;
        }
        ...
    }
```
- 개선안 :
수많은 몬스터가 동시에 검사되는 문제를 해결하기 위해,동일한 타일엔 NavMesh 경로 탐색을 한 번만 해도 되는 방식으로 최적화하여 성능을 개선함.

</details>

<details><summary>네브매쉬 모서리</summary>
<img src="https://github.com/user-attachments/assets/1d8485c9-9f60-4e94-894c-e987737711f8" width="350" height="200"/><br>

- 상황 
몬스터들이 모서리 부분에 막혀서 진행을 하지않음<br>

- 문제점 
NavMash가 실시간으로 변화는 상황에서 타일의 크기가 작아 지역을 나누는 과정에서 코너부분이 넓게 측정되어 경로가 코너에 붙어버림<br>

- 해결시도 
>1. 덩어리 진 NavMesh를 나누기 위해 일일히 Modifire를 지정함 -> 이전과 같은 문제 발견 <br>
>2. 특정 지점마다 포인트를 줘서 경로를 유도 -> 유저의 타워설치에 따라 동적으로 변경되는 경로에 포인트를 지정할 수 없음 지정 할 수 없음.<br>
>3. NavMashObstacle의 모양을 사각형에서 원으로 변경 -> 몬스터들이 원사이로 통과하여 타워를 지나감<br>

<img src="https://github.com/user-attachments/assets/cee59815-22fa-47f0-b879-0224f8ce7b0d" width="350" height="200"/><br>

- 최종 방안  
3번안(사각형에서 원으로 변경)채택 하되 몬스터에게 NavMesh 충돌용 충돌체를 부착하여 크기를 키워줌<br>

- 장점 
충돌체가 사이의 길 폭을 좁혀 자연스러운 움직임을 유도하고, 구역이 세분화되어 벽에 비비는 현상이 제거됨 (원 의도)

</details>

<details><summary>프레임드랍과 최적화</summary>

- 상황
<img src="https://github.com/user-attachments/assets/35062735-3083-41e9-ac82-a37d71076c7e" width="800" height="400"/><br>

낮은 웨이브에서도 몬스터들이 생성되면 렉이 발생함(50웨이브 평균 25프레임)

- 문제점 
유튜브 프로파일러에서 분석한 결과, Physics, Renderer, GC 부분에서 많은 부하가 발생하는 것이 발견됨.

- 해결시도


>1. Renderer :
>     1. 외부 리소스중 Enemy의 Sprite를 Atlas로 연결.<br>
>     2. 적은 분기의 애니매이션을 에니메이터가 아닌 함수로 클립 조정.<br> 
>2. GC : 타워에서 적을 찾는 FindTarget부분의 ```OverlapPointAll```함수를 미리 캐싱해둔 배열에 저장하는 자체 유틸 함수인 ```OverlapCircleAllSorted```로 배정    
>```  
>    public static Collider2D[] OverlapCircleAllSorted(Vector2 center, float radius, int layerMask)
>    {
>       int count = Physics2D.OverlapCircleNonAlloc(center, radius, buffer, layerMask);
>       return buffer
>           .Take(count) // 사용된 요소까지만 정렬
>           .ToArray();
>    }
>```
>3. physics : 가장 많이 발생하는 투사체에서, 투사체가 직접 관여하는 몬스터를 제외한 모든 충돌을 무시<br>
><img src="https://github.com/user-attachments/assets/99a1ff99-1b7e-4806-b733-2a63951df84a" width="400" height="500"/><br>

- 개선점
1. Renderer

    1.<img src="https://github.com/user-attachments/assets/99f8a550-8620-4de8-ac13-d708b16efe13" width="800" height="500"/><br>
    50웨이브 기준 평균 프레임 60FPS → 두 배 이상 개선, 낮은 웨이브부터 시작되던 프레임 드랍 사라짐, Batch는 20% 수준으로 감소 (1000 → 200).
    
    2.<img src="https://github.com/user-attachments/assets/fc396c1c-cb69-4bac-a2ef-ac6244bacc1f" width="800" height="500"/><br>
    2안의 경우 오류와 더 심한 프레임 드랍으로 폐기.
    
2. GC : 최종 최적화 후 적용.

3. physics : 최종 최적화 후 적용.

4. 디버그로그 삭제 : string 로드를 줄여 부하를 줄임

- 최종 개선
<img src="https://github.com/user-attachments/assets/c760a77a-4656-4b6f-8402-64c3451f60c5" width="800" height="500"/><br>

최종 프레임 (50웨이브 기준) 평균 100FPS.
400%의 프레임 증가량 확보.

</details>

<details><summary>앱플레이어 UI 글씨 문제</summary>
<img src="https://github.com/user-attachments/assets/7b02f6d4-35a2-4e01-864d-cd805da16512" width="800" height="400"/>
<br>
UI환경에서 피격효과에 쉐이더를 적용했더니 알파값이 0이된 쉐이더가 UI글씨를 가리는 문제 발생<br> 
쉐이더는 메인카메라로, UI는 UI전용 카메라를 배치하여 쉐이더가 UI를 가리지 않게 설정해 해결
</details>

<details><summary>플레이어 이동 방식 추가 문제</summary>

- 상황:
기존에는 드래그 후 터치 해제로 목적지를 설정하고, NavMeshAgent를 통해 자동 이동하도록 구성됨.<br> 
조이스틱 방식도 추가하면서, 처음엔 Rigidbody.MovePosition() 또는 transform.position += 같은 일반적인 위치 이동 방식을 사용하려고 했음.<br> 
- 문제점:
NavMeshAgent.speed와 일반적인 Vector3 이동(속도 speed)이 같은 수치(ex. 2f)를 사용하더라도 실제 체감 속도가 일치하지 않음.<br> 
이로 인해 조이스틱 이동 시 시각적/체감적 속도가 다르게 느껴졌고, 실제 스텟과는 일관성 없는 이동 경험이 발생함.<br> 
- 원인 분석
NavMeshAgent는 내부적으로 가속도, 회전 속도, 경로 보정 등 다양한 요소를 포함하여 이동 속도를 계산함<br> 
반면, Rigidbody나 Transform 이동은 단순히 speed * Time.deltaTime 만큼만 이동하므로 체감 속도가 더 빠르거나 느려질 수 있음<br> 
같은 값(speed = 2f)을 넣어도 이동 시스템 자체가 다르므로 속도는 정확히 일치하지 않음<br> 
- 해결 시도
Rigidbody.MovePosition() 기반 조이스틱 이동 / agent.speed와 체감 속도 불일치<br> 
transform.position += direction * speed * Time.deltaTime / 더 빠르게 움직임<br> 
NavMeshAgent를 계속 활용하되 조이스틱 입력으로 SetDestination 갱신 / 문제가 없어 보여 선택<br> 
- 최종 적용 방식
조이스틱 입력으로 매 프레임 짧은 거리(0.5f)의 목적지를 계산하여 SetDestination() 호출<br> 
agent.speed를 그대로 활용하므로 스탯 기반 이동 속도 반영이 일관적<br> 
기존 드래그 이동과 동일한 NavMeshAgent 기반으로 이동 방식 통일<br> 
- 장점 및 의도
NavMeshAgent의 속도/회전/장애물 회피 등 기존 이동 시스템과 완전히 일치,<br> 
이동 속도 디버프 등 스탯 기반 로직 수정 없이 일관된 적용 가능<br> 
드래그 / 조이스틱 등 다양한 입력 방식에서도 하나의 이동 시스템으로 통일<br> 

</details>

<br><br>
[목차로](#목차)<br>
