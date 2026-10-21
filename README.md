#Sistema de Gestión de Inventario

Sistema de escritorio desarrollado en C# (.NET 8 / Windows Forms) y SQL Server para la administración de productos, categorías y suplidores con soporte de borrado lógico, validaciones mediante expresiones regulares, registro de logs y consultas analíticas con LINQ.

**Tecnologías y Arquitectura**

* **Lenguaje:** C# (.NET 8.0)
* **Interfaz de Usuario:** Windows Forms 
* **Base de Datos:** Microsoft SQL Server
* **Arquitectura:** N Capas 
* **Patrones de Diseño:**
  * **Singleton:** Para la conexión a SQL Server
  * **Repository:** Patrón genérico (`IRepositorio<T>`) para la persistencia.
  * **Generic Result:** Encapsulamiento de respuestas (`ResultadoOperacion<T>`).

 Pasos de instalacion:

 1. Descargue o copie todo el contenido del repositorio.
 2. Abra la carpeta build/ y ejecute el arcivo presentacion.exe (aplicacion).
 3. Asegurese de contar con SQL Server instalado y configurar el data source para establecer la conexion.

