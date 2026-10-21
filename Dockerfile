# Default target: the web front-end on port 8080.
#   docker build -t gameoflife .
#   docker run --rm -p 8080:8080 gameoflife
# Tests:
#   docker build --target test -t gameoflife-tests . && docker run --rm gameoflife-tests
# The WPF app (net10.0-windows) cannot run in a Linux container, so it is not built here.
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

# Copy project files first so the restore layer is cached until a package changes.
COPY GameOfLife.Model/GameOfLife.Model.csproj GameOfLife.Model/
COPY GameOfLife.Tests/GameOfLife.Tests.csproj GameOfLife.Tests/
COPY GameOfLife.WebApp/GameOfLife.WebApp.csproj GameOfLife.WebApp/
RUN dotnet restore GameOfLife.Tests/GameOfLife.Tests.csproj
RUN dotnet restore GameOfLife.WebApp/GameOfLife.WebApp.csproj

COPY GameOfLife.Model/ GameOfLife.Model/
COPY GameOfLife.Tests/ GameOfLife.Tests/
COPY GameOfLife.WebApp/ GameOfLife.WebApp/

FROM build AS test
CMD ["dotnet", "test", "GameOfLife.Tests/GameOfLife.Tests.csproj", "--no-restore"]

FROM build AS publish
RUN dotnet publish GameOfLife.WebApp/GameOfLife.WebApp.csproj --no-restore -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=publish /app .
EXPOSE 8080
ENTRYPOINT ["dotnet", "GameOfLife.WebApp.dll"]
