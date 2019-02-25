FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src

# Copy csproj and do restore.
COPY ./Earbook.Web.UI.csproj ./
RUN dotnet restore

# Copy everything else and build.
COPY . ./
RUN dotnet publish -c Release -r linux-arm -o /app

# Build final image.
FROM microsoft/dotnet:2.2.1-aspnetcore-runtime-stretch-slim-arm32v7
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "Earbook.Web.UI.dll"]