FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MA.SlotService.Host/MA.SlotService.Host.csproj", "MA.SlotService.Host/"]
RUN dotnet restore "MA.SlotService.Host/MA.SlotService.Host.csproj"
COPY . .
WORKDIR "/src/MA.SlotService.Host"
RUN dotnet build "MA.SlotService.Host.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "MA.SlotService.Host.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_HTTP_PORTS 80
ENTRYPOINT ["dotnet", "MA.SlotService.Host.dll"]
