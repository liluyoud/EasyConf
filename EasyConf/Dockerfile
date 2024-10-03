FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["EasyConf/EasyConf.csproj", "EasyConf/"]
RUN dotnet restore "./EasyConf/EasyConf.csproj"

COPY . .
WORKDIR /src/EasyConf
RUN dotnet build "./EasyConf.csproj" -c Release -o /app/build
FROM build AS publish
RUN dotnet publish "./EasyConf.csproj" -c Release -o /app/publish

FROM nginx:alpine
WORKDIR /usr/share/nginx/html
RUN rm -rf ./*
COPY --from=publish /app/publish/wwwroot .
COPY EasyConf/nginx.conf /etc/nginx/nginx.conf
EXPOSE 8080 
ENTRYPOINT ["nginx", "-g", "daemon off;"]
