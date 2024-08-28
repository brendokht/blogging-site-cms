# Followed https://www.youtube.com/watch?v=XTqwvQAv9Lg

# Inital setup for publish
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
# Set working directory
WORKDIR /app
# Expose Port 8080 and 8081 (.NET 8.0 Default)
EXPOSE 8080
EXPOSE 8081
# Building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
# Argument for build configuration
ARG BUILD_CONFIGURATION=Release
# Set working directory
WORKDIR /src
# Copy the project files (this was a nightmare for some reason)
COPY ["BloggingSiteCMS.WebAPI/BloggingSiteCMS.WebAPI.csproj", "BloggingSiteCMS.WebAPI/"]
COPY ["BloggingSiteCMS.DTOs/BloggingSiteCMS.DTOs.csproj", "BloggingSiteCMS.DTOs/"]
COPY ["BloggingSiteCMS.ViewModels/BloggingSiteCMS.ViewModels.csproj", "BloggingSiteCMS.ViewModels/"]
COPY ["BloggingSiteCMS.DAL/BloggingSiteCMS.DAL.csproj", "BloggingSiteCMS.DAL/"]
# Restore the project files and dependencies
RUN dotnet restore "BloggingSiteCMS.WebAPI/BloggingSiteCMS.WebAPI.csproj"
RUN dotnet restore "BloggingSiteCMS.DTOs/BloggingSiteCMS.DTOs.csproj"
RUN dotnet restore "BloggingSiteCMS.ViewModels/BloggingSiteCMS.ViewModels.csproj"
RUN dotnet restore "BloggingSiteCMS.DAL/BloggingSiteCMS.DAL.csproj"
 # Copy the rest of the files
COPY . .
# Set working directory
WORKDIR "/src/BloggingSiteCMS.WebAPI"
# Build the application
RUN dotnet build "BloggingSiteCMS.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build
# Publish the application
FROM build as publish
# Argument for build configuration
ARG BUILD_CONFIGURATION=Release
# Run the dotnet publish command
RUN dotnet publish "BloggingSiteCMS.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /P:UseAppHost=false
# Finalize and run the application
FROM base AS final
# Set working directory
WORKDIR /app
# Copy the published application
COPY --from=publish /app/publish .
# Run the application
ENTRYPOINT [ "dotnet", "BloggingSiteCMS.WebAPI.dll" ]