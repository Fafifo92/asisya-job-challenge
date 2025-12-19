# Stage 1: Build API
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/PruebaFullStack.API/PruebaFullStack.API.csproj", "src/PruebaFullStack.API/"]
COPY ["src/PruebaFullStack.Application/PruebaFullStack.Application.csproj", "src/PruebaFullStack.Application/"]
COPY ["src/PruebaFullStack.Domain/PruebaFullStack.Domain.csproj", "src/PruebaFullStack.Domain/"]
COPY ["src/PruebaFullStack.Infrastructure/PruebaFullStack.Infrastructure.csproj", "src/PruebaFullStack.Infrastructure/"]
RUN dotnet restore "src/PruebaFullStack.API/PruebaFullStack.API.csproj"
COPY . .
WORKDIR "/src/src/PruebaFullStack.API"
RUN dotnet build "PruebaFullStack.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PruebaFullStack.API.csproj" -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 80
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PruebaFullStack.API.dll"]
