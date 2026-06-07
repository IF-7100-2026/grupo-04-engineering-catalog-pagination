# Entorno de Base de Datos de Catálogo (PostgreSQL)

## Descripción General
Este repositorio contiene la infraestructura y los scripts de inicialización necesarios para desplegar una instancia local de PostgreSQL mediante Docker. El entorno está optimizado para alojar la tabla `catalog_parts` y ejecutar pruebas de rendimiento sobre 1.000.000 de registros sintéticos. El objetivo principal es analizar la eficiencia de las consultas estructuradas utilizando índices B-tree y GIN.

## Estructura del Directorio
El directorio `database/` se organiza de la siguiente manera:

* `docker-compose.yml`: Define la configuración del contenedor de la base de datos, mapeo de puertos y volúmenes persistentes.
* `schema.sql`: Establece la estructura de la tabla `catalog_parts` y habilita la extensión `pg_trgm`. Se ejecuta automáticamente en el primer despliegue.
* `indexes.sql`: Define los índices B-tree (para métricas exactas, fechas y UUID) y GIN (para búsqueda de texto completo). Se ejecuta automáticamente en el primer despliegue.
* `seed_data_generator.sql`: Script de inserción masiva que utiliza `generate_series` para poblar la base de datos con un millón de entradas.

## Requisitos Previos
Para ejecutar este entorno, el sistema anfitrión (ya sea una distribución Linux o similar) debe contar con:
1. **Docker Engine** y **Docker Compose** instalados.
2. Acceso a una terminal con privilegios suficientes para la gestión de contenedores.
3. Un cliente de administración de bases de datos (como DBeaver o `psql`) para las consultas y la gestión remota.

## Instrucciones de Despliegue e Inicialización

**1. Levantar la Infraestructura**
Navegue a la raíz del repositorio y ejecute el siguiente comando para iniciar el contenedor en segundo plano:

docker-compose up -d

Nota: Si es la primera vez que se levanta el contenedor y el volumen de datos está vacío, PostgreSQL ejecutará automáticamente los scripts `schema.sql` e `indexes.sql`.*

**2. Población de Datos (Seed)**
Una vez que el contenedor esté en funcionamiento y aceptando conexiones, inyecte el millón de registros ejecutando:

```bash
docker exec -i wayne_catalog_db psql -U batman -d catalog_db < database/seed_data_generator.sql

```

Este proceso puede tomar unos instantes dependiendo de los recursos del sistema anfitrión.

## Colaboración y Acceso Remoto

Para el trabajo conjunto en la extracción de datos y la optimización del backend, el acceso a esta base de datos local puede compartirse mediante una red superpuesta (como Tailscale).

El apartado de desarrollo puede conectarse utilizando las siguientes credenciales en su cliente de preferencia (por ejemplo, DBeaver):

* **Host:** [Dirección IP de la red superpuesta del anfitrión]
* **Puerto:** 5432
* **Base de datos:** `catalog_db` (o el valor definido en `.env`)
* **Usuario:** `batman` (o el valor definido en `.env`)
* **Contraseña:** `iamvengeance` (o el valor definido en `.env`)

## Guía para la Demostración de Métricas

Para presentar el progreso del proyecto, validar la eficiencia de la base de datos y demostrar el uso de los índices ante terceros, utilice el siguiente flujo de comandos a través de la terminal:

**Acceso interactivo:**

```bash
docker exec -it wayne_catalog_db psql -U batman -d catalog_db

```

**Verificación de uso de índices:**

```sql
SELECT 
    indexrelname AS index_name, 
    idx_scan AS number_of_scans,
    idx_tup_read AS tuples_read,
    idx_tup_fetch AS tuples_fetched
FROM 
    pg_stat_user_indexes
WHERE 
    relname = 'catalog_parts'
ORDER BY 
    idx_scan DESC;

```

**Análisis de planes de ejecución:**
Para demostrar la optimización de una consulta específica frente a un escaneo secuencial, anteponga el comando `EXPLAIN ANALYZE` a cualquier consulta de prueba:

```sql
EXPLAIN ANALYZE SELECT * FROM catalog_parts WHERE weight_kg > 250 AND size_meters < 5.00;

```
