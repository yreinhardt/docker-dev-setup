# Development Setup .NET 6, PostgreSQL, pgAdmin, Docker

In this example project a .NET 6 api, a Postgres db and pgAdmin are containerized and started with docker-compose.This setup enables application development without the need to install postgres locally, for example. At the same time, the data contained in pgadmin and postgres are persistently stored.

# Get started
- Requirements: Docker, Docker-Compose, .Net
- Clone repository
```bash
git clone https://github.com/yreinhardt/docker-dev-setup.git
```
- Build services
```bash
docker-compose build
```bash
- Run docker compose in detached mode
```bash
docker-compose up -d
```
- Test things out

[Open pgAdmin](localhost:5433)

[Hello World](localhost:3000/hello)

[Get all rows from db](localhost:3000/data)


# Recreate by your own

### 1. Project creation
```bash
mkdir myproject
```
```bash
dotnet new sln --name basicapi
```
```bash
dotnet new web -o basicapi
```
```bash
dotnet sln add ./basicapi/basicapi.csproj
```

### 2. Set up docker-compose

```bash
cd myproject
```
```bash
touch docker-compose.yml
```
- create services (see docker-compose.yml in project)
    - PostgreSQL
    - pgAdmin
- check if working
```bash
docker-compose up -d
```
-  view container logs
```bash
docker logs postgres_container
docker logs pgadmin_container
```
- check pgAdmin
    - [Open pgAdmin](localhost:5433)
    - login (highly recommended to change given credentials)
    - add server
        - general: name of your choice
        - connection:  host=postgresql_db, port=5432, username=basic_dev

### 3. Create .NET 6 api with Entity Framework
- recommended minimal api because its a easy way to create fast rest api
- copy given api from project
- code first migration is used. Based on DbContext a db is automatically created.
- runbash docker-compose
```bash
docker-compose up -d
```
- run initial ef migration
```bash
dotnet ef migrations add init_migration
```
- create idempotent script and use output to create table in pgadmin query tool
```bash
dotnet ef migrations script --idempotent -o 00_init_db.sql
```
- copy content
```bash
cat 00_init_db.sql
```
- [Open pgAdmin](localhost:5433)
- run query tool and insert copied content to create table
- go to api directory and create Dockerfile
```bash
cd basicapi
touch Dockerfile
```
- Add rest api service to docker-compose
- build services and run 
```bash
cd ..
docker-compose build
docker-compose up -d
```

### 4. Troubleshooting
- check correct portmapping
- check connectionstring from api (host=postres service)
- as default bridge network is used. Every container should ping the other ones. If this is not the case check network settings.
- check if container can reach other container with ping (most of the time ping is preinstalled on most images)
```bash
docker exec container1 ping container2 -c2
```
- Nice article about communication between container (https://maximorlov.com/4-reasons-why-your-docker-containers-cant-talk-to-each-other/)