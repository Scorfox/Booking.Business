FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Src/Services/Booking/Booking.csproj", "Src/Services/Booking/"]
RUN dotnet restore "Src/Services/Booking/Booking.csproj"
COPY . .
WORKDIR "/src/Src/Services/Booking"
RUN dotnet build "Booking.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Booking.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Booking.dll"]
