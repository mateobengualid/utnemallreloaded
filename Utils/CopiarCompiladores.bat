rem Copia archivos de los compiladores LayerD al server janus
rem @echo off
set SERVER_PATH=..\Main\Server\WPFCore
set SERVER_COMPILER_PATH=%SERVER_PATH%\Compiler

md %SERVER_PATH%\Compiler

copy %ZOE_COMPILER_PATH%\zoec.exe %SERVER_COMPILER_PATH%
copy %METADPP_COMPILER_PATH%\metadppc.exe %SERVER_COMPILER_PATH%
copy %ZOE_COMPILER_PATH%\lib_zoe_cginterface.dll %SERVER_COMPILER_PATH%
copy %ZOE_COMPILER_PATH%\lib_layerd_xpl_codedom_net.dll %SERVER_COMPILER_PATH%
copy %ZOE_COMPILER_PATH%\lib_zoe_outmod_cs.dll %SERVER_COMPILER_PATH%
copy %ZOE_COMPILER_PATH%\lib_zoec_core.dll %SERVER_COMPILER_PATH%
copy %ZOE_COMPILER_PATH%\lib_zoec_outputmoduleslib.dll %SERVER_COMPILER_PATH%
copy %ZOE_COMPILER_PATH%\zoe_codegenerators.xml %SERVER_COMPILER_PATH%
copy %ZOE_COMPILER_PATH%\OutputModules\lib_zoe_outmod_cs.dll %SERVER_COMPILER_PATH%\OutputModules
copy %ZOE_COMPILER_PATH%\OutputModules\lib_zoe_outmod_dpp.dll %SERVER_COMPILER_PATH%\OutputModules

rem xcopy %ZOE_COMPILER_PATH%\ProgramsLib %SERVER_COMPILER_PATH%\ProgramsLib  /Y
rem xcopy %ZOE_COMPILER_PATH%\OutputModules %SERVER_COMPILER_PATH%\OutputModules  /Y
rem xcopy %ZOE_COMPILER_PATH%\FactoriesLib %SERVER_COMPILER_PATH%\FactoriesLib  /Y

cd ..\Main\Server\WPFCore\assemblies
mkdir .\libsclient
mkdir .\libsserver
cd ..\..\..\..\Utils

copy ..\Main\Common\BaseMobile\bin\Debug\BaseMobile.dll ..\Main\Server\WPFCore\assemblies\libsclient\
copy ..\Main\Client\EntityModel\bin\Debug\EntityModel.dll ..\Main\Server\WPFCore\assemblies\libsclient\
copy ..\Main\Client\SmartClientLayer\bin\Debug\SmartClientLayer.dll ..\Main\Server\WPFCore\assemblies\libsclient\
copy ..\Main\Common\BaseMobile\bin\Debug\System.Data.SqlServerCe.dll ..\Main\Server\WPFCore\assemblies\libsclient\
copy ..\Main\Client\BusinessLogic\bin\Debug\BusinessLogic.dll ..\Main\Server\WPFCore\assemblies\libsclient\
copy ..\Main\Client\DataModel\bin\Debug\DataModel.dll ..\Main\Server\WPFCore\assemblies\libsclient\

rem copy ..\Main\Common\BaseDesktop\bin\Debug\BaseDesktop.dll ..\Main\Server\WPFCore\assemblies\libsserver\
rem copy ..\Main\Server\WPFCore\bin\Debug\WPFCore.exe ..\Main\Server\WPFCore\assemblies\libsserver\
rem copy ..\Main\Server\EntityModel\bin\Debug\EntityModel.dll ..\Main\Server\WPFCore\assemblies\libsserver\

pause