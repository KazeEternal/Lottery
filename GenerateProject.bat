@echo off

echo Run Sharp Make

call ..\Sharpmake\Bin\Sharpmake.Application.exe /sources(@"SharpMake/Lottery.Solution.sharpmake.cs") %*

"raffle bot_vs2017.sln"

pause