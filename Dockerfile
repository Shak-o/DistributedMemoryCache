FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app
COPY ./ ./
RUN dotnet publish DistributedMemm.Api/*.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/out .
ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000
ENTRYPOINT [ "dotnet", "DistributedMemm.Api.dll" ]