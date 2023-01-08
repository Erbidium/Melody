FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal AS base
ARG PROJECT_PORT
WORKDIR /app
EXPOSE $PROJECT_PORT

FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /src
COPY ["Melody.SharedKernel/Melody.SharedKernel.csproj", "Melody.SharedKernel/Melody.SharedKernel.csproj"]
COPY ["Melody.Core/Melody.Core.csproj", "Melody.Core/Melody.Core.csproj"]
COPY ["Melody.Infrastructure/Melody.Infrastructure.csproj", "Melody.Infrastructure/Melody.Infrastructure.csproj"]
COPY ["Melody.WebAPI/Melody.WebAPI.csproj", "Melody.WebAPI/Melody.WebAPI.csproj"]


RUN dotnet restore "Melody.WebAPI/Melody.WebAPI.csproj"
COPY . .
WORKDIR "/src/Melody.WebAPI"
RUN dotnet build "Melody.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Melody.WebAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS http://*:5050
ENTRYPOINT ["dotnet", "Melody.WebAPI.dll"]