FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app


FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /app
COPY TransportManagmentSystemAPI.csproj .
RUN dotnet restore
COPY . .
RUN dotnet build -c Release

FROM build AS publish
RUN dotnet publish -c Release -o /publish



FROM base AS final
WORKDIR /app
COPY --from=publish /publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "TransportManagmentSystemAPI.dll"]
