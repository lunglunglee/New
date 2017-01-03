ECHO OFF
REM Starting PostBuild.bat
REM Note: Beware that .bat files in VS have junk characters at beginning that must be removed via Binary Editor.
REM PostBuildEvent: Call $(ProjectDir)PostBuild.Bat $(TargetDir) $(TargetName)
REM Common are: $(TargetPath) = output file, $(TargetDir) = full bin path , $(OutDir) = bin\debug

REM ***
REM *** Variables
REM ***
SET LibFolder=\lib\GenesysFramework

REM Copying project output to build location
Echo Input:  %1 to %LibFolder%

MD %LibFolder%
%WINDIR%\system32\attrib.exe %LibFolder%\*.* -r /s
%WINDIR%\system32\xcopy.exe %1 %LibFolder%\*.* /f/s/e/r/c/y

REM ***
Echo *** Merge to Lib
REM ***
REM %WINDIR%\system32\attrib.exe %LibFolder%\MergeFramework.bat -r /s
REM %WINDIR%\system32\xcopy.exe .\MergeFramework.bat %LibFolder%\*.bat /y
REM CD %LibFolder%
REM if $(ConfigurationName) == Release (Call %LibFolder%\MergeFramework.bat)

Echo *** Postbuild Complete ***
exit 0