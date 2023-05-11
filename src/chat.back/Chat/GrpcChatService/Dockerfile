FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["GrpcChatService/GrpcChatService.csproj", "GrpcChatService/"]
RUN dotnet restore "GrpcChatService/GrpcChatService.csproj"
COPY . .
WORKDIR "/src/GrpcChatService"
RUN dotnet build "GrpcChatService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GrpcChatService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GrpcChatService.dll"]
