@echo off
cls
dotnet pack -o build src\NTwain
dotnet pack -o build src\NTwain.Win
