@echo off
Echo Linea de comandos para los compiladores de .NET Framework 3.5
Echo.
rem %comspec% /k "net_bin.bat"
rem

@SET FrameworkDir=C:\WINDOWS\Microsoft.NET\Framework
@SET FrameworkVersion=v3.5

@set PATH=%FrameworkDir%\v3.5;%FrameworkDir%\%FrameworkVersion%;%PATH%
@set LIBPATH=%FrameworkDir%\v3.5;%FrameworkDir%\%FrameworkVersion%;%LIBPATH%

%frameworkdir%\%frameworkversion%\msbuild.exe /p:Configuration=Debug /t:Rebuild ..\Main\Server\ServerSolution\ServerSolution.sln
%frameworkdir%\%frameworkversion%\msbuild.exe /p:Configuration=Debug /t:Rebuild ..\Main\Client\Client.sln
%frameworkdir%\%frameworkversion%\msbuild.exe /p:Configuration=Debug /t:Rebuild ..\Main\ServerManager\ServerManager.sln
%frameworkdir%\%frameworkversion%\msbuild.exe /p:Configuration=Debug /t:Rebuild ..\Main\StoreManager\PresentationLayer\StoreManager.sln

pause