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
|------|------|-------------|
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
-시간과 웨이브에 따른 다양한 효과를 가진 날씨 시스템

#### &emsp;&emsp;현상금소환


<img src="https://github.com/user-attachments/assets/91d5bb2a-bd0e-42a3-8b85-1d26de7c29e1" width="800" height="400"/><br>

-추가보상을 제공하는 소환가능한 현상금몬스터

#### &emsp;&emsp;게임종료
 
 
<img src="https://github.com/user-attachments/assets/268c2e94-5a8c-46e1-8f9f-e52276977be6" width="800" height="400"/><br>

-게임 종료시 클리어한 웨이브에따라 보상을 받을수있다.



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

<br><br>
[목차로](#목차)<br>

## 트러블 슈팅
<details><summary>타워설치 검사 길막힘</summary>
히히길막힘!
</details>

<details><summary>네브매쉬 모서리</summary>
히히모서리!
</details>

<details><summary>프레임드랍과 최적화</summary>
히히최적화!
</details>

<details><summary>앱플레이어 UI 글씨 문제</summary>
<img src="https://github.com/user-attachments/assets/7b02f6d4-35a2-4e01-864d-cd805da16512" width="800" height="400"/>
<br>
UI환경에서 피격효과에 쉐이더를 적용했더니 알파값이 0이된 쉐이더가 UI글씨를 가리는 문제 발생<br> 
쉐이더는 메인카메라로, UI는 UI전용 카메라를 배치하여 쉐이더가 UI를 가리지 않게 설정해 해결
</details>


<br><br>
[목차로](#목차)<br>