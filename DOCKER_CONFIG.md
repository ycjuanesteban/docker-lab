
## Instalación de docker

Para instalar docker se debe ingresar a la página de *Docker Hub* y descargarse el [instalador][link_docket_hub].

La instalación es el tipo "siguiente siguiente"

![[image_docker_instalacion]: /images/docker_instalacion.png
][image_docker_instalacion]

Es posible que les pida reiniciar el equipo cuando se finalice la instalación.

Para comprobar que la instalación se complemeto correctamente, se abre la consola de windows y se ejecuta el comando `docker version`

![image_docker_version][image_docker_version]


## Configuración de Dockerfile para cada aplicación

## React

Se debe crear un archivo `Dockerfile` en la raíz de la carpeta donde se creó el proyecto de React 

![react_app_dockerfile][react_app_dockerfile]

y luego se incluye el siguiente código

``` docker
# se indica qué imagen se va utilizar como base
FROM node:current-slim

# se crea un directorio de trabajo 
WORKDIR /react/app

# se copia el archivo package.json en el directorio de trabajo
COPY package.json .

# se ejecuta la instalación de los paquetes necesarios
RUN npm install

# se copia todo el contenido de la carpeta raiz del proyecto al directorio de trabajo de la imagen
COPY . .

# se ejecuta el comando para dar inicio a la aplicación
CMD ["npm", "start"]
```

## .Net Core

Se debe crear un archivo `Dockerfile` en la raíz de la carpeta donde se creó el proyecto de .Net

![dotnet_core_dockerfile][dotnet_core_dockerfile]

luego se debe incluír el siguiente código.

```docker
# se indica qué imagen se va utilizar como base
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base

# se crea un directorio de trabajo 
WORKDIR /app

# se crea un puerto por defecto y se expone
ENV ASPNETCORE_URLS http://+:4242
EXPOSE 4242

# se indica la imagen base
FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS publish
# se crea un directorio de trabajo
WORKDIR /src
#se copia el archivo .csproj a la Service
COPY ["Service/Service.csproj", "Service/"]
# se hace la restauración de los nuggets
RUN dotnet restore "Service/Service.csproj"
# se copia todo el contenido de la carpeta de la aplicación a directorio de trabajo
COPY . .
# se indica la ruta del nuevo directorio de trabajo
WORKDIR "/src/Service"
# se ejecuta compila la aplicación para publicar
RUN dotnet publish "Service.csproj" -c Release -o /app/publish

# se usa como referecian la imagen del inicio para crear la imagen final
FROM base AS final
# se indica el directorio de trabajo
WORKDIR /app
# se copia toda la información del paso anterior 
COPY --from=publish /app/publish .
# finalmente se ejecuta la aplicación
ENTRYPOINT ["dotnet", "Service.dll"]
```

Al finalizar estos pasos se tienen ya listos los archivos para comenzar a trabajar con *Docker*.

## Creación de imagenes y contenedores



[link_docket_hub]: https://hub.docker.com/editions/community/docker-ce-desktop-windows/

[image_docker_instalacion]: /images/docker_instalacion.png
[image_docker_version]: /images/docker_version.png
[react_app_dockerfile]: /images/react_app_dockerfile.png
[dotnet_core_dockerfile]: /images/dotnet_core_dockerfile.png



[<- Volver al inicio](README.md)