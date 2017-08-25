#!/bin/bash

if [ $# -eq 0 ]
  then
    echo "No version number supplied"
    exit 0
fi

dotnet restore serilog-sinks-rollingfile.sln

dotnet build --configuration Release serilog-sinks-rollingfile.sln

dotnet test ./test/Serilog.Sinks.RollingFileAlternate.Tests/Serilog.Sinks.RollingFileAlternate.Tests.csproj

dotnet pack ./src/Serilog.Sinks.RollingFileAlternate/Serilog.Sinks.RollingFileAlternate.csproj --configuration Release --output ../../artifacts /p:PackageVersion=$1