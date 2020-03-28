
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

Al finalizar estos pasos se tienen listos los archivos para comenzar a trabajar con *Docker*.

## Creación de imagenes y contenedores

Para iniciar con el proceso de la creación de las imagenes, se debe abrir una terminal y dirigirse a la carpeta raíz de cada una de las aplicaciones.

luego ejecutar el comando 
```
docker build . -t nombre_app
```

Donde nombre_app para la aplicación de React será `react_app` y para la aplicación de .Net core será `dotnet_app`.

Para ambas aplicaciones el proceso es muy similar, docker lee el archivo `Dockerfile` y comienza a ejecutar cada uno de los comandos que se tienen en el archivo comenzando por descargar la imagen base, pasando por la instalacion de las dependencias y paquetes y finalizando con la ejecución del comando de inicio para cada aplicación.

![docker_build][docker_build]

Al finalizar ambos procesos y para comprobar que todo salio bien, se ejecuta en una consola el comando `docker image ls` que listará las imágenes que tiene el sistema.

![docker_image_ls][docker_image_ls]

Cuando ya se tienen las imágenes en el sistema, lo siguiente es crear los contenedores teniendo como base las imágenes anteriormente creadas.

Para crear un contenedor, docker cuenta con el comando `docker run`.

Para crear cada contenedor procedemos con los siguientes comandos

### React
``` cmd
docker run -d -p 4241:3000 react_app
```

### .Net Core
``` cmd
docker run -d -p 4242:4242 react_app
```
Donde el parámetro `-d` nos sirve para ejecutar los contenedores aislados del proceso principal y el parametro `-p` nos indica que puertos se van habiliar en el siguiente orden *host : contenedor*.

### Consideraciones sobre los puertos.

Al momento de seleccionar los puertos que habilita el conentenedor se tiene que tener muy en cuenta el puerto que se habilita al momento de ejecuar cada una de las aplicaciones; en el caso de la aplicación de React como se está ejecutando con el comando *npm start* esta se inicia por defecto en el puerto *3000* y en el caso de la aplicación de .Net core en el archivo *Dockerfile* se le indicó en la variable de entorno *ASPNETCORE_URLS* que el puerto para exponer la aplicación era el *4242*.

El resultado final es el siguiente:

Aplicación de react ejecutandose desde el contenedor

![app_react][app_react]

Aplicación de .net core ejecutandose desde el contenedor

![app_dotnet_core][app_dotnet_core]


[link_docket_hub]: https://hub.docker.com/editions/community/docker-ce-desktop-windows/

[image_docker_instalacion]: /images/docker_instalacion.png
[image_docker_version]: /images/docker_version.png
[react_app_dockerfile]: /images/react_app_dockerfile.png
[dotnet_core_dockerfile]: /images/dotnet_core_dockerfile.png
[docker_build]: /images/docker_build.png
[docker_image_ls]: /images/docker_image_ls.png
[app_react]: /images/app_react.png
[app_dotnet_core]: /images/app_dotnet_core.png


[<- Volver al inicio](README.md)