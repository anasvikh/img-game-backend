FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["ImgGameBackend/ImgGameBackend.csproj", "ImgGameBackend/"]
RUN dotnet restore "ImgGameBackend/ImgGameBackend.csproj"
COPY . .
WORKDIR "/src/ImgGameBackend"
RUN dotnet build "ImgGameBackend.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "ImgGameBackend.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ImgGameBackend.dll"]