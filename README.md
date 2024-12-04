# docker compose commands

Build the images:

`docker compose -f .\docker-compose.yml -f .\docker-compose.override.yml build`

Run the docker stack:

`docker compose -f .\docker-compose.yml -f .\docker-compose.override.yml up`

Go here to test:
http://localhost:5050/swagger/index.html


## Updating the EF Model
To update the model based on changes to the database, run this command from the root of the UID.Participant.Api project (not the root of the solution):

`dotnet ef dbcontext scaffold "Name=ConnectionStrings:uid2_selfserve" Microsoft.EntityFrameworkCore.SqlServer --context ParticipantApiContext --output-dir Models --force`
