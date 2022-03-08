#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM microsoft/dotnet:2.1-aspnetcore-runtime-nanoserver-1803 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk-nanoserver-1803 AS build
WORKDIR /src
COPY ["BPCloud.VendorAPIGateWay/BPCloud.VendorAPIGateWay.csproj", "BPCloud.VendorAPIGateWay/"]
RUN dotnet restore "BPCloud.VendorAPIGateWay/BPCloud.VendorAPIGateWay.csproj"
COPY . .
WORKDIR "/src/BPCloud.VendorAPIGateWay"
RUN dotnet build "BPCloud.VendorAPIGateWay.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "BPCloud.VendorAPIGateWay.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "BPCloud.VendorAPIGateWay.dll"]