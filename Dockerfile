#NuGet Restore
FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /CentralErros
COPY *.sln .
RUN dotnet restore
COPY . .

# publish
FROM build AS publish
WORKDIR /CentralErros
RUN dotnet publish -c Release -o /src/publish
COPY . ./

FROM mcr.microsoft.com/dotnet/core/sdk:22 AS runtime
WORKDIR /CentralErros
COPY --from=publish /src/publish .

CMD ASPNETCORE_URLS=http://*:502 dotnet CentralErros.dll