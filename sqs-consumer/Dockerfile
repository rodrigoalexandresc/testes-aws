FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY [".", "."]
RUN dotnet restore
RUN dotnet build -c Release -o /app/build

FROM mcr.microsoft.com/dotnet/runtime:10.0
WORKDIR /app
COPY --from=build /app/build .
ENTRYPOINT ["dotnet", "sqs-consumer.dll"]