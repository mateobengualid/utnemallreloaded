rem echo off

set PATH_UTILS=.\MainTemplate
set PATH_UTILS_2=C:\Users\Alexis\Desktop\Janus\Utils\MainTemplate
set PATH_COMPILADOR=%zoe_compiler_path%
set LOGFILE=%PATH_UTILS%\compilarfactories.log

echo ------------------------------------------------------
echo  Compilando Meta D++
echo ------------------------------------------------------
for %%f in (%PATH_UTILS%\*.dpp) do %PATH_COMPILADOR%\metadppc.exe %%f

cd %PATH_COMPILADOR%

echo ------------------------------------------------------
echo  Compilando Zoe
echo ------------------------------------------------------
for %%f in (%PATH_UTILS_2%\*.zoe) do zoec.exe %%f -ae

cd %PATH_UTILS_2%

pause
