# Use the official .NET Core SDK as a parent image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Ensure we listen on any IP Address 
ENV DOTNET_URLS=http://+:8080

WORKDIR /app

# Copy the project file and restore any dependencies (use .csproj for the project name)
#COPY *.csproj ./
#RUN dotnet restore

# Copy the rest of the application code
COPY . ./
RUN dotnet restore
# Publish the application
RUN dotnet publish  -c Release -o out

# Build the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

# Start the application
ENTRYPOINT ["dotnet", "uid2-participant-api.dll"]
