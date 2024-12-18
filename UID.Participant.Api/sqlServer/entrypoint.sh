#!/bin/bash
set -e

echo "Starting entrypoint: $1, $2"

function update_database_schema() {
    while [ ! -f /tmp/app-initialized ]; 
    do
        sleep 1s
    done

    for FILE in /schema/*.sql
    do 
        echo "Executing file: $FILE"
        /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P $SA_PASSWORD -C -i $FILE
    done

    for FILE in /seedData/*.sql
    do 
        echo "Executing file: $FILE"
        /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P $SA_PASSWORD -C -i $FILE
    done
}

if [ "$1" = '/opt/mssql/bin/sqlservr' ]; then
  # If this is the container's first run, initialize the application database
  if [ ! -f /tmp/app-initialized ]; then
    # Initialize the application database asynchronously in a background process. This allows a) the SQL Server process to be the main process in the container, which allows graceful shutdown and other goodies, and b) us to only start the SQL Server process once, as opposed to starting, stopping, then starting it again.
    function initialize_app_database() {
      # Wait a bit for SQL Server to start. SQL Server's process doesn't provide a clever way to check if it's up or not, and it needs to be up before we can import the application database
      sleep 15s

      #run the setup script to create the DB and the schema in the DB
      /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P $SA_PASSWORD -C -d master -i setup.sql

      # Note that the container has been initialized so future starts won't wipe changes to the data
      touch /tmp/app-initialized
    }

    initialize_app_database &
  fi

  update_database_schema &
fi

exec "$@"