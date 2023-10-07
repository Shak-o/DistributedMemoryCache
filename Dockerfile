FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app
COPY ./ ./
RUN dotnet publish DistributedMemm.Api/*.csproj -c Release -o out
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT [ "dotnet", "DistributedMemm.Api.dll" ]