# Engineering Catalog Pagination

## Contexto

Wayne Industries necesita un catálogo web para consultar partes de ingeniería utilizadas en proyectos relacionados con barcos, aviones y otros vehículos.

El sistema trabaja con más de un millón de registros, por lo que no es conveniente cargar todos los datos al mismo tiempo en el frontend. Para resolver esto, se implementó una paginación desde el backend.

El frontend solo muestra los registros que recibe desde la API.

## Problema a resolver

Se debe implementar la paginación en el backend del catálogo.

Cada solicitud debe devolver solo una parte de los registros, según los parámetros enviados por el usuario:

```http
GET /api/parts?page=0&size=10
```

El rendimiento se mide con el tiempo que tarda el backend en devolver los registros. Para la kata, una solicitud de 300 registros debe responder en menos de 1 segundo.

## Tecnologías utilizadas

* .NET Web API
* PostgreSQL
* Entity Framework Core
* HTML
* CSS
* JavaScript
* Swagger

## Cómo instalarlo

Primero, se clona el repositorio:

```bash
git clone https://github.com/IF-7100-2026/grupo-04-engineering-catalog-pagination.git
```

Luego, se entra a la carpeta del proyecto:

```bash
cd grupo-04-engineering-catalog-pagination
```

Después, se restauran las dependencias:

```bash
dotnet restore
```

Y se compila el proyecto:

```bash
dotnet build
```

## Configuración de la base de datos

La conexión a PostgreSQL se configura en el archivo:

```txt
WaynePartsCatalog.Api/appsettings.json
```

Ejemplo:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=HOST;Port=5432;Database=catalog_db;Username=USER;Password=PASSWORD"
  }
}
```

La tabla utilizada es:

```txt
catalog_parts
```

## Cómo ejecutarlo

Para levantar el backend, se utiliza:

```bash
dotnet run --project WaynePartsCatalog.Api
```

Luego, se puede abrir el frontend en el navegador:

```txt
http://localhost:5240
```

También se puede acceder a Swagger desde:

```txt
http://localhost:5240/swagger
```

## Cómo utilizarlo

El endpoint principal es:

```http
GET http://localhost:5240/api/parts?page=0&size=10
```

Ejemplos:

```http
GET http://localhost:5240/api/parts?page=0&size=5
```

```http
GET http://localhost:5240/api/parts?page=1&size=5
```

```http
GET http://localhost:5240/api/parts?page=0&size=300
```

## Validaciones

La API valida que:

* `page` no sea menor que 0.
* `size` no sea menor que 1.
* `size` no sea mayor que 300.

Ejemplo de error:

```json
{
  "message": "Page size cannot be greater than 300."
}
```
