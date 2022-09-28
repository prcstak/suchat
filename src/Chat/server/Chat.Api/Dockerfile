FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Chat.Api/Chat.Api.csproj", "Chat.Api/"]
COPY ["Chat.Infrastructure/Chat.Infrastructure.csproj", "Chat.Infrastructure/"]
COPY ["Chat.Domain/Chat.Domain.csproj", "Chat.Domain/"]
COPY ["Chat.Application/Chat.Application.csproj", "Chat.Application/"]
RUN dotnet restore "Chat.Api/Chat.Api.csproj"
COPY . .
WORKDIR "/src/Chat.Api"
RUN dotnet build "Chat.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Chat.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Chat.Api.dll"]
