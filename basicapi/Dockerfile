FROM mcr.microsoft.com/dotnet/runtime-deps:6.0-alpine AS base
WORKDIR /webapi_app
EXPOSE 80
EXPOSE 443

RUN adduser -u 1234 --disabled-password --gecos "" webapiuser && chmod -R +x /webapi_app
USER webapiuser

FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build
WORKDIR /src
COPY ["basicapi.csproj", "./"]
RUN dotnet restore "basicapi.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "basicapi.csproj" -c Release -o /webapi_app/build

FROM build AS publish
RUN dotnet publish "basicapi.csproj" -c Release -o /webapi_app/publish \
   -r alpine-x64 \
   --self-contained true \
   -p:PublishTrimmed=true \
   -p:PublishSingleFile=true

FROM base AS final
WORKDIR /webapi_app
COPY --from=publish /webapi_app/publish .
ENTRYPOINT ["./basicapi"]