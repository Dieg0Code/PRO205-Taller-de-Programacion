# 📚 Clase 02: Historia y Evolución de los Paradigmas, VSCode, Terminal y Git

- Unidad 01: **Introducción a la Programación Orientada a Objetos**
- Fecha: Martes 12 de agosto, 2025
- Horario: 10:50 - 13:00
- Docente: Diego Obando

## 🎯 Objetivos de la Clase

Al finalizar esta clase, serás capaz de:

- Comprender la historia y evolución de los paradigmas de programación.
- Familiarizarte con el entorno de desarrollo Visual Studio Code.
- Utilizar la terminal de comandos para interactuar con el sistema operativo.
- Configurar y utilizar Git para el control de versiones.

## 📖 Contenidos

1. Historia y evolución de los paradigmas de programación.
2. Introducción a Visual Studio Code.
3. Uso de la terminal de comandos.
4. Configuración y uso de Git.

## Configuración de VS Code

Visual Studio Code es un editor de código fuente desarrollado por Microsoft. Nos permite escribir, editar y depurar nuestro código de manera eficiente, tiene soporte para múltiples lenguajes de programación y una amplia gama de extensiones que mejoran su funcionalidad.

Para comenzar a usar VS Code, sigue estos pasos:

1. **Instalación**: Descarga e instala Visual Studio Code desde su [sitio oficial](https://code.visualstudio.com/).
2. **Extensiones recomendadas**:
   - **Error Lens**: Resalta errores y advertencias en el código.
   - **Prettier**: Formatea el código automáticamente.
   - **Auto rename tag**: Renombra etiquetas HTML/XML automáticamente.
   - **DotENV**: Permite trabajar con archivos `.env` para manejar variables de entorno.
   - **ESLint**: Herramienta para identificar y reportar patrones problemáticos en el código JavaScript.
   - **Live Server**: Permite lanzar un servidor local para ver los cambios en tiempo real.
   - **GitLens**: Mejora la funcionalidad de Git en VS Code, mostrando información detallada sobre el historial de cambios y autores.
   - **JavaScript (ES6) code snippets**: Proporciona fragmentos de código para JavaScript, facilitando la escritura de código común.
3. **Configuración**: Abre la configuración de VS Code (`Ctrl + ,`) y ajusta las preferencias según tus necesidades. Puedes buscar configuraciones específicas como "format on save" para formatear el código automáticamente al guardarlo.
4. **Terminal integrada**: Utiliza la terminal integrada de VS Code (`Ctrl + ñ`) para ejecutar comandos directamente desde el editor, lo que facilita el flujo de trabajo sin necesidad de cambiar de aplicación.

## Uso de la Terminal

La terminal es una herramienta poderosa que permite interactuar con el sistema operativo mediante comandos. En esta clase, aprenderemos a utilizar la terminal para realizar tareas básicas como navegar por el sistema de archivos, crear y eliminar directorios y archivos, y ejecutar programas desde Windows.

### Comandos Básicos de la Terminal en Windows

- `cd`: Cambia el directorio actual.
- `cd ..`: Sube un nivel en la jerarquía de directorios, en otras palabras, vuelves un directorio hacia atrás.
- `cd [ruta]`: Cambia al directorio especificado por la ruta.
- `cd [nombre_del_directorio]`: Cambia al directorio especificado.
- `dir`: Muestra el contenido del directorio actual.
- `mkdir`: Crea un nuevo directorio.
- `rmdir`: Elimina un directorio vacío.
- `cls`: Limpia la pantalla de la terminal.
- `code .`: Abre el directorio actual en Visual Studio Code, solo funciona si ya tienes VS Code instalado.
- `exit`: Cierra la terminal.

## Introducción a Git

En la clase pasada, aprendimos como descargar git, configurar nuestro usuario, crear un repositorio local, crear commits y agregar un repositorio remoto. En esta clase, profundizaremos en el uso de Git para el control de versiones, veremos como clonar un repositorio remoto, manejar ramas y realizar operaciones comunes.

### Clonar un Repositorio

Para clonar un repositorio remoto, utilizamos el comando `git clone` seguido de la URL del repositorio. Por ejemplo:

```bash
git clone https://github.com/Dieg0Code/PRO205-Taller-de-Programacion.git
```

Este comando descargará una copia a su máquina local del repositorio de esta clase. De ahora en adelante es obligatorio haber descargado el repositorio para poder trabajar en las actividades de la clase, ademas deberán actualizarlo diariamente para que no se pierdan las actividades que se vayan subiendo.

Para actualizar el repositorio, puedes utilizar el comando `git pull`:

1. Abre la terminal en el directorio del repositorio clonado.
2. Ejecuta el comando:

   ```bash
   git pull
   ```

Esto descargará los últimos cambios que yo haya realizado en el repositorio remoto y los aplicará a tu copia local.

### Manejo de Ramas

Las ramas en Git son en esencia copias paralelas del proyecto base. Permiten trabajar en diferentes características o correcciones de errores sin afectar la rama principal (generalmente llamada `main` o `master`).

Para crear una nueva rama, utilizamos el comando `git branch` seguido del nombre de la nueva rama:

```bash
git branch nombre_de_la_rama
```

Para cambiar a una rama específica, utilizamos el comando `git checkout`:

```bash
git checkout nombre_de_la_rama
```

También podemos combinar ambos comandos en uno solo utilizando `git checkout -b`:

```bash
git checkout -b nombre_de_la_rama
```

Este comando crea una nueva rama y nos cambia a ella al mismo tiempo.

### Flujos de Trabajo con Git

Un flujo de trabajo común con Git implica los siguientes pasos:

1. **Crear una nueva rama** para trabajar en una característica o corrección de errores.
2. **Realizar cambios** en el código y guardar esos cambios.
3. **Agregar los cambios** al área de preparación (staging area) utilizando `git add`.
4. **Confirmar los cambios** (commit) con un mensaje descriptivo utilizando `git commit -m "Mensaje del commit"`.
5. **Subir los cambios** al repositorio remoto utilizando `git push origin nombre_de_la_rama`.
6. **Crear un Pull Request** en la plataforma de Git (como GitHub) para solicitar la revisión y fusión de los cambios en la rama principal.

### GitFlow

GitFlow es un modelo de ramificación que define un conjunto de reglas para trabajar con ramas en Git. Este modelo ayuda a organizar el trabajo en equipo y facilita la gestión de versiones del software.

GitFlow define dos ramas principales:

- **`main`**: Contiene el código en producción.
- **`develop`**: Contiene el código en desarrollo, donde se integran las nuevas características antes de ser fusionadas a `main`.
- Además, se crean ramas de características (`feature`), correcciones (`hotfix`) y versiones (`release`) para organizar el trabajo.

Por ejemplo, supongamos que estamos trabajando en una nueva característica llamada "login". Para crear una rama de características, ejecutaríamos:

```bash
git checkout -b feature/login
```

Luego, realizaríamos los cambios necesarios, agregaríamos y confirmaríamos los cambios:

```bash
git add .
git commit -m "Implementación de la funcionalidad de login"
```

Después, subiríamos la rama al repositorio remoto:

```bash
git push origin feature/login
```

Luego, crearíamos un Pull Request para solicitar la revisión y fusión de la rama `feature/login` en `develop`.

## 🧠 Historia y Evolución de los Paradigmas de Programación

### ¿Qué es un Paradigma de Programación?

Un **paradigma de programación** es un estilo o enfoque fundamental que define cómo se estructura y organiza el código para resolver problemas. Cada paradigma proporciona un conjunto de conceptos, principios y abstracciones que guían el desarrollo de software.

> 💡 **Analogía:** Si el código fuera arquitectura, los paradigmas serían como los estilos arquitectónicos (gótico, moderno, minimalista) que determinan cómo se diseñan y construyen los edificios.

### Línea de Tiempo: Evolución de los Paradigmas

```
1940s          1950s         1960s         1970s         1980s         1990s         2000s+
[Código máquina]→[Ensamblador]→[Imperativo]→[Estructurado]→[Orientado a objetos]→[Declarativo]→[Multiparadigma]
                                             |
                                     [Procedural]→[Funcional]
```

### Principales Paradigmas de Programación

#### 1️⃣ Programación Imperativa (1950s)

**Concepto:** Se enfoca en describir **cómo** realizar una tarea mediante secuencias de instrucciones que cambian el estado del programa.

**Características:**

- Uso intensivo de variables y estados
- Secuencias de instrucciones paso a paso
- Control de flujo mediante bucles y condicionales

**Ejemplo (en pseudocódigo):**

```
x = 10
y = 20
suma = 0
suma = x + y
imprimir suma
```

**Lenguajes:** FORTRAN, COBOL, BASIC

#### 2️⃣ Programación Estructurada (1960s-1970s)

**Concepto:** Refinamiento de la programación imperativa que introduce **estructuras de control claras** y desalienta el uso de saltos incondicionales (GOTO).

**Características:**

- Secuencia: instrucciones ejecutadas en orden
- Selección: estructuras condicionales (if-then-else)
- Iteración: bucles estructurados (for, while)
- Subrutinas o procedimientos

**Ejemplo (en pseudocódigo):**

```
FUNCIÓN calcularPromedio(notas)
    suma = 0
    PARA CADA nota EN notas
        suma = suma + nota
    FIN PARA

    SI longitud(notas) > 0 ENTONCES
        RETORNAR suma / longitud(notas)
    SINO
        RETORNAR 0
    FIN SI
FIN FUNCIÓN
```

**Lenguajes:** C, Pascal, Ada

#### 3️⃣ Programación Orientada a Objetos (1970s-1980s)

**Concepto:** Organiza el código en torno a **objetos** que combinan datos (atributos) y comportamientos (métodos) en unidades encapsuladas.

**Características principales:**

- **Encapsulamiento:** Ocultar los detalles internos de un objeto
- **Herencia:** Reutilizar código extendiendo clases existentes
- **Polimorfismo:** Utilizar la misma interfaz para diferentes implementaciones
- **Abstracción:** Representar conceptos complejos de forma simplificada

**Ejemplo (en JavaScript):**

```javascript
class Estudiante {
  constructor(nombre, edad) {
    this.nombre = nombre;
    this.edad = edad;
    this.notas = [];
  }

  agregarNota(nota) {
    this.notas.push(nota);
  }

  calcularPromedio() {
    if (this.notas.length === 0) return 0;

    const suma = this.notas.reduce((a, b) => a + b, 0);
    return suma / this.notas.length;
  }
}

// Uso
const estudiante1 = new Estudiante("Ana López", 20);
estudiante1.agregarNota(85);
estudiante1.agregarNota(92);
console.log(estudiante1.calcularPromedio()); // 88.5
```

**Lenguajes:** C++, Java, C#, JavaScript, Python, Ruby

#### 4️⃣ Programación Funcional (1950s, popularizada en 2000s)

**Concepto:** Trata la computación como la evaluación de **funciones matemáticas** y evita cambiar el estado y los datos mutables.

**Características:**

- Funciones de primera clase (pueden ser asignadas a variables)
- Funciones puras (sin efectos secundarios)
- Inmutabilidad (datos no cambian después de crearse)
- Composición de funciones

**Ejemplo (en JavaScript):**

```javascript
// Programación funcional
const numeros = [1, 2, 3, 4, 5];

const duplicar = (x) => x * 2;
const esPar = (x) => x % 2 === 0;

const resultado = numeros
  .filter(esPar) // [2, 4]
  .map(duplicar) // [4, 8]
  .reduce((a, b) => a + b, 0); // 12

console.log(resultado); // 12
```

**Lenguajes:** Haskell, Lisp, Clojure, Erlang (puros); JavaScript, Python (con características funcionales)

#### 5️⃣ Programación Declarativa (1980s en adelante)

**Concepto:** Se enfoca en describir **qué** resultado se desea, no cómo lograrlo.

**Características:**

- Énfasis en la lógica, no en el control de flujo
- Abstracción de la implementación subyacente
- Código más conciso y legible

**Ejemplo (SQL):**

```sql
-- Declarativo (SQL)
SELECT nombre, promedio
FROM estudiantes
WHERE promedio > 70
ORDER BY promedio DESC;
```

**Lenguajes:** SQL, HTML, CSS, Prolog

### 🔄 Comparación Entre Paradigmas

| Paradigma               | Enfoque                           | Fortalezas                             | Debilidades                                 | Ejemplo de Problema Ideal                      |
| ----------------------- | --------------------------------- | -------------------------------------- | ------------------------------------------- | ---------------------------------------------- |
| **Imperativo**          | Cómo hacer algo                   | Simple de entender                     | Difícil de mantener en proyectos grandes    | Algoritmos matemáticos, tareas secuenciales    |
| **Estructurado**        | Cómo hacer algo de forma ordenada | Código más legible                     | Limitado para sistemas complejos            | Aplicaciones de consola, scripts               |
| **Orientado a Objetos** | Modelado del mundo real           | Reutilización de código, escalabilidad | Sobrecarga de diseño para problemas simples | Sistemas complejos, aplicaciones empresariales |
| **Funcional**           | Transformación de datos           | Concurrencia, testabilidad             | Curva de aprendizaje, abstracción           | Procesamiento de datos, paralelismo            |
| **Declarativo**         | Qué se quiere lograr              | Concisión, abstracción                 | Menos control sobre la implementación       | Consultas de bases de datos, interfaces        |

### 🌟 ¿Por qué la Programación Orientada a Objetos se volvió dominante?

La POO ganó popularidad por varias razones clave:

1. **Modelado intuitivo:** Representa el mundo real de manera natural (objetos con propiedades y comportamientos)
2. **Reutilización:** Permite construir sobre código existente mediante herencia y composición
3. **Mantenibilidad:** Facilita la organización del código en unidades lógicas (clases)
4. **Encapsulamiento:** Oculta detalles de implementación, exponiendo solo interfaces
5. **Ecosistema:** Amplia adopción en plataformas, frameworks y bibliotecas

### 🔮 Tendencias Actuales: Programación Multiparadigma

Los lenguajes modernos tienden a adoptar características de múltiples paradigmas:

- **JavaScript:** Soporta programación procedural, orientada a objetos y funcional
- **Python:** Combina características imperativas, orientadas a objetos y funcionales
- **C#:** Incorpora elementos de programación funcional a su base orientada a objetos
- **Kotlin/Swift:** Diseñados desde el principio como lenguajes multiparadigma

### 🏆 Paradigmas en Acción: Resolviendo un Mismo Problema

**Problema:** Calcular la suma de los números pares del 1 al 10

**Solución Imperativa:**

```javascript
// Enfoque imperativo
let suma = 0;
for (let i = 1; i <= 10; i++) {
  if (i % 2 === 0) {
    suma += i;
  }
}
console.log(suma); // 30
```

**Solución Orientada a Objetos:**

```javascript
// Enfoque orientado a objetos
class ColeccionNumeros {
  constructor(inicio, fin) {
    this.inicio = inicio;
    this.fin = fin;
  }

  filtrarPares() {
    let pares = [];
    for (let i = this.inicio; i <= this.fin; i++) {
      if (i % 2 === 0) pares.push(i);
    }
    return pares;
  }

  sumar(numeros) {
    let total = 0;
    for (let num of numeros) {
      total += num;
    }
    return total;
  }

  sumarPares() {
    const pares = this.filtrarPares();
    return this.sumar(pares);
  }
}

const coleccion = new ColeccionNumeros(1, 10);
console.log(coleccion.sumarPares()); // 30
```

**Solución Funcional:**

```javascript
// Enfoque funcional
const rango = (inicio, fin) =>
  Array.from({ length: fin - inicio + 1 }, (_, i) => i + inicio);

const esPar = (numero) => numero % 2 === 0;

const sumar = (numeros) => numeros.reduce((a, b) => a + b, 0);

const resultado = sumar(rango(1, 10).filter(esPar));
console.log(resultado); // 30
```

**Solución Declarativa:**

```javascript
// Enfoque declarativo (con bibliotecas)
import { range, sum } from "lodash";

const resultado = sum(range(1, 11).filter((n) => n % 2 === 0));
console.log(resultado); // 30
```

### 📝 Actividad Práctica: Identificación de Paradigmas

**Instrucciones:** Para cada fragmento de código, identifica qué paradigma de programación representa principalmente:

**Código 1:**

```python
def factorial(n):
    if n <= 1:
        return 1
    else:
        return n * factorial(n - 1)

resultado = factorial(5)
print(resultado)  # 120
```

**Código 2:**

```javascript
class Calculadora {
  sumar(a, b) {
    return a + b;
  }
  restar(a, b) {
    return a - b;
  }
  multiplicar(a, b) {
    return a * b;
  }
  dividir(a, b) {
    if (b === 0) throw new Error("División por cero");
    return a / b;
  }
}

const calc = new Calculadora();
console.log(calc.sumar(5, 3)); // 8
```

**Código 3:**

```sql
SELECT producto, SUM(ventas) as total_ventas
FROM ventas
WHERE fecha BETWEEN '2025-01-01' AND '2025-07-31'
GROUP BY producto
HAVING SUM(ventas) > 1000
ORDER BY total_ventas DESC;
```

**Código 4:**

```javascript
const numeros = [1, 2, 3, 4, 5];
const cuadrados = numeros.map((x) => x * x);
const suma = cuadrados.reduce((a, b) => a + b, 0);
console.log(suma); // 55
```

### 🎓 Conclusión: ¿Qué paradigma elegir?

No existe un paradigma "perfecto" para todos los problemas. La elección depende de:

1. **Naturaleza del problema:** Algunos problemas se adaptan mejor a ciertos paradigmas
2. **Requisitos del proyecto:** Escalabilidad, mantenibilidad, rendimiento
3. **Equipo de desarrollo:** Experiencia y preferencias del equipo
4. **Ecosistema tecnológico:** Frameworks, bibliotecas y herramientas disponibles

> 💡 **Consejo profesional:** Un buen programador debe conocer múltiples paradigmas y saber cuándo aplicar cada uno. La versatilidad es clave en el desarrollo moderno.

## 📝 Actividades de la Clase

### Actividad 1: Configuración de VS Code

1. Instala VS Code si aún no lo has hecho.
2. Instala las extensiones recomendadas.
3. Configura la opción "Format on Save" para formatear automáticamente tu código al guardar.
4. Familiarízate con los atajos de teclado básicos (Ctrl+S para guardar, Ctrl+ñ para abrir la terminal).

### Actividad 2: Práctica con Git

1. Clona el repositorio del curso usando la terminal:
   ```bash
   git clone https://github.com/Dieg0Code/PRO205-Taller-de-Programacion.git
   ```
2. Crea una rama con tu nombre:
   ```bash
   git checkout -b nombre-apellido
   ```
3. Crea un archivo llamado "paradigmas.md" con tus respuestas a la actividad de identificación de paradigmas.
4. Añade, realiza commit y sube tu rama al repositorio remoto:
   ```bash
   git add paradigmas.md
   git commit -m "Actividad: Identificación de paradigmas"
   git push origin nombre-apellido
   ```

## 📖 Material Complementario

- [Historia de los lenguajes de programación (línea de tiempo visual)](https://www.computerhistory.org/timeline/software-languages/)
- [Paradigmas de programación - Mozilla Developer Network](https://developer.mozilla.org/es/docs/Glossary/Programming_paradigm)
- [Git - La guía sencilla](https://rogerdudler.github.io/git-guide/index.es.html)
- [VS Code Shortcuts para Windows](https://code.visualstudio.com/shortcuts/keyboard-shortcuts-windows.pdf)

## 🎯 Para la próxima clase

- Leer sobre los principios fundamentales de la Programación Orientada a Objetos.
- Explorar la sintaxis básica de JavaScript para la creación de clases y objetos.
- Revisar el material complementario sobre paradigmas de programación.
