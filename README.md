# docker compose commands

Build the images:
`docker compose -f .\docker-compose.yml -f .\docker-compose.override.yml build`

Run the docker stack:
`docker compose -f .\docker-compose.yml -f .\docker-compose.override.yml up`

Go here to test:
http://localhost:5050/swagger/index.html