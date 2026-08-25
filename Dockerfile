# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

# Restore
COPY ["src/MovieReservation.API/MovieReservation.API.csproj", "MovieReservation.API/"]
COPY ["src/MovieReservation.Services/MovieReservation.Services.csproj", "MovieReservation.Services/"]
COPY ["src/MovieReservation.Domain/MovieReservation.Domain.csproj", "MovieReservation.Domain/"]
RUN dotnet restore "MovieReservation.API/MovieReservation.API.csproj"

# Build
COPY ["src/MovieReservation.API", "MovieReservation.API/"]
COPY ["src/MovieReservation.Services", "MovieReservation.Services/"]
COPY ["src/MovieReservation.Domain", "MovieReservation.Domain/"]
WORKDIR /src/MovieReservation.API
RUN dotnet build "MovieReservation.API.csproj" -c Release -o /app/build

# Stage 2: Publish
FROM build AS publish
RUN dotnet publish "MovieReservation.API.csproj" -c Release -o /app/publish

# Stage 3: Run
FROM mcr.microsoft.com/dotnet/aspnet:10.0
ENV ASPNETCORE_HTTP_PORTS=3600
EXPOSE 3600
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MovieReservation.API.dll"]
