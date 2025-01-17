# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

WORKDIR /app

# Copy the necessary files for restoring packages
COPY *.sln ./
COPY GeminiEducationAPI.API/*.csproj ./GeminiEducationAPI.API/
COPY GeminiEducationAPI.Application/*.csproj ./GeminiEducationAPI.Application/
COPY GeminiEducationAPI.Domain/*.csproj ./GeminiEducationAPI.Domain/
COPY GeminiEducationAPI.Infrastructure/*.csproj ./GeminiEducationAPI.Infrastructure/
COPY GeminiEducationAPI.Persistence/*.csproj ./GeminiEducationAPI.Persistence/

# Restore NuGet packages
RUN dotnet restore

# Copy the rest of the application code
COPY . ./

# Build the application
WORKDIR /app/GeminiEducationAPI.API
RUN dotnet publish -c Release -o out

# Stage 2: Create the final image
FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

# Copy the published output from the build stage
COPY --from=build-env /app/GeminiEducationAPI.API/out .

# Expose port 80 for the application
EXPOSE 80

# Set the entry point for the container
ENTRYPOINT ["dotnet", "GeminiEducationAPI.API.dll"]

#Bu Dockerfile, iki a�amal� (multi-stage) bir build i�lemi kullan�r:
#
#Stage 1 (build-env):
#
#FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env: .NET 8.0 SDK i�eren bir base image kullan�r ve bu a�amaya build-env ad�n� verir.
#WORKDIR /app: �al��ma dizinini /app olarak ayarlar.
#COPY *.sln ./ ...: Gerekli proje dosyalar�n� ve solution dosyas�n� container'a kopyalar.
#RUN dotnet restore: NuGet paketlerini restore eder.
#COPY . ./: T�m proje dosyalar�n� container'a kopyalar.
#WORKDIR /app/GeminiEducationAPI.API: �al��ma dizinini /app/GeminiEducationAPI.API olarak de�i�tirir.
#RUN dotnet publish -c Release -o out: Uygulamay� Release modunda derler ve ��kt� dosyalar�n� /app/GeminiEducationAPI.API/out dizinine yerle�tirir.
#Stage 2:
#
#FROM mcr.microsoft.com/dotnet/aspnet:8.0: .NET 8.0 runtime i�eren daha k���k bir base image kullan�r.
#WORKDIR /app: �al��ma dizinini /app olarak ayarlar.
#COPY --from=build-env /app/GeminiEducationAPI.API/out .: �lk a�amada derlenen ��kt� dosyalar�n� container'a kopyalar.
#EXPOSE 80: Container'�n 80 numaral� portu d��ar�ya a�aca��n� belirtir.
#ENTRYPOINT ["dotnet", "GeminiEducationAPI.API.dll"]: Container ba�lat�ld���nda �al��t�r�lacak komutu belirtir.