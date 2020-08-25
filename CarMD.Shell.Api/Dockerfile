#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS publish
WORKDIR /src
COPY ["CarMD.Shell.Api/CarMD.Shell.Api.csproj", "CarMD.Shell.Api/"]
COPY ["CarMD.Fleet.Repository/CarMD.Fleet.Repository.csproj", "CarMD.Fleet.Repository/"]
COPY ["CarMD.Fleet.Core/CarMD.Fleet.Core.csproj", "CarMD.Fleet.Core/"]
COPY ["CarMD.Fleet.Data/CarMD.Fleet.Data.csproj", "CarMD.Fleet.Data/"]
COPY ["CarMD.Fleet.Common/CarMD.Fleet.Common.csproj", "CarMD.Fleet.Common/"]
COPY ["CarMD.Fleet.Adapter/CarMD.Fleet.Adapter.csproj", "CarMD.Fleet.Adapter/"]
COPY ["CarMD.Fleet.Service/CarMD.Fleet.Service.csproj", "CarMD.Fleet.Service/"]
RUN dotnet restore "CarMD.Shell.Api/CarMD.Shell.Api.csproj"
COPY . .
WORKDIR "/src/CarMD.Shell.Api"
RUN dotnet publish "CarMD.Shell.Api.csproj" --no-restore -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CarMD.Shell.Api.dll"]