@echo off
rem ---------------------------------------------------------------------------
rem  Opens an XAF model (.xafml) in the DevExpress 19.2 standalone Model Editor.
rem  Use while Visual Studio 2019 has no XAF Model Editor integration.
rem
rem    ModelEditor.bat            -> FT_FixedAsset.Module.Web  (default)
rem    ModelEditor.bat module     -> FT_FixedAsset.Module
rem    ModelEditor.bat web        -> FT_FixedAsset.Module.Web
rem    ModelEditor.bat app        -> FT_FixedAsset.Web (application Model.xafml)
rem
rem  Build the solution (Debug) first - the editor reads the compiled module.
rem  Save in the editor, then REBUILD so the change is embedded in the dll.
rem ---------------------------------------------------------------------------
setlocal
set "DXME=C:\Program Files (x86)\DevExpress 19.2\Components\Tools\eXpressAppFramework\Model Editor\DevExpress.ExpressApp.ModelEditor.v19.2.exe"
rem The solution is under OneDrive, which .NET treats as a remote location
rem ("Operation is not supported ... loadFromRemoteSources"). Use a local copy of
rem the editor whose .config enables loadFromRemoteSources (created on first run).
set "MEDIR=%LOCALAPPDATA%\FT_ModelEditor19.2"
set "ME=%MEDIR%\DevExpress.ExpressApp.ModelEditor.v19.2.exe"
if not exist "%ME%.config" (
    if not exist "%MEDIR%" mkdir "%MEDIR%"
    copy /Y "%DXME%" "%ME%" >nul
    > "%ME%.config" (
        echo ^<?xml version="1.0" encoding="utf-8"?^>
        echo ^<configuration^>
        echo   ^<startup^>^<supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.5.2" /^>^</startup^>
        echo   ^<runtime^>^<loadFromRemoteSources enabled="true" /^>^</runtime^>
        echo ^</configuration^>
    )
)
set "SLN=%~dp0"
set "WHICH=%~1"
if "%WHICH%"=="" set "WHICH=web"

if /I "%WHICH%"=="module" (
    set "DLL=%SLN%FT_FixedAsset.Module\bin\Debug\FT_FixedAsset.Module.dll"
    set "DIR=%SLN%FT_FixedAsset.Module"
) else if /I "%WHICH%"=="web" (
    set "DLL=%SLN%FT_FixedAsset.Module.Web\bin\Debug\FT_FixedAsset.Module.Web.dll"
    set "DIR=%SLN%FT_FixedAsset.Module.Web"
) else if /I "%WHICH%"=="app" (
    start "" "%ME%" "%SLN%FT_FixedAsset.Web\Web.config" "%SLN%FT_FixedAsset.Web\bin\FT_FixedAsset.Web.dll"
    goto :eof
) else (
    echo Unknown option "%WHICH%". Use: module ^| web ^| app
    exit /b 1
)

if not exist "%DLL%" (
    echo %DLL% not found - build the solution in Debug first.
    pause
    exit /b 1
)
start "" "%ME%" "%DLL%" "%DIR%"
