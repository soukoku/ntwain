@echo off
cls
dotnet pack src\NTwain -c Release -o build
dotnet pack src\NTwain.Win -c Release -o build
