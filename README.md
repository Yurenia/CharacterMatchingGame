# CharacterMatchingGame
- 그림 맞추기 게임
- 개발기간: 2019.02.25 ~ 2019.03.01
- 참여인원: 1명 (Raynia)
- 주제: tvN 예능 '신서유기'에서 등장한 그림 맞추기 게임을 WinForm을 활용하여 개발한 프로그램
- 목적: 동아리 MT에서 사용하기 위해 개발

# 개발환경
- Visual Studio 2017
- C#
- WinForm

# 실행화면
## 메인화면
<image src = "https://user-images.githubusercontent.com/46100945/138601956-86615995-faee-4475-9d07-c8db47173bc3.png" width = "854" height = "480">  
  
- 참가자 수를 설정하지 않을 경우, 게임 시작이 불가능
- 반드시 참가자 수를 먼저 설정해야함
<image src = "https://user-images.githubusercontent.com/46100945/138601963-3ed84380-91ae-4c77-b50b-d294ce42c629.png">
  
<image src = "https://user-images.githubusercontent.com/46100945/138601964-a51a84ba-5cbb-46c4-8c95-bdcd36cd14d9.png">
  
- 참가자 수는 1~20명이며 초과하거나 미만일 경우 설정되지 않음
<image src = "https://user-images.githubusercontent.com/46100945/138601966-31e4e2cc-8ed4-462f-99ce-ae0b5ec1732b.png">
  
- 참가자 수를 올바르게 입력했다면, 다음 메세지와 함께 참가자 수가 설정됨
- 참가자 수 설정 시, 규칙에 따라 게임에 사용될 사진의 갯수를 자동으로 설정 (참가자 수 * 2 - 1)
<image src = "https://user-images.githubusercontent.com/46100945/138601974-aae69ef4-ee38-4b00-bd25-4348cc16844f.png">

- 게임 시작 버튼을 눌러 시작
  
## 게임화면
  
<image src = "https://user-images.githubusercontent.com/46100945/138601975-4201102c-1a43-428b-9c84-8f37918330b9.png" width = "854" height = "480">

- 키보드 화살표 오른쪽 버튼이나 엔터키를 누르면 다음 사진으로 이동
- 키보드 화살표 왼쪽 버튼을 누르면 이전 사진으로 이동
  
## 결과화면 
- 마지막 사진에서 다음 사진으로 넘기면 게임에서 승리한 것으로 처리
- 키보드 ESC 버튼을 누르면 게임에서 패배한 것으로 처리
- 재시작 버튼을 누를 경우, 즉시 사진을 섞어 새로운 사진 리스트 생성
- 메인으로 버튼을 누르면 이전 게임의 설정이 모두 초기화 되며, 참가자 수를 다시 지정해야함
  
<image src = "https://user-images.githubusercontent.com/46100945/138601978-c422a5e3-0591-45cd-a2d1-b20d524883f3.png" width = "854" height = "480">
  
<image src = "https://user-images.githubusercontent.com/46100945/138601981-445b3063-e6ea-441d-bf01-82f406cab9c4.png" width = "854" height = "480">

  
# 참조 코드
- Picture_Test/Form1.cs

# 알려진 문제점
- ~~이미지 파일이 존재하지 않을 경우, 예외 처리가 되지 않음~~ (21.10.25 수정됨)
- 이미지 파일의 최대치가 하드 코딩됨
- 1.png와 같이 파일의 이름이 숫자이고 png 확장자가 아닌 이미지 파일은 인식할 수 없음
