# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ./ProzorroDataMining.sln ./
COPY ./ProzorroDataMining.Api/ProzorroDataMining.Api.csproj ./ProzorroDataMining.Api/
COPY ./ProzorroDataMining.Application/ProzorroDataMining.Application.csproj ./ProzorroDataMining.Application/
COPY ./ProzorroDataMining.Data/ProzorroDataMining.Data.csproj ./ProzorroDataMining.Data/
COPY ./ProzorroDataMining.Domain/ProzorroDataMining.Domain.csproj ./ProzorroDataMining.Domain/
COPY ./ProzorroDataMining.Tests/ProzorroDataMining.Tests.csproj ./ProzorroDataMining.Tests/

# Restore dependencies
RUN dotnet restore "ProzorroDataMining.sln"

# Copy all source code
COPY . .

# Publish the API project
RUN dotnet publish "ProzorroDataMining.Api/ProzorroDataMining.Api.csproj" -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Copy published files
COPY --from=build /app/publish .

# Copy HTTPS certificate
COPY ./ProzorroDataMining.Api/https/aspnetapp.pfx /https/aspnetapp.pfx

# Configure Kestrel to use the certificate
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=password123

# Run the API
ENTRYPOINT ["dotnet", "ProzorroDataMining.Api.dll"]
