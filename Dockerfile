#NuGet Restore

FROM microsoft/dotnet:2.2-sdk
WORKDIR /CentalErros
EXPOSE 80

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out
CMD ASPNETCORE_URLS=http://*:502 dotnet CentralErros.dll