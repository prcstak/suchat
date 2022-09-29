FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Chat.BackgroundService/Chat.BackgroundService.csproj", "Chat.BackgroundService/"]
COPY ["Chat.Infrastructure/Chat.Infrastructure.csproj", "Chat.Infrastructure/"]
COPY ["Chat.Domain/Chat.Domain.csproj", "Chat.Domain/"]
RUN dotnet restore "Chat.BackgroundService/Chat.BackgroundService.csproj"
COPY . .
WORKDIR "/src/Chat.BackgroundService"
RUN dotnet build "Chat.BackgroundService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Chat.BackgroundService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Chat.BackgroundService.dll"]
