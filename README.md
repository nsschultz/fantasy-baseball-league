[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=nsschultz_fantasy-baseball-league&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=nsschultz_fantasy-baseball-league)

## League Service

- This is the source of truth for the league data.
- Exposes the CRUD functionality needed to maintain the league data.
- Exposes endpoints to allow the user to merge in new projection data.

---

### Healthcheck:

- The service will fail a healthcheck if database cannot be accessed.
- The service will also fail if it cannot connect to the position service.

---

### Build Image

```
version=$(cat version.txt) && docker build -t nschultz/fantasy-baseball-league:$version .
```

---

### Dev Containers

- VS Code should auto-prompt to reopen the workspace in a contaienr, which will start the rest of the containers as well.
- Tasks are setup in tasks.json.
- Command for manually starting/stopping dev containers:

```
docker compose -f .docker-compose/docker-compose-dev.yaml -p fantasy-baseball-league up --build -d
docker compose -f .docker-compose/docker-compose-dev.yaml -p fantasy-baseball-league down
```

---

### Runtime Containers

- Command for starting/stopping runtime containers:

```
docker compose -f .docker-compose/docker-compose-runtime.yaml -p fantasy-baseball-league up --build -d
docker compose -f .docker-compose/docker-compose-runtime.yaml -p fantasy-baseball-league down
```

---

### Local Connections

- League API
  - View Swagger/Test Endpoints: http://localhost:8080/api/swagger/index.html
- PG Admin
  - GUI: http://localhost:9000
- Postgres Database
  - Available at database:5432 (not outside of the containers)
- Montebank
  - GUI: http://localhost:2525
  - Postions API available on http://mock-positions:5555
