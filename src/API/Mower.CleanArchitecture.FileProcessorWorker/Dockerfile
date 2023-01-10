FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/API/Mower.CleanArchitecture.FileProcessorWorker/Mower.CleanArchitecture.FileProcessorWorker.csproj", "src/API/Mower.CleanArchitecture.FileProcessorWorker/"]
RUN dotnet restore "src/API/Mower.CleanArchitecture.FileProcessorWorker/Mower.CleanArchitecture.FileProcessorWorker.csproj"
COPY . .
WORKDIR "/src/src/API/Mower.CleanArchitecture.FileProcessorWorker"
RUN dotnet build "Mower.CleanArchitecture.FileProcessorWorker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Mower.CleanArchitecture.FileProcessorWorker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Mower.CleanArchitecture.FileProcessorWorker.dll"]
