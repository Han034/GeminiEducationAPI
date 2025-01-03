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

#Bu Dockerfile, iki aþamalý (multi-stage) bir build iþlemi kullanýr:
#
#Stage 1 (build-env):
#
#FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env: .NET 8.0 SDK içeren bir base image kullanýr ve bu aþamaya build-env adýný verir.
#WORKDIR /app: Çalýþma dizinini /app olarak ayarlar.
#COPY *.sln ./ ...: Gerekli proje dosyalarýný ve solution dosyasýný container'a kopyalar.
#RUN dotnet restore: NuGet paketlerini restore eder.
#COPY . ./: Tüm proje dosyalarýný container'a kopyalar.
#WORKDIR /app/GeminiEducationAPI.API: Çalýþma dizinini /app/GeminiEducationAPI.API olarak deðiþtirir.
#RUN dotnet publish -c Release -o out: Uygulamayý Release modunda derler ve çýktý dosyalarýný /app/GeminiEducationAPI.API/out dizinine yerleþtirir.
#Stage 2:
#
#FROM mcr.microsoft.com/dotnet/aspnet:8.0: .NET 8.0 runtime içeren daha küçük bir base image kullanýr.
#WORKDIR /app: Çalýþma dizinini /app olarak ayarlar.
#COPY --from=build-env /app/GeminiEducationAPI.API/out .: Ýlk aþamada derlenen çýktý dosyalarýný container'a kopyalar.
#EXPOSE 80: Container'ýn 80 numaralý portu dýþarýya açacaðýný belirtir.
#ENTRYPOINT ["dotnet", "GeminiEducationAPI.API.dll"]: Container baþlatýldýðýnda çalýþtýrýlacak komutu belirtir.