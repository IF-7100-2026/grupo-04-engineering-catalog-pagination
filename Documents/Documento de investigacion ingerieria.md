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

	Además de lo anterior, PostgreSQL puede integrarse de manera nativa con instaladores JDBC frecuentemente utilizados en aplicaciones de backend en .NET, lo cual simplifica la complejidad a la hora de acoplar este sistema de bases de datos con un backend funcional. Otro añadido es su extendida capacidad de portabilidad por medio de contenerización por Docker, una característica que fue explotaba en el desarrollo de este proyecto y que beneficio la ejecución e insertado-modificación de datos en la base a lo largo de diferentes ambientes, beneficiando una presencia interconectada de los datos a excepción de sutiles diferencias arquitectónicas.

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

## Referencias bibliográficas

Microsoft. (2026). *Create a controller-based web API with ASP.NET Core*. Microsoft Learn. https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-web-api

Microsoft. (2024). *Overview of Entity Framework Core*. Microsoft Learn. https://learn.microsoft.com/en-us/ef/core/

PostgreSQL Global Development Group. (s. f.). *PostgreSQL documentation*. PostgreSQL. https://www.postgresql.org/docs/

xUnit.net. (s. f.). *xUnit.net*. https://xunit.net/

SmartBear Software. (s. f.). *What is OpenAPI?* Swagger. https://swagger.io/docs/specification/v3_0/about/

Stack Overflow. (2023). *How to configure Swagger in ASP.NET Core?* https://stackoverflow.com/questions/76635962/how-to-configure-swagger-in-asp-net-core

Stack Overflow. (2009). *Entity Framework PostgreSQL*. https://stackoverflow.com/questions/1211475/entity-framework-postgresql
