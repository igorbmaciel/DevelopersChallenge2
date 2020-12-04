FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY nuget.docker ./nuget.config
COPY OfxImports ./OFXIMPORTS

WORKDIR /app/OFXIMPORTS/src/OfxImports.Web
RUN dotnet restore
RUN dotnet publish -c Release -o /app/OFXIMPORTS/out

# Build runtime image
FROM microsoft/dotnet:2.2-aspnetcore-runtime AS runtime
WORKDIR /app
COPY --from=build /app/OFXIMPORTS/out .
ENTRYPOINT ["dotnet", "OfxImports.Web.dll"]
