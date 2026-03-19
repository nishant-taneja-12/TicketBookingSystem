FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files first to maximize restore cache hits.
COPY ["BookingHub.API/BookingHub.API.csproj", "BookingHub.API/"]
COPY ["BookingHub.Application/BookingHub.Application.csproj", "BookingHub.Application/"]
COPY ["BookingHub.Domain/BookingHub.Domain.csproj", "BookingHub.Domain/"]
COPY ["BookingHub.Infrastructure/BookingHub.Infrastructure.csproj", "BookingHub.Infrastructure/"]

RUN dotnet restore "BookingHub.API/BookingHub.API.csproj"

# Copy source and publish.
COPY . .
RUN dotnet publish "BookingHub.API/BookingHub.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Expose API port.
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "BookingHub.API.dll"]
