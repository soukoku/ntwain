@echo off
dotnet build -c Release src/NTwain/NTwain.csproj
dotnet pack -c Release /p:ContinuousIntegrationBuild=true -o ./build src/NTwain/NTwain.csproj
