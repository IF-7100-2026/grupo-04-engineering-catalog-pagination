Universidad de Costa Rica

Sede del Atlántico

Sección de Informática Empresarial

\- PROYECTO DE CURSO  \-

Proyecto de Investigacion: Implementacion de paginación backend para catálogo de partes

IF-7100  \- Ingeniería de Software 

I N T E G R A N T E S

MICHAEL BONILLA ESPINOZA      C38843

KATHERINE BRENES ARCE	    C31269

JOSÉ DAVID CALVO GAMBOA       C31492

D O C E N T E

MSc. Leonardo Camacho Navarro

I SEMESTRE 2026

# Tabla de Contenido

[Introducción	4](#introducción)

[Objetivos	5](#objetivos)

[Objetivo general	5](#objetivo-general)

[Objetivos específicos	5](#objetivos-específicos)

[Tecnologías utilizadas	5](#tecnologías-utilizadas)

[.NET Web API	5](#.net-web-api)

[PostgreSQL	6](#postgresql)

[Entity Framework Core	6](#entity-framework-core)

[HTML y JavaScript	7](#html-y-javascript)

[Problema Propuesto	7](#problema-propuesto)

[Descripción del problema	7](#descripción-del-problema)

[Problema de paginación	8](#problema-de-paginación)

[Diseño de la Solución	9](#diseño-de-la-solución)

[Arquitectura general de la solución	9](#arquitectura-general-de-la-solución)

[Componentes principales	10](#componentes-principales)

[EngineeringPart	10](#engineeringpart)

[AppDbContext	11](#appdbcontext)

[PartResponseDto	11](#partresponsedto)

[PaginatedResponseDto	11](#paginatedresponsedto)

[PartService	12](#partservice)

[PartsController	12](#partscontroller)

[Frontend en HTML y JavaScript	12](#frontend-en-html-y-javascript)

[Endpoint de paginación	13](#endpoint-de-paginación)

[Validaciones implementadas	14](#validaciones-implementadas)

[Resultados preliminares de pruebas	15](#resultados-preliminares-de-pruebas)

[Manejo de base de datos	16](#manejo-de-base-de-datos)

[PostgreSQL	16](#postgresql-1)

[Modelo de datos	16](#modelo-de-datos)

[Generación de 1 millón de registros	17](#generación-de-1-millón-de-registros)

[Índices y optimización	18](#índices-y-optimización)

[Diagrama entidad-relación	19](#diagrama-entidad-relación)

[Implementación de filtros dinámicos	21](#implementación-de-filtros-dinámicos)

[Diseño de filtros	21](#diseño-de-filtros)

[Tipos de filtros	22](#tipos-de-filtros)

[Filtros de rango específico de búsqueda	22](#filtros-de-rango-específico-de-búsqueda)

[Fechas	22](#fechas)

[Timestamp	23](#timestamp)

[Numéricos enteros	23](#numéricos-enteros)

[Numéricos decimales	23](#numéricos-decimales)

[Filtro de coincidencia exacta para cadenas cortas	23](#filtro-de-coincidencia-exacta-para-cadenas-cortas)

[Filtro de coincidencia parcial sobre descripciones	24](#filtro-de-coincidencia-parcial-sobre-descripciones)

[Integración con paginación	24](#integración-con-paginación)

[Combinación de filtros	25](#combinación-de-filtros)

[Plan de pruebas	25](#plan-de-pruebas)

[Resultados Obtenidos	28](#resultados-obtenidos)

[Resultados manuales	28](#resultados-manuales)

[Resultados automáticos	29](#resultados-automáticos)

[Métricas de rendimiento	30](#métricas-de-rendimiento)

[Análisis Arquitectónico	31](#análisis-arquitectónico)

[Performance	31](#performance)

[Maintainability	32](#maintainability)

[Testability	32](#testability)

[Usability	33](#usability)

[10.5 Scalability	33](#10.5-scalability)

[Conclusiones	36](#conclusiones)

# 

# 

# 

# 

# 

# Introducción {#introducción}

Este proyecto nace a partir de la kata de paginación, la cual plantea un problema bastante común cuando se desarrollan sistemas que trabajan con una cantidad enorme de datos. En este caso, el escenario propuesto se basa en Wayne Industries, una empresa que necesita un catálogo web para que su departamento de ingeniería pueda consultar partes utilizadas en proyectos relacionados con barcos, aviones y otros vehículos.

El punto importante del problema es que el catálogo no tiene pocos registros. Al contrario, se trabaja con más de un millón de partes, por lo que no tendría sentido cargar toda esa información cada vez que un usuario entra al sistema. Aunque una base de datos como PostgreSQL puede almacenar esa cantidad de datos sin problema, enviar todo al frontend en una sola respuesta sería una mala decisión desde el punto de vista técnico.

Si el frontend recibiera el millón de registros completo, la aplicación se volvería mucho más pesada. El backend tendría que consultar y preparar demasiada información, la respuesta HTTP sería muy grande y el navegador tendría que procesar una cantidad de datos que el usuario probablemente ni siquiera va a revisar completa. Esto afectaría tanto el rendimiento como la experiencia de uso.

Por eso, la solución se enfocó en implementar paginación desde el backend. Con este enfoque, el usuario solicita una página específica y una cantidad determinada de registros, y el backend responde únicamente con esa parte del catálogo. Por ejemplo, puede devolver 10 registros, 50 registros o hasta 300 registros por página, según el parámetro enviado.

Para esta implementación se utilizaron .NET Web API, PostgreSQL, Entity Framework Core, HTML y JavaScript. .NET Web API se usó para construir el backend y exponer el endpoint de consulta; PostgreSQL se utilizó para almacenar los datos del catálogo; Entity Framework Core permitió conectar el backend con la base de datos de una forma más ordenada; y HTML con JavaScript sirvió para crear una interfaz mínima desde la cual se pudiera probar visualmente la paginación.

El enfoque principal no fue crear una aplicación visualmente compleja, sino demostrar que la paginación funciona correctamente desde el backend y que el sistema puede responder en menos de un segundo cuando se solicitan 300 registros, que es el benchmark planteado por la kata.

# Objetivos {#objetivos}

## Objetivo general {#objetivo-general}

Implementar una solución de paginación desde el backend para un catálogo de partes de ingeniería, utilizando .NET Web API y PostgreSQL, con el fin de controlar la cantidad de registros enviados al frontend y comprobar si las consultas paginadas mantienen un tiempo de respuesta adecuado.

## Objetivos específicos {#objetivos-específicos}

1. Crear una API backend capaz de consultar registros desde una base de datos PostgreSQL de forma paginada.  
2. Implementar un endpoint HTTP que reciba los parámetros page y size, para indicar qué página se quiere consultar y cuántos registros se desean recibir.  
3. Aplicar la paginación directamente en la consulta hacia la base de datos, evitando cargar todos los registros en memoria antes de responder.  
4. Crear clases DTOs para devolver una respuesta más clara al frontend y no exponer directamente el modelo interno de la base de datos.  
5. Validar parámetros básicos de paginación, como el número de página y el tamaño de página, para evitar solicitudes incorrectas.  
6. Medir el tiempo de respuesta del backend, ya que esta métrica permite comprobar si la solución cumple con el criterio de rendimiento establecido por la kata.  
7. Crear una interfaz mínima con HTML y JavaScript para consumir el endpoint y visualizar los resultados sin tener que probar todo solamente desde el navegador o Swagger.  
8. Dejar la estructura preparada para que posteriormente se puedan agregar filtros dinámicos sobre el catálogo sin tener que rehacer la lógica principal de paginación.

# Tecnologías utilizadas {#tecnologías-utilizadas}

## .NET Web API {#.net-web-api}

.NET Web API fue la tecnología utilizada para construir el backend del proyecto. En este caso, se necesitaba una API que pudiera recibir solicitudes desde el frontend, comunicarse con la base de datos y devolver una respuesta en formato JSON. Por esa razón, .NET Web API encajaba bien con la solución, ya que permite crear endpoints HTTP de forma ordenada y trabajar con una estructura clara.

Dentro del proyecto, la API funciona como un puente entre la interfaz y PostgreSQL. El frontend no se conecta directamente a la base de datos, sino que envía una solicitud al backend. Luego, el backend interpreta los parámetros recibidos, aplica las validaciones necesarias, consulta la base de datos y devuelve solamente los registros correspondientes a la página solicitada.

Una ventaja importante de usar .NET Web API es que permite dividir el proyecto en partes más fáciles de entender. En nuestra implementación, el controlador recibe las solicitudes HTTP, el servicio contiene la lógica principal de paginación y el contexto de datos se encarga de la comunicación con PostgreSQL. Esta separación ayuda bastante, porque evita que todo el código quede mezclado en una sola clase.

También se utilizó Swagger durante el desarrollo. Esto fue útil porque permitió probar el endpoint sin depender siempre del frontend. Desde Swagger se podían enviar diferentes valores para page y size, revisar la respuesta de la API y comprobar si las validaciones estaban funcionando correctamente.

En el proyecto, el endpoint principal se expone desde PartsController, mientras que la lógica de consulta paginada está en PartService. Esta decisión hace que el controlador se mantenga más simple y que el servicio sea el lugar donde se concentre el trabajo más importante de paginación.

## PostgreSQL {#postgresql}

PostgreSQL fue utilizado como sistema de base de datos relacional para almacenar el catálogo de partes. En este proyecto, la tabla principal utilizada fue catalog\_parts, la cual contiene un millón de registros simulados. Esto permitió probar la solución con una cantidad muy grande de datos, parecida al escenario que plantea la kata.

La tabla no contiene únicamente campos simples. Su estructura incluye diferentes tipos de datos, tal como lo solicitaba el problema. Entre ellos hay una fecha, un timestamp, un número entero, un número decimal, cadenas cortas y una descripción larga. Esto hace que la base de datos sea más realista y también deja espacio para agregar filtros en una etapa posterior.

PostgreSQL permite trabajar con muchísimos registros y ejecutar consultas paginadas mediante operaciones como OFFSET y LIMIT. En nuestro caso, estas operaciones no se escribieron manualmente en SQL, sino que fueron generadas por Entity Framework Core a partir de los métodos Skip y Take.

Esto nos permitió mantener el código del backend más legible, sin perder el beneficio de que la paginación se aplicará directamente en la consulta hacia la base de datos.

## Entity Framework Core {#entity-framework-core}

Entity Framework Core se utilizó como herramienta de acceso a datos. Su función principal fue permitir que el backend trabajara con clases de C\# en lugar de escribir todas las consultas SQL directamente. Esto ayuda a que el código sea más fácil de leer y también más cómodo de mantener.

En este proyecto, el modelo EngineeringPart representa los registros de la tabla catalog\_parts. Gracias a este modelo, el backend puede trabajar con las partes del catálogo como objetos de C\#, aunque realmente los datos estén almacenados dentro de PostgreSQL.

El archivo AppDbContext se encarga de definir el mapeo entre el modelo y la tabla real. Ahí se indica el nombre de la tabla, la llave primaria y el nombre de cada columna. Esto fue necesario porque las columnas de la base de datos utilizan nombres en formato snake\_case, como part\_id, mientras que las propiedades del modelo en C\# utilizan PascalCase, como PartId.

También se utilizó AsNoTracking() en la consulta principal. Esto fue una decisión útil porque el endpoint solamente consulta datos, no los modifica. Al no rastrear cambios en las entidades recuperadas, Entity Framework Core realiza menos trabajo interno, lo cual ayuda a que las consultas de solo lectura sean más eficientes.

## HTML y JavaScript {#html-y-javascript}

HTML y JavaScript se utilizaron para crear un frontend mínimo que permitiera probar la paginación desde una interfaz visual. La intención no era hacer una aplicación completa con muchas pantallas, sino tener una página sencilla donde se pudiera ver si el backend estaba enviando correctamente los datos paginados.

La interfaz permite seleccionar la cantidad de registros por página, avanzar o retroceder entre páginas y observar información adicional, como la página actual, el total de registros y el tiempo de respuesta del backend. Esto ayuda a comprobar de forma más clara que la paginación realmente se está haciendo desde el servidor.

JavaScript utiliza fetch para enviar solicitudes al endpoint /api/parts. Cada vez que el usuario cambia la cantidad de registros por página o presiona los botones de navegación, el frontend construye una nueva URL con los parámetros page y size, y vuelve a consultar la API.

Un punto importante es que el frontend no pagina los datos por su cuenta. Es decir, no recibe el millón de registros para luego dividirlos en JavaScript. El frontend solamente muestra lo que el backend le envía. Esto confirma que la responsabilidad fuerte de la paginación queda en el backend, como lo solicitaba la kata.

# Problema Propuesto {#problema-propuesto}

## Descripción del problema {#descripción-del-problema}

El problema propuesto por la kata se basa en una situación bastante común en sistemas reales: tener que trabajar con una grandísima cantidad de datos sin afectar el rendimiento de la aplicación. En este caso, el catálogo maneja más de un millón de partes de ingeniería. Aunque la base de datos puede almacenar esa información, no sería práctico enviarla completa al frontend cada vez que el usuario quiera consultar el catálogo.

Si el sistema intenta cargar todos los registros al mismo tiempo, aparecen varios problemas. El backend tendría que consultar, procesar y serializar demasiados datos. Además, la respuesta que viaja por la red sería mucho más grande de lo necesario. Después de eso, el navegador tendría que recibir toda esa información, guardarla en memoria y tratar de mostrarla en pantalla.

En la práctica, eso haría que la aplicación se sintiera lenta y pesada. Además, la mayoría de usuarios no necesita ver un millón de registros de una sola vez. Normalmente se revisa una página, se avanza a la siguiente o se cambia la cantidad de registros visibles. Por eso, enviar todo desde el inicio no sería una buena decisión técnica.

La paginación resuelve este problema dividiendo el conjunto completo de datos en páginas más pequeñas. El usuario solicita una página específica y define cuántos registros quiere recibir. El backend responde únicamente con ese bloque de datos y agrega información adicional para que el frontend sepa si hay más páginas disponibles.

En otras palabras, la paginación permite que el sistema trabaje con una cantidad enorme de información, pero sin obligar al usuario ni al navegador a cargarla toda de una sola vez.

## Problema de paginación {#problema-de-paginación}

La paginación backend consiste en aplicar el límite de registros desde el servidor antes de enviar la respuesta al frontend. Esto significa que la base de datos devuelve únicamente los registros necesarios para la página solicitada. De esta forma, el backend no trae todos los datos para después acortarlos, sino que consulta directamente el segmento que se necesita.

El endpoint implementado recibe dos parámetros principales:

GET /api/parts?page=0\&size=10

El parámetro page indica el número de página solicitada. En esta implementación, la numeración inicia en cero. Por eso, page=0 representa la primera página, page=1 representa la segunda página, y así sucesivamente.

El parámetro size indica cuántos registros se quieren recibir por página. Si no se envía este parámetro, el endpoint utiliza 10 registros por defecto. Sin embargo, el usuario puede solicitar otros tamaños, como 5, 50, 100 o 300 registros.

La consulta se implementa utilizando Skip y Take:

.Skip(page \* size)

.Take(size)

Skip se encarga de omitir los registros que pertenecen a páginas anteriores. Por ejemplo, si se solicita la segunda página con un tamaño de 10 registros, el backend omite los primeros 10\. Luego, Take limita la cantidad de registros que se devuelven, según el valor de size.

Este enfoque permite que la consulta se aplique directamente sobre la base de datos. Eso es importante porque evita cargar información de más y mantiene la respuesta más ligera. Incluso con una tabla de un millón de registros, el backend solo devuelve la cantidad solicitada por el usuario.

Además, la respuesta no solo incluye los registros. También incluye datos como el total de elementos, el total de páginas, si existe una página siguiente, si existe una página anterior y el tiempo que tardó el backend en responder. Esto hace que el frontend pueda mostrar información útil sin tener que calcularla por su cuenta.

# Diseño de la Solución {#diseño-de-la-solución}

## Arquitectura general de la solución {#arquitectura-general-de-la-solución}

Para mantener el código ordenado, la solución se dividió en capas simples. La idea fue que cada parte del proyecto tuviera una responsabilidad clara y que no todo quedara mezclado en una sola clase.

El flujo general de la solución es el siguiente:

![][image1]

*Figura \#1: Arquitectura / Flujo general de la solución .*

El proceso inicia cuando el usuario interactúa con el frontend. Por ejemplo, puede cambiar la cantidad de registros por página o presionar el botón de siguiente página. En ese momento, JavaScript envía una solicitud HTTP al endpoint /api/parts, incluyendo los valores de page y size.

Luego, PartsController recibe la solicitud. El controlador no hace directamente la consulta a la base de datos, sino que llama a PartService. Esta separación es importante porque permite que el controlador se mantenga sencillo y que la lógica principal esté concentrada en el servicio.

Después, PartService valida los parámetros recibidos, construye la consulta, mide el tiempo de ejecución, aplica la paginación y convierte los resultados en DTOs. Finalmente, AppDbContext es el componente que se comunica con PostgreSQL para obtener los datos desde la tabla catalog\_parts.

Esta arquitectura ayuda a mantener el proyecto más limpio. Si en el futuro se necesita cambiar la lógica de paginación o agregar filtros, el cambio se haría principalmente en el servicio y no en todo el proyecto. También facilita la lectura del código, porque cada archivo cumple una función específica.

## Componentes principales  {#componentes-principales}

### EngineeringPart {#engineeringpart}

EngineeringPart es el modelo que representa una parte de ingeniería dentro del catálogo. Esta clase contiene las propiedades que corresponden con las columnas de la tabla catalog\_parts.

Entre sus propiedades se encuentran:

* PartId  
* ManufactureDate  
* RegistrationTimestamp  
* WeightKg  
* SizeMeters  
* PartType  
* Material  
* LongDescription

Este modelo permite que Entity Framework Core trabaje con los registros de PostgreSQL como objetos de C\#. Es decir, aunque los datos realmente están en una tabla, el backend puede consultarlos y manipularlos usando una clase del proyecto.

### AppDbContext {#appdbcontext}

AppDbContext representa el contexto de datos de la aplicación. Su función es conectar los modelos de C\# con las tablas de PostgreSQL. En este archivo se define que EngineeringPart corresponde a la tabla catalog\_parts.

También se especifica cuál es la llave primaria y cuál es el nombre real de cada columna en la base de datos. Esto fue necesario porque los nombres de la base de datos no son exactamente iguales a los nombres usados en el modelo. Por ejemplo, la base usa nombres como part\_id, mientras que en C\# se utiliza PartId.

Este mapeo permite que el resto del código trabaje de forma más cómoda, sin perder la relación con la estructura real de la base de datos.

### PartResponseDto {#partresponsedto}

PartResponseDto define la forma en que cada registro se devuelve al frontend. En vez de enviar directamente el modelo de base de datos, se utiliza un DTO para controlar mejor la respuesta.

Esto es útil porque separa la estructura interna del backend de la estructura que consume el frontend. Si en el futuro se cambia el modelo de base de datos, no necesariamente tendría que cambiar la respuesta de la API.

### PaginatedResponseDto {#paginatedresponsedto}

PaginatedResponseDto\<T\> define la estructura general de una respuesta paginada. Esta clase contiene tanto la lista de registros como la información necesaria para que el frontend pueda manejar la navegación entre páginas.

Los campos principales son:

Content

Page

Size

TotalElements

TotalPages

HasNext

HasPrevious

ResponseTimeMs

El campo Content contiene los registros devueltos. Page y Size indican la página y el tamaño usados en la consulta. TotalElements y TotalPages permiten saber cuántos registros existen y cuántas páginas se pueden recorrer. HasNext y HasPrevious ayudan a habilitar o deshabilitar los botones de navegación. Finalmente, ResponseTimeMs muestra cuánto tardó el backend en procesar la consulta.

Este último dato es especialmente importante para el proyecto, porque permite evaluar si la implementación cumple con el criterio de rendimiento.

### PartService {#partservice}

PartService contiene la lógica principal de la paginación. Este servicio valida los parámetros, construye la consulta base, cuenta los registros totales, aplica Skip y Take, convierte los resultados a DTOs y calcula el total de páginas.

Concentrar esta lógica en un servicio hace que el código sea más fácil de mantener. Si la lógica estuviera directamente en el controlador, el endpoint sería más difícil de leer y de modificar. En cambio, al tener un servicio, el controlador solo se encarga de recibir la solicitud y devolver la respuesta.

Además, este archivo es el punto principal donde se podrían agregar filtros dinámicos más adelante. La consulta base ya está ubicada antes del conteo total y antes de la paginación, lo cual permite extenderla sin romper el comportamiento actual.

### PartsController {#partscontroller}

PartsController expone el endpoint HTTP utilizado por el frontend. Este controlador recibe los parámetros page y size, llama al servicio y devuelve la respuesta generada.

También maneja errores básicos de validación. Si el usuario envía una página negativa o un tamaño inválido, el servicio lanza una excepción controlada y el controlador responde con un BadRequest. Esto evita que el sistema devuelva errores desordenados o difíciles de entender para quien consume la API.

### Frontend en HTML y JavaScript {#frontend-en-html-y-javascript}

El frontend se encuentra en la carpeta wwwroot. Esta interfaz permite probar visualmente el funcionamiento de la paginación. Incluye un selector para cambiar la cantidad de registros por página, botones para avanzar o retroceder y una tabla donde se muestran los datos recibidos.

Además de mostrar los registros, el frontend enseña información útil como la página actual, el total de registros y el tiempo de respuesta del backend. Esto permite comprobar rápidamente si la API está respondiendo bien y si la paginación se está aplicando correctamente.

La interfaz es mínima, pero cumple su propósito: demostrar que el frontend solo muestra los datos que el backend le envía.

## Endpoint de paginación {#endpoint-de-paginación}

El endpoint principal implementado fue:

GET /api/parts?page=0\&size=10

Este endpoint permite consultar el catálogo de partes de forma paginada. Si no se envían parámetros, se utilizan valores por defecto: page=0 y size=10.

La respuesta tiene la siguiente estructura:

{

  "content": \[\],

  "page": 0,

  "size": 10,

  "totalElements": 1000000,

  "totalPages": 100000,

  "hasNext": true,

  "hasPrevious": false,

  "responseTimeMs": 327

}

El campo content contiene los registros de la página solicitada. Los campos page y size indican los parámetros usados para la consulta. totalElements indica cuántos registros existen en total. totalPages muestra cuántas páginas se generan según el tamaño solicitado. hasNext y hasPrevious permiten saber si se puede avanzar o retroceder. Por último, responseTimeMs indica cuánto tardó el backend en procesar la solicitud.

Esta estructura hace que el frontend sea más simple. No necesita calcular por sí mismo el total de páginas ni decidir manualmente si debe habilitar los botones. Solo recibe la respuesta del backend y actualiza la interfaz.

## Validaciones implementadas {#validaciones-implementadas}

Se implementaron validaciones para evitar solicitudes incorrectas. Las reglas utilizadas fueron:

page \>= 0

size \>= 1

size \<= 300

La primera regla evita que el usuario solicite páginas negativas. La segunda evita tamaños de página menores a uno, ya que no tendría sentido pedir una página con cero registros. La tercera limita el máximo de registros por página a 300, que es el valor usado para el benchmark de la kata.

Cuando una solicitud no cumple estas reglas, el backend responde con un error controlado. Por ejemplo, si se solicita:

GET /api/parts?page=-1\&size=10

La respuesta es:

{

  "message": "Page number must be greater than or equal to 0."

}

Y si se solicita:

GET /api/parts?page=0\&size=301

La respuesta es:

{

  "message": "Page size cannot be greater than 300."

}

Estas validaciones hacen que el endpoint sea más seguro y predecible. También ayudan a que las pruebas sean más claras, porque se puede verificar fácilmente qué ocurre cuando se envían valores correctos o incorrectos.

### Resultados preliminares de pruebas {#resultados-preliminares-de-pruebas}

Se realizaron pruebas manuales para comprobar que la paginación funcionara correctamente, que la navegación entre páginas cambiara los registros, que las validaciones respondieran bien y que el rendimiento estuviera dentro del tiempo esperado.

| Prueba | Parámetros | Resultado | Tiempo |
| :---- | :---- | :---- | :---- |
| Página inicial pequeña | page=0\&size=5 | Devuelve 5 registros | 327 ms |
| Segunda página pequeña | page=1\&size=5 | Devuelve otros 5 registros | 375 ms |
| Página inválida | page=-1\&size=10 | Error controlado | — |
| Tamaño inválido | page=0\&size=301 | Error controlado | — |
| Benchmark | page=0\&size=300 | Devuelve 300 registros | 469 ms |

*Tabla \#1: Resultados preliminares de pruebas manuales para comprobar la paginación*

El resultado más importante es el benchmark con page=0\&size=300. La kata establece que una solicitud paginada de 300 registros debe responder en menos de 1 segundo. En la prueba realizada, el backend respondió en 469 ms, por lo que la solución cumple con la meta de rendimiento.

Estos resultados también muestran que la paginación no solo funciona para páginas pequeñas, sino también para el tamaño máximo permitido en la implementación.

# Manejo de base de datos {#manejo-de-base-de-datos}

## PostgreSQL {#postgresql-1}

	PostgreSQL es un sistema de manejo de bases de datos relacional reconocido por su confiabilidad, integridad de datos y estricto apegamiento a estándares de SQL. Da soporte por completo a las propiedades ACID (atomicidad, consistencia, ‘isolation’, durabilidad), permitiendo que cada transacción se maneje de una manera confiable incluso cuando se presenta alta concurrencia. Las mayores ventajas de PostgreSQL que se alinean con la estructura deseada por Wayne Industries son su manejo de altos volúmenes de datos, que va de acuerdo a la inserción de un millón de registros propuestos y que son manejados sin degradación progresiva del rendimiento dado un buen manejo de índices; su manejo de índices es también una ventaja, puesto que presenta integración con índices de árboles balanceados (maneja coincidencias exactas de datos y filtros por rango), y de GIN (Generalized Inverted Index, que se utiliza especialmente en columnas de strings largos para acortar el tiempo de ejecución).

	Además de lo anterior, PostgreSQL puede integrarse de manera nativa con instaladores JDBC frecuentemente utilizados en aplicaciones de backend en Spring Boot, lo cual simplifica la complejidad a la hora de acoplar este sistema de bases de datos con un backend funcional. Otro añadido es su extendida capacidad de portabilidad por medio de contenerización por Docker, una característica que fue explotaba en el desarrollo de este proyecto y que beneficio la ejecución e insertado-modificación de datos en la base a lo largo de diferentes ambientes, beneficiando una presencia interconectada de los datos a excepción de sutiles diferencias arquitectónicas.

## Modelo de datos {#modelo-de-datos}

	La estructura de los datos fue desarrollada con la posibilidad de poder filtrar, organizar y extraer datos de una cierta forma dependiendo de las necesidades presentadas por la persona usuaria de la aplicación final. Se han optimizado las columnas de tabla principal de manera que no se siguen reglas tradicionales de la normalización para eliminar costos excesivos de la CPU, asegurando que la mayoría de respuestas relevantes se obtengan en un tiempo menor que 1 segundo. En sí, se integraron las columnas necesarias para responder a 5 necesidades claves del diseño de la aplicación:

1. Filtrado categórico  
   Se crearon columnas que permiten la segmentación precisa por categorías de productos, como el tipo de parte o el material usado para su fabricación. El mapa de ubicación de datos utiliza índices de árboles balanceados dado que el motor de la base de datos necesita buscar únicamente.  
     
2. Filtrado cuantitativo ordenado  
   Columnas hechas para guardar datos relevantes al peso y tamaño del producto funcionan como una manera de extraer componentes por restricciones físicas (encontrar partes entre dos determinados pesos, por ejemplo). Esta capacidad brilla especialmente en un ambiente donde se filtran los componentes en órdenes ascendentes o descendentes dada la propiedad secuencial de los índices.  
     
3. Auditoría con base en tiempo y rastreo  
   Se agregaron columnas específicas para el rastreo de aspectos administrativos como la fecha de manufactura o de registro de un producto en específico a modo de facilitar la trazabilidad a nivel de todo el catálogo de productos, permitiendo que ingenieros agreguen filtros a las búsquedas necesarias para verificar etapas de producción en ventanas de tiempo específicas.  
     
4. Búsqueda por texto sin estructura  
   Una columna que resguarda información más extensa y descriptiva del producto es utilizada para búsquedas inexactas y sin estar restringidas a igualdades exactas entre un string y otro. Este recae en el índice GIN para evitar degradación de rendimiento.  
     
5. Integridad del sistema e identificación única  
   Se implementaron las claves únicas para la tabla de productos con la finalidad de asegurar la identidad individual de cada producto registrado en la base de datos  se mantengan íntegros y sin repeticiones a lo largo del millón de registros.

## Generación de 1 millón de registros {#generación-de-1-millón-de-registros}

	La generación de los datos fue realizada por medio de un script de Python que se aseguró de la unicidad de cada uno de los registros generados. Dado que generar 1 millón de datos en Python se presta para la posibilidad de crear objetos repetidos, fue esencial la adición de determinados componentes a dicho script para asegurar este principio de unicidad:   
*import csv*  
*import random*  
*import uuid*  
*from datetime import datetime, timedelta*

*types \= \['Valve', 'Engine', 'Turbine', 'Panel', 'Rotor', 'Bearing', 'Strut'\]*  
*materials \= \['Steel', 'Aluminum', 'Titanium', 'Carbon Fiber', 'Ceramic'\]*  
*descriptions \= \[*  
    *"High stress tolerance component for aerospace applications",*  
    *"Standard maritime replacement part for cargo vessels",*  
    *"Experimental lightweight construct for vehicular chassis",*  
    *"Heavy duty industrial specification with reinforced casing",*  
    *"Refurbished engine block component with standard calibration"*  
*\]*

*start\_date \= datetime(2015, 1, 1\)*

*print("Generating 1000000 records. This may take a moment...")*

*with open('catalog\_data.csv', 'w', newline='') as file:*  
    *writer \= csv.writer(file)*  
    *for \_ in range(1000000):*  
        *part\_id \= str(uuid.uuid4())*  
        *days\_offset \= random.randint(0, 3650\)*  
        *m\_date \= (start\_date \+ timedelta(days=days\_offset)).date()*  
        *seconds\_offset \= random.randint(0, 86400\)*  
        *r\_timestamp \= datetime.combine(m\_date, datetime.min.time()) \+ timedelta(seconds=seconds\_offset)*  
        *weight \= random.randint(1, 5000\)*  
        *size \= round(random.uniform(0.1, 50.0), 2\)*  
        *p\_type \= random.choice(types)*  
        *material \= random.choice(materials)*  
        *desc \= random.choice(descriptions) \+ " \[REF: " \+ str(uuid.uuid4())\[:8\] \+ "\]"*  
          
        *writer.writerow(\[part\_id, m\_date, r\_timestamp, weight, size, p\_type, material, desc\])*

*print("CSV generation complete: catalog\_data.csv")*  
	Dichos componentes incluyen la inclusión de semillas con alta entropía incluida, particularmente en la columna dedicada a la descripción del producto en la que, luego de que se recolectan los primeros 8 carácteres de un UUID al azar, el script inyecta un string hexadecimal con 168 combinaciones posibles en cada descripción, asegurando que incluso cuando dos o más registros poseen el mismo tipo, material, tamaño, peso y fechas, su descripción será completamente diferente. De la misma forma, los datos relevantes a fechas y *timestamps* exactos se generaron en un periodo de 10 años simulados, incrementando las posibilidades de que no hayan dos registros con los mismos atributos de fechas en la base de datos. De la misma forma, datos cuantitativos como el tamaño cuentan con un rango de variabilidad de 0.10 y 50.00 metros que se redondean a 2 dígitos decimales, dando como resultado 4.990 posibilidades de punto flotante distintas (esto aún sin tomar en cuenta con las 5.000 posibilidades diferentes de la columna relacionada al peso, lo cual daría paso a 25 millones de combinaciones distintas sin contar descripciones o fechas).

## Índices y optimización {#índices-y-optimización}

	La lógica seguida para determina los índices es una práctica en la ingeniería de bases de datos conocida como *Query-Driven Design*, en la que, en lugar de adivinar qué indexar en cada columna individual, lo que inflaría la base innecesariamente, se busca estrictamente en los requerimientos propuestos: centrarse en cómo los ingenieros podrían buscar los datos. En la base de datos se mantienen dos tipos de requerimientos de búsqueda, los que corresponden con dos estructuras matemáticas diferentes dentro de sus componentes.

* Índices de árboles balanceados: consta de la estructura por default de los índices en PostgreSQL. Estos trabajan como un diccionario en el que los datos se mantienen en un orden estrictamente ordenado; en lugar de buscar todos los registros por la palabra “Titanio”, este mira al del medio, determina si “Titanio” está antes o después de esa entrada, e instantáneamente descarta la mitad de la base de datos para centrarse en la restante para buscar. La división se repite hasta encontrar el registro con “Titanio”. Como los nodos buscados están ordenados, los árboles balanceados también son matemáticamente perfectos al evaluarlos con queries basados en restricciones *BETWEEN*, *\<,* y *\>*.  
    
* GIN e índice trigrama: los índices estándar de PostgreSQL fallan frecuentemente cuando se trata de utilizar parámetros ‘contains’ en sus consultas. Si un ingeniero necesita buscar una fracción de una oración que contenga el conjunto de letras “ero” en ella, los árboles balanceados no funcionan dada su rigidez al buscar una palabra completa. Para acabar con este problema en la columna de descripción de cada producto registrado, se inició una extensión por trigrama en la que se divide matemáticamente cada descripción en tres conjuntos de letras (la palabra “aeroespacio” se vuelve “aer”, “ero”, “roe”, “oes”, “esp”, “spa”, “pac”, “aci”, “cio”). Luego, se invierte el mapa de búsqueda con un *Generalized Inverted Index (GIN)*, el cual guarda una lista masiva de cada cúmulo de 3 letras que existe en cualquier lugar de la base de datos, apuntando directamente a los identificadores de las filas que las contienen.

## Diagrama entidad-relación {#diagrama-entidad-relación}

	

   

       *Figura \#2: Diagrama de entidad relación de la base de datos.*

* **part\_id**: actúa como la clave primaria de la tabla. Su tipo de dato es UUID.  
* **manufacture\_date**: almacena una fecha. Su tipo de dato es DATE y tiene la restricción NOT NULL, que no permite valores nulos.  
* **registration\_timestamp**: almacena una fecha y hora exacta. Su tipo de dato es TIMESTAMP y tiene la restricción NOT NULL.  
* **weight\_kg**: almacena el peso en números enteros. Su tipo de dato es INTEGER y tiene la restricción NOT NULL.  
* **size\_meters**: almacena el tamaño con valores decimales. Su tipo de dato es DECIMAL(10,2), indicando un máximo de 10 dígitos en total con 2 de ellos para la parte decimal. Tiene la restricción NOT NULL.  
* **part\_type**: almacena cadenas de texto cortas. Su tipo de dato es VARCHAR(50), limitando la entrada a 50 caracteres. Tiene la restricción NOT NULL.  
* **material**: almacena el tipo de material en formato de texto. Su tipo de dato es VARCHAR(50) y tiene la restricción NOT NULL.  
* **long\_description**: almacena cadenas de texto extensas. Su tipo de dato es TEXT y tiene la restricción NOT NULL.

# 

# Implementación de filtros dinámicos {#implementación-de-filtros-dinámicos}

## Diseño de filtros {#diseño-de-filtros}

Si bien es cierto que una “Engineering Kata” no solo busca la resolución de problemas comunes que se dan en el desarrollo de software, sino que también implica un enfoque metódico en el que, la mayoría del tiempo es repetible con la finalidad de mejorar continuamente tanto individualmente como en equipo.

Ahora bien, en el caso específico de un problema común que suele darse en diversos contextos en el desarrollo de software como lo es la paginación, en este caso, el kata original está enfocado en la implementación de la paginación desde el backend, pero hay un detalle a tomar en cuenta. Una resolución real a este problema, no solo implica navegar entre páginas, sino que también, debe de incluir mecanismos de búsqueda y filtrado con el objetivo de que los usuarios localicen de una forma más eficiente y ágil, información específica dentro de millones de registros.

Es debido a lo anterior que la presente investigación se complementa con la implementación de filtros dinámicos como se mencionó unas secciones atrás. La funcionalidad principal de estas herramientas radica en que, permiten restringir los resultados antes de aplicar la paginación correspondiente, en consecuencia, se reduce en cierto porcentaje, la información procesada y se retornan los filtros necesarios para la consulta realizada, por ejemplo.

En la investigación, este punto se abarcó mediante la clase denominada PartSpecification, la cual es la encargada de construir dinámicamente la consulta Entity Framework en concordancia con los parámetros que se reciben desde el endpoint.

La implementación se realizó utilizando una clase denominada PartSpecification, cuya responsabilidad es construir dinámicamente la consulta de Entity Framework según los parámetros recibidos por el endpoint. Por ejemplo, si fueron recibidos parámetros referentes a una medida en la que se encuentra un objeto, lo anterior se cubre de la siguiente manera:

if (filters.SizeFrom.HasValue)  
{  
	query \= query.Where(p \=\>  
    	p.SizeMeters \>= filters.SizeFrom.Value);  
}  
   
if (filters.SizeTo.HasValue)  
{  
	query \= query.Where(p \=\>  
    	p.SizeMeters \<= filters.SizeTo.Value);  
}

Estrategias como la anterior, no solo contribuyen a evitar la repetición de múltiples métodos que combinen filtros, sino que también ayuda a mejorar el rendimiento del programa al hacer consultas o cargar por primera vez, mejorar la mantenibilidad del código y facilita la incorporación de nuevos filtros en futuras versiones del sistema.

A diferencia de una construcción convencional, en este caso la consulta se va construyendo progresivamente y en ese proceso, se le agregan condiciones únicamente cuando el usuario le provee algún criterio de búsqueda.

Por ejemplo, si el usuario únicamente solicita un filtro por material, la consulta solamente agrega la condición correspondiente a dicho campo. Si además solicita un rango de peso y una palabra clave en la descripción, todas esas condiciones se incorporan automáticamente a la misma consulta. Ejemplos como el anterior, más adelante se trabajan en los test.

## Tipos de filtros  {#tipos-de-filtros}

Como se pudo visualizar en el diagrama Entidad-Relación en el apartado referente a la base de datos, la estructura que se define para la tabla catalog\_parts contiene diferentes tipos de atributos los cuales, a su vez, también son diferentes tipos de datos entre sí. Debido a lo anterior es que, se implementaron diversos mecanismos de filtrado adaptados a los dichos tipos de datos.

Estos filtros pueden separarse en tres categorías importantes. La primera son los que poseen un rango específico de búsqueda, la segunda son coincidencias exactas de valores cortos y en el caso del que busca por descripción, este último es de coincidencia parcial para evitar poner toda la descripción de un producto en el filtro.

### Filtros de rango específico de búsqueda {#filtros-de-rango-específico-de-búsqueda}

Al necesitar un rango para realizar la búsqueda, se necesitan dos datos de búsqueda para obtener un resultado más preciso, sin embargo, si solo se busca por alguno de los dos datos, funciona, pero no es lo ideal.

#### Fechas {#fechas}

En el caso de las fechas, se implementaron los siguientes parámetros:

* manufactureDateFrom  
* manufactureDateTo

Si se envían ambos parámetros, la consulta devuelve únicamente los registros cuya fecha de manufactura se encuentre dentro del rango especificado. Mientras que, si solo se especifica uno de los dos, por ejemplo, manufactureDateFrom, retornará los registros que se hayan realizado a partir de esa fecha en adelante. Este tipo de búsqueda resulta útil para localizar componentes producidos durante un período determinado.

#### Timestamp {#timestamp}

Aparte de tener la fecha exacta de manufactura, el catálogo almacena la fecha y hora exacta de registro de cada componente. Los siguientes parámetros hacen referencia a este filtro:

* registrationFrom  
* registrationTo

Estos filtros permiten realizar búsquedas con una precisión mayor que la proporcionada por las fechas simples. Al igual que el caso anterior, se puede realizar la búsqueda por uno solo de los filtros, pero no es lo ideal.

#### Numéricos enteros {#numéricos-enteros}

Si se dejan las fechas de lado, ahora se tiene un atributo que hace referencia al peso de cada componente. WeightKg representa el peso utilizando valores enteros.

Se implementaron los parámetros:

* weightFrom  
* weightTo

Estos filtros permiten localizar componentes dentro de un intervalo específico de peso, ya que, en la vida real cuando una persona va a comprar algún objeto, este puede venir en diferentes presentaciones y por ende, pesos.

#### Numéricos decimales {#numéricos-decimales}

El atributo SizeMeters utiliza valores decimales para representar dimensiones físicas.

Para este campo se implementaron:

* sizeFrom  
* sizeTo

Gracias a estos filtros es posible consultar únicamente componentes que cumplan determinadas restricciones dimensionales.

### Filtro de coincidencia exacta para cadenas cortas {#filtro-de-coincidencia-exacta-para-cadenas-cortas}

Ahora bien, en el caso de los filtros específicos para coincidencias de cadenas cortas, sí se necesita que la palabra esté especificada en su totalidad, es decir, si una persona necesita buscar Titanium, pero solo escribió “nium” o “Tita, el filtro no retornará nada precisamente porque debe de ser una coincidencia exacta. Lo anterior no solo evita que el usuario tenga que buscar entre los millones de registros, un objeto en específico hecho de titanio o aluminio.

Para este filtro, atributos como “Material” y “PartType” contienen valores categóricos relativamente pequeños. Por esta razón se optó por utilizar coincidencia exacta mediante el operador de igualdad.

Ejemplos:

* material \= Steel  
* material \= Aluminum  
* partType \= Engine  
* partType \= Turbine

Este tipo de búsqueda es eficiente porque se apoya directamente en comparaciones simples dentro de la base de datos.

### Filtro de coincidencia parcial sobre descripciones {#filtro-de-coincidencia-parcial-sobre-descripciones}

El campo LongDescription almacena información textual extensa debido a que trae la descripción de las “parts” y eso incluye una referencia alfanumérica. Cuando ocurre este tipo de inconvenientes, un usuario no podrá aprenderse un millón de referencias o sus descripciones completas, por lo que se optó por un filtro basado en coincidencia parcial utilizando el método Contains(

Esto permite localizar registros cuya descripción incluya una palabra o fragmento específico.

Por ejemplo:

* engine  
* aerospace  
* industrial

La búsqueda parcial es especialmente útil cuando el usuario no conoce exactamente el contenido completo de la descripción.

## Integración con paginación {#integración-con-paginación}

Un aspecto importante de la implementación consiste en el orden en que se ejecutan las operaciones.

La consulta sigue el siguiente flujo:

Consulta base

→ Aplicación de filtros

→ Conteo de registros filtrados

→ Aplicación de Skip y Take

→ Conversión a DTO

→ Respuesta al cliente

Este orden garantiza que la información de paginación refleje únicamente los registros que cumplen los filtros seleccionados.

Si la paginación se aplicara antes de filtrar, únicamente se procesarían los registros de una página específica, produciendo resultados incorrectos y metadatos inconsistentes y no solo eso, sino que significaría más tiempo de procesamiento en la consulta hacia al backend o incluso, la consulta realizada a la base de datos.

## Combinación de filtros {#combinación-de-filtros}

La implementación permite utilizar varios filtros simultáneamente.

Por ejemplo, una consulta puede combinar:

* Material \= Steel  
* Peso entre 1000 y 2000 kg  
* Tamaño entre 10 y 50 metros  
* Descripción que contenga la palabra engine

La consulta resultante devuelve únicamente los registros que cumplen todas las condiciones indicadas. Cuanto más específica sea una persona cuando aplica los filtros, el tiempo de espera puede ser un poco mayor al de una consulta normal, sin embargo, la capacidad de combinar varios filtros resulta especialmente importante en escenarios donde existen millones de registros y se requiere reducir significativamente el conjunto de resultados.

# Plan de pruebas {#plan-de-pruebas}

Con el objetivo de validar el comportamiento de la solución se diseñó un conjunto de más de veinte pruebas automatizadas de integración. Las pruebas cubren tanto la funcionalidad de paginación como el comportamiento de los filtros implementados desde diversas perspectivas.

Los casos evaluados fueron:

* Paginación por defecto con 10 registros.  
* Paginación con 300 registros.  
* Filtro por fecha.  
* Filtro por timestamp.  
* Filtro por peso.  
* Filtro por tamaño.  
* Filtro por material.  
* Filtro por descripción.  
* Combinación de múltiples filtros.  
* Parámetros inválidos.

A continuación se adjunta una tabla en donde se puede observar más información sobre la gran mayoría las pruebas realizadas:

| ID | Caso de prueba | Entrada | Resultado esperado |
| :---- | :---- | :---- | :---- |
| TP-01 | Paginación por defecto | page=0\&size=10 | Retorna 10 registros y metadatos válidos |
| TP-02 | Paginación máxima | page=0\&size=300 | Retorna 300 registros y metadatos válidos |
| TP-03 | Filtro por fecha | manufactureDateFrom / manufactureDateTo | Todos los registros dentro del rango |
| TP-04 | Filtro por timestamp | registrationFrom / registrationTo | Todos los registros dentro del rango |
| TP-05 | Filtro por peso | weightFrom / weightTo | Todos los registros cumplen el rango |
| TP-06 | Filtro por tamaño | sizeFrom / sizeTo | Todos los registros cumplen el rango |
| TP-07 | Filtro por material | material=Steel | Todos los registros son Steel |
| TP-08 | Filtro por descripción | descriptionContains=engine | Todas las descripciones contienen engine |
| TP-09 | Filtros combinados | material \+ peso | Se cumplen ambas condiciones |
| TP-10 | Todos los filtros combinados | múltiples parámetros | Se cumplen todas las condiciones |
| TP-11 | Página inválida | page=-1 | HTTP 400 |
| TP-12 | Tamaño inválido | size=0 | HTTP 400 |
| TP-13 | Tamaño excesivo | size=9999 | HTTP 400 |
| TP-14 | Parámetros omitidos | /api/parts | Comportamiento por defecto |
| TP-15 | Benchmark | page=0\&size=300 | Tiempo menor al límite definido |

*Tabla \#2: Pruebas realizadas en su mayoría*

Cada prueba verifica que los resultados obtenidos cumplen las condiciones esperadas y que el endpoint responde correctamente ante distintos escenarios de uso. Es importante señalar que, el hecho de que las pruebas pasen o fallen, de acuerdo a como están construidas, va a depender de cuanta latencia haya, cuanto le tome construir la aplicación y todo lo relacionado con ella, la realización de la consulta, si el Query Planner decide recorrer toda la tabla o utilizar un índice, etc.

# Resultados Obtenidos {#resultados-obtenidos}

## Resultados manuales {#resultados-manuales}

La ejecución de las pruebas mostró resultados satisfactorios en todos los escenarios evaluados. A continuación, se presenta una tabla que resume lo que sucedió en cada prueba. 

| ID | Caso de prueba | Resultado obtenido | Estado |
| ----- | ----- | ----- | ----- |
| TP-01 | Paginación por defecto | 10 registros retornados correctamente | OK |
| TP-02 | Paginación máxima | 300 registros retornados correctamente | OK |
| TP-03 | Filtro por fecha | Registros dentro del rango esperado | OK |
| TP-04 | Filtro por timestamp | Registros dentro del rango esperado | OK |
| TP-05 | Filtro por peso | Registros cumplen el rango definido | OK |
| TP-06 | Filtro por tamaño | Registros cumplen el rango definido | OK |
| TP-07 | Filtro por material | Filtrado correcto por material | OK |
| TP-08 | Filtro por descripción | Coincidencia correcta en texto | OK |
| TP-09 | Filtros combinados | Ambas condiciones aplicadas correctamente | OK |
| TP-10 | Filtros múltiples | Todas las condiciones aplicadas correctamente | OK |
| TP-11 | Página inválida | Validación manejada sin error crítico | OK |
| TP-12 | Tamaño inválido | Manejo correcto de validación | OK |
| TP-13 | Tamaño excesivo | Validación aplicada correctamente | OK |
| TP-14 | Parámetros omitidos | Comportamiento por defecto correcto | OK |
| TP-15 | Benchmark | Tiempo dentro del límite establecido | OK |

*Tabla \#3: Resultados de pruebas manuales*

Las pruebas de filtrado confirmaron que cada criterio restringe adecuadamente el conjunto de registros recuperados.

Asimismo, las pruebas de combinación demostraron que múltiples filtros pueden coexistir dentro de una misma consulta sin afectar la consistencia de los resultados.

Las pruebas de validación confirmaron que parámetros inválidos relacionados con la paginación generan respuestas controladas mediante códigos HTTP apropiados.

## Resultados automáticos {#resultados-automáticos}

Para validar el comportamiento de la solución se desarrolló un conjunto de pruebas automatizadas de integración utilizando xUnit, FluentAssertions y WebApplicationFactory. 

| Categoría | Cantidad |
| ----- | ----- |
| Paginación | 2 |
| Filtros individuales | 6 |
| Filtros combinados | 3 |
| Rendimiento | 2 |
| Validaciones | 8 |
| Total | 21 |

*Tabla \#4: Cantidad de pruebas realizadas por categoría*

A continuación, se presenta evidencia que muestra el éxito de las pruebas realizadas. 

![][image2]

*Figura \#3: Pruebas desde el Explorador de Pruebas*  
*.*  
También, se ejecutaron las pruebas con el comando “dotnet test \--logger "console;verbosity=detailed"” con la finalidad de retornar información referente a las pruebas. Durante la ejecución se validaron situaciones como: 

* Consultas SQL generadas por Entity Framework Core  
* Aplicación correcta de filtros dinámicos  
* Paginación mediante LIMIT/OFFSET  
* Manejo de parámetros inválidos  
* Integración completa con base de datos de prueba

## Métricas de rendimiento {#métricas-de-rendimiento}

Para medir el rendimiento de la solución se incorporó un mecanismo de monitoreo basado en la clase Stopwatch de .NET. El tiempo total de procesamiento es almacenado en el atributo ResponseTimeMs, el cual forma parte de la respuesta enviada al frontend. Esto permite visualizar directamente el tiempo consumido por cada consulta. Las métricas obtenidas mostraron tiempos significativamente inferiores al límite establecido por el kata. Sin embargo, los factores que hacen que una prueba tenga éxito o falle, son muy circunstanciales, por lo que, es importante que no se conviertan en “bugs” silenciosos. La siguiente tabla muestra un aproximado del tiempo por cada prueba referente a las métricas de rendimiento. 

| Tipo de prueba | Tiempo observado |
| ----- | ----- |
| Paginación (10 registros) | \~300 ms |
| Paginación (300 registros) | \~400–430 ms |
| Filtros simples | \~100–200 ms |
| Filtros combinados | \~250–450 ms |
| Benchmark general | \< 1 segundo |

*Tabla \#5: Tipos de pruebas y su tiempo aproximado de ejecución.* 

# Análisis Arquitectónico {#análisis-arquitectónico}

## Performance {#performance}

La solución fue diseñada para ofrecer tiempos de respuesta adecuados incluso cuando trabaja con un catálogo superior a un millón de registros.

Desde el punto de vista de la paginación, el uso de Skip() y Take() evita cargar todos los registros simultáneamente. El backend únicamente recupera la porción de información correspondiente a la página solicitada por el usuario, reduciendo la cantidad de datos transferidos entre PostgreSQL, la API y el frontend.

Asimismo, se utiliza AsNoTracking() en Entity Framework Core para optimizar consultas de solo lectura, eliminando el costo asociado al seguimiento de cambios sobre las entidades recuperadas.

Por otra parte, los filtros dinámicos contribuyen al rendimiento al ejecutarse directamente sobre la consulta antes de aplicar la paginación. Esto permite que PostgreSQL procese únicamente los registros relevantes para cada búsqueda. Por ejemplo, una consulta que filtre componentes de acero con pesos entre 1000 y 2000 kilogramos reduce significativamente la cantidad de registros evaluados y transferidos.

Las condiciones de filtrado son traducidas automáticamente a SQL mediante LINQ y ejecutadas por PostgreSQL, aprovechando las capacidades de optimización del motor de base de datos. Además, PostgreSQL utiliza mecanismos internos como el Query Planner para determinar la estrategia más eficiente de ejecución.

Finalmente, la métrica responseTimeMs permite monitorear el tiempo consumido por cada solicitud, facilitando la evaluación continua del rendimiento de la aplicación.

## Maintainability {#maintainability}

La mantenibilidad fue considerada desde el diseño inicial mediante la separación clara de responsabilidades entre los distintos componentes de la aplicación.

La arquitectura divide la solución en controlador, servicio, contexto de datos, entidades y DTOs, permitiendo que cada componente tenga una responsabilidad específica. Esta organización facilita la comprensión del código y reduce el impacto de futuros cambios.

La implementación de filtros dinámicos se centralizó mediante la clase PartSpecification, evitando distribuir la lógica de búsqueda en múltiples lugares del sistema. Esto reduce la duplicación de código, mejora la trazabilidad de errores y facilita la incorporación de nuevos criterios de búsqueda.

Asimismo, el uso de DTOs permite desacoplar el modelo interno de la estructura expuesta al frontend. Gracias a ello, futuras modificaciones en la base de datos pueden realizarse sin afectar directamente la interfaz de usuario.

Como resultado, la solución puede evolucionar de forma controlada y con menor esfuerzo de mantenimiento.

## Testability {#testability}

La solución fue desarrollada para facilitar tanto pruebas manuales como automatizadas.

El endpoint recibe parámetros simples mediante Query String, lo que permite verificar fácilmente distintos escenarios utilizando herramientas como Swagger, Postman, navegadores web o el propio frontend desarrollado para el proyecto.

Adicionalmente, se implementaron pruebas automatizadas para validar:

* Paginación.  
* Rangos de fechas.  
* Rangos de timestamp.  
* Rangos de peso.  
* Rangos de tamaño.  
* Filtros por material.  
* Búsquedas por descripción.  
* Combinaciones de filtros.  
* Parámetros inválidos.  
* Tiempos de respuesta.

La estructura uniforme de las respuestas facilita la validación automática mediante atributos como:

* content  
* page  
* size  
* totalElements  
* totalPages  
* hasNext  
* hasPrevious  
* responseTimeMs

Esta estrategia permite detectar regresiones de manera temprana y aumenta la confiabilidad de la solución.

## Usability {#usability}

La usabilidad se fortaleció mediante un frontend sencillo que permite interactuar con la API sin necesidad de construir manualmente las URLs de consulta.

El usuario puede:

* Navegar entre páginas.  
* Seleccionar el tamaño de página.  
*  Visualizar el total de registros.  
* Consultar el tiempo de respuesta.  
*  Aplicar filtros de búsqueda.

La combinación de paginación y filtros dinámicos mejora significativamente la experiencia de uso, ya que permite localizar información específica dentro de grandes volúmenes de datos sin necesidad de revisar registros irrelevantes.

Además, la retroalimentación visual proporcionada por la interfaz permite comprender fácilmente el estado actual de la consulta y los resultados obtenidos.

## 10.5 Scalability {#10.5-scalability}

La solución presenta características que favorecen su crecimiento futuro sin requerir modificaciones arquitectónicas importantes.

La paginación backend garantiza que el volumen de información transferido permanezca controlado independientemente del tamaño total del catálogo.

Por otra parte, los filtros dinámicos fueron implementados sobre una consulta base extensible. Esto permite agregar nuevos criterios de búsqueda sin alterar significativamente la estructura existente del sistema.

El flujo de procesamiento definido es:

![][image3]

*Figura \#4: Flujo de procesamiento de los filtros dinámicos*

Este orden asegura que las métricas de paginación reflejen correctamente los resultados filtrados.

Además, al delegar el procesamiento de consultas a PostgreSQL, la aplicación evita cargar grandes cantidades de información en memoria, permitiendo mantener un comportamiento estable incluso ante incrementos significativos en el volumen de datos.

Como resultado, la arquitectura puede adaptarse a escenarios con varios millones de registros sin requerir rediseños sustanciales.

# Conclusiones {#conclusiones}

La presente investigación permitió diseñar e implementar una solución de paginación backend robusta para un catálogo de partes de ingeniería a gran escala, integrando además un sistema de filtros dinámicos que amplía significativamente la capacidad de consulta del sistema.

A partir de la implementación realizada en .NET Web API y PostgreSQL, se comprobó que la paginación basada en Skip() y Take() permite controlar eficientemente el volumen de datos transferidos, evitando la sobrecarga tanto en el backend como en el frontend. Asimismo, la incorporación de Entity Framework Core facilitó la construcción de consultas mantenibles y optimizadas, delegando la ejecución final al motor de base de datos.

Los resultados obtenidos en las pruebas, tanto manuales como automatizadas, evidencian la consistencia funcional de la solución. En todos los escenarios evaluados —incluyendo paginación por defecto, manejo de grandes volúmenes de datos, aplicación de filtros individuales y combinados, así como validación de parámetros inválidos— el sistema respondió conforme a lo esperado, manteniendo tiempos de respuesta dentro de los límites establecidos por la kata.

Adicionalmente, los mecanismos de medición de rendimiento implementados mediante Stopwatch permitieron observar que incluso en escenarios de máxima carga (300 registros por página), la solución se mantiene por debajo del umbral de un segundo, cumpliendo así los criterios de eficiencia definidos.

Desde el punto de vista arquitectónico, la separación de responsabilidades entre controlador, servicio, contexto de datos, DTOs y especificaciones dinámicas contribuyó a una solución altamente mantenible y extensible, facilitando la incorporación futura de nuevos filtros sin afectar la estructura base del sistema.

En conjunto, los resultados validan que la estrategia implementada no solo resuelve el problema de paginación de manera efectiva, sino que también constituye una base escalable y optimizada para la gestión de grandes volúmenes de datos en entornos reales.

[image1]: <data:image/png;base64,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>

[image2]: <data:image/png;base64,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>

[image3]: <data:image/png;base64,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>