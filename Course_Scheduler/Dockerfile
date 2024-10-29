FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5274

ENV ASPNETCORE_URLS=http://+:5274

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["Course_Scheduler/Course_Scheduler.csproj", "Course_Scheduler/"]
RUN dotnet restore "Course_Scheduler/Course_Scheduler.csproj"
COPY . .
WORKDIR "/src/Course_Scheduler"
RUN dotnet build "Course_Scheduler.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "Course_Scheduler.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Course_Scheduler.dll"]
