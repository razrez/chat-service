FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Chat.DB/Chat.DB.csproj", "Chat.DB/"]
RUN dotnet restore "Chat.DB/Chat.DB.csproj"
COPY . .
WORKDIR "/src/Chat.DB"
RUN dotnet build "Chat.DB.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Chat.DB.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Chat.DB.dll"]
