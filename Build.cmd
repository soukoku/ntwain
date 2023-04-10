@echo off
cls
dotnet clean src\NTwain -c Release
dotnet pack src\NTwain -c Release -o build
pause