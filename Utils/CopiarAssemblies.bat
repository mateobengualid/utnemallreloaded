rem Copia archivos de los compiladores LayerD al server janus
rem @echo off
set SERVER_PATH=..\Main\Server\WPFCore\
set SERVER_COMPILER_PATH=%SERVER_PATH%\Compiler

cd ..\Main\Server\WPFCore\assemblies
mkdir .\libsclient
mkdir .\libsserver
cd ..\..\..\..\Utils

copy ..\Main\Common\BaseMobile\bin\Debug\BaseMobile.dll ..\Main\Server\WPFCore\assemblies\libsclient\
copy ..\Main\Client\EntityModel\bin\Debug\EntityModel.dll ..\Main\Server\WPFCore\assemblies\libsclient\
copy ..\Main\Client\DataModel\bin\Debug\DataModel.dll ..\Main\Server\WPFCore\assemblies\libsclient\
copy ..\Main\Client\SmartClientLayer\bin\Debug\SmartClientLayer.dll ..\Main\Server\WPFCore\assemblies\libsclient\
copy ..\Main\Common\BaseMobile\bin\Debug\System.Data.SqlServerCe.dll ..\Main\Server\WPFCore\assemblies\libsclient\
copy ..\Main\Client\BusinessLogic\bin\Debug\BusinessLogic.dll ..\Main\Server\WPFCore\assemblies\libsclient\

copy ..\Main\Common\BaseDesktop\bin\Debug\BaseDesktop.dll ..\Main\Server\WPFCore\assemblies\libsserver\
copy ..\Main\Server\WPFCore\bin\Debug\WPFCore.exe ..\Main\Server\WPFCore\assemblies\libsserver\
copy ..\Main\Server\EntityModel\bin\Debug\EntityModel.dll ..\Main\Server\WPFCore\assemblies\libsserver\

pause