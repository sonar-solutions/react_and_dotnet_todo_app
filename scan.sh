#!/bin/bash
# scan.sh - Run SonarScanner on both backend and frontend in a single scan for the monorepo

set -e

# Ensure SonarScanner and .NET tools are in PATH for this script
export PATH="$PATH:/Users/joshua.quek/.dotnet/tools"

SONAR_PROJECT_KEY="solutions_react_and_dotnet_todo_app"
SONAR_ORG="sonar-solutions" # Optional: set if using SonarCloud
SONAR_HOST_URL="https://sonarcloud.io" # Change if using SonarCloud or a different server
SONAR_TOKEN="sonar_token_here" # Set your token here or export SONAR_TOKEN in your environment

# Clean previous reports
rm -f backend/coverage.opencover.xml
rm -f frontend/coverage/lcov.info

# Run .NET test with OpenCover coverage for backend (before SonarScanner begin)
cd backend
dotnet test ../backendTests/backend.Tests.csproj \
  --collect:"XPlat Code Coverage" \
  --results-directory ../TestResults \
  -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Format=opencover
OPEN_COVER_PATH=$(find ../TestResults -name 'coverage.opencover.xml' | head -n 1)
cp "$OPEN_COVER_PATH" coverage.opencover.xml
cd ..

# Now start SonarScanner for .NET (required for C# coverage)
OPEN_COVER_ABS_PATH="$(pwd)/backend/coverage.opencover.xml"
dotnet sonarscanner begin \
  /k:"$SONAR_PROJECT_KEY" \
  /n:"ReactDotnetTodoApp" \
  /v:"1.0" \
  /o:"$SONAR_ORG" \
  /d:sonar.host.url="$SONAR_HOST_URL" \
  /d:sonar.token="$SONAR_TOKEN" \
  /d:sonar.exclusions="**/node_modules/**,**/bin/**,**/obj/**,**/backendTests/**,.github/**,scan.sh" \
  /d:sonar.test.exclusions="**/backendTests/**" \
  /d:sonar.sourceEncoding="UTF-8" \
  /d:sonar.dotnet.excludeTestProjects=true \
  /d:sonar.cs.opencover.reportsPaths="$OPEN_COVER_ABS_PATH" \
  /d:sonar.javascript.lcov.reportPaths="frontend/coverage/lcov.info" \
  /d:sonar.coverage.exclusions="**/*.test.js,**/setupTests.js" \
  /d:sonar.scm.provider=git \
  /d:sonar.verbose=true


# Run frontend tests with coverage (assumes npm test generates lcov.info)
cd frontend
npm ci
npm test -- --coverage --watchAll=false || true
# Fix lcov paths so SF:src/ becomes SF:frontend/src/
if [ -f coverage/lcov.info ]; then
  sed 's|SF:src/|SF:frontend/src/|g' coverage/lcov.info > fixed_lcov.info
  mv fixed_lcov.info coverage/lcov.info
fi
cd ..

dotnet build
dotnet sonarscanner end "/d:sonar.token=$SONAR_TOKEN"
