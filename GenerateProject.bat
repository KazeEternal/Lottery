@echo off
setlocal

REM ---------------------------------------
REM Paths
REM ---------------------------------------
set SHARPMakeExe=D:\Development\Sharpmake\Sharpmake.Application\bin\Debug\net6.0\Sharpmake.Application.exe
set SOLUTION_FILE=SharpMake/Lottery.Solution.sharpmake.cs

REM ---------------------------------------
REM Run Sharpmake
REM ---------------------------------------
echo Running Sharpmake...
"%SHARPMakeExe%"  /sources('%SOLUTION_FILE%')

REM ---------------------------------------
REM Done
REM ---------------------------------------
echo Sharpmake finished.
pause
endlocal
