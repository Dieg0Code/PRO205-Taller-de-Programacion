# Clase 01 - Semana 02 - Pilares de la POO con TypeScript

- Unidad 01: **Introducción a la POO**
- Fecha: Lunes 18 de agosto, 2025.
- Horario: 10:50 - 13:30
- Docente: Diego Obando

## 🎯 Objetivos de la Clase

Al finalizar la clase seras capaz de:

- Comprender la sintaxis de TypeScript.
- Comprender la diferencia entre JavaScript y TypeScript, y el propósito de TypeScript.
- Entender en profundidad los pilares de la Programación Orientada a Objetos (POO) y cómo se implementan en TypeScript, incluyendo conceptos como abstracción, encapsulamiento, herencia y polimorfismo.
- Aplicar los conocimientos adquiridos en ejercicios prácticos y ejemplos reales.

### ¿Qué es TypeScript?

#### **Historia y Origen**

TypeScript fue **desarrollado por Microsoft** y anunciado públicamente en **octubre de 2012**. Su creador principal fue **Anders Hejlsberg**, quien también fue el arquitecto detrás de lenguajes como **C#**, **Turbo Pascal** y **Delphi**.

**Contexto histórico de su creación:**

En 2010-2012, JavaScript experimentaba un crecimiento exponencial. **Node.js** había sido lanzado en 2009, permitiendo JavaScript en el servidor, y las aplicaciones web se volvían cada vez más complejas. Sin embargo, JavaScript tenía limitaciones significativas:

- **Falta de tipos**: Errores difíciles de detectar hasta tiempo de ejecución
- **Tooling limitado**: IDEs con autocompletado básico o inexistente
- **Escalabilidad problemática**: Proyectos grandes se volvían difíciles de mantener
- **Refactoring riesgoso**: Cambiar código sin garantías de no romper otras partes

Microsoft enfrentaba estos problemas al desarrollar aplicaciones web complejas y necesitaba una solución. **Anders Hejlsberg** lideró un equipo para crear TypeScript, aplicando su experiencia en sistemas de tipos de lenguajes como C#.

**Adopción y crecimiento:**

- **2012**: Versión 0.8 - Recepción inicial mixta en la comunidad JavaScript
- **2014**: TypeScript 1.0 - Primera versión estable, comenzó la adopción seria
- **2015**: Microsoft open-source el compilador, mejorando la confianza de la comunidad
- **2016**: Angular 2+ adoptó TypeScript como lenguaje principal, acelerando su adopción
- **2019**: TypeScript superó a JavaScript en descargas de npm por primera vez
- **2024**: Más del 60% de los proyectos JavaScript nuevos usan TypeScript

#### **Definición Técnica Profunda**

**TypeScript es un superconjunto sintáctico estrictamente tipado de JavaScript** que se compila a JavaScript plano y legible.

**Desglosando esta definición:**

**"Superconjunto sintáctico":** Significa que TypeScript incluye toda la sintaxis válida de JavaScript, pero extiende el lenguaje con nuevas construcciones sintácticas. Matemáticamente hablando:

```
Si J = conjunto de todas las construcciones válidas en JavaScript
Si T = conjunto de todas las construcciones válidas en TypeScript
Entonces: J ⊆ T (JavaScript es subconjunto de TypeScript)
```

**"Estrictamente tipado":** TypeScript introduce un sistema de tipos que:

- Verifica la compatibilidad de tipos en tiempo de compilación
- Infiere tipos automáticamente cuando es posible
- Permite tipos explícitos para mayor claridad y control
- Detecta errores relacionados con tipos antes de la ejecución

**"Compilación a JavaScript plano":** El código TypeScript se transforma (transpila) a JavaScript estándar que puede ejecutarse en cualquier entorno que soporte JavaScript.

#### **Filosofía de Diseño**

TypeScript sigue varios principios fundamentales:

1. **Compatibilidad hacia atrás**: Todo JavaScript válido debe ser TypeScript válido
2. **Tipado gradual**: Puedes adoptar tipos progresivamente, no todo a la vez
3. **Preservar semántica en tiempo de ejecución**: El comportamiento del programa no cambia al compilar
4. **Erradicación de tipos**: Los tipos solo existen durante la compilación, no en tiempo de ejecución
5. **Interoperabilidad**: Debe funcionar bien con bibliotecas JavaScript existentes

#### **Arquitectura del Sistema**

TypeScript consta de varios componentes clave:

**1. Compilador (tsc):**

- **Parser**: Convierte código fuente en Abstract Syntax Tree (AST)
- **Binder**: Conecta declaraciones con referencias
- **Checker**: Realiza análisis semántico y verificación de tipos
- **Emitter**: Genera código JavaScript de salida

**2. Servidor de Lenguaje (TSServer):**

- Proporciona servicios a editores e IDEs
- Autocompletado, navegación de código, refactoring
- Diagnósticos en tiempo real

**3. Declaraciones de Tipos:**

- Archivos `.d.ts` que describen APIs de JavaScript
- DefinitelyTyped: Repositorio comunitario de definiciones de tipos

#### **Ejemplos Prácticos para Entender el Concepto**

**Ejemplo 1: Variables con tipos y arrays**

```javascript
// JavaScript - Propenso a errores en runtime
let estudiantes = ["Juan", "Ana", 25, true]; // Mezcla de tipos
let notas = [8.5, "9", 7.2, "diez"]; // Inconsistente

function calcularPromedio(notas) {
  let suma = 0;
  for (let nota of notas) {
    suma += nota; // ¿Qué pasa con "9" y "diez"?
  }
  return suma / notas.length;
}
```

```typescript
// TypeScript - Tipos consistentes y seguros
let estudiantes: string[] = ["Juan", "Ana", "Carlos"]; // Solo strings
let notas: number[] = [8.5, 9.0, 7.2, 8.8]; // Solo números

function calcularPromedio(notas: number[]): number {
  let suma = 0;
  for (let nota of notas) {
    suma += nota; // TypeScript garantiza que nota es number
  }
  return suma / notas.length;
}

// Uso seguro
const promedio = calcularPromedio(notas); // ✅ 8.375
// calcularPromedio(["8", "9"]); // ❌ Error: string[] no es number[]
```

**Ejemplo 2: Funciones con múltiples parámetros y opcionales**

```javascript
// JavaScript - Parámetros inciertos
function crearUsuario(nombre, email, edad, activo) {
  return {
    nombre: nombre,
    email: email,
    edad: edad || 18, // ¿Qué pasa si edad es 0?
    activo: activo !== undefined ? activo : true,
  };
}

// Llamadas problemáticas
crearUsuario("Juan"); // ¿email undefined?
crearUsuario("Ana", "ana@email.com", "25"); // ¿edad como string?
```

```typescript
// TypeScript - Parámetros claros y opcionales
function crearUsuario(
  nombre: string,
  email: string,
  edad?: number, // Parámetro opcional
  activo: boolean = true // Valor por defecto
): { nombre: string; email: string; edad: number; activo: boolean } {
  return {
    nombre,
    email,
    edad: edad ?? 18, // Nullish coalescing
    activo,
  };
}

// Uso correcto y seguro
const usuario1 = crearUsuario("Juan", "juan@email.com");
const usuario2 = crearUsuario("Ana", "ana@email.com", 25, false);
// crearUsuario("Carlos"); // ❌ Error: email es requerido
```

**Ejemplo 3: Interfaces y objetos complejos**

```typescript
// Definiendo interfaces para estructuras claras
interface Estudiante {
  id: number;
  nombre: string;
  email: string;
  carrera: string;
  notas: number[];
  activo?: boolean; // Propiedad opcional
}

// Función que procesa estudiantes
function procesarEstudiante(estudiante: Estudiante): string {
  const promedio =
    estudiante.notas.reduce((a, b) => a + b, 0) / estudiante.notas.length;
  const estado = estudiante.activo !== false ? "Activo" : "Inactivo";

  return `${estudiante.nombre} (${
    estudiante.carrera
  }): Promedio ${promedio.toFixed(1)} - ${estado}`;
}

// Creación y uso
const estudiante: Estudiante = {
  id: 1,
  nombre: "María González",
  email: "maria@aiep.cl",
  carrera: "Programación",
  notas: [8.5, 9.2, 7.8, 8.9],
  activo: true,
};

console.log(procesarEstudiante(estudiante));
// "María González (Programación): Promedio 8.6 - Activo"
```

**Ejemplo 4: Union Types y Type Guards**

```typescript
// Union types para múltiples posibilidades
type Resultado = number | string | boolean;
type Estado = "pendiente" | "aprobado" | "rechazado";

function procesarResultado(resultado: Resultado): string {
  // Type guards para manejar diferentes tipos
  if (typeof resultado === "number") {
    return `Nota numérica: ${resultado.toFixed(1)}`;
  } else if (typeof resultado === "string") {
    return `Comentario: ${resultado.toUpperCase()}`;
  } else {
    return `Estado booleano: ${resultado ? "Aprobado" : "Reprobado"}`;
  }
}

function cambiarEstado(estadoActual: Estado, aprobar: boolean): Estado {
  if (estadoActual === "pendiente") {
    return aprobar ? "aprobado" : "rechazado";
  }
  return estadoActual; // No se puede cambiar si ya está decidido
}

// Uso práctico
console.log(procesarResultado(8.5)); // "Nota numérica: 8.5"
console.log(procesarResultado("Excelente trabajo")); // "Comentario: EXCELENTE TRABAJO"
console.log(procesarResultado(true)); // "Estado booleano: Aprobado"

const nuevoEstado = cambiarEstado("pendiente", true); // "aprobado"
```

#### **Proceso de Compilación Detallado**

Cuando ejecutas `tsc archivo.ts`, ocurre lo siguiente:

**Fase 1: Análisis Léxico**

- El código se divide en tokens (palabras clave, identificadores, operadores, etc.)

**Fase 2: Análisis Sintáctico**

- Los tokens se organizan en un Abstract Syntax Tree (AST)
- Se verifica que la sintaxis sea válida

**Fase 3: Análisis Semántico**

- Se construye la tabla de símbolos
- Se realiza la inferencia y verificación de tipos
- Se detectan errores semánticos

**Fase 4: Generación de Código**

- Se produce código JavaScript equivalente
- Se eliminan todas las anotaciones de tipos
- Se aplican transformaciones según la versión target de JavaScript

**Ejemplo de transformación:**

```typescript
// Código TypeScript original
class Calculadora {
  private resultado: number = 0;

  sumar(valor: number): this {
    this.resultado += valor;
    return this;
  }

  obtenerResultado(): number {
    return this.resultado;
  }
}

const calc = new Calculadora().sumar(5).sumar(3);
```

```javascript
// JavaScript generado (ES2015 target)
class Calculadora {
  constructor() {
    this.resultado = 0;
  }

  sumar(valor) {
    this.resultado += valor;
    return this;
  }

  obtenerResultado() {
    return this.resultado;
  }
}

const calc = new Calculadora().sumar(5).sumar(3);
```

**💡 Punto clave:** Observa cómo los tipos desaparecen completamente en el JavaScript final, pero nos ayudaron durante el desarrollo a escribir código más seguro y mantenible.

### Instalación y Configuración de TypeScript

#### **Requisitos Previos**

Para trabajar con TypeScript necesitas:

- **Node.js** instalado (versión 14 o superior)
- Un editor de código (VS Code recomendado)
- Terminal o línea de comandos

#### **Instalación de TypeScript**

**Opción 1: Instalación Global**

```bash
# Instalar TypeScript globalmente
npm install -g typescript

# Verificar la instalación
tsc --version
```

**Opción 2: Instalación Local en el proyecto**

```bash
# Crear un nuevo proyecto
mkdir mi-proyecto-typescript
cd mi-proyecto-typescript

# Inicializar package.json
npm init -y

# Instalar TypeScript como dependencia de desarrollo
npm install --save-dev typescript

# Verificar instalación local
npx tsc --version
```

#### **Uso del Compilador TypeScript**

**Compilación básica:**

```bash
# Compilar un archivo específico
tsc archivo.ts

# Resultado: se crea archivo.js
```

**Ejemplo práctico:**

1. Crea un archivo `saludo.ts`:

```typescript
function saludarEstudiante(nombre: string, carrera: string): string {
  return `¡Hola ${nombre}! Bienvenido a ${carrera}`;
}

const mensaje = saludarEstudiante("Ana", "Programación");
console.log(mensaje);
```

2. Compila el archivo:

```bash
tsc saludo.ts
```

3. Se genera `saludo.js`:

```javascript
function saludarEstudiante(nombre, carrera) {
  return "¡Hola " + nombre + "! Bienvenido a " + carrera;
}
var mensaje = saludarEstudiante("Ana", "Programación");
console.log(mensaje);
```

4. Ejecuta el JavaScript generado:

```bash
node saludo.js
# Salida: ¡Hola Ana! Bienvenido a Programación
```

#### **Configuración Avanzada con tsconfig.json**

Para proyectos más complejos, crea un archivo `tsconfig.json`:

```bash
# Generar tsconfig.json automáticamente
tsc --init
```

**Ejemplo de tsconfig.json básico:**

```json
{
  "compilerOptions": {
    "target": "ES2020", // Versión de JavaScript objetivo
    "module": "commonjs", // Sistema de módulos
    "outDir": "./dist", // Carpeta de salida
    "rootDir": "./src", // Carpeta de código fuente
    "strict": true, // Modo estricto
    "esModuleInterop": true, // Interoperabilidad con CommonJS
    "skipLibCheck": true, // Saltar verificación de librerías
    "forceConsistentCasingInFileNames": true
  },
  "include": ["src/**/*"], // Archivos a incluir
  "exclude": ["node_modules"] // Archivos a excluir
}
```

**Estructura de proyecto recomendada:**

```
mi-proyecto/
├── src/
│   ├── main.ts
│   └── utils/
│       └── helpers.ts
├── dist/           (generado automáticamente)
├── package.json
└── tsconfig.json
```

#### **Comandos del Compilador más Útiles**

```bash
# Compilar todo el proyecto (usa tsconfig.json)
tsc

# Compilar en modo watch (recompila automáticamente)
tsc --watch

# Compilar a una versión específica de JavaScript
tsc archivo.ts --target ES2015

# Compilar sin generar archivos (solo verificar errores)
tsc --noEmit

# Compilar con información detallada
tsc --verbose

# Compilar ignorando errores (no recomendado)
tsc --noEmitOnError false
```

#### **Integración con VS Code**

VS Code tiene soporte nativo para TypeScript:

1. **Instalación automática**: VS Code incluye TypeScript
2. **Detección de errores en tiempo real**
3. **Autocompletado inteligente**
4. **Refactoring automático**

**Configuración recomendada en VS Code:**

Crea `.vscode/settings.json`:

```json
{
  "typescript.preferences.quoteStyle": "double",
  "typescript.preferences.semicolons": "insert",
  "typescript.suggest.autoImports": true,
  "editor.formatOnSave": true
}
```

#### **Flujo de Trabajo Típico**

1. **Escribir código TypeScript** en archivos `.ts`
2. **Ver errores en tiempo real** en el editor
3. **Compilar** con `tsc` o `tsc --watch`
4. **Ejecutar JavaScript generado** con Node.js
5. **Repetir el ciclo**

**Ejemplo de flujo completo:**

```bash
# 1. Crear proyecto
mkdir calculadora-ts && cd calculadora-ts
npm init -y

# 2. Instalar TypeScript
npm install --save-dev typescript

# 3. Crear configuración
npx tsc --init

# 4. Crear código fuente
mkdir src
echo "console.log('¡Proyecto TypeScript listo!');" > src/main.ts

# 5. Compilar
npx tsc

# 6. Ejecutar
node dist/main.js
```

### Diferencias con JavaScript

#### **Comparación Fundamental**

La principal diferencia entre JavaScript y TypeScript es que **JavaScript es dinámicamente tipado** mientras que **TypeScript es estáticamente tipado**. Esto significa:

**JavaScript (Tipado Dinámico):**

- Los tipos se determinan en **tiempo de ejecución**
- Puedes cambiar el tipo de una variable en cualquier momento
- Los errores de tipo se descubren cuando el código se ejecuta

**TypeScript (Tipado Estático):**

- Los tipos se determinan en **tiempo de compilación**
- Una vez declarado el tipo, no puede cambiar
- Los errores de tipo se descubren antes de ejecutar el código

#### **Diferencias Principales por Categoría**

#### **1. Sistema de Tipos**

```javascript
// JavaScript - Tipado dinámico
let variable = "Hola"; // string
variable = 42; // ahora es number - ✅ válido
variable = true; // ahora es boolean - ✅ válido
variable = [1, 2, 3]; // ahora es array - ✅ válido

function procesar(dato) {
  // No sabemos qué tipo es 'dato' hasta que se ejecute
  return dato.toUpperCase(); // ¿Qué pasa si dato no es string?
}
```

```typescript
// TypeScript - Tipado estático
let variable: string = "Hola"; // Declaramos que es string
variable = 42; // ❌ Error: Type 'number' is not assignable to type 'string'
variable = true; // ❌ Error: Type 'boolean' is not assignable to type 'string'

function procesar(dato: string): string {
  // TypeScript garantiza que 'dato' es string
  return dato.toUpperCase(); // ✅ Seguro, sabemos que tiene toUpperCase()
}
```

#### **2. Detección de Errores**

```javascript
// JavaScript - Errores en tiempo de ejecución
function dividir(a, b) {
  return a / b;
}

console.log(dividir(10, 2)); // 5 - ✅ funciona
console.log(dividir(10, "2")); // 5 - funciona por coerción
console.log(dividir(10, "abc")); // NaN - resultado inesperado
console.log(dividir(10)); // NaN - parámetro faltante
console.log(dividir("10", "2")); // 5 - coerción automática

// Los problemas se ven solo al ejecutar
```

```typescript
// TypeScript - Errores en tiempo de compilación
function dividir(a: number, b: number): number {
  return a / b;
}

console.log(dividir(10, 2)); // ✅ 5
console.log(dividir(10, "2")); // ❌ Error: Argument of type 'string' is not assignable
console.log(dividir(10, "abc")); // ❌ Error: Argument of type 'string' is not assignable
console.log(dividir(10)); // ❌ Error: Expected 2 arguments, but got 1
console.log(dividir("10", "2")); // ❌ Error: Argument of type 'string' is not assignable

// Todos los errores se detectan ANTES de ejecutar
```

#### **3. Autocompletado y Herramientas**

```javascript
// JavaScript - Autocompletado limitado
let estudiante = {
  nombre: "Juan",
  edad: 20,
  carrera: "Programación",
};

// El editor no sabe qué propiedades tiene 'estudiante'
estudiante.nom; // ¿nombre? ¿nombreCompleto? No hay sugerencias claras
estudiante.carre; // ¿carrera? ¿carretera? Ambiguo
```

```typescript
// TypeScript - Autocompletado preciso
interface Estudiante {
  nombre: string;
  edad: number;
  carrera: string;
  email?: string;
}

let estudiante: Estudiante = {
  nombre: "Juan",
  edad: 20,
  carrera: "Programación",
};

// El editor conoce exactamente las propiedades disponibles
estudiante.nom; // Sugiere automáticamente 'nombre'
estudiante.edad; // Autocompletado perfecto
estudiante.email; // Sugiere 'email' como opcional
```

#### **4. Refactoring y Mantenimiento**

```javascript
// JavaScript - Refactoring riesgoso
function calcularPromedio(notas) {
  // Si cambio el nombre de esta función, tengo que buscar
  // manualmente todas las referencias en el proyecto
  return notas.reduce((a, b) => a + b) / notas.length;
}

// En otro archivo...
let resultado = calcularPromedio([8, 9, 7]); // ¿Cómo saber si cambió?
```

```typescript
// TypeScript - Refactoring seguro
function calcularPromedioEstudiante(notas: number[]): number {
  // Si cambio el nombre, TypeScript actualiza automáticamente
  // todas las referencias en todo el proyecto
  return notas.reduce((a, b) => a + b) / notas.length;
}

// En otro archivo...
let resultado = calcularPromedioEstudiante([8, 9, 7]); // Se actualiza automáticamente
```

#### **5. Documentación Integrada**

```javascript
// JavaScript - Documentación separada
/**
 * Crea un usuario nuevo
 * @param {string} nombre - El nombre del usuario
 * @param {number} edad - La edad del usuario
 * @param {boolean} activo - Si el usuario está activo
 * @returns {Object} El objeto usuario creado
 */
function crearUsuario(nombre, edad, activo) {
  // La documentación puede estar desactualizada
  return { nombre, edad, activo };
}
```

```typescript
// TypeScript - Documentación automática
function crearUsuario(nombre: string, edad: number, activo: boolean): Usuario {
  // Los tipos SON la documentación
  // Siempre está actualizada porque el código no compila si no es correcta
  return { nombre, edad, activo };
}

interface Usuario {
  nombre: string;
  edad: number;
  activo: boolean;
}
```

#### **6. Características Avanzadas**

**JavaScript NO tiene:**

```javascript
// ❌ No hay interfaces
// ❌ No hay enums nativos
// ❌ No hay genéricos
// ❌ No hay modificadores de acceso (private, protected)
// ❌ No hay decoradores estándar
// ❌ No hay verificación de null/undefined
```

**TypeScript SÍ tiene:**

```typescript
// ✅ Interfaces
interface Vehiculo {
  marca: string;
  modelo: string;
  año: number;
}

// ✅ Enums
enum EstadoVehiculo {
  Nuevo = "nuevo",
  Usado = "usado",
  Dañado = "dañado",
}

// ✅ Genéricos
function obtenerPrimero<T>(lista: T[]): T | undefined {
  return lista[0];
}

// ✅ Modificadores de acceso
class Auto {
  private motor: string; // Solo accesible dentro de la clase
  protected chasis: string; // Accesible en clases hijas
  public marca: string; // Accesible desde cualquier lugar
}

// ✅ Verificación de null/undefined
function procesarNombre(nombre: string | null): string {
  if (nombre === null) {
    return "Sin nombre";
  }
  return nombre.toUpperCase(); // TypeScript sabe que aquí nombre no es null
}
```

#### **7. Caso Práctico: Comparación Real**

Veamos un ejemplo real de cómo se ve el mismo código en ambos lenguajes:

**JavaScript:**

```javascript
// archivo: estudiantes.js
function crearSistemaEstudiantes() {
  let estudiantes = [];

  function agregar(nombre, edad, carrera, notas) {
    // ¿Qué pasa si paso los parámetros en orden incorrecto?
    // ¿Qué pasa si notas no es un array?
    estudiantes.push({
      id: estudiantes.length + 1,
      nombre: nombre,
      edad: edad,
      carrera: carrera,
      notas: notas,
      fechaIngreso: new Date(),
    });
  }

  function obtenerPromedio(id) {
    let estudiante = estudiantes.find((e) => e.id == id); // ¿== o ===?
    if (!estudiante) return null;

    // ¿Qué pasa si notas está vacío?
    let suma = estudiante.notas.reduce((a, b) => a + b);
    return suma / estudiante.notas.length;
  }

  return { agregar, obtenerPromedio };
}

// Uso propenso a errores
let sistema = crearSistemaEstudiantes();
sistema.agregar("Juan", "Programación", 20, [8, 9]); // ¡Orden incorrecto!
let promedio = sistema.obtenerPromedio("1"); // ¿String o número?
```

**TypeScript:**

```typescript
// archivo: estudiantes.ts
interface Estudiante {
  id: number;
  nombre: string;
  edad: number;
  carrera: string;
  notas: number[];
  fechaIngreso: Date;
}

interface SistemaEstudiantes {
  agregar(nombre: string, edad: number, carrera: string, notas: number[]): void;
  obtenerPromedio(id: number): number | null;
}

function crearSistemaEstudiantes(): SistemaEstudiantes {
  let estudiantes: Estudiante[] = [];

  function agregar(
    nombre: string,
    edad: number,
    carrera: string,
    notas: number[]
  ): void {
    // TypeScript garantiza que los parámetros son correctos
    estudiantes.push({
      id: estudiantes.length + 1,
      nombre,
      edad,
      carrera,
      notas,
      fechaIngreso: new Date(),
    });
  }

  function obtenerPromedio(id: number): number | null {
    const estudiante = estudiantes.find((e) => e.id === id);
    if (!estudiante) return null;

    if (estudiante.notas.length === 0) return null;

    const suma = estudiante.notas.reduce((a, b) => a + b);
    return suma / estudiante.notas.length;
  }

  return { agregar, obtenerPromedio };
}

// Uso seguro y guiado
const sistema = crearSistemaEstudiantes();
sistema.agregar("Juan", 20, "Programación", [8, 9]); // ✅ Orden correcto forzado
const promedio = sistema.obtenerPromedio(1); // ✅ Debe ser número
```

#### **Ventajas y Desventajas**

| Aspecto                             | JavaScript | TypeScript                   |
| ----------------------------------- | ---------- | ---------------------------- |
| **Curva de aprendizaje**            | Menor      | Mayor inicial                |
| **Velocidad de desarrollo inicial** | Rápida     | Más lenta al principio       |
| **Mantenimiento a largo plazo**     | Difícil    | Más fácil                    |
| **Detección de errores**            | Runtime    | Compile-time                 |
| **Tamaño de archivos**              | Menor      | Mayor (pero se compila a JS) |
| **Compatibilidad**                  | Universal  | Necesita compilación         |
| **Tooling**                         | Básico     | Excelente                    |
| **Escalabilidad**                   | Limitada   | Excelente                    |

**💡 Conclusión clave:** TypeScript requiere más esfuerzo inicial, pero paga dividendos enormes en proyectos grandes y equipos de desarrollo, especialmente para aplicaciones que requieren mantenimiento a largo plazo.

### Ventajas del tipado estático

El tipado estático es una de las características más poderosas de TypeScript. A diferencia del tipado dinámico de JavaScript, donde los tipos se determinan en tiempo de ejecución, **el tipado estático verifica y garantiza los tipos en tiempo de compilación**.

#### **1. Detección Temprana de Errores**

**Ventaja principal:** Los errores se detectan **antes** de que el código se ejecute, no durante la ejecución.

```typescript
// ❌ Este error se detecta ANTES de ejecutar
function calcularDescuento(precio: number, descuento: number): number {
  return precio - precio * descuento;
}

// TypeScript detecta inmediatamente estos errores:
calcularDescuento("100", 0.1); // Error: "100" no es number
calcularDescuento(100); // Error: falta parámetro descuento
calcularDescuento(100, "10%"); // Error: "10%" no es number
```

**Comparación con JavaScript:**

```javascript
// JavaScript - Error se descubre AL EJECUTAR
function calcularDescuento(precio, descuento) {
  return precio - precio * descuento;
}

console.log(calcularDescuento("100", 0.1)); // NaN - ¡Error silencioso!
console.log(calcularDescuento(100)); // NaN - ¡Error silencioso!
console.log(calcularDescuento(100, "10%")); // NaN - ¡Error silencioso!
```

#### **2. Autocompletado Inteligente e IntelliSense Superior**

**Ventaja:** El editor conoce exactamente qué propiedades y métodos están disponibles.

```typescript
interface Producto {
    id: number;
    nombre: string;
    precio: number;
    categoria: string;
    disponible: boolean;
    fechaCreacion: Date;
    calcularPrecioConIva(): number;
    aplicarDescuento(porcentaje: number): number;
}

function procesarProducto(producto: Producto) {
    // El editor sugiere automáticamente todas las propiedades:
    producto.   // ← Al escribir el punto, aparecen todas las opciones
    /*
    Sugerencias automáticas:
    - id
    - nombre
    - precio
    - categoria
    - disponible
    - fechaCreacion
    - calcularPrecioConIva()
    - aplicarDescuento()
    */

    // Además, conoce los tipos de cada propiedad:
    const precio = producto.precio;        // TypeScript sabe que precio es number
    const nombre = producto.nombre;        // TypeScript sabe que nombre es string
    const fecha = producto.fechaCreacion;  // TypeScript sabe que fecha es Date

    // Y puede sugerir métodos específicos del tipo:
    nombre.toUpperCase();     // Métodos de string
    precio.toFixed(2);        // Métodos de number
    fecha.getFullYear();      // Métodos de Date
}
```

#### **3. Refactoring Seguro y Automático**

**Ventaja:** Cambios en el código se propagan automáticamente y de forma segura.

```typescript
// Archivo: modelos.ts
interface Estudiante {
  id: number;
  nombre: string;
  apellido: string;
  edad: number;
  carrera: string;
}

// Archivo: servicios.ts
function crearEstudiante(datos: Estudiante): Estudiante {
  return {
    id: Date.now(),
    nombre: datos.nombre,
    apellido: datos.apellido,
    edad: datos.edad,
    carrera: datos.carrera,
  };
}

function mostrarEstudiante(estudiante: Estudiante): string {
  return `${estudiante.nombre} ${estudiante.apellido} - ${estudiante.carrera}`;
}

// Si cambio la interfaz Estudiante (por ejemplo, añado 'email: string'),
// TypeScript me avisa INMEDIATAMENTE en TODOS los lugares donde se usa:
// - crearEstudiante() necesitará el campo email
// - Cualquier objeto que implemente Estudiante necesitará email
// - El refactoring es automático y seguro
```

#### **4. Documentación Automática y Viva**

**Ventaja:** Los tipos sirven como documentación que nunca se desactualiza.

```typescript
// Esta función está perfectamente documentada solo con sus tipos:
function procesarCalificaciones(
  estudiante: {
    nombre: string;
    carrera: "Programación" | "Testing" | "Análisis de Sistemas";
    semestre: 1 | 2 | 3 | 4 | 5 | 6;
  },
  materias: Array<{
    nombre: string;
    creditos: number;
    notas: number[];
    aprobada: boolean;
  }>
): {
  promedioGeneral: number;
  creditosAprobados: number;
  creditosTotales: number;
  estadoAcademico: "Regular" | "Probatorio" | "Excelencia";
} {
  // La implementación...
  const creditosAprobados = materias
    .filter((m) => m.aprobada)
    .reduce((sum, m) => sum + m.creditos, 0);

  const creditosTotales = materias.reduce((sum, m) => sum + m.creditos, 0);

  const promedioGeneral =
    materias.reduce((sum, materia) => {
      const promedio =
        materia.notas.reduce((a, b) => a + b) / materia.notas.length;
      return sum + promedio;
    }, 0) / materias.length;

  let estadoAcademico: "Regular" | "Probatorio" | "Excelencia";
  if (promedioGeneral >= 6.5) estadoAcademico = "Excelencia";
  else if (promedioGeneral >= 4.0) estadoAcademico = "Regular";
  else estadoAcademico = "Probatorio";

  return {
    promedioGeneral,
    creditosAprobados,
    creditosTotales,
    estadoAcademico,
  };
}
```

#### **5. Prevención de Errores Comunes**

**Ventaja:** TypeScript previene automáticamente errores típicos de JavaScript.

```typescript
// 1. Previene acceso a propiedades undefined
function obtenerNombreCompleto(persona: {
  nombre: string;
  apellido?: string;
}): string {
  // TypeScript obliga a verificar propiedades opcionales
  return persona.apellido
    ? `${persona.nombre} ${persona.apellido}`
    : persona.nombre;

  // ❌ Esto daría error:
  // return persona.apellido.toUpperCase(); // Error: apellido might be undefined
}

// 2. Previene comparaciones incorrectas
function compararIds(id1: number, id2: number): boolean {
  return id1 === id2; // ✅ Correcto
  // return id1 == id2;   // TypeScript prefiere === para mayor seguridad
}

// 3. Previene mutaciones no deseadas
function procesarConfiguracion(
  config: Readonly<{ host: string; port: number; ssl: boolean }>
) {
  // ❌ Esto daría error:
  // config.host = "nuevo-host";  // Error: Cannot assign to 'host' because it is read-only

  // ✅ Para modificar, hay que crear una nueva configuración:
  return { ...config, host: "nuevo-host" };
}

// 4. Previene llamadas a métodos inexistentes
function procesarTexto(texto: string | null): string {
  if (texto === null) {
    return "Sin texto";
  }

  // TypeScript sabe que aquí texto NO es null
  return texto.toUpperCase(); // ✅ Seguro

  // Si no hubiera la verificación anterior:
  // return texto.toUpperCase();  // ❌ Error: texto might be null
}
```

#### **6. Mejor Mantenimiento en Equipos**

**Ventaja:** Facilita el trabajo colaborativo y la comprensión del código.

```typescript
// Archivo creado por el Desarrollador A
export interface APIResponse<T> {
  success: boolean;
  data?: T;
  error?: {
    code: number;
    message: string;
    details?: string;
  };
  metadata: {
    timestamp: Date;
    version: string;
    requestId: string;
  };
}

export async function obtenerEstudiantes(): Promise<APIResponse<Estudiante[]>> {
  // Implementación...
  return {
    success: true,
    data: [
      /* estudiantes */
    ],
    metadata: {
      timestamp: new Date(),
      version: "1.0",
      requestId: "req-123",
    },
  };
}

// Archivo usado por el Desarrollador B (6 meses después)
async function cargarEstudiantes() {
  const response = await obtenerEstudiantes();

  // El Desarrollador B entiende inmediatamente la estructura:
  if (response.success && response.data) {
    // TypeScript garantiza que response.data existe y es Estudiante[]
    response.data.forEach((estudiante) => {
      console.log(estudiante.nombre); // Autocompletado perfecto
    });
  } else if (response.error) {
    // TypeScript garantiza que response.error existe y tiene la estructura correcta
    console.error(`Error ${response.error.code}: ${response.error.message}`);
  }
}
```

#### **7. Escalabilidad en Proyectos Grandes**

**Ventaja:** TypeScript brilla especialmente en proyectos grandes y complejos.

```typescript
// En un proyecto grande, TypeScript mantiene la consistencia:

// Archivo: tipos/estudiante.ts
export interface Estudiante {
  id: number;
  nombre: string;
  apellido: string;
  email: string;
  carrera: CarreraTecnica;
  materias: Materia[];
  estado: EstadoEstudiante;
}

export type CarreraTecnica =
  | "Programación y Análisis de Sistemas"
  | "Testing y Calidad de Software"
  | "Metodologías de Desarrollo";

export type EstadoEstudiante =
  | "Activo"
  | "Suspendido"
  | "Graduado"
  | "Retirado";

// Archivo: servicios/estudianteService.ts
import { Estudiante, CarreraTecnica } from "../tipos/estudiante";

export class EstudianteService {
  async crearEstudiante(datos: Omit<Estudiante, "id">): Promise<Estudiante> {
    // TypeScript verifica que se pasen todos los campos requeridos excepto 'id'
    // ...
  }

  async obtenerPorCarrera(carrera: CarreraTecnica): Promise<Estudiante[]> {
    // TypeScript garantiza que 'carrera' es uno de los valores válidos
    // ...
  }
}

// Archivo: componentes/estudianteList.ts
import { EstudianteService } from "../servicios/estudianteService";

class EstudianteListComponent {
  private service = new EstudianteService();

  async cargarEstudiantes() {
    // Autocompletado perfecto en toda la cadena:
    const estudiantes = await this.service.obtenerPorCarrera(
      "Programación y Análisis de Sistemas"
    );

    estudiantes.forEach((est) => {
      // Perfecto autocompletado y verificación de tipos:
      console.log(`${est.nombre} ${est.apellido} - ${est.carrera}`);
    });
  }
}
```

#### **8. Casos de Uso Específicos en Desarrollo Web**

```typescript
// 1. APIs type-safe
interface PeticionLogin {
  email: string;
  password: string;
  recordarme?: boolean;
}

interface RespuestaLogin {
  token: string;
  usuario: {
    id: number;
    nombre: string;
    rol: "estudiante" | "profesor" | "admin";
  };
  expiresIn: number;
}

async function login(credenciales: PeticionLogin): Promise<RespuestaLogin> {
  // TypeScript garantiza que la petición y respuesta tienen la estructura correcta
  const response = await fetch("/api/login", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(credenciales),
  });

  return response.json() as RespuestaLogin;
}

// 2. Configuraciones type-safe
interface ConfiguracionBD {
  host: string;
  puerto: number;
  baseDatos: string;
  usuario: string;
  contraseña: string;
  ssl: boolean;
  pool: {
    min: number;
    max: number;
    timeout: number;
  };
}

function conectarBD(config: ConfiguracionBD): Promise<void> {
  // TypeScript verifica que todas las configuraciones requeridas estén presentes
  // ...
}
```

#### **Resumen de Ventajas Clave**

| Ventaja                           | Beneficio Directo                       | Ejemplo de Impacto                     |
| --------------------------------- | --------------------------------------- | -------------------------------------- |
| **Detección temprana de errores** | Bugs encontrados antes de producción    | 60-80% menos errores en runtime        |
| **Autocompletado inteligente**    | Desarrollo más rápido y preciso         | 30-50% menos tiempo escribiendo código |
| **Refactoring seguro**            | Cambios sin miedo a romper código       | Proyectos escalables sin deuda técnica |
| **Documentación viva**            | Código autodocumentado                  | Menos tiempo entendiendo código ajeno  |
| **Trabajo en equipo**             | Interfaces claras entre desarrolladores | Menos conflictos y malentendidos       |
| **Mantenimiento**                 | Código más fácil de mantener            | Proyectos sostenibles a largo plazo    |

**💡 Punto clave para estudiantes:** El tipado estático puede parecer "más trabajo" al principio, pero es una inversión que se paga exponencialmente. Es como aprender a manejar con cinturón de seguridad: inicialmente puede sentirse restrictivo, pero te salva de accidentes graves.

### Sintaxis fundamental de TypeScript

Ahora que entendemos qué es TypeScript y sus ventajas, es hora de aprender su sintaxis fundamental. TypeScript extiende JavaScript con **anotaciones de tipos** y otras características modernas.

#### **1. Tipos Primitivos**

Los tipos básicos en TypeScript corresponden a los tipos de JavaScript, pero con anotaciones explícitas:

```typescript
// Tipos primitivos básicos
let nombre: string = "Juan Pérez";
let edad: number = 25;
let esEstudiante: boolean = true;
let indefinido: undefined = undefined;
let nulo: null = null;

// Números soportan diferentes formatos
let entero: number = 42;
let decimal: number = 3.14159;
let hexadecimal: number = 0xff;
let binario: number = 0b1010;
let octal: number = 0o744;

// Strings soportan template literals
let saludo: string = `Hola ${nombre}, tienes ${edad} años`;
let mensaje: string = "Bienvenido al AIEP";
let texto: string = "Carrera: Programación";
```

#### **2. Arrays y Tipos de Colecciones**

TypeScript ofrece varias formas de definir arrays y colecciones:

```typescript
// Arrays - Sintaxis tradicional
let numeros: number[] = [1, 2, 3, 4, 5];
let nombres: string[] = ["Ana", "Carlos", "María"];
let activos: boolean[] = [true, false, true];

// Arrays - Sintaxis genérica (alternativa)
let calificaciones: Array<number> = [8.5, 9.2, 7.8];
let materias: Array<string> = ["Matemáticas", "Programación", "Base de Datos"];

// Arrays multidimensionales
let matriz: number[][] = [
  [1, 2, 3],
  [4, 5, 6],
  [7, 8, 9],
];

// Arrays mixtos con union types
let datos: (string | number)[] = ["Juan", 25, "Programación", 2025];

// Tuplas - Arrays con longitud y tipos fijos
let estudiante: [string, number, string] = ["Ana García", 22, "Testing"];
let coordenadas: [number, number] = [10.5, -33.8];

// Tuplas con elementos opcionales y rest
let puntoCompleto: [number, number, string?] = [10, 20, "Punto A"];
let configuracion: [string, ...number[]] = ["servidor", 80, 443, 8080];
```

#### **3. Objetos y Tipos de Objetos**

```typescript
// Objetos con tipos implícitos
let persona = {
  nombre: "Diego",
  edad: 30,
  esProfesor: true,
}; // TypeScript infiere los tipos automáticamente

// Objetos con tipos explícitos
let estudiante: {
  nombre: string;
  edad: number;
  carrera: string;
  notas: number[];
  activo: boolean;
} = {
  nombre: "María González",
  edad: 20,
  carrera: "Programación",
  notas: [8.5, 9.0, 7.5],
  activo: true,
};

// Propiedades opcionales
let profesor: {
  nombre: string;
  especialidad: string;
  telefono?: string; // Propiedad opcional
  email?: string; // Propiedad opcional
} = {
  nombre: "Carlos Ruiz",
  especialidad: "Desarrollo Web",
  // telefono y email no son requeridos
};

// Propiedades readonly
let configuracionSistema: {
  readonly version: string;
  readonly fechaCreacion: Date;
  nombre: string;
} = {
  version: "1.0.0",
  fechaCreacion: new Date(),
  nombre: "Sistema Académico",
};

// configuracionSistema.version = "2.0.0";  // ❌ Error: Cannot assign to 'version' because it is a read-only property
configuracionSistema.nombre = "Nuevo Sistema"; // ✅ Permitido
```

#### **4. Funciones y sus Tipos**

```typescript
// Función básica con tipos
function sumar(a: number, b: number): number {
  return a + b;
}

// Función con parámetros opcionales
function saludar(nombre: string, apellido?: string): string {
  if (apellido) {
    return `Hola ${nombre} ${apellido}`;
  }
  return `Hola ${nombre}`;
}

// Función con parámetros por defecto
function crearUsuario(
  nombre: string,
  rol: string = "estudiante",
  activo: boolean = true
): object {
  return { nombre, rol, activo };
}

// Función con rest parameters
function calcularPromedio(nombre: string, ...notas: number[]): number {
  const suma = notas.reduce((total, nota) => total + nota, 0);
  return suma / notas.length;
}

// Funciones como expresiones
const multiplicar = function (x: number, y: number): number {
  return x * y;
};

// Arrow functions
const dividir = (a: number, b: number): number => {
  if (b === 0) throw new Error("División por cero");
  return a / b;
};

// Función que retorna función (Higher-order function)
function crearSaludo(prefijo: string): (nombre: string) => string {
  return function (nombre: string): string {
    return `${prefijo} ${nombre}`;
  };
}

const saludoFormal = crearSaludo("Buenos días");
console.log(saludoFormal("María")); // "Buenos días María"

// Función con múltiples tipos de retorno
function procesarDato(dato: string | number): string | number {
  if (typeof dato === "string") {
    return dato.toUpperCase();
  }
  return dato * 2;
}
```

#### **5. Interfaces - Contratos de Código**

Las interfaces definen la estructura que deben seguir los objetos:

```typescript
// Interface básica
interface Estudiante {
  id: number;
  nombre: string;
  apellido: string;
  edad: number;
  carrera: string;
  activo: boolean;
}

// Usando la interface
const nuevoEstudiante: Estudiante = {
  id: 1,
  nombre: "Ana",
  apellido: "Martínez",
  edad: 19,
  carrera: "Programación",
  activo: true,
};

// Interface con propiedades opcionales
interface Materia {
  codigo: string;
  nombre: string;
  creditos: number;
  prerequisito?: string; // Opcional
  profesor?: string; // Opcional
}

// Interface con métodos
interface CalculadoraNotas {
  materias: Materia[];
  agregarMateria(materia: Materia): void;
  calcularPromedio(): number;
  obtenerMateriasPorCreditos(creditos: number): Materia[];
}

// Interface extendida (herencia)
interface EstudianteAvanzado extends Estudiante {
  semestre: number;
  especialidad: string;
  proyectoTitulo?: string;
}

// Interface para funciones
interface FuncionCalculadora {
  (a: number, b: number): number;
}

const sumarNumeros: FuncionCalculadora = (x, y) => x + y;
const restarNumeros: FuncionCalculadora = (x, y) => x - y;

// Interface con propiedades indexables
interface ListaCalificaciones {
  [materia: string]: number;
}

const misCalificaciones: ListaCalificaciones = {
  Matemáticas: 8.5,
  Programación: 9.2,
  "Base de Datos": 7.8,
};
```

#### **6. Types vs Interfaces**

```typescript
// Type alias - Similar a interface pero más flexible
type Punto = {
  x: number;
  y: number;
};

type Coordenada3D = {
  x: number;
  y: number;
  z: number;
};

// Union types con type alias
type EstadoConexion = "conectado" | "desconectado" | "conectando";
type ResultadoOperacion = "exito" | "error" | "pendiente";

// Intersection types - Combinar tipos
type PuntoConColor = Punto & {
  color: string;
};

const puntoRojo: PuntoConColor = {
  x: 10,
  y: 20,
  color: "rojo",
};

// Conditional types
type TipoMensaje<T> = T extends string ? string : number;

type Mensaje1 = TipoMensaje<string>; // string
type Mensaje2 = TipoMensaje<number>; // number

// Mapped types
type Opcional<T> = {
  [K in keyof T]?: T[K];
};

type EstudianteOpcional = Opcional<Estudiante>;
// Todos los campos de Estudiante ahora son opcionales
```

#### **7. Union Types y Type Guards**

```typescript
// Union types - Múltiples tipos posibles
type ID = number | string;
type EstadoRespuesta = "cargando" | "exito" | "error";

function obtenerUsuario(id: ID): string {
  // Type guard con typeof
  if (typeof id === "string") {
    return `Usuario: ${id.toUpperCase()}`; // TypeScript sabe que id es string
  } else {
    return `Usuario ID: ${id.toString()}`; // TypeScript sabe que id es number
  }
}

// Discriminated unions
interface CargandoState {
  estado: "cargando";
  progreso: number;
}

interface ExitoState {
  estado: "exito";
  datos: any[];
}

interface ErrorState {
  estado: "error";
  mensaje: string;
}

type AppState = CargandoState | ExitoState | ErrorState;

function manejarEstado(state: AppState): string {
  // Type guard con propiedades discriminantes
  switch (state.estado) {
    case "cargando":
      return `Cargando... ${state.progreso}%`;
    case "exito":
      return `Datos cargados: ${state.datos.length} elementos`;
    case "error":
      return `Error: ${state.mensaje}`;
    default:
      // TypeScript garantiza que todos los casos están cubiertos
      const _exhaustive: never = state;
      return _exhaustive;
  }
}
```

#### **8. Enums - Enumeraciones**

```typescript
// Enum numérico básico
enum DiaSemana {
  Lunes, // 0
  Martes, // 1
  Miercoles, // 2
  Jueves, // 3
  Viernes, // 4
  Sabado, // 5
  Domingo, // 6
}

// Enum con valores personalizados
enum EstadoMatricula {
  Pendiente = "PENDIENTE",
  Aprobada = "APROBADA",
  Rechazada = "RECHAZADA",
  Cancelada = "CANCELADA",
}

// Enum mixto
enum CodigoRespuesta {
  Exito = 200,
  NoEncontrado = 404,
  ErrorInterno = 500,
  SinAutorizacion = "UNAUTHORIZED",
}

// Uso de enums
let hoy: DiaSemana = DiaSemana.Lunes;
let estadoActual: EstadoMatricula = EstadoMatricula.Pendiente;

function procesarMatricula(estado: EstadoMatricula): string {
  switch (estado) {
    case EstadoMatricula.Pendiente:
      return "Matrícula en proceso de revisión";
    case EstadoMatricula.Aprobada:
      return "Matrícula aprobada exitosamente";
    case EstadoMatricula.Rechazada:
      return "Matrícula rechazada";
    case EstadoMatricula.Cancelada:
      return "Matrícula cancelada por el estudiante";
    default:
      return "Estado desconocido";
  }
}

// Const enums (más eficiente)
const enum Direccion {
  Arriba = "ARRIBA",
  Abajo = "ABAJO",
  Izquierda = "IZQUIERDA",
  Derecha = "DERECHA",
}

let movimiento: Direccion = Direccion.Arriba;
```

#### **9. Generics - Tipos Genéricos**

```typescript
// Función genérica básica
function identity<T>(arg: T): T {
  return arg;
}

// Uso de la función genérica
let numeroId = identity<number>(42); // T = number
let textoId = identity<string>("Hola"); // T = string
let autoId = identity("Auto"); // TypeScript infiere T = string

// Clase genérica
class Lista<T> {
  private items: T[] = [];

  agregar(item: T): void {
    this.items.push(item);
  }

  obtener(index: number): T | undefined {
    return this.items[index];
  }

  obtenerTodos(): T[] {
    return [...this.items];
  }

  filtrar(callback: (item: T) => boolean): T[] {
    return this.items.filter(callback);
  }
}

// Uso de clase genérica
const listaNumeros = new Lista<number>();
listaNumeros.agregar(1);
listaNumeros.agregar(2);

const listaEstudiantes = new Lista<Estudiante>();
listaEstudiantes.agregar(nuevoEstudiante);

// Interface genérica
interface Repositorio<T> {
  crear(item: T): Promise<T>;
  obtenerPorId(id: number): Promise<T | null>;
  actualizar(id: number, item: Partial<T>): Promise<T>;
  eliminar(id: number): Promise<boolean>;
  obtenerTodos(): Promise<T[]>;
}

// Implementación del repositorio
class EstudianteRepositorio implements Repositorio<Estudiante> {
  async crear(estudiante: Estudiante): Promise<Estudiante> {
    // Lógica para crear estudiante
    return estudiante;
  }

  async obtenerPorId(id: number): Promise<Estudiante | null> {
    // Lógica para obtener estudiante
    return null;
  }

  async actualizar(
    id: number,
    datos: Partial<Estudiante>
  ): Promise<Estudiante> {
    // Lógica para actualizar estudiante
    throw new Error("Método no implementado");
  }

  async eliminar(id: number): Promise<boolean> {
    // Lógica para eliminar estudiante
    return true;
  }

  async obtenerTodos(): Promise<Estudiante[]> {
    // Lógica para obtener todos los estudiantes
    return [];
  }
}

// Constraints en genéricos
interface TieneId {
  id: number;
}

function actualizarEntidad<T extends TieneId>(
  entidad: T,
  datos: Partial<T>
): T {
  return { ...entidad, ...datos };
}

// Solo funciona con objetos que tienen propiedad 'id'
const estudianteActualizado = actualizarEntidad(nuevoEstudiante, {
  nombre: "Ana María",
});
```

#### **10. Ejemplo Integrado: Sistema de Gestión Académica**

```typescript
// Definición de tipos base
enum TipoUsuario {
  Estudiante = "ESTUDIANTE",
  Profesor = "PROFESOR",
  Administrador = "ADMIN",
}

interface UsuarioBase {
  id: number;
  nombre: string;
  apellido: string;
  email: string;
  tipo: TipoUsuario;
  activo: boolean;
  fechaRegistro: Date;
}

interface EstudianteCompleto extends UsuarioBase {
  tipo: TipoUsuario.Estudiante;
  matricula: string;
  semestre: number;
  carrera: string;
  materias: MateriaEstudiante[];
}

interface ProfesorCompleto extends UsuarioBase {
  tipo: TipoUsuario.Profesor;
  especialidades: string[];
  materiasImpartidas: string[];
  oficina?: string;
}

type Usuario =
  | EstudianteCompleto
  | ProfesorCompleto
  | (UsuarioBase & { tipo: TipoUsuario.Administrador });

interface MateriaEstudiante {
  codigo: string;
  nombre: string;
  creditos: number;
  profesor: string;
  notas: Nota[];
  estado: "cursando" | "aprobada" | "reprobada";
}

interface Nota {
  tipo: "parcial" | "final" | "proyecto" | "tarea";
  valor: number;
  fecha: Date;
  observaciones?: string;
}

// Servicio genérico
class ServicioAcademico<T extends UsuarioBase> {
  private usuarios: T[] = [];

  agregar(usuario: T): void {
    this.usuarios.push(usuario);
  }

  buscarPorId(id: number): T | undefined {
    return this.usuarios.find((u) => u.id === id);
  }

  filtrarPorTipo<U extends T>(tipo: TipoUsuario): U[] {
    return this.usuarios.filter((u) => u.tipo === tipo) as U[];
  }

  calcularPromedio(estudianteId: number): number | null {
    const usuario = this.buscarPorId(estudianteId);

    if (!usuario || usuario.tipo !== TipoUsuario.Estudiante) {
      return null;
    }

    const estudiante = usuario as EstudianteCompleto;
    let totalPuntos = 0;
    let totalCreditos = 0;

    estudiante.materias.forEach((materia) => {
      if (materia.estado === "aprobada" && materia.notas.length > 0) {
        const promedioMateria =
          materia.notas.reduce((sum, nota) => sum + nota.valor, 0) /
          materia.notas.length;
        totalPuntos += promedioMateria * materia.creditos;
        totalCreditos += materia.creditos;
      }
    });

    return totalCreditos > 0 ? totalPuntos / totalCreditos : 0;
  }
}

// Uso del sistema
const sistemaAcademico = new ServicioAcademico<Usuario>();

const estudianteEjemplo: EstudianteCompleto = {
  id: 1,
  nombre: "María",
  apellido: "González",
  email: "maria.gonzalez@aiep.cl",
  tipo: TipoUsuario.Estudiante,
  activo: true,
  fechaRegistro: new Date(),
  matricula: "2025001",
  semestre: 3,
  carrera: "Programación y Análisis de Sistemas",
  materias: [
    {
      codigo: "PRO205",
      nombre: "Taller de Programación",
      creditos: 4,
      profesor: "Diego Obando",
      notas: [
        {
          tipo: "parcial",
          valor: 8.5,
          fecha: new Date(),
          observaciones: "Excelente comprensión",
        },
        { tipo: "proyecto", valor: 9.0, fecha: new Date() },
      ],
      estado: "cursando",
    },
  ],
};

sistemaAcademico.agregar(estudianteEjemplo);
const promedio = sistemaAcademico.calcularPromedio(1);
console.log(`Promedio del estudiante: ${promedio?.toFixed(2)}`);
```

**💡 Puntos clave de la sintaxis:**

1. **Anotaciones de tipos**: Proporcionan claridad y seguridad
2. **Inferencia de tipos**: TypeScript puede deducir tipos automáticamente
3. **Flexibilidad**: Puedes ser tan específico o general como necesites
4. **Escalabilidad**: Los tipos complejos se construyen a partir de tipos simples
5. **Reutilización**: Interfaces y tipos genéricos promueven código reutilizable

---

## Fundamentos de Programación Orientada a Objetos

Ahora que dominamos la sintaxis básica de TypeScript, es momento de adentrarnos en **la Programación Orientada a Objetos (POO)**. Este paradigma de programación se basa en el concepto de **"objetos"** que contienen datos (propiedades) y código (métodos).

### ¿Qué es la Programación Orientada a Objetos?

#### **Definición**

**La Programación Orientada a Objetos (POO) es un paradigma de programación** que utiliza **objetos** y sus **interacciones** para diseñar aplicaciones y programas de computadora.

**Conceptos fundamentales:**

- **Objeto**: Una entidad que combina datos (atributos) y comportamientos (métodos)
- **Clase**: Un molde o plantilla para crear objetos
- **Instancia**: Un objeto específico creado a partir de una clase

#### **¿Por qué usar POO?**

1. **Modelado del mundo real**: Los objetos de software representan entidades del mundo real
2. **Reutilización de código**: Las clases pueden ser utilizadas múltiples veces
3. **Mantenibilidad**: El código está organizado en unidades lógicas
4. **Escalabilidad**: Fácil agregar nuevas características
5. **Colaboración en equipos**: Diferentes desarrolladores pueden trabajar en diferentes clases

#### **Ejemplo del mundo real**

Pensemos en un **estudiante del AIEP**:

```typescript
// En el mundo real, un estudiante tiene:
// - Características (nombre, edad, carrera)
// - Acciones que puede realizar (estudiar, tomar examen, matricularse)

// En POO, esto se modela como:
class Estudiante {
  // Propiedades (características)
  nombre: string;
  edad: number;
  carrera: string;
  notas: number[];

  // Constructor (cómo se crea un estudiante)
  constructor(nombre: string, edad: number, carrera: string) {
    this.nombre = nombre;
    this.edad = edad;
    this.carrera = carrera;
    this.notas = [];
  }

  // Métodos (acciones que puede realizar)
  estudiar(materia: string): void {
    console.log(`${this.nombre} está estudiando ${materia}`);
  }

  tomarExamen(nota: number): void {
    this.notas.push(nota);
    console.log(`${this.nombre} obtuvo ${nota} en el examen`);
  }

  calcularPromedio(): number {
    if (this.notas.length === 0) return 0;
    const suma = this.notas.reduce((total, nota) => total + nota, 0);
    return suma / this.notas.length;
  }
}

// Crear objetos (instancias) de la clase
const estudiante1 = new Estudiante("Ana García", 20, "Programación");
const estudiante2 = new Estudiante("Carlos López", 22, "Testing");

// Usar los objetos
estudiante1.estudiar("TypeScript");
estudiante1.tomarExamen(8.5);
estudiante1.tomarExamen(9.0);

console.log(
  `Promedio de ${estudiante1.nombre}: ${estudiante1.calcularPromedio()}`
);
```

### Clases en TypeScript

#### **Sintaxis Básica de Clases**

```typescript
class NombreDeLaClase {
  // Propiedades (atributos)
  propiedad1: tipo;
  propiedad2: tipo;

  // Constructor
  constructor(parametros) {
    // Inicialización
  }

  // Métodos
  metodo1(): tipoRetorno {
    // Lógica del método
  }
}
```

#### **Ejemplo Detallado: Clase Producto**

```typescript
class Producto {
  // Propiedades
  private id: number;
  public nombre: string;
  public precio: number;
  public categoria: string;
  private stock: number;
  public activo: boolean;

  // Constructor
  constructor(
    id: number,
    nombre: string,
    precio: number,
    categoria: string,
    stock: number = 0
  ) {
    this.id = id;
    this.nombre = nombre;
    this.precio = precio;
    this.categoria = categoria;
    this.stock = stock;
    this.activo = true;
  }

  // Métodos públicos
  obtenerInfo(): string {
    return `${this.nombre} - $${this.precio} (${this.categoria})`;
  }

  calcularPrecioConIva(iva: number = 0.19): number {
    return this.precio * (1 + iva);
  }

  estaDisponible(): boolean {
    return this.activo && this.stock > 0;
  }

  // Métodos para manejo de stock
  agregarStock(cantidad: number): void {
    if (cantidad > 0) {
      this.stock += cantidad;
      console.log(
        `Se agregaron ${cantidad} unidades. Stock actual: ${this.stock}`
      );
    }
  }

  vender(cantidad: number): boolean {
    if (cantidad <= this.stock && this.activo) {
      this.stock -= cantidad;
      console.log(`Venta realizada. Stock restante: ${this.stock}`);
      return true;
    } else {
      console.log("No hay suficiente stock o el producto no está activo");
      return false;
    }
  }

  // Getter y Setter
  get stockActual(): number {
    return this.stock;
  }

  set stockActual(cantidad: number) {
    if (cantidad >= 0) {
      this.stock = cantidad;
    }
  }

  get idProducto(): number {
    return this.id;
  }

  // Método estático
  // Un método estático se puede llamar sin crear una instancia de la clase
  static compararPrecios(producto1: Producto, producto2: Producto): string {
    if (producto1.precio > producto2.precio) {
      return `${producto1.nombre} es más caro que ${producto2.nombre}`;
    } else if (producto1.precio < producto2.precio) {
      return `${producto2.nombre} es más caro que ${producto1.nombre}`;
    } else {
      return `${producto1.nombre} y ${producto2.nombre} tienen el mismo precio`;
    }
  }
}

// Uso de la clase
const laptop = new Producto(1, "Laptop Dell", 899990, "Computadores", 5);
const mouse = new Producto(2, "Mouse Logitech", 25990, "Accesorios", 15);

// Usar métodos
console.log(laptop.obtenerInfo());
console.log(`Precio con IVA: $${laptop.calcularPrecioConIva()}`);

laptop.vender(2);
console.log(`Stock actual: ${laptop.stockActual}`);

// Usar método estático
console.log(Producto.compararPrecios(laptop, mouse));
```

#### **Modificadores de Acceso**

TypeScript proporciona tres modificadores de acceso:

```typescript
class EjemploModificadores {
  public propiedadPublica: string; // Accesible desde cualquier lugar
  private propiedadPrivada: number; // Solo accesible dentro de la clase
  protected propiedadProtegida: boolean; // Accesible en la clase y subclases

  constructor() {
    this.propiedadPublica = "Visible desde fuera";
    this.propiedadPrivada = 42;
    this.propiedadProtegida = true;
  }

  public metodoPublico(): void {
    console.log("Método público");
    this.metodoPrivado(); // ✅ Puede llamar método privado desde dentro
  }

  private metodoPrivado(): void {
    console.log("Método privado");
  }

  protected metodoProtegido(): void {
    console.log("Método protegido");
  }
}

const ejemplo = new EjemploModificadores();
console.log(ejemplo.propiedadPublica); // ✅ Funciona
// console.log(ejemplo.propiedadPrivada);    // ❌ Error: Property is private
// console.log(ejemplo.propiedadProtegida);  // ❌ Error: Property is protected

ejemplo.metodoPublico(); // ✅ Funciona
// ejemplo.metodoPrivado();                  // ❌ Error: Method is private
// ejemplo.metodoProtegido();                // ❌ Error: Method is protected
```

#### **Propiedades de Solo Lectura**

```typescript
class Usuario {
  readonly id: number;
  readonly fechaCreacion: Date;
  nombre: string;
  email: string;

  constructor(id: number, nombre: string, email: string) {
    this.id = id;
    this.fechaCreacion = new Date();
    this.nombre = nombre;
    this.email = email;
  }

  actualizarPerfil(nombre: string, email: string): void {
    this.nombre = nombre;
    this.email = email;

    // ❌ Estas líneas darían error:
    // this.id = 999;              // Error: Cannot assign to 'id' because it is read-only
    // this.fechaCreacion = new Date(); // Error: Cannot assign to 'fechaCreacion' because it is read-only
  }
}
```

#### **Sintaxis Abreviada para Constructores**

TypeScript permite una sintaxis más concisa para definir propiedades en el constructor:

```typescript
// Forma tradicional
class EstudianteTradicional {
  nombre: string;
  edad: number;
  carrera: string;

  constructor(nombre: string, edad: number, carrera: string) {
    this.nombre = nombre;
    this.edad = edad;
    this.carrera = carrera;
  }
}

// Forma abreviada (equivalente)
class EstudianteAbreviado {
  constructor(
    public nombre: string,
    public edad: number,
    public carrera: string,
    private id: number = Math.floor(Math.random() * 1000)
  ) {
    // El cuerpo puede estar vacío o contener lógica adicional
    console.log(`Nuevo estudiante creado: ${this.nombre}`);
  }

  obtenerInfo(): string {
    return `${this.nombre}, ${this.edad} años, carrera: ${this.carrera}`;
  }
}

// Ambas formas funcionan igual
const estudiante = new EstudianteAbreviado(
  "María González",
  21,
  "Programación"
);
console.log(estudiante.obtenerInfo());
```

### Ejemplo Práctico Integrado: Sistema de Biblioteca

Veamos un ejemplo más complejo que integra todos los conceptos:

```typescript
// Enums para estados
enum EstadoLibro {
  Disponible = "DISPONIBLE",
  Prestado = "PRESTADO",
  Reservado = "RESERVADO",
  Mantenimiento = "MANTENIMIENTO",
}

enum TipoUsuario {
  Estudiante = "ESTUDIANTE",
  Profesor = "PROFESOR",
  Bibliotecario = "BIBLIOTECARIO",
}

// Clase base para usuarios
class UsuarioBiblioteca {
  private static contadorId: number = 1;

  constructor(
    public readonly id: number = UsuarioBiblioteca.contadorId++,
    public nombre: string,
    public email: string,
    public tipo: TipoUsuario,
    private activo: boolean = true
  ) {}

  public estaActivo(): boolean {
    return this.activo;
  }

  public desactivar(): void {
    this.activo = false;
  }

  public activar(): void {
    this.activo = true;
  }

  public obtenerInfo(): string {
    return `${this.nombre} (${this.tipo}) - ${this.email}`;
  }
}

// Clase para libros
class Libro {
  private static contadorId: number = 1;

  constructor(
    public readonly id: number = Libro.contadorId++,
    public titulo: string,
    public autor: string,
    public isbn: string,
    public categoria: string,
    private estado: EstadoLibro = EstadoLibro.Disponible,
    private fechaAdquisicion: Date = new Date()
  ) {}

  // Getters
  get estadoActual(): EstadoLibro {
    return this.estado;
  }

  get fechaDeAdquisicion(): Date {
    return this.fechaAdquisicion;
  }

  // Métodos para cambiar estado
  public prestar(): boolean {
    if (this.estado === EstadoLibro.Disponible) {
      this.estado = EstadoLibro.Prestado;
      return true;
    }
    return false;
  }

  public devolver(): boolean {
    if (this.estado === EstadoLibro.Prestado) {
      this.estado = EstadoLibro.Disponible;
      return true;
    }
    return false;
  }

  public reservar(): boolean {
    if (this.estado === EstadoLibro.Disponible) {
      this.estado = EstadoLibro.Reservado;
      return true;
    }
    return false;
  }

  public enviarAMantenimiento(): void {
    this.estado = EstadoLibro.Mantenimiento;
  }

  public obtenerInfo(): string {
    return `"${this.titulo}" por ${this.autor} - Estado: ${this.estado}`;
  }

  public estaDisponible(): boolean {
    return this.estado === EstadoLibro.Disponible;
  }
}

// Clase para préstamos
class Prestamo {
  private static contadorId: number = 1;

  constructor(
    public readonly id: number = Prestamo.contadorId++,
    public usuario: UsuarioBiblioteca,
    public libro: Libro,
    public fechaPrestamo: Date = new Date(),
    public fechaDevolucionEsperada: Date = new Date(
      Date.now() + 14 * 24 * 60 * 60 * 1000
    ), // 14 días
    public fechaDevolucionReal?: Date
  ) {}

  public devolver(): void {
    this.fechaDevolucionReal = new Date();
    this.libro.devolver();
  }

  public estaVencido(): boolean {
    if (this.fechaDevolucionReal) {
      return false; // Ya fue devuelto
    }
    return new Date() > this.fechaDevolucionEsperada;
  }

  public diasRetraso(): number {
    if (this.fechaDevolucionReal || !this.estaVencido()) {
      return 0;
    }
    const hoy = new Date();
    const diferencia = hoy.getTime() - this.fechaDevolucionEsperada.getTime();
    return Math.floor(diferencia / (1000 * 60 * 60 * 24));
  }

  public obtenerInfo(): string {
    const estado = this.fechaDevolucionReal
      ? "DEVUELTO"
      : this.estaVencido()
      ? "VENCIDO"
      : "ACTIVO";
    return `Préstamo ${this.id}: ${this.libro.titulo} - ${this.usuario.nombre} - Estado: ${estado}`;
  }
}

// Sistema de gestión de biblioteca
class SistemaBiblioteca {
  private usuarios: UsuarioBiblioteca[] = [];
  private libros: Libro[] = [];
  private prestamos: Prestamo[] = [];

  // Gestión de usuarios
  public registrarUsuario(
    nombre: string,
    email: string,
    tipo: TipoUsuario
  ): UsuarioBiblioteca {
    const usuario = new UsuarioBiblioteca(undefined, nombre, email, tipo);
    this.usuarios.push(usuario);
    console.log(`Usuario registrado: ${usuario.obtenerInfo()}`);
    return usuario;
  }

  // Gestión de libros
  public agregarLibro(
    titulo: string,
    autor: string,
    isbn: string,
    categoria: string
  ): Libro {
    const libro = new Libro(undefined, titulo, autor, isbn, categoria);
    this.libros.push(libro);
    console.log(`Libro agregado: ${libro.obtenerInfo()}`);
    return libro;
  }

  // Búsqueda de libros
  public buscarLibrosPorTitulo(titulo: string): Libro[] {
    return this.libros.filter((libro) =>
      libro.titulo.toLowerCase().includes(titulo.toLowerCase())
    );
  }

  public buscarLibrosPorAutor(autor: string): Libro[] {
    return this.libros.filter((libro) =>
      libro.autor.toLowerCase().includes(autor.toLowerCase())
    );
  }

  public obtenerLibrosDisponibles(): Libro[] {
    return this.libros.filter((libro) => libro.estaDisponible());
  }

  // Sistema de préstamos
  public prestarLibro(usuarioId: number, libroId: number): Prestamo | null {
    const usuario = this.usuarios.find((u) => u.id === usuarioId);
    const libro = this.libros.find((l) => l.id === libroId);

    if (!usuario) {
      console.log("Usuario no encontrado");
      return null;
    }

    if (!libro) {
      console.log("Libro no encontrado");
      return null;
    }

    if (!usuario.estaActivo()) {
      console.log("Usuario no está activo");
      return null;
    }

    if (!libro.estaDisponible()) {
      console.log("Libro no está disponible");
      return null;
    }

    if (libro.prestar()) {
      const prestamo = new Prestamo(undefined, usuario, libro);
      this.prestamos.push(prestamo);
      console.log(`Préstamo realizado: ${prestamo.obtenerInfo()}`);
      return prestamo;
    }

    return null;
  }

  public devolverLibro(prestamoId: number): boolean {
    const prestamo = this.prestamos.find((p) => p.id === prestamoId);

    if (!prestamo) {
      console.log("Préstamo no encontrado");
      return false;
    }

    if (prestamo.fechaDevolucionReal) {
      console.log("El libro ya fue devuelto");
      return false;
    }

    prestamo.devolver();
    console.log(`Libro devuelto: ${prestamo.obtenerInfo()}`);

    if (prestamo.diasRetraso() > 0) {
      console.log(
        `⚠️  Devolución con ${prestamo.diasRetraso()} días de retraso`
      );
    }

    return true;
  }

  // Reportes
  public obtenerPrestamosVencidos(): Prestamo[] {
    return this.prestamos.filter((prestamo) => prestamo.estaVencido());
  }

  public obtenerPrestamosActivos(): Prestamo[] {
    return this.prestamos.filter((prestamo) => !prestamo.fechaDevolucionReal);
  }

  public obtenerEstadisticas(): object {
    return {
      totalUsuarios: this.usuarios.length,
      totalLibros: this.libros.length,
      librosDisponibles: this.obtenerLibrosDisponibles().length,
      prestamosActivos: this.obtenerPrestamosActivos().length,
      prestamosVencidos: this.obtenerPrestamosVencidos().length,
    };
  }
}

// Ejemplo de uso del sistema
const biblioteca = new SistemaBiblioteca();

// Registrar usuarios
const estudiante1 = biblioteca.registrarUsuario(
  "Ana García",
  "ana@aiep.cl",
  TipoUsuario.Estudiante
);
const profesor1 = biblioteca.registrarUsuario(
  "Carlos Ruiz",
  "carlos@aiep.cl",
  TipoUsuario.Profesor
);

// Agregar libros
const libro1 = biblioteca.agregarLibro(
  "Clean Code",
  "Robert C. Martin",
  "978-0132350884",
  "Programación"
);
const libro2 = biblioteca.agregarLibro(
  "TypeScript Handbook",
  "Microsoft",
  "978-1234567890",
  "Programación"
);

// Realizar préstamos
const prestamo1 = biblioteca.prestarLibro(estudiante1.id, libro1.id);
const prestamo2 = biblioteca.prestarLibro(profesor1.id, libro2.id);

// Ver estadísticas
console.log("\n--- Estadísticas de la Biblioteca ---");
console.log(biblioteca.obtenerEstadisticas());

// Buscar libros
console.log("\n--- Búsqueda: libros con 'code' en el título ---");
const resultadoBusqueda = biblioteca.buscarLibrosPorTitulo("code");
resultadoBusqueda.forEach((libro) => console.log(libro.obtenerInfo()));
```

**💡 Conceptos clave aprendidos:**

1. **Clases como plantillas**: Definen la estructura de los objetos
2. **Encapsulación**: Uso de modificadores de acceso (public, private, protected)
3. **Constructor**: Inicializa objetos cuando se crean
4. **Métodos**: Definen el comportamiento de los objetos
5. **Propiedades estáticas**: Pertenecen a la clase, no a instancias específicas
6. **Getters y Setters**: Controlan el acceso a las propiedades
7. **Readonly**: Propiedades que no pueden modificarse después de la inicialización

---

## Los 4 Pilares de la Programación Orientada a Objetos

Los **4 pilares de la POO** son principios fundamentales que definen cómo diseñamos y estructuramos nuestras aplicaciones orientadas a objetos. Estos pilares son:

1. **🔒 Encapsulamiento** - Ocultar detalles internos
2. **🏗️ Abstracción** - Definir contratos y interfaces
3. **👨‍👩‍👧‍👦 Herencia** - Reutilizar código y crear jerarquías
4. **🎭 Polimorfismo** - Un mismo interface, múltiples comportamientos

### 🔒 1. Encapsulamiento

#### **¿Qué es el Encapsulamiento?**

**El encapsulamiento es el principio que consiste en ocultar los detalles internos de una clase** y exponer solo lo necesario a través de métodos públicos. Es como una **cápsula** que protege el contenido interno.

#### **Conceptos Clave:**

- **Datos privados**: No accesibles directamente desde fuera
- **Métodos públicos**: Interface controlada para interactuar con los datos
- **Validación**: Control sobre cómo se modifican los datos
- **Mantenimiento**: Cambios internos no afectan el código externo

#### **Ejemplo Práctico: Cuenta Bancaria**

```typescript
class CuentaBancaria {
  // Propiedades privadas - NO accesibles desde fuera
  private saldo: number;
  private numeroCuenta: string;
  private titular: string;
  private pin: string;
  private bloqueada: boolean;
  private intentosFallidos: number;

  constructor(
    numeroCuenta: string,
    titular: string,
    pin: string,
    saldoInicial: number = 0
  ) {
    this.numeroCuenta = numeroCuenta;
    this.titular = titular;
    this.pin = pin;
    this.saldo = saldoInicial >= 0 ? saldoInicial : 0;
    this.bloqueada = false;
    this.intentosFallidos = 0;
  }

  // Métodos públicos - Interface controlada
  public consultarSaldo(pinIngresado: string): number | null {
    if (!this.validarPin(pinIngresado)) {
      return null;
    }

    return this.saldo;
  }

  public depositar(cantidad: number, pinIngresado: string): boolean {
    if (!this.validarPin(pinIngresado)) {
      return false;
    }

    if (cantidad <= 0) {
      console.log("❌ La cantidad debe ser mayor a 0");
      return false;
    }

    this.saldo += cantidad;
    console.log(`✅ Depósito exitoso. Nuevo saldo: $${this.saldo}`);
    return true;
  }

  public retirar(cantidad: number, pinIngresado: string): boolean {
    if (!this.validarPin(pinIngresado)) {
      return false;
    }

    if (cantidad <= 0) {
      console.log("❌ La cantidad debe ser mayor a 0");
      return false;
    }

    if (cantidad > this.saldo) {
      console.log("❌ Saldo insuficiente");
      return false;
    }

    this.saldo -= cantidad;
    console.log(`✅ Retiro exitoso. Nuevo saldo: $${this.saldo}`);
    return true;
  }

  public transferir(
    cantidad: number,
    cuentaDestino: CuentaBancaria,
    pinIngresado: string
  ): boolean {
    if (!this.validarPin(pinIngresado)) {
      return false;
    }

    if (this.retirar(cantidad, pinIngresado)) {
      // Aquí normalmente usarías el PIN de la cuenta destino, pero para el ejemplo
      // asumimos que las transferencias no requieren PIN de destino
      if (cuentaDestino.recibirTransferencia(cantidad)) {
        console.log(`✅ Transferencia exitosa de $${cantidad}`);
        return true;
      } else {
        // Revertir el retiro si la transferencia falló
        this.saldo += cantidad;
        console.log("❌ Error en transferencia - Operación revertida");
        return false;
      }
    }

    return false;
  }

  // Método público de solo lectura
  public obtenerInformacionBasica(): object {
    return {
      numeroCuenta: this.numeroCuenta,
      titular: this.titular,
      bloqueada: this.bloqueada,
      // Nota: NO exponemos el saldo, PIN ni otros datos sensibles
    };
  }

  // Métodos privados - Lógica interna
  private validarPin(pinIngresado: string): boolean {
    if (this.bloqueada) {
      console.log("❌ Cuenta bloqueada. Contacte al banco.");
      return false;
    }

    if (pinIngresado !== this.pin) {
      this.intentosFallidos++;
      console.log(
        `❌ PIN incorrecto. Intentos fallidos: ${this.intentosFallidos}/3`
      );

      if (this.intentosFallidos >= 3) {
        this.bloquearCuenta();
      }

      return false;
    }

    // PIN correcto - resetear intentos fallidos
    this.intentosFallidos = 0;
    return true;
  }

  private bloquearCuenta(): void {
    this.bloqueada = true;
    console.log("🚫 Cuenta bloqueada por múltiples intentos fallidos");
  }

  private recibirTransferencia(cantidad: number): boolean {
    if (this.bloqueada) {
      return false;
    }

    this.saldo += cantidad;
    console.log(`✅ Transferencia recibida: $${cantidad}`);
    return true;
  }

  // Método para desbloquear (solo para administradores)
  public desbloquearCuenta(codigoAdmin: string): boolean {
    if (codigoAdmin === "ADMIN123") {
      // En la vida real esto sería más seguro
      this.bloqueada = false;
      this.intentosFallidos = 0;
      console.log("✅ Cuenta desbloqueada por administrador");
      return true;
    }
    return false;
  }
}

// Ejemplo de uso - Demostración del Encapsulamiento
const cuentaAna = new CuentaBancaria("12345-6", "Ana García", "1234", 1000);
const cuentaCarlos = new CuentaBancaria("78910-1", "Carlos López", "5678", 500);

// ✅ Formas CORRECTAS de interactuar (métodos públicos)
console.log("Saldo de Ana:", cuentaAna.consultarSaldo("1234"));
cuentaAna.depositar(500, "1234");
cuentaAna.retirar(200, "1234");
cuentaAna.transferir(100, cuentaCarlos, "1234");

// ❌ Estas líneas darían ERROR (propiedades privadas):
// console.log(cuentaAna.saldo);           // Error: Property 'saldo' is private
// console.log(cuentaAna.pin);             // Error: Property 'pin' is private
// cuentaAna.saldo = 999999;               // Error: Property 'saldo' is private
// cuentaAna.bloquearCuenta();             // Error: Property 'bloquearCuenta' is private

// Ejemplo de PIN incorrecto
cuentaAna.consultarSaldo("0000"); // Intento 1
cuentaAna.consultarSaldo("1111"); // Intento 2
cuentaAna.consultarSaldo("2222"); // Intento 3 - Cuenta se bloquea
cuentaAna.consultarSaldo("1234"); // Ya no funciona, cuenta bloqueada

// Desbloquear cuenta
cuentaAna.desbloquearCuenta("ADMIN123");
```

#### **Ventajas del Encapsulamiento:**

1. **Seguridad**: Los datos críticos están protegidos
2. **Validación**: Control sobre cómo se modifican los datos
3. **Mantenibilidad**: Cambios internos no afectan el código externo
4. **Debugging**: Más fácil encontrar errores
5. **Reutilización**: Interface clara y bien definida

### 🏗️ 2. Abstracción

#### **¿Qué es la Abstracción?**

**La abstracción consiste en definir interfaces y contratos** sin especificar la implementación concreta. Es como un **plano** que define qué se debe hacer, pero no cómo hacerlo.

#### **Conceptos Clave:**

- **Interfaces**: Contratos que definen qué métodos debe tener una clase
- **Clases abstractas**: Clases que no se pueden instanciar directamente
- **Métodos abstractos**: Métodos que deben ser implementados por las subclases
- **Ocultamiento de complejidad**: El usuario no necesita conocer los detalles internos

#### **Interfaces en TypeScript**

```typescript
// Interface que define el contrato para un sistema de pagos
interface SistemaPago {
  procesarPago(monto: number, referencia: string): Promise<boolean>;
  verificarPago(transaccionId: string): Promise<EstadoPago>;
  reembolsar(transaccionId: string, monto?: number): Promise<boolean>;
  obtenerComision(monto: number): number;
}

enum EstadoPago {
  Pendiente = "PENDIENTE",
  Aprobado = "APROBADO",
  Rechazado = "RECHAZADO",
  Reembolsado = "REEMBOLSADO",
}

// Implementación concreta para PayPal
class PayPalPago implements SistemaPago {
  private apiKey: string;
  private sandbox: boolean;

  constructor(apiKey: string, sandbox: boolean = true) {
    this.apiKey = apiKey;
    this.sandbox = sandbox;
  }

  async procesarPago(monto: number, referencia: string): Promise<boolean> {
    console.log(`💳 Procesando pago PayPal: $${monto} - Ref: ${referencia}`);

    // Simulación de llamada a API de PayPal
    const simulacionExito = Math.random() > 0.1; // 90% de éxito

    if (simulacionExito) {
      console.log("✅ Pago PayPal aprobado");
      return true;
    } else {
      console.log("❌ Pago PayPal rechazado");
      return false;
    }
  }

  async verificarPago(transaccionId: string): Promise<EstadoPago> {
    console.log(`🔍 Verificando pago PayPal: ${transaccionId}`);
    // Simulación
    return EstadoPago.Aprobado;
  }

  async reembolsar(transaccionId: string, monto?: number): Promise<boolean> {
    console.log(
      `↩️ Reembolsando PayPal: ${transaccionId} - $${monto || "total"}`
    );
    return true;
  }

  obtenerComision(monto: number): number {
    return monto * 0.035; // 3.5% comisión PayPal
  }
}

// Implementación concreta para Mercado Pago
class MercadoPagoPago implements SistemaPago {
  private accessToken: string;

  constructor(accessToken: string) {
    this.accessToken = accessToken;
  }

  async procesarPago(monto: number, referencia: string): Promise<boolean> {
    console.log(
      `💳 Procesando pago Mercado Pago: $${monto} - Ref: ${referencia}`
    );

    // Simulación de llamada a API de Mercado Pago
    const simulacionExito = Math.random() > 0.15; // 85% de éxito

    if (simulacionExito) {
      console.log("✅ Pago Mercado Pago aprobado");
      return true;
    } else {
      console.log("❌ Pago Mercado Pago rechazado");
      return false;
    }
  }

  async verificarPago(transaccionId: string): Promise<EstadoPago> {
    console.log(`🔍 Verificando pago Mercado Pago: ${transaccionId}`);
    return EstadoPago.Aprobado;
  }

  async reembolsar(transaccionId: string, monto?: number): Promise<boolean> {
    console.log(
      `↩️ Reembolsando Mercado Pago: ${transaccionId} - $${monto || "total"}`
    );
    return true;
  }

  obtenerComision(monto: number): number {
    return monto * 0.029; // 2.9% comisión Mercado Pago
  }
}

// Implementación para transferencia bancaria
class TransferenciaBancaria implements SistemaPago {
  private numeroCuenta: string;
  private banco: string;

  constructor(numeroCuenta: string, banco: string) {
    this.numeroCuenta = numeroCuenta;
    this.banco = banco;
  }

  async procesarPago(monto: number, referencia: string): Promise<boolean> {
    console.log(
      `🏦 Procesando transferencia bancaria: $${monto} - Ref: ${referencia}`
    );
    console.log(`Banco: ${this.banco} - Cuenta: ${this.numeroCuenta}`);

    // Las transferencias bancarias suelen ser más lentas pero más confiables
    const simulacionExito = Math.random() > 0.05; // 95% de éxito

    if (simulacionExito) {
      console.log("✅ Transferencia bancaria completada");
      return true;
    } else {
      console.log("❌ Transferencia bancaria falló");
      return false;
    }
  }

  async verificarPago(transaccionId: string): Promise<EstadoPago> {
    console.log(`🔍 Verificando transferencia: ${transaccionId}`);
    return EstadoPago.Aprobado;
  }

  async reembolsar(transaccionId: string, monto?: number): Promise<boolean> {
    console.log(
      `↩️ Reembolsando transferencia: ${transaccionId} - $${monto || "total"}`
    );
    return true;
  }

  obtenerComision(monto: number): number {
    return 1500; // Comisión fija de $1500
  }
}

// Procesador de pagos que utiliza la abstracción
class ProcesadorPagos {
  private sistemasPago: Map<string, SistemaPago> = new Map();

  registrarSistemaPago(nombre: string, sistema: SistemaPago): void {
    this.sistemasPago.set(nombre, sistema);
    console.log(`📝 Sistema de pago registrado: ${nombre}`);
  }

  async procesarPedido(
    monto: number,
    metodoPago: string,
    referencia: string
  ): Promise<{ exito: boolean; comision: number; transaccionId?: string }> {
    const sistema = this.sistemasPago.get(metodoPago);

    if (!sistema) {
      console.log(`❌ Sistema de pago no encontrado: ${metodoPago}`);
      return { exito: false, comision: 0 };
    }

    console.log(`\n🛒 Procesando pedido: $${monto} con ${metodoPago}`);

    const comision = sistema.obtenerComision(monto);
    console.log(`💰 Comisión calculada: $${comision}`);

    const exito = await sistema.procesarPago(monto, referencia);

    if (exito) {
      const transaccionId = `TXN-${Date.now()}-${Math.random()
        .toString(36)
        .substr(2, 9)}`;
      console.log(`✅ Pedido procesado exitosamente. ID: ${transaccionId}`);
      return { exito: true, comision, transaccionId };
    } else {
      console.log(`❌ Error procesando pedido con ${metodoPago}`);
      return { exito: false, comision };
    }
  }

  // Método que demuestra el polimorfismo
  async compararComisiones(monto: number): Promise<void> {
    console.log(`\n📊 Comparación de comisiones para pago de $${monto}:`);

    for (const [nombre, sistema] of this.sistemasPago) {
      const comision = sistema.obtenerComision(monto);
      const porcentaje = ((comision / monto) * 100).toFixed(2);
      console.log(`${nombre}: $${comision} (${porcentaje}%)`);
    }
  }
}
```

#### **Clases Abstractas**

```typescript
// Clase abstracta que define un contrato parcial
abstract class Vehiculo {
  // Propiedades concretas
  protected marca: string;
  protected modelo: string;
  protected año: number;
  protected velocidadActual: number = 0;

  constructor(marca: string, modelo: string, año: number) {
    this.marca = marca;
    this.modelo = modelo;
    this.año = año;
  }

  // Método concreto (implementado)
  public obtenerInfo(): string {
    return `${this.marca} ${this.modelo} (${this.año})`;
  }

  public acelerar(incremento: number): void {
    this.velocidadActual += incremento;
    console.log(`${this.obtenerInfo()} acelera a ${this.velocidadActual} km/h`);
  }

  public frenar(decremento: number): void {
    this.velocidadActual = Math.max(0, this.velocidadActual - decremento);
    console.log(`${this.obtenerInfo()} frena a ${this.velocidadActual} km/h`);
  }

  // Métodos abstractos (deben ser implementados por subclases)
  public abstract encender(): void;
  public abstract apagar(): void;
  public abstract obtenerTipoCombustible(): string;
  public abstract calcularConsumo(distancia: number): number;
}

// Implementación concreta - Auto
class Auto extends Vehiculo {
  private encendido: boolean = false;
  private tipoCombustible: string = "Gasolina";

  public encender(): void {
    if (!this.encendido) {
      this.encendido = true;
      console.log(`🚗 ${this.obtenerInfo()} encendido`);
    } else {
      console.log(`🚗 ${this.obtenerInfo()} ya está encendido`);
    }
  }

  public apagar(): void {
    if (this.encendido && this.velocidadActual === 0) {
      this.encendido = false;
      console.log(`🚗 ${this.obtenerInfo()} apagado`);
    } else if (this.velocidadActual > 0) {
      console.log(
        `🚗 No se puede apagar ${this.obtenerInfo()} mientras está en movimiento`
      );
    }
  }

  public obtenerTipoCombustible(): string {
    return this.tipoCombustible;
  }

  public calcularConsumo(distancia: number): number {
    // Consumo típico de auto: 8 litros per 100km
    return (distancia / 100) * 8;
  }
}

// Implementación concreta - Motocicleta
class Motocicleta extends Vehiculo {
  private encendida: boolean = false;
  private tipoCombustible: string = "Gasolina";

  public encender(): void {
    if (!this.encendida) {
      this.encendida = true;
      console.log(`🏍️ ${this.obtenerInfo()} encendida`);
    } else {
      console.log(`🏍️ ${this.obtenerInfo()} ya está encendida`);
    }
  }

  public apagar(): void {
    if (this.encendida && this.velocidadActual === 0) {
      this.encendida = false;
      console.log(`🏍️ ${this.obtenerInfo()} apagada`);
    } else if (this.velocidadActual > 0) {
      console.log(
        `🏍️ No se puede apagar ${this.obtenerInfo()} mientras está en movimiento`
      );
    }
  }

  public obtenerTipoCombustible(): string {
    return this.tipoCombustible;
  }

  public calcularConsumo(distancia: number): number {
    // Las motocicletas consumen menos: 4 litros por 100km
    return (distancia / 100) * 4;
  }
}

// Implementación concreta - Auto Eléctrico
class AutoElectrico extends Vehiculo {
  private encendido: boolean = false;
  private nivelBateria: number = 100;

  public encender(): void {
    if (this.nivelBateria > 0) {
      if (!this.encendido) {
        this.encendido = true;
        console.log(
          `⚡ ${this.obtenerInfo()} encendido (Batería: ${this.nivelBateria}%)`
        );
      } else {
        console.log(`⚡ ${this.obtenerInfo()} ya está encendido`);
      }
    } else {
      console.log(
        `⚡ ${this.obtenerInfo()} no puede encender - Batería agotada`
      );
    }
  }

  public apagar(): void {
    if (this.encendido && this.velocidadActual === 0) {
      this.encendido = false;
      console.log(`⚡ ${this.obtenerInfo()} apagado`);
    } else if (this.velocidadActual > 0) {
      console.log(
        `⚡ No se puede apagar ${this.obtenerInfo()} mientras está en movimiento`
      );
    }
  }

  public obtenerTipoCombustible(): string {
    return "Eléctrico";
  }

  public calcularConsumo(distancia: number): number {
    // Consumo eléctrico: 15 kWh por 100km
    const consumoKWh = (distancia / 100) * 15;
    this.nivelBateria = Math.max(0, this.nivelBateria - consumoKWh * 2); // Simulación simple
    return consumoKWh;
  }

  public cargarBateria(tiempo: number): void {
    const cargaAnterior = this.nivelBateria;
    this.nivelBateria = Math.min(100, this.nivelBateria + tiempo * 10); // 10% por unidad de tiempo
    console.log(`🔋 Carga: ${cargaAnterior}% → ${this.nivelBateria}%`);
  }
}

// Ejemplo de uso de abstracción
const vehiculos: Vehiculo[] = [
  new Auto("Toyota", "Corolla", 2023),
  new Motocicleta("Honda", "CBR600", 2022),
  new AutoElectrico("Tesla", "Model 3", 2024),
];

console.log("=== Demo de Abstracción con Vehículos ===");

vehiculos.forEach((vehiculo) => {
  console.log(`\n--- ${vehiculo.obtenerInfo()} ---`);
  console.log(`Tipo de combustible: ${vehiculo.obtenerTipoCombustible()}`);

  vehiculo.encender();
  vehiculo.acelerar(50);

  const distancia = 100;
  const consumo = vehiculo.calcularConsumo(distancia);
  console.log(
    `Consumo para ${distancia}km: ${consumo} ${
      vehiculo.obtenerTipoCombustible() === "Eléctrico" ? "kWh" : "litros"
    }`
  );

  vehiculo.frenar(50);
  vehiculo.apagar();
});

// Ejemplo de uso del procesador de pagos
console.log("\n=== Demo de Abstracción con Sistemas de Pago ===");

const procesador = new ProcesadorPagos();

// Registrar diferentes sistemas de pago
procesador.registrarSistemaPago("paypal", new PayPalPago("pk_test_123"));
procesador.registrarSistemaPago(
  "mercadopago",
  new MercadoPagoPago("access_token_456")
);
procesador.registrarSistemaPago(
  "transferencia",
  new TransferenciaBancaria("12345-6", "Banco Chile")
);

// Procesar pedidos con diferentes métodos
await procesador.procesarPedido(50000, "paypal", "PEDIDO-001");
await procesador.procesarPedido(75000, "mercadopago", "PEDIDO-002");
await procesador.procesarPedido(100000, "transferencia", "PEDIDO-003");

// Comparar comisiones
await procesador.compararComisiones(100000);
```

#### **Ventajas de la Abstracción:**

1. **Flexibilidad**: Fácil cambiar implementaciones sin afectar el código cliente
2. **Mantenibilidad**: Código más organizado y estructurado
3. **Reutilización**: Interfaces pueden ser implementadas por múltiples clases
4. **Testing**: Fácil crear mocks e implementaciones de prueba
5. **Escalabilidad**: Fácil agregar nuevas implementaciones

### 👨‍👩‍👧‍👦 3. Herencia

#### **¿Qué es la Herencia?**

**La herencia permite que una clase (subclase o clase hija) derive de otra clase (superclase o clase padre)**, heredando sus propiedades y métodos. Es como una **relación familiar** donde los hijos heredan características de sus padres.

#### **Conceptos Clave:**

- **Clase base (padre)**: La clase de la cual se hereda
- **Clase derivada (hija)**: La clase que hereda de otra
- **extends**: Palabra clave para establecer herencia
- **super**: Palabra clave para acceder a la clase padre
- **Reutilización de código**: No repetir código común
- **Jerarquía**: Estructura organizacional de clases

#### **Ejemplo Práctico: Sistema Académico AIEP**

```typescript
// Clase base - Persona
class Persona {
  protected nombre: string;
  protected apellido: string;
  protected rut: string;
  protected email: string;
  protected telefono: string;
  protected fechaNacimiento: Date;
  protected activo: boolean;

  constructor(
    nombre: string,
    apellido: string,
    rut: string,
    email: string,
    telefono: string,
    fechaNacimiento: Date
  ) {
    this.nombre = nombre;
    this.apellido = apellido;
    this.rut = rut;
    this.email = email;
    this.telefono = telefono;
    this.fechaNacimiento = fechaNacimiento;
    this.activo = true;
  }

  // Métodos comunes a todas las personas
  public obtenerNombreCompleto(): string {
    return `${this.nombre} ${this.apellido}`;
  }

  public calcularEdad(): number {
    const hoy = new Date();
    const edad = hoy.getFullYear() - this.fechaNacimiento.getFullYear();
    const mesActual = hoy.getMonth();
    const diaActual = hoy.getDate();
    const mesNacimiento = this.fechaNacimiento.getMonth();
    const diaNacimiento = this.fechaNacimiento.getDate();

    if (
      mesActual < mesNacimiento ||
      (mesActual === mesNacimiento && diaActual < diaNacimiento)
    ) {
      return edad - 1;
    }
    return edad;
  }

  public actualizarContacto(email: string, telefono: string): void {
    this.email = email;
    this.telefono = telefono;
    console.log(`✅ Contacto actualizado para ${this.obtenerNombreCompleto()}`);
  }

  public activar(): void {
    this.activo = true;
    console.log(`✅ ${this.obtenerNombreCompleto()} activado en el sistema`);
  }

  public desactivar(): void {
    this.activo = false;
    console.log(`❌ ${this.obtenerNombreCompleto()} desactivado del sistema`);
  }

  public estaActivo(): boolean {
    return this.activo;
  }

  public obtenerInformacionBasica(): object {
    return {
      nombre: this.obtenerNombreCompleto(),
      rut: this.rut,
      email: this.email,
      edad: this.calcularEdad(),
      activo: this.activo,
    };
  }
}

// Clase derivada - Estudiante
class Estudiante extends Persona {
  private matricula: string;
  private carrera: string;
  private semestre: number;
  private materias: MateriaEstudiante[];
  private beca: boolean;
  private porcentajeBeca: number;

  constructor(
    nombre: string,
    apellido: string,
    rut: string,
    email: string,
    telefono: string,
    fechaNacimiento: Date,
    matricula: string,
    carrera: string,
    semestre: number = 1,
    beca: boolean = false,
    porcentajeBeca: number = 0
  ) {
    // Llamar al constructor de la clase padre
    super(nombre, apellido, rut, email, telefono, fechaNacimiento);

    // Propiedades específicas del estudiante
    this.matricula = matricula;
    this.carrera = carrera;
    this.semestre = semestre;
    this.materias = [];
    this.beca = beca;
    this.porcentajeBeca = porcentajeBeca;

    console.log(
      `👨‍🎓 Estudiante creado: ${this.obtenerNombreCompleto()} - ${this.carrera}`
    );
  }

  // Métodos específicos del estudiante
  public matricularMateria(materia: MateriaEstudiante): boolean {
    if (!this.estaActivo()) {
      console.log(
        `❌ No se puede matricular ${this.obtenerNombreCompleto()} - Estudiante inactivo`
      );
      return false;
    }

    const yaMatriculada = this.materias.find(
      (m) => m.codigo === materia.codigo
    );
    if (yaMatriculada) {
      console.log(
        `❌ ${this.obtenerNombreCompleto()} ya está matriculado en ${
          materia.nombre
        }`
      );
      return false;
    }

    this.materias.push(materia);
    console.log(
      `✅ ${this.obtenerNombreCompleto()} matriculado en ${materia.nombre}`
    );
    return true;
  }

  public agregarNota(
    codigoMateria: string,
    nota: number,
    tipo: string = "Evaluación"
  ): boolean {
    const materia = this.materias.find((m) => m.codigo === codigoMateria);

    if (!materia) {
      console.log(
        `❌ Materia ${codigoMateria} no encontrada para ${this.obtenerNombreCompleto()}`
      );
      return false;
    }

    if (nota < 1.0 || nota > 7.0) {
      console.log(`❌ Nota inválida: ${nota}. Debe estar entre 1.0 y 7.0`);
      return false;
    }

    materia.notas.push({ valor: nota, tipo, fecha: new Date() });
    console.log(
      `📝 Nota ${nota} agregada a ${
        materia.nombre
      } para ${this.obtenerNombreCompleto()}`
    );
    return true;
  }

  public calcularPromedioMateria(codigoMateria: string): number | null {
    const materia = this.materias.find((m) => m.codigo === codigoMateria);

    if (!materia || materia.notas.length === 0) {
      return null;
    }

    const suma = materia.notas.reduce((total, nota) => total + nota.valor, 0);
    return suma / materia.notas.length;
  }

  public calcularPromedioGeneral(): number {
    if (this.materias.length === 0) return 0;

    let sumaPromedios = 0;
    let materiasConNotas = 0;

    this.materias.forEach((materia) => {
      if (materia.notas.length > 0) {
        const promedio =
          materia.notas.reduce((sum, nota) => sum + nota.valor, 0) /
          materia.notas.length;
        sumaPromedios += promedio;
        materiasConNotas++;
      }
    });

    return materiasConNotas > 0 ? sumaPromedios / materiasConNotas : 0;
  }

  public avanzarSemestre(): boolean {
    const promedioGeneral = this.calcularPromedioGeneral();

    if (promedioGeneral >= 4.0) {
      this.semestre++;
      console.log(
        `🎓 ${this.obtenerNombreCompleto()} avanza a semestre ${this.semestre}`
      );
      return true;
    } else {
      console.log(
        `❌ ${this.obtenerNombreCompleto()} no puede avanzar - Promedio insuficiente: ${promedioGeneral.toFixed(
          2
        )}`
      );
      return false;
    }
  }

  // Sobrescribir método de la clase padre
  public obtenerInformacionBasica(): object {
    // Llamar al método de la clase padre y agregar información específica
    const infoBasica = super.obtenerInformacionBasica();

    return {
      ...infoBasica,
      matricula: this.matricula,
      carrera: this.carrera,
      semestre: this.semestre,
      promedioGeneral: this.calcularPromedioGeneral().toFixed(2),
      beca: this.beca,
      porcentajeBeca: this.porcentajeBeca,
    };
  }

  // Getters específicos
  get getMatricula(): string {
    return this.matricula;
  }

  get getCarrera(): string {
    return this.carrera;
  }

  get getSemestre(): number {
    return this.semestre;
  }
}

// Clase derivada - Profesor
class Profesor extends Persona {
  private especialidades: string[];
  private materiasImpartidas: string[];
  private oficina: string;
  private horasSemanales: number;
  private tipoContrato: "Planta" | "Honorarios" | "Part-time";

  constructor(
    nombre: string,
    apellido: string,
    rut: string,
    email: string,
    telefono: string,
    fechaNacimiento: Date,
    especialidades: string[],
    oficina: string,
    tipoContrato: "Planta" | "Honorarios" | "Part-time" = "Honorarios"
  ) {
    // Llamar al constructor de la clase padre
    super(nombre, apellido, rut, email, telefono, fechaNacimiento);

    this.especialidades = especialidades;
    this.materiasImpartidas = [];
    this.oficina = oficina;
    this.horasSemanales = 0;
    this.tipoContrato = tipoContrato;

    console.log(
      `👨‍🏫 Profesor creado: ${this.obtenerNombreCompleto()} - ${especialidades.join(
        ", "
      )}`
    );
  }

  public asignarMateria(
    codigoMateria: string,
    nombreMateria: string,
    horas: number
  ): boolean {
    if (!this.estaActivo()) {
      console.log(
        `❌ No se puede asignar materia a ${this.obtenerNombreCompleto()} - Profesor inactivo`
      );
      return false;
    }

    if (this.materiasImpartidas.includes(codigoMateria)) {
      console.log(
        `❌ ${this.obtenerNombreCompleto()} ya tiene asignada la materia ${nombreMateria}`
      );
      return false;
    }

    this.materiasImpartidas.push(codigoMateria);
    this.horasSemanales += horas;
    console.log(
      `✅ Materia ${nombreMateria} asignada a ${this.obtenerNombreCompleto()}`
    );
    console.log(`📊 Horas semanales: ${this.horasSemanales}`);
    return true;
  }

  public evaluarEstudiante(
    estudiante: Estudiante,
    codigoMateria: string,
    nota: number
  ): boolean {
    if (!this.materiasImpartidas.includes(codigoMateria)) {
      console.log(
        `❌ ${this.obtenerNombreCompleto()} no imparte la materia ${codigoMateria}`
      );
      return false;
    }

    return estudiante.agregarNota(
      codigoMateria,
      nota,
      "Evaluación del profesor"
    );
  }

  public obtenerCargaAcademica(): object {
    return {
      profesor: this.obtenerNombreCompleto(),
      materiasImpartidas: this.materiasImpartidas,
      horasSemanales: this.horasSemanales,
      tipoContrato: this.tipoContrato,
      oficina: this.oficina,
    };
  }

  // Sobrescribir método de la clase padre
  public obtenerInformacionBasica(): object {
    const infoBasica = super.obtenerInformacionBasica();

    return {
      ...infoBasica,
      especialidades: this.especialidades,
      materiasImpartidas: this.materiasImpartidas.length,
      horasSemanales: this.horasSemanales,
      tipoContrato: this.tipoContrato,
      oficina: this.oficina,
    };
  }
}

// Clase derivada - Administrador
class Administrador extends Persona {
  private cargo: string;
  private departamento: string;
  private permisos: string[];
  private nivelAcceso: number;

  constructor(
    nombre: string,
    apellido: string,
    rut: string,
    email: string,
    telefono: string,
    fechaNacimiento: Date,
    cargo: string,
    departamento: string,
    nivelAcceso: number = 1
  ) {
    super(nombre, apellido, rut, email, telefono, fechaNacimiento);

    this.cargo = cargo;
    this.departamento = departamento;
    this.nivelAcceso = nivelAcceso;
    this.permisos = this.asignarPermisosPorNivel(nivelAcceso);

    console.log(
      `👨‍💼 Administrador creado: ${this.obtenerNombreCompleto()} - ${cargo}`
    );
  }

  private asignarPermisosPorNivel(nivel: number): string[] {
    const todosLosPermisos = [
      "ver_estudiantes",
      "editar_estudiantes",
      "crear_estudiantes",
      "ver_profesores",
      "editar_profesores",
      "crear_profesores",
      "ver_materias",
      "editar_materias",
      "crear_materias",
      "generar_reportes",
      "configurar_sistema",
      "administrar_usuarios",
    ];

    switch (nivel) {
      case 3:
        return todosLosPermisos; // Super admin
      case 2:
        return todosLosPermisos.slice(0, 9); // Admin regular
      case 1:
        return todosLosPermisos.slice(0, 6); // Admin básico
      default:
        return ["ver_estudiantes", "ver_profesores"];
    }
  }

  public tienePermiso(permiso: string): boolean {
    return this.permisos.includes(permiso);
  }

  public gestionarEstudiante(estudiante: Estudiante, accion: string): boolean {
    if (!this.tienePermiso(`${accion}_estudiantes`)) {
      console.log(
        `❌ ${this.obtenerNombreCompleto()} no tiene permisos para ${accion} estudiantes`
      );
      return false;
    }

    switch (accion) {
      case "ver":
        console.log(
          `📋 Información del estudiante:`,
          estudiante.obtenerInformacionBasica()
        );
        return true;
      case "editar":
        console.log(
          `✏️ Editando estudiante: ${estudiante.obtenerNombreCompleto()}`
        );
        return true;
      case "crear":
        console.log(`➕ Creando nuevo estudiante`);
        return true;
      default:
        console.log(`❌ Acción no reconocida: ${accion}`);
        return false;
    }
  }

  public generarReporte(tipo: string): boolean {
    if (!this.tienePermiso("generar_reportes")) {
      console.log(
        `❌ ${this.obtenerNombreCompleto()} no tiene permisos para generar reportes`
      );
      return false;
    }

    console.log(`📊 Generando reporte de ${tipo}...`);
    console.log(`✅ Reporte generado por ${this.obtenerNombreCompleto()}`);
    return true;
  }

  // Sobrescribir método de la clase padre
  public obtenerInformacionBasica(): object {
    const infoBasica = super.obtenerInformacionBasica();

    return {
      ...infoBasica,
      cargo: this.cargo,
      departamento: this.departamento,
      nivelAcceso: this.nivelAcceso,
      permisos: this.permisos,
    };
  }
}

// Interfaces de apoyo
interface MateriaEstudiante {
  codigo: string;
  nombre: string;
  creditos: number;
  notas: Nota[];
}

interface Nota {
  valor: number;
  tipo: string;
  fecha: Date;
}

// Ejemplo de uso de Herencia
console.log("=== Demo de Herencia - Sistema Académico AIEP ===");

// Crear personas de diferentes tipos
const fechaNacimiento1 = new Date(2003, 5, 15); // 15 de junio de 2003
const fechaNacimiento2 = new Date(1980, 2, 22); // 22 de marzo de 1980
const fechaNacimiento3 = new Date(1975, 8, 10); // 10 de septiembre de 1975

const estudiante = new Estudiante(
  "María",
  "González",
  "12.345.678-9",
  "maria.gonzalez@aiep.cl",
  "+56912345678",
  fechaNacimiento1,
  "2025001",
  "Programación y Análisis de Sistemas",
  2,
  true,
  50
);

const profesor = new Profesor(
  "Carlos",
  "Ruiz",
  "98.765.432-1",
  "carlos.ruiz@aiep.cl",
  "+56987654321",
  fechaNacimiento2,
  ["Programación", "Base de Datos"],
  "Oficina 201",
  "Planta"
);

const admin = new Administrador(
  "Ana",
  "Martínez",
  "11.111.111-1",
  "ana.martinez@aiep.cl",
  "+56911111111",
  fechaNacimiento3,
  "Directora Académica",
  "Dirección",
  3
);

// Demostrar herencia - todos son Personas
const personas: Persona[] = [estudiante, profesor, admin];

console.log("\n--- Información de todas las personas ---");
personas.forEach((persona) => {
  console.log(
    `Nombre: ${persona.obtenerNombreCompleto()}, Edad: ${persona.calcularEdad()} años`
  );
});

// Usar métodos específicos de cada clase
console.log("\n--- Operaciones específicas ---");

// Operaciones de estudiante
const materiaTS: MateriaEstudiante = {
  codigo: "PRO205",
  nombre: "Taller de Programación",
  creditos: 4,
  notas: [],
};

const materiaBD: MateriaEstudiante = {
  codigo: "BDA301",
  nombre: "Base de Datos Avanzada",
  creditos: 5,
  notas: [],
};

estudiante.matricularMateria(materiaTS);
estudiante.matricularMateria(materiaBD);

// Operaciones de profesor
profesor.asignarMateria("PRO205", "Taller de Programación", 8);
profesor.asignarMateria("BDA301", "Base de Datos Avanzada", 6);

// Profesor evalúa estudiante
profesor.evaluarEstudiante(estudiante, "PRO205", 8.5);
profesor.evaluarEstudiante(estudiante, "PRO205", 9.0);
profesor.evaluarEstudiante(estudiante, "BDA301", 7.8);

// Operaciones de administrador
admin.gestionarEstudiante(estudiante, "ver");
admin.generarReporte("notas_semestrales");

console.log("\n--- Información detallada ---");
console.log("Estudiante:", estudiante.obtenerInformacionBasica());
console.log("Profesor:", profesor.obtenerCargaAcademica());
console.log("Administrador:", admin.obtenerInformacionBasica());
```

#### **Ventajas de la Herencia:**

1. **Reutilización de código**: No repetir código común en múltiples clases
2. **Mantenimiento**: Cambios en la clase base se propagan automáticamente
3. **Organización**: Estructura jerárquica clara y lógica
4. **Extensibilidad**: Fácil agregar nuevas clases derivadas
5. **Consistencia**: Comportamiento uniforme en clases relacionadas

### 🎭 4. Polimorfismo

#### **¿Qué es el Polimorfismo?**

**El polimorfismo permite que objetos de diferentes clases respondan a la misma interface de maneras diferentes**. La palabra viene del griego "poly" (muchos) y "morph" (formas): **muchas formas**.

#### **Tipos de Polimorfismo:**

1. **Polimorfismo de herencia**: Métodos sobrescritos en clases derivadas
2. **Polimorfismo de interface**: Diferentes clases implementan la misma interface
3. **Sobrecarga de métodos**: Mismo nombre, diferentes parámetros (limitado en TypeScript)

#### **Ejemplo Práctico: Sistema de Notificaciones**

```typescript
// Interface base para el polimorfismo
interface ServicioNotificacion {
  enviar(
    destinatario: string,
    mensaje: string,
    asunto?: string
  ): Promise<boolean>;
  verificarEntrega(id: string): Promise<boolean>;
  obtenerTipo(): string;
  obtenerCosto(mensaje: string): number;
}

// Implementación 1: Email
class NotificacionEmail implements ServicioNotificacion {
  private servidor: string;
  private puerto: number;
  private usuario: string;
  private password: string;

  constructor(
    servidor: string,
    puerto: number,
    usuario: string,
    password: string
  ) {
    this.servidor = servidor;
    this.puerto = puerto;
    this.usuario = usuario;
    this.password = password;
  }

  async enviar(
    destinatario: string,
    mensaje: string,
    asunto: string = "Notificación"
  ): Promise<boolean> {
    console.log(`📧 Enviando email a: ${destinatario}`);
    console.log(`📧 Asunto: ${asunto}`);
    console.log(`📧 Servidor: ${this.servidor}:${this.puerto}`);
    console.log(`📧 Mensaje: ${mensaje.substring(0, 50)}...`);

    // Simulación de envío
    await this.simularDelay(2000);
    const exito = Math.random() > 0.1; // 90% de éxito

    if (exito) {
      console.log("✅ Email enviado exitosamente");
      return true;
    } else {
      console.log("❌ Error enviando email");
      return false;
    }
  }

  async verificarEntrega(id: string): Promise<boolean> {
    console.log(`🔍 Verificando entrega de email: ${id}`);
    await this.simularDelay(1000);
    return Math.random() > 0.2; // 80% entregado
  }

  obtenerTipo(): string {
    return "Email";
  }

  obtenerCosto(mensaje: string): number {
    return 0; // Email es gratis
  }

  private async simularDelay(ms: number): Promise<void> {
    return new Promise((resolve) => setTimeout(resolve, ms));
  }
}

// Implementación 2: SMS
class NotificacionSMS implements ServicioNotificacion {
  private apiKey: string;
  private proveedor: string;

  constructor(apiKey: string, proveedor: string = "Twilio") {
    this.apiKey = apiKey;
    this.proveedor = proveedor;
  }

  async enviar(
    destinatario: string,
    mensaje: string,
    asunto?: string
  ): Promise<boolean> {
    console.log(`📱 Enviando SMS a: ${destinatario}`);
    console.log(`📱 Proveedor: ${this.proveedor}`);

    // Los SMS tienen límite de caracteres
    const mensajeLimitado =
      mensaje.length > 160 ? mensaje.substring(0, 157) + "..." : mensaje;

    console.log(`📱 Mensaje: ${mensajeLimitado}`);

    await this.simularDelay(1500);
    const exito = Math.random() > 0.05; // 95% de éxito

    if (exito) {
      console.log("✅ SMS enviado exitosamente");
      return true;
    } else {
      console.log("❌ Error enviando SMS");
      return false;
    }
  }

  async verificarEntrega(id: string): Promise<boolean> {
    console.log(`🔍 Verificando entrega de SMS: ${id}`);
    await this.simularDelay(500);
    return Math.random() > 0.1; // 90% entregado
  }

  obtenerTipo(): string {
    return "SMS";
  }

  obtenerCosto(mensaje: string): number {
    // Costo por SMS
    const numSMS = Math.ceil(mensaje.length / 160);
    return numSMS * 50; // $50 por SMS
  }

  private async simularDelay(ms: number): Promise<void> {
    return new Promise((resolve) => setTimeout(resolve, ms));
  }
}

// Implementación 3: Push Notification
class NotificacionPush implements ServicioNotificacion {
  private appId: string;
  private apiKey: string;

  constructor(appId: string, apiKey: string) {
    this.appId = appId;
    this.apiKey = apiKey;
  }

  async enviar(
    destinatario: string,
    mensaje: string,
    asunto: string = "Nueva notificación"
  ): Promise<boolean> {
    console.log(`🔔 Enviando push notification a: ${destinatario}`);
    console.log(`🔔 App ID: ${this.appId}`);
    console.log(`🔔 Título: ${asunto}`);
    console.log(`🔔 Mensaje: ${mensaje.substring(0, 100)}...`);

    await this.simularDelay(800);
    const exito = Math.random() > 0.15; // 85% de éxito

    if (exito) {
      console.log("✅ Push notification enviada exitosamente");
      return true;
    } else {
      console.log("❌ Error enviando push notification");
      return false;
    }
  }

  async verificarEntrega(id: string): Promise<boolean> {
    console.log(`🔍 Verificando entrega de push notification: ${id}`);
    await this.simularDelay(300);
    return Math.random() > 0.25; // 75% entregado (usuarios pueden tener notificaciones desactivadas)
  }

  obtenerTipo(): string {
    return "Push Notification";
  }

  obtenerCosto(mensaje: string): number {
    return 10; // Costo fijo por push notification
  }

  private async simularDelay(ms: number): Promise<void> {
    return new Promise((resolve) => setTimeout(resolve, ms));
  }
}

// Implementación 4: Slack
class NotificacionSlack implements ServicioNotificacion {
  private webhookUrl: string;
  private canal: string;

  constructor(webhookUrl: string, canal: string = "#general") {
    this.webhookUrl = webhookUrl;
    this.canal = canal;
  }

  async enviar(
    destinatario: string,
    mensaje: string,
    asunto?: string
  ): Promise<boolean> {
    console.log(`💬 Enviando mensaje Slack a: ${this.canal}`);
    console.log(`💬 Mencionando: @${destinatario}`);
    console.log(`💬 Mensaje: ${mensaje}`);

    const mensajeSlack = asunto
      ? `*${asunto}*\n@${destinatario}\n${mensaje}`
      : `@${destinatario}\n${mensaje}`;

    await this.simularDelay(1200);
    const exito = Math.random() > 0.08; // 92% de éxito

    if (exito) {
      console.log("✅ Mensaje Slack enviado exitosamente");
      return true;
    } else {
      console.log("❌ Error enviando mensaje Slack");
      return false;
    }
  }

  async verificarEntrega(id: string): Promise<boolean> {
    console.log(`🔍 Verificando entrega de mensaje Slack: ${id}`);
    await this.simularDelay(200);
    return true; // Slack casi siempre entrega inmediatamente
  }

  obtenerTipo(): string {
    return "Slack";
  }

  obtenerCosto(mensaje: string): number {
    return 0; // Slack es gratis para uso básico
  }

  private async simularDelay(ms: number): Promise<void> {
    return new Promise((resolve) => setTimeout(resolve, ms));
  }
}

// Gestor de notificaciones que demuestra el polimorfismo
class GestorNotificaciones {
  private servicios: Map<string, ServicioNotificacion> = new Map();
  private historialEnvios: Array<{
    servicio: string;
    destinatario: string;
    mensaje: string;
    exito: boolean;
    fecha: Date;
    costo: number;
  }> = [];

  registrarServicio(nombre: string, servicio: ServicioNotificacion): void {
    this.servicios.set(nombre, servicio);
    console.log(
      `📝 Servicio registrado: ${nombre} (${servicio.obtenerTipo()})`
    );
  }

  async enviarNotificacion(
    servicio: string,
    destinatario: string,
    mensaje: string,
    asunto?: string
  ): Promise<boolean> {
    const servicioNotificacion = this.servicios.get(servicio);

    if (!servicioNotificacion) {
      console.log(`❌ Servicio no encontrado: ${servicio}`);
      return false;
    }

    console.log(`\n🚀 Enviando con ${servicioNotificacion.obtenerTipo()}...`);

    const costo = servicioNotificacion.obtenerCosto(mensaje);
    const exito = await servicioNotificacion.enviar(
      destinatario,
      mensaje,
      asunto
    );

    this.historialEnvios.push({
      servicio: servicioNotificacion.obtenerTipo(),
      destinatario,
      mensaje: mensaje.substring(0, 50) + "...",
      exito,
      fecha: new Date(),
      costo,
    });

    return exito;
  }

  // Polimorfismo en acción: mismo método, diferentes comportamientos
  async enviarAMultiplesServicios(
    servicios: string[],
    destinatario: string,
    mensaje: string,
    asunto?: string
  ): Promise<void> {
    console.log(`\n🎯 Enviando mensaje a múltiples servicios...`);

    const promesas = servicios.map(async (nombreServicio) => {
      const servicio = this.servicios.get(nombreServicio);
      if (!servicio) {
        console.log(`⚠️ Servicio no encontrado: ${nombreServicio}`);
        return;
      }

      console.log(`\n--- ${servicio.obtenerTipo()} ---`);
      const costo = servicio.obtenerCosto(mensaje);
      console.log(`💰 Costo estimado: $${costo}`);

      const exito = await servicio.enviar(destinatario, mensaje, asunto);

      this.historialEnvios.push({
        servicio: servicio.obtenerTipo(),
        destinatario,
        mensaje: mensaje.substring(0, 50) + "...",
        exito,
        fecha: new Date(),
        costo,
      });
    });

    await Promise.all(promesas);
  }

  async compararServicios(mensaje: string): Promise<void> {
    console.log(
      `\n📊 Comparación de servicios para mensaje de ${mensaje.length} caracteres:`
    );

    for (const [nombre, servicio] of this.servicios) {
      const costo = servicio.obtenerCosto(mensaje);
      console.log(`${servicio.obtenerTipo()}: $${costo} - ${nombre}`);
    }
  }

  obtenerEstadisticas(): object {
    const totalEnvios = this.historialEnvios.length;
    const exitosos = this.historialEnvios.filter((e) => e.exito).length;
    const costoTotal = this.historialEnvios.reduce(
      (sum, e) => sum + e.costo,
      0
    );

    const estadisticasPorServicio = new Map<
      string,
      {
        total: number;
        exitosos: number;
        costo: number;
      }
    >();

    this.historialEnvios.forEach((envio) => {
      if (!estadisticasPorServicio.has(envio.servicio)) {
        estadisticasPorServicio.set(envio.servicio, {
          total: 0,
          exitosos: 0,
          costo: 0,
        });
      }

      const stats = estadisticasPorServicio.get(envio.servicio)!;
      stats.total++;
      if (envio.exito) stats.exitosos++;
      stats.costo += envio.costo;
    });

    return {
      resumen: {
        totalEnvios,
        exitosos,
        porcentajeExito:
          totalEnvios > 0
            ? ((exitosos / totalEnvios) * 100).toFixed(2) + "%"
            : "0%",
        costoTotal,
      },
      porServicio: Object.fromEntries(estadisticasPorServicio),
    };
  }

  mostrarHistorial(): void {
    console.log(`\n📜 Historial de envíos (últimos 10):`);
    this.historialEnvios.slice(-10).forEach((envio, index) => {
      const estado = envio.exito ? "✅" : "❌";
      console.log(
        `${estado} ${envio.servicio} → ${envio.destinatario} ($${
          envio.costo
        }) - ${envio.fecha.toLocaleTimeString()}`
      );
    });
  }
}

// Ejemplo de uso del Polimorfismo
console.log("\n=== Demo de Polimorfismo - Sistema de Notificaciones ===");

const gestor = new GestorNotificaciones();

// Registrar diferentes servicios (todos implementan la misma interface)
gestor.registrarServicio(
  "email_principal",
  new NotificacionEmail("smtp.gmail.com", 587, "sistema@aiep.cl", "password123")
);
gestor.registrarServicio(
  "sms_urgente",
  new NotificacionSMS("sk_test_123456", "Twilio")
);
gestor.registrarServicio(
  "push_movil",
  new NotificacionPush("app_12345", "key_67890")
);
gestor.registrarServicio(
  "slack_equipo",
  new NotificacionSlack("https://hooks.slack.com/services/...", "#desarrollo")
);

const mensaje =
  "Su calificación ha sido publicada. Ingrese al sistema para revisar sus notas del semestre actual.";
const asunto = "Nueva Calificación Disponible";

// Polimorfismo en acción: el mismo código funciona con diferentes implementaciones
await gestor.enviarNotificacion(
  "email_principal",
  "maria.gonzalez@aiep.cl",
  mensaje,
  asunto
);
await gestor.enviarNotificacion("sms_urgente", "+56912345678", mensaje);
await gestor.enviarNotificacion(
  "push_movil",
  "user_token_abc123",
  mensaje,
  asunto
);

// Enviar a múltiples servicios simultáneamente
await gestor.enviarAMultiplesServicios(
  ["email_principal", "push_movil", "slack_equipo"],
  "carlos.lopez@aiep.cl",
  "Recordatorio: Entrega de proyecto final mañana a las 23:59",
  "⚠️ Recordatorio Importante"
);

// Comparar costos
await gestor.compararServicios(mensaje);

// Mostrar estadísticas
console.log("\n📊 Estadísticas finales:");
console.log(gestor.obtenerEstadisticas());

gestor.mostrarHistorial();
```

#### **Ventajas del Polimorfismo:**

1. **Flexibilidad**: El mismo código puede trabajar con diferentes implementaciones
2. **Extensibilidad**: Fácil agregar nuevos tipos sin cambiar código existente
3. **Mantenibilidad**: Cambios localizados en cada implementación
4. **Testing**: Fácil crear mocks y stubs para pruebas
5. **Abstracción**: El código cliente no necesita conocer detalles de implementación

#### **💡 Resumen de los 4 Pilares**

| Pilar                  | Propósito                       | Beneficio Principal            | Ejemplo Clave             |
| ---------------------- | ------------------------------- | ------------------------------ | ------------------------- |
| **🔒 Encapsulamiento** | Ocultar detalles internos       | Seguridad y control            | Cuenta bancaria con PIN   |
| **🏗️ Abstracción**     | Definir contratos               | Flexibilidad de implementación | Interfaces de pago        |
| **👨‍👩‍👧‍👦 Herencia**        | Reutilizar código común         | Evitar repetición              | Sistema académico         |
| **🎭 Polimorfismo**    | Una interface, múltiples formas | Extensibilidad                 | Sistema de notificaciones |

**🎯 Punto clave final:** Los 4 pilares trabajan juntos para crear código más **mantenible**, **escalable**, **seguro** y **reutilizable**. Son la base fundamental de cualquier sistema orientado a objetos profesional.

---

## Ejercicios Prácticos

### 🎯 Instrucciones Generales

- **Crea un nuevo archivo TypeScript** para cada ejercicio
- **Compila y ejecuta** tu código para verificar que funcione
- **Usa todos los pilares de POO** cuando sea apropiado
- **Comenta tu código** explicando las decisiones de diseño
- **Prueba casos extremos** para validar tu implementación

### 📚 Ejercicio 1: Sistema de Biblioteca Personal (Nivel Básico)

**Objetivo:** Aplicar conceptos básicos de clases, encapsulamiento e interfaces.

**Requerimientos:**

1. Crear una clase `LibroPersonal` con las siguientes propiedades:

   - `titulo` (string)
   - `autor` (string)
   - `año` (number)
   - `genero` (string)
   - `leido` (boolean, privado)
   - `calificacion` (number del 1-5, privado, opcional)

2. Implementar métodos públicos:

   - `marcarComoLeido()`
   - `calificar(puntos: number)` - solo si está leído
   - `obtenerInformacion()` - retorna string con datos del libro
   - `esRecomendable()` - retorna true si calificación >= 4

3. Crear una clase `BibliotecaPersonal` que:
   - Mantenga un array privado de libros
   - Tenga métodos para agregar libros, buscar por título/autor
   - Obtenga estadísticas (total, leídos, por género, etc.)

**Código base para empezar:**

```typescript
// Completa esta implementación
class LibroPersonal {
  // TODO: Definir propiedades

  constructor(/* TODO: parámetros */) {
    // TODO: Inicializar propiedades
  }

  // TODO: Implementar métodos
}

class BibliotecaPersonal {
  // TODO: Implementar clase completa
}

// Pruebas
const libro1 = new LibroPersonal(
  "Clean Code",
  "Robert Martin",
  2008,
  "Programación"
);
const biblioteca = new BibliotecaPersonal();
biblioteca.agregarLibro(libro1);
```

### 💰 Ejercicio 2: Sistema de Cuentas Bancarias (Nivel Intermedio)

**Objetivo:** Aplicar herencia, encapsulamiento avanzado y polimorfismo.

**Requerimientos:**

1. Crear una clase base abstracta `CuentaBancaria` con:

   - Propiedades: número de cuenta, titular, saldo (protegido)
   - Métodos abstractos: `calcularInteres()`, `obtenerTipoCuenta()`
   - Métodos concretos: `depositar()`, `retirar()`, `obtenerSaldo()`

2. Crear clases derivadas:

   - `CuentaCorriente`: sin interés, permite sobregiro hasta $100,000
   - `CuentaAhorro`: 2% de interés anual, no permite sobregiro
   - `CuentaVista`: 0.5% interés anual, comisión $500 por retiro

3. Crear una clase `Banco` que:
   - Maneje múltiples cuentas
   - Permita transferencias entre cuentas
   - Genere reportes por tipo de cuenta

**Estructura sugerida:**

```typescript
abstract class CuentaBancaria {
  protected numeroCuenta: string;
  protected titular: string;
  protected saldo: number;

  constructor(numeroCuenta: string, titular: string, saldoInicial: number = 0) {
    // TODO: Implementar
  }

  public depositar(monto: number): boolean {
    // TODO: Implementar con validaciones
    return false;
  }

  public retirar(monto: number): boolean {
    // TODO: Implementar (cada subclase puede tener reglas diferentes)
    return false;
  }

  public obtenerSaldo(): number {
    return this.saldo;
  }

  // Métodos abstractos
  public abstract calcularInteres(): number;
  public abstract obtenerTipoCuenta(): string;
}

class CuentaCorriente extends CuentaBancaria {
  private limiteSobregiro: number = 100000;

  // TODO: Implementar métodos abstractos y lógica específica
}

// TODO: Implementar CuentaAhorro y CuentaVista
```

### 🏥 Ejercicio 3: Sistema de Gestión Hospitalaria (Nivel Avanzado)

**Objetivo:** Integrar todos los pilares de POO en un sistema complejo.

**Requerimientos:**

1. Crear jerarquía de personas:

   - `Persona` (clase base)
   - `Paciente`, `Doctor`, `Enfermero`, `Administrativo` (clases derivadas)

2. Crear sistema de especialidades médicas:

   - Interface `EspecialidadMedica` con métodos para diagnóstico y tratamiento
   - Implementaciones: `Cardiologia`, `Neurologia`, `Pediatria`

3. Sistema de citas:

   - `CitaMedica` con fecha, hora, paciente, doctor, estado
   - `GestorCitas` para programar y administrar citas

4. Sistema de historiales:
   - `HistorialMedico` por paciente
   - `Diagnostico`, `Tratamiento`, `Medicamento`

**Funcionalidades requeridas:**

- Programar citas verificando disponibilidad del doctor
- Registrar diagnósticos y tratamientos
- Generar reportes por especialidad
- Sistema de alertas para medicamentos vencidos
- Búsquedas avanzadas de pacientes y doctores

**Código inicial:**

```typescript
interface EspecialidadMedica {
  nombre: string;
  diagnosticar(sintomas: string[]): string;
  prescribirTratamiento(diagnostico: string): string[];
  duracionConsulta(): number; // minutos
}

abstract class Persona {
  // TODO: Implementar clase base
}

class Doctor extends Persona {
  private especialidades: EspecialidadMedica[];
  private horarioAtencion: { inicio: string; fin: string };

  // TODO: Implementar lógica del doctor
}

// TODO: Implementar resto del sistema
```

### 🎮 Ejercicio 4: Juego de Rol Simplificado (Nivel Avanzado+)

**Objetivo:** Crear un sistema de juego que demuestre maestría en POO.

**Requerimientos:**

1. Sistema de personajes con herencia:

   - `Personaje` (base): vida, mana, nivel, experiencia
   - `Guerrero`, `Mago`, `Arquero` (especialidades)

2. Sistema de combate polimórfico:

   - Interface `Habilidad` con diferentes implementaciones
   - Cálculo de daño basado en stats y habilidades

3. Sistema de inventario:

   - Items, armas, armaduras con diferentes efectos
   - Gestión de peso y capacidad

4. Sistema de progresión:
   - Subida de nivel automática por experiencia
   - Aprendizaje de nuevas habilidades

**Funcionalidades avanzadas:**

- Combate por turnos entre personajes
- Sistema de buff/debuff
- Guardado y carga de partida
- Generación aleatoria de encuentros

### 🏪 Ejercicio 5: E-commerce con Carrito de Compras (Nivel Experto)

**Objetivo:** Sistema comercial completo aplicando patrones de diseño y principios SOLID.

**Requerimientos:**

1. Catálogo de productos con herencia:

   - `Producto` (base), `ProductoFisico`, `ProductoDigital`
   - Sistema de categorías y filtros

2. Sistema de usuarios y autenticación:

   - `Usuario`, `Cliente`, `Administrador`
   - Sistema de roles y permisos

3. Carrito de compras con estado:

   - Agregar/quitar productos
   - Cálculo de totales con descuentos e impuestos
   - Persistencia del carrito

4. Sistema de pagos polimórfico:

   - Diferentes métodos de pago
   - Validación y procesamiento

5. Sistema de órdenes:
   - Estados de orden (pendiente, pagada, enviada, entregada)
   - Historial de compras

**Características avanzadas:**

- Sistema de descuentos y cupones
- Recomendaciones de productos
- Sistema de reviews y calificaciones
- Panel de administración

### 🎯 Criterios de Evaluación

Para cada ejercicio, tu implementación será evaluada en:

1. **Correcta aplicación de los 4 pilares de POO** (25%)

   - Encapsulamiento: Uso correcto de modificadores de acceso
   - Abstracción: Interfaces y clases abstractas bien diseñadas
   - Herencia: Jerarquías lógicas y reutilización de código
   - Polimorfismo: Múltiples implementaciones de interfaces

2. **Calidad del código TypeScript** (25%)

   - Tipos correctos y bien definidos
   - Interfaces claras y consistentes
   - Uso de características modernas de TS

3. **Funcionalidad y lógica** (25%)

   - El código funciona correctamente
   - Manejo de casos extremos
   - Validaciones apropiadas

4. **Diseño y arquitectura** (25%)
   - Código bien organizado
   - Responsabilidades claras
   - Facilidad de mantenimiento y extensión

### 📝 Entregables

Para cada ejercicio, entregar:

1. **Archivo TypeScript** con el código completo
2. **Archivo JavaScript compilado** (resultado de `tsc`)
3. **Archivo de pruebas** que demuestre el funcionamiento
4. **Documentación breve** (README.md) explicando:
   - Decisiones de diseño
   - Cómo ejecutar el código
   - Casos de prueba implementados

### 🚀 Desafíos Adicionales

Una vez completados los ejercicios básicos, intenta estos desafíos:

1. **Patrón Observer**: Implementa un sistema de notificaciones
2. **Patrón Factory**: Crea fábricas para generar objetos
3. **Patrón Singleton**: Implementa configuraciones globales
4. **Testing**: Escribe pruebas unitarias para tus clases
5. **Persistencia**: Guarda y carga datos en archivos JSON

### 💡 Consejos para el Éxito

1. **Planifica antes de codificar**: Diseña tu arquitectura primero
2. **Empieza simple**: Implementa funcionalidad básica, luego mejora
3. **Prueba frecuentemente**: Compila y ejecuta después de cada cambio
4. **Usa los pilares naturalmente**: No fuerces su uso, que surjan del diseño
5. **Pide ayuda**: Si te atascas, consulta con tus compañeros o profesor

¡**Manos a la obra**! Estos ejercicios te darán la experiencia práctica necesaria para dominar la POO con TypeScript. Recuerda que la programación se aprende programando. 💪

---

## Recursos Adicionales

### 📚 Documentación Oficial

- [TypeScript Handbook](https://www.typescriptlang.org/docs/)
- [TypeScript Playground](https://www.typescriptlang.org/play) - Para probar código online

### 🛠️ Herramientas Recomendadas

- **VS Code** con extensión TypeScript
- **Node.js** para ejecutar el código
- **Git** para control de versiones

### 📖 Lecturas Complementarias

- "Clean Code" by Robert C. Martin
- "Design Patterns" by Gang of Four
- "Refactoring" by Martin Fowler

¡Éxito en tu aprendizaje de TypeScript y POO! 🎉
