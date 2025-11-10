# Multi-stage Dockerfile for ExemploGRPC
# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY Solution/ExemploGRPC.sln ./
COPY Src/ExemploGRPC.Domain/ExemploGRPC.Domain.csproj ./Src/ExemploGRPC.Domain/
COPY Src/ExemploGRPC.Application/ExemploGRPC.Application.csproj ./Src/ExemploGRPC.Application/
COPY Src/ExemploGRPC.Infrastructure/ExemploGRPC.Infrastructure.csproj ./Src/ExemploGRPC.Infrastructure/
COPY Src/ExemploGRPC.GRPC/ExemploGRPC.GRPC.csproj ./Src/ExemploGRPC.GRPC/
COPY Directory.Build.props ./
COPY build/Directory.Build.props ./build/

# Restore dependencies
RUN dotnet restore

# Copy the rest of the source code
COPY Src/ ./Src/

# Build the application
RUN dotnet build -c Release --no-restore

# Publish the application
RUN dotnet publish Src/ExemploGRPC.GRPC/ExemploGRPC.GRPC.csproj -c Release -o /app/publish --no-build

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy the published application from the build stage
COPY --from=build /app/publish .

# Expose ports (gRPC uses HTTP/2)
EXPOSE 8080
EXPOSE 8081

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080;https://+:8081
ENV ASPNETCORE_ENVIRONMENT=Production

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
  CMD curl --fail http://localhost:8080/ || exit 1

# Run the application
ENTRYPOINT ["dotnet", "ExemploGRPC.GRPC.dll"]
