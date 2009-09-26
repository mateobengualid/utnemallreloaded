@echo off
rem -------------------------------------------------------------
rem  Script file to compile Services files with LayerD Compilers
rem -------------------------------------------------------------
rem
rem	%1 : application folder
rem %2 : output folder name
rem %3 : output filename wihtout extension
rem %4 : output module build arguments
rem
rem -------------------------------------------------------------
rem	  Read compiler\readme.htm for more information on LayerD
rem -------------------------------------------------------------

echo ------------------------------ >> compilation.log
echo Compiling Services with LayerD >> compilation.log
echo ------------------------------ >> compilation.log

echo Compiling Services with LayerD

cd %1\compiler

%1\compiler\metadppc.exe %2\%3.dpp -s 
if not errorlevel 0 (
	zoec.exe %2\%3.zoe -bp -bwe -pn:%3 -ba:%4 -lib -s -opath:%2 >> compilation.log
)else (
	echo Error compiling Meta D++
)

cd ..

