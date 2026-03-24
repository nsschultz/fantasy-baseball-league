#!/bin/bash
set -e
EXIT_CODE=0
docker compose -f .docker-compose/docker-compose-ci.yaml -p fantasy-baseball-league up --build --exit-code-from api || EXIT_CODE=$?
docker compose -f .docker-compose/docker-compose-ci.yaml -p fantasy-baseball-league down
docker volume rm fantasy-baseball-league_data_volume
exit $EXIT_CODE
