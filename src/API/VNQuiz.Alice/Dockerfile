#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src

COPY ["Core/VNQuiz.Core/VNQuiz.Core.csproj", "Core/VNQuiz.Core/"]
RUN dotnet restore "Core/VNQuiz.Core/VNQuiz.Core.csproj"

COPY ["Core/VNQuiz.Core.Tests/VNQuiz.Core.Tests.csproj", "Core/VNQuiz.Core.Tests/"]
RUN dotnet restore "Core/VNQuiz.Core.Tests/VNQuiz.Core.Tests.csproj"

COPY ["API/VNQuiz.Alice/VNQuiz.Alice.csproj", "API/VNQuiz.Alice/"]
RUN dotnet restore "API/VNQuiz.Alice/VNQuiz.Alice.csproj"

COPY ["API/VNQuiz.Alice.Tests/VNQuiz.Alice.Tests.csproj", "API/VNQuiz.Alice.Tests/"]
RUN dotnet restore "API/VNQuiz.Alice.Tests/VNQuiz.Alice.Tests.csproj"

COPY . .
RUN dotnet test . -c Release

FROM build AS publish
RUN dotnet publish "API/VNQuiz.Alice/VNQuiz.Alice.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VNQuiz.Alice.dll"]