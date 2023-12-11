#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build_apin5now
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ApiN5now/ApiN5now.csproj", "ApiN5now/"]
RUN dotnet restore "./ApiN5now/ApiN5now.csproj"
COPY . .
WORKDIR "/src/ApiN5now"
RUN dotnet build "./ApiN5now.csproj" -c $BUILD_CONFIGURATION -o /app/build_apin5now

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build_connection
WORKDIR /src
COPY ["Connection/Connection.csproj", "Connection/"]
RUN dotnet restore "./Connection/Connection.csproj"
COPY . .
WORKDIR "/src/Connection"
RUN dotnet build "./Connection.csproj" -c Release -o /app/build_connection

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build_tests
WORKDIR /src
COPY ["TestApiN5now/TestApiN5now.csproj", "TestApiN5now/"]
RUN dotnet restore "./TestApiN5now/TestApiN5now.csproj"
COPY . .
WORKDIR "/src/TestApiN5now"
RUN dotnet build "./TestApiN5now.csproj" -c Release -o /app/build_TestApiN5now

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app
COPY --from=build_apin5now /app/build_apin5now .
COPY --from=build_connection /app/build_connection ./Connection
COPY --from=build_tests /app/build_TestApiN5now ./TestApiN5now

ENTRYPOINT ["dotnet", "ApiN5now.dll"]