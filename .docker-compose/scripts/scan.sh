#!/bin/bash
rm -rf coverage-results/
VERSION=$(cat version.txt)
dotnet sonarscanner begin \
  /o:"nsschultz" \
  /k:nsschultz_fantasy-baseball-league \
  /n:fantasy-baseball-league \
  /d:sonar.token="$SONAR_TOKEN" \
  /v:$VERSION \
  /d:sonar.cs.opencover.reportsPaths=coverage-results/coverage.opencover.xml \
  /d:sonar.dotnet.excludeTestProjects=true \
  "/d:sonar.coverage.exclusions=\"**/Database/Migrations/*.cs\"" \
  "/d:sonar.exclusions=\"**/Database/Migrations/*.cs\",\"**/*.sql\"" \
  /d:sonar.host.url="https://sonarcloud.io"
dotnet test \
  "/p:CollectCoverage=true" \
  "/p:CoverletOutput=../coverage-results/" \
  "/p:CoverletOutputFormat=\"json,opencover\"" \
  "/p:MergeWith=../coverage-results/coverage.json"
dotnet sonarscanner end /d:sonar.token="$SONAR_TOKEN"
