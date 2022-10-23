#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Instruction.Api/Instruction.Api.csproj", "Instruction.Api/"]
RUN dotnet restore "Instruction.Api/Instruction.Api.csproj"
COPY . .
WORKDIR "/src/Instruction.Api"
RUN dotnet build "Instruction.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Instruction.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Instruction.Api.dll"]