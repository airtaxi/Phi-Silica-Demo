## 프로젝트 개요
BMW202503은 마이크로소프트의 Windows AI 기반 Phi Silica 모델을 활용한 데모 애플리케이션 프로젝트입니다. 이 프로젝트는 Windows App SDK를 활용한 WinUI 3 기반의 애플리케이션으로, 로컬 AI 기능을 시연하는 것을 목적으로 합니다.

## 주요 기능

### Phi Silica 텍스트 기능
- **프롬프트 생성**: AI를 활용한 맞춤형 프롬프트 생성
- **텍스트 요약**: 긴 텍스트를 간결하게 요약
- **텍스트 고쳐 쓰기**: 기존 텍스트를 개선하거나 다른 스타일로 변경

### 이미지 처리 기능
- **문자 인식 (OCR)**: 이미지 내 텍스트 추출
- **이미지 업스케일링**: 저해상도 이미지 품질 개선
- **이미지 설명**: 이미지 내용 자동 분석 및 설명 생성

## 기술 스택
- **.NET 9.0**: 최신 .NET 프레임워크 활용
- **Windows App SDK 1.7**: 최신 Windows 앱 개발 기술 적용
- **Windows Copilot Runtime**: 로컬 AI 모델 통합을 위한 Microsoft Windows AI 라이브러리 활용
- **WinUI 3**: 모던 Windows UI 구현

## 환경 요구사항
- **Windows 11 Insider Preview 빌드 26120.3073(개발 및 베타 채널)** 이상 (주의: 발표일 기준 카나리아 채널 미지원)
- **지원 아키텍처**: Qualcomm Snapdragon® X 프로세서 (이후 Lunar Lake 등 x64 아키텍쳐 지원 추가 예정)

## 개발 환경 설정
1. **필수 도구**:
   - Visual Studio 2022 이상
   - .NET 9.0 SDK
   - Windows App SDK 확장

2. **프로젝트 빌드**:
   ```
   git clone https://github.com/airtaxi/Phi-Silica-Demo
   cd BMW202503
   ```
   - Visual Studio에서 BMW202503.sln 솔루션 파일 열기
   - ARM64 플랫폼 선택 후 빌드

## 프로젝트 구조
- **PhiSilicaDemo**: 메인 애플리케이션 프로젝트
  - **Pages**: 애플리케이션 페이지
    - **MainPage**: 메인 내비게이션
    - **Menus**: 기능별 페이지
      - PromptGenerationPage
      - SummaryPage
      - RewritePage
      - TextRecognitionPage
      - UpscaleImagePage
      - DescribeImagePage
  - **Assets**: 이미지 및 리소스 파일
    - **Scripts**: 예제 텍스트 파일
	
## 발표 자료
`Presentation.pptx`를 참고해주세요.

## 라이선스
이 프로젝트는 MIT 라이선스 조건에 따라 라이선스가 부여됩니다.