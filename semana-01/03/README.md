# 📚 Clase 03: Componentes POO (Objetos, Clases) y JavaScript Básico

- Unidad 01: **Introducción a la Programación Orientada a Objetos**
- Fecha: Miércoles 13 de Agosto, 2025
- Horario: 10:50 - 13:00
- Docente: Diego Obando

## 🎯 Objetivos de la Clase

Al finalizar esta clase, serás capaz de:

- Comprender los conceptos básicos de la Programación Orientada a Objetos (POO).
- Identificar y crear objetos y clases en JavaScript.
- Familiarizarte con el uso de un repositorio para gestionar tu código.
- Aplicar los fundamentos de JavaScript básico en la creación de objetos y clases.

## 📖 Contenidos

1. Conceptos básicos de la POO, Clases y Objetos (propiedades y métodos).
2. Sintaxis básica de JavaScript y su uso en la creación de objetos y clases.
3. Ejercicios prácticos para aplicar los conceptos aprendidos.

## Conceptos Básicos de POO 🧩

La **Programación Orientada a Objetos (POO)** es un paradigma que revolucionó el desarrollo de software, permitiéndonos modelar problemas complejos de una forma más natural e intuitiva.

### ¿Qué es la POO? 🤔

La POO es un enfoque de programación que organiza el código alrededor de **"objetos"** en lugar de funciones y lógica. Estos objetos son representaciones de entidades del mundo real o conceptos abstractos que tienen:

- **Características** (llamadas propiedades o atributos)
- **Comportamientos** (llamados métodos)

> 💡 **Analogía:** Piensa en los objetos como "sustantivos" y los métodos como "verbos" en el lenguaje humano.

### Los 4 Pilares de la POO 🏛️

#### 1️⃣ Encapsulamiento 📦

**Concepto:** Agrupar datos y métodos relacionados en una unidad (la clase) y restringir el acceso directo a algunos de sus componentes.

**Beneficios:**

- Protege los datos de modificaciones accidentales
- Oculta la complejidad interna
- Simplifica la interfaz de uso

**Ejemplo del mundo real:** Un auto - puedes usar los controles (interfaz pública) sin necesidad de entender cómo funciona el motor (detalles encapsulados).

```javascript
class CuentaBancaria {
  #saldo = 0; // El # hace que sea un campo privado

  constructor(titular, saldoInicial) {
    this.titular = titular;
    this.#saldo = saldoInicial;
  }

  depositar(monto) {
    if (monto > 0) {
      this.#saldo += monto;
      return true;
    }
    return false;
  }

  getSaldo() {
    return this.#saldo;
  }
}

const miCuenta = new CuentaBancaria("Diego", 1000);
miCuenta.depositar(500);
console.log(miCuenta.getSaldo()); // 1500
// console.log(miCuenta.#saldo);  // Error: campo privado
```

#### 2️⃣ Herencia 👨‍👦

**Concepto:** Mecanismo por el cual una clase (hija) puede heredar propiedades y métodos de otra clase (padre).

**Beneficios:**

- Reutilización de código
- Jerarquías lógicas de objetos
- Extensibilidad del código

**Ejemplo del mundo real:** Un smartphone es un teléfono con características adicionales - hereda todas las funcionalidades básicas de un teléfono y añade nuevas.

```javascript
class Vehiculo {
  constructor(marca, modelo) {
    this.marca = marca;
    this.modelo = modelo;
    this.encendido = false;
  }

  encender() {
    this.encendido = true;
    console.log("Vehículo encendido");
  }

  apagar() {
    this.encendido = false;
    console.log("Vehículo apagado");
  }
}

class Automovil extends Vehiculo {
  constructor(marca, modelo, puertas) {
    super(marca, modelo); // Llama al constructor de la clase padre
    this.puertas = puertas;
    this.velocidad = 0;
  }

  acelerar(kmh) {
    if (this.encendido) {
      this.velocidad += kmh;
      console.log(`Acelerando a ${this.velocidad} km/h`);
    } else {
      console.log("Primero debes encender el automóvil");
    }
  }
}

const miAuto = new Automovil("Toyota", "Corolla", 4);
miAuto.encender(); // Método heredado
miAuto.acelerar(50); // Método propio
```

#### 3️⃣ Polimorfismo 🦎→🐉

**Concepto:** Capacidad de diferentes clases de responder al mismo mensaje (método) de diferentes maneras.

**Beneficios:**

- Flexibilidad en el diseño
- Código más genérico y reutilizable
- Interfaces uniformes

**Ejemplo del mundo real:** El botón "Reproducir" funciona en diferentes dispositivos (TV, radio, smartphone) pero cada uno lo implementa de forma distinta.

```javascript
class Animal {
  hacerSonido() {
    console.log("El animal hace un sonido");
  }
}

class Perro extends Animal {
  hacerSonido() {
    console.log("¡Guau guau!");
  }
}

class Gato extends Animal {
  hacerSonido() {
    console.log("¡Miau!");
  }
}

// Polimorfismo en acción
function hacerSonarAnimal(animal) {
  animal.hacerSonido(); // Mismo método, comportamiento diferente
}

const miPerro = new Perro();
const miGato = new Gato();

hacerSonarAnimal(miPerro); // ¡Guau guau!
hacerSonarAnimal(miGato); // ¡Miau!
```

#### 4️⃣ Abstracción 🧠

**Concepto:** Simplificar sistemas complejos ocultando detalles y mostrando solo la funcionalidad esencial.

**Beneficios:**

- Reduce la complejidad
- Facilita los cambios en la implementación
- Mejora la claridad del código

**Ejemplo del mundo real:** Al conducir un auto, usas el volante y los pedales sin preocuparte por cómo funcionan internamente.

```javascript
// En JavaScript no hay clases abstractas nativas, pero podemos simularlas
class DispositivoElectronico {
  encender() {
    throw new Error("El método encender() debe ser implementado");
  }

  apagar() {
    throw new Error("El método apagar() debe ser implementado");
  }
}

class Televisor extends DispositivoElectronico {
  encender() {
    console.log("TV encendida");
  }

  apagar() {
    console.log("TV apagada");
  }

  cambiarCanal(canal) {
    console.log(`Cambiando al canal ${canal}`);
  }
}

const miTV = new Televisor();
miTV.encender();
miTV.cambiarCanal(7);
```

### Clase vs Objeto: ¿Cuál es la diferencia? 🤷‍♂️

Esta es una de las confusiones más comunes para quienes se inician en POO:

- **Clase:** Es la "plantilla" o "plano" que define cómo será un tipo de objeto
- **Objeto:** Es una "instancia" o "ejemplar" creado a partir de una clase

> 💡 **Analogía:** La clase es como el plano de una casa, mientras que el objeto es la casa construida siguiendo ese plano. Puedes construir muchas casas diferentes (objetos) usando el mismo plano (clase).

```javascript
// Clase - El plano
class Smartphone {
  constructor(marca, modelo, color) {
    this.marca = marca;
    this.modelo = modelo;
    this.color = color;
    this.encendido = false;
  }

  encender() {
    this.encendido = true;
    return `${this.marca} ${this.modelo} encendido`;
  }
}

// Objetos - Las instancias concretas
const iphone = new Smartphone("Apple", "iPhone 15", "Negro");
const samsung = new Smartphone("Samsung", "Galaxy S25", "Azul");

console.log(iphone.encender()); // "Apple iPhone 15 encendido"
console.log(samsung.marca); // "Samsung"
```

### Componentes de una Clase 🧰

Una clase en JavaScript típicamente contiene:

1. **Constructor:** Método especial que se ejecuta al crear un objeto
2. **Propiedades:** Variables que almacenan datos del objeto
3. **Métodos:** Funciones que definen el comportamiento del objeto
4. **Getters/Setters:** Métodos especiales para acceder y modificar propiedades

```javascript
class Estudiante {
  // Propiedades privadas (solo accesibles dentro de la clase)
  #notas = [];

  // Constructor - inicializa el objeto
  constructor(nombre, edad) {
    this.nombre = nombre; // Propiedad pública
    this.edad = edad; // Propiedad pública
  }

  // Método para agregar una nota
  agregarNota(nota) {
    if (nota >= 0 && nota <= 100) {
      this.#notas.push(nota);
      return true;
    }
    return false;
  }

  // Getter - retorna un valor calculado
  get promedio() {
    if (this.#notas.length === 0) return 0;

    const suma = this.#notas.reduce((a, b) => a + b, 0);
    return suma / this.#notas.length;
  }

  // Método estático - pertenece a la clase, no a instancias
  static edadPromedio(estudiantes) {
    const suma = estudiantes.reduce((total, est) => total + est.edad, 0);
    return suma / estudiantes.length;
  }
}

// Crear objetos
const juan = new Estudiante("Juan", 20);
juan.agregarNota(85);
juan.agregarNota(90);

console.log(juan.nombre); // "Juan"
console.log(juan.promedio); // 87.5

const ana = new Estudiante("Ana", 22);
console.log(Estudiante.edadPromedio([juan, ana])); // 21
```

### POO en el Mundo Real 🌎

Los conceptos de POO nos rodean en nuestro día a día:

| Concepto   | Ejemplo Real   | Propiedades                     | Métodos                |
| ---------- | -------------- | ------------------------------- | ---------------------- |
| **Clase**  | Smartphone     |                                 |                        |
| **Objeto** | Mi iPhone      | marca, modelo, color, batería   | llamar(), tomarFoto()  |
| **Clase**  | Automóvil      |                                 |                        |
| **Objeto** | Mi Toyota      | marca, modelo, año, kilometraje | encender(), acelerar() |
| **Clase**  | CuentaBancaria |                                 |                        |
| **Objeto** | Mi cuenta      | número, saldo, titular          | depositar(), retirar() |

### Ventajas de la POO ✅

- **Modularidad:** El código se divide en unidades lógicas y reutilizables
- **Mantenibilidad:** Es más fácil actualizar y corregir el código
- **Escalabilidad:** Facilita el crecimiento de aplicaciones complejas
- **Naturalidad:** Modela problemas de forma similar a como pensamos en el mundo real

### ¿Cómo pensar en objetos? 🤔

Para identificar objetos y clases en un problema:

1. **Busca sustantivos** en la descripción del problema (posibles objetos/clases)
2. **Identifica atributos** (propiedades) de esos objetos
3. **Busca verbos** relacionados con esos sustantivos (posibles métodos)
4. **Establece relaciones** entre los objetos (herencia, composición)

**Ejemplo:** Sistema de biblioteca

- **Sustantivos:** Libro, Usuario, Préstamo, Biblioteca
- **Atributos:** título, autor, ISBN (para Libro); nombre, dirección (para Usuario)
- **Verbos:** prestar, devolver, buscar, reservar
- **Relaciones:** Biblioteca contiene Libros, Usuario realiza Préstamos

### 🔄 Ejercicio Rápido

Piensa en un **sistema de reproducción de música**. Identifica:

1. Tres posibles clases

   - Canción
   - Reproductor
   - ListaDeReproducción

2. Tres propiedades para cada clase

   - Canción: título, artista, duración
   - Reproductor: marca, modelo, volumen
   - ListaDeReproducción: nombre, canciones, repetición

3. Tres métodos para cada clase

   - Canción: reproducir(), pausar(), detener()
   - Reproductor: encender(), apagar(), ajustarVolumen()
   - ListaDeReproducción: agregarCanción(), eliminarCanción(), reproducirTodo()

4. Al menos una relación entre clases

   - La clase Reproductor puede contener una o más instancias de ListaDeReproducción.
   - La clase ListaDeReproducción puede contener múltiples instancias de Canción.
   - La clase Canción puede ser reproducida por una instancia de Reproductor.

## Sintaxis básica de JavaScript y su uso en la creación de objetos y clases 🧑‍💻

### 1. Variables y Tipos de Datos 📊

```javascript
// Declaración de variables
let nombre = "Diego"; // String - Para texto
const edad = 30; // Number - Para números
var esActivo = true; // Boolean - true o false
let noDefinido; // undefined - Variable sin valor

// Preferir let y const sobre var (var tiene scope de función)
const PI = 3.14159; // const: no puede cambiar
let contador = 0; // let: puede cambiar

// Arrays y Objetos
let colores = ["rojo", "verde", "azul"];
let estudiante = {
  nombre: "Ana",
  edad: 25,
  carrera: "Informática",
};
```

### 2. Operadores Básicos 🔣

```javascript
// Operadores aritméticos
let suma = 5 + 3; // 8
let resta = 10 - 4; // 6
let multiplicacion = 3 * 2; // 6
let division = 8 / 2; // 4
let modulo = 7 % 2; // 1 (resto de división)

// Operadores de comparación
let igual = 5 == "5"; // true (compara valor)
let estrictamenteIgual = 5 === "5"; // false (compara valor y tipo)
let mayor = 10 > 5; // true
let menorIgual = 3 <= 3; // true

// Operadores lógicos
let and = true && false; // false
let or = true || false; // true
let not = !true; // false
```

### 3. Estructuras Condicionales 🔀

```javascript
// if, else if, else
let nota = 85;

if (nota >= 90) {
  console.log("Sobresaliente");
} else if (nota >= 70) {
  console.log("Notable");
} else if (nota >= 60) {
  console.log("Aprobado");
} else {
  console.log("Reprobado");
}

// Operador ternario
let mensaje = edad >= 18 ? "Adulto" : "Menor";

// switch
let dia = 3;
switch (dia) {
  case 1:
    console.log("Lunes");
    break;
  case 2:
    console.log("Martes");
    break;
  case 3:
    console.log("Miércoles");
    break;
  default:
    console.log("Otro día");
}
```

### 4. Ciclos (Bucles) 🔄

```javascript
// for
for (let i = 0; i < 5; i++) {
  console.log(`Iteración ${i}`);
}

// while
let contador = 0;
while (contador < 3) {
  console.log(`While: ${contador}`);
  contador++;
}

// do...while (ejecuta al menos una vez)
let j = 0;
do {
  console.log(`Do while: ${j}`);
  j++;
} while (j < 3);

// for...of (para arrays)
const frutas = ["manzana", "naranja", "plátano"];
for (const fruta of frutas) {
  console.log(fruta);
}

// for...in (para objetos)
const persona = { nombre: "Juan", edad: 30 };
for (const propiedad in persona) {
  console.log(`${propiedad}: ${persona[propiedad]}`);
}
```

### 5. Funciones 🛠️

```javascript
// Declaración de función
function saludar(nombre) {
  return `¡Hola, ${nombre}!`;
}

// Expresión de función
const despedir = function (nombre) {
  return `¡Adiós, ${nombre}!`;
};

// Función flecha (arrow function)
const duplicar = (numero) => numero * 2;

// Función con parámetros por defecto
function configurar(opciones = { color: "azul", tamaño: "mediano" }) {
  console.log(`Color: ${opciones.color}, Tamaño: ${opciones.tamaño}`);
}

// Funciones con número variable de argumentos
function sumar(...numeros) {
  return numeros.reduce((total, n) => total + n, 0);
}
console.log(sumar(1, 2, 3, 4)); // 10
```

### 6. Módulos (Import/Export) 📦

```javascript
// archivo: matematicas.js
export function sumar(a, b) {
  return a + b;
}

export const PI = 3.14159;

export default class Calculadora {
  multiplicar(a, b) {
    return a * b;
  }
}

// archivo: app.js
import Calculadora, { sumar, PI } from "./matematicas.js";

console.log(sumar(5, 3)); // 8
console.log(PI); // 3.14159

const calc = new Calculadora();
console.log(calc.multiplicar(4, 2)); // 8
```

### 7. Objetos y Clases (Sintaxis Moderna) 🏗️

```javascript
// Clase ES6
class Usuario {
  // Constructor - se ejecuta al crear un objeto
  constructor(nombre, email) {
    this.nombre = nombre;
    this.email = email;
    this.activo = true;
  }

  // Métodos
  desactivar() {
    this.activo = false;
    return `Usuario ${this.nombre} desactivado`;
  }

  // Getters y setters
  get nombreEmail() {
    return `${this.nombre} <${this.email}>`;
  }

  set nombreCompleto(nombreCompleto) {
    const partes = nombreCompleto.split(" ");
    this.nombre = partes[0];
    this.apellido = partes[1];
  }

  // Método estático
  static buscarPorDominio(usuarios, dominio) {
    return usuarios.filter((u) => u.email.endsWith(dominio));
  }
}

// Herencia
class Administrador extends Usuario {
  constructor(nombre, email, nivel) {
    super(nombre, email); // Llama al constructor padre
    this.nivel = nivel;
    this.permisos = ["lectura", "escritura", "eliminación"];
  }

  agregarPermiso(permiso) {
    this.permisos.push(permiso);
  }
}

// Uso
const usuario1 = new Usuario("Carlos", "carlos@ejemplo.com");
console.log(usuario1.nombreEmail); // "Carlos <carlos@ejemplo.com>"

const admin = new Administrador("Ana", "ana@ejemplo.com", 2);
admin.agregarPermiso("administración");
console.log(admin.permisos); // ['lectura', 'escritura', 'eliminación', 'administración']
```

### 8. Async/Await y Promesas ⏱️

```javascript
// Promesas básicas
const promesa = new Promise((resolve, reject) => {
  setTimeout(() => {
    const exito = true;
    if (exito) {
      resolve("¡Operación completada!");
    } else {
      reject("¡Error en la operación!");
    }
  }, 1000);
});

// Manejo de promesas
promesa
  .then((mensaje) => console.log(mensaje))
  .catch((error) => console.error(error));

// Async/Await (manera más legible de trabajar con promesas)
async function obtenerDatos() {
  try {
    const respuesta = await fetch("https://api.ejemplo.com/datos");
    const datos = await respuesta.json();
    return datos;
  } catch (error) {
    console.error("Error al obtener datos:", error);
  }
}
```

### 9. Ejemplo Práctico Integrado 🚀

```javascript
// Sistema de gestión de estudiantes
class Persona {
  constructor(nombre, edad) {
    this.nombre = nombre;
    this.edad = edad;
  }

  presentarse() {
    return `Hola, soy ${this.nombre} y tengo ${this.edad} años.`;
  }
}

class Estudiante extends Persona {
  #notas = [];

  constructor(nombre, edad, carrera) {
    super(nombre, edad);
    this.carrera = carrera;
  }

  agregarNota(asignatura, valor) {
    if (valor < 0 || valor > 100) {
      throw new Error("La nota debe estar entre 0 y 100");
    }
    this.#notas.push({ asignatura, valor });
  }

  get promedio() {
    if (this.#notas.length === 0) return 0;
    const suma = this.#notas.reduce((total, nota) => total + nota.valor, 0);
    return suma / this.#notas.length;
  }

  // Sobrescribe el método del padre
  presentarse() {
    return `${super.presentarse()} Estudio ${this.carrera} y mi promedio es ${
      this.promedio
    }.`;
  }
}

// Uso del sistema
const estudiante1 = new Estudiante("María", 22, "Informática");
estudiante1.agregarNota("Programación", 85);
estudiante1.agregarNota("Bases de Datos", 92);

console.log(estudiante1.presentarse());
// "Hola, soy María y tengo 22 años. Estudio Informática y mi promedio es 88.5."
```

## Ejercicios prácticos para aplicar los conceptos aprendidos 💻

Vamos a poner en práctica los conceptos aprendidos con ejercicios progresivos que reforzarán tu comprensión de la POO en JavaScript.

### Ejercicio 1: Creación básica de clases y objetos 🏁

**Objetivo:** Familiarizarte con la definición de clases y la creación de objetos.

#### Instrucciones:

Crea una clase `Producto` con las siguientes características:

- Propiedades: `nombre`, `precio`, `disponible` (booleano)
- Método constructor que inicialice estas propiedades
- Método `mostrarInfo()` que devuelva un string con el formato: "Producto: [nombre], Precio: $[precio]"
- Método `toggleDisponibilidad()` que cambie el estado de disponibilidad

```javascript
// Plantilla inicial
class Producto {
  // Tu código aquí
}

// Código de prueba
const producto1 = new Producto("Laptop", 1200, true);
console.log(producto1.mostrarInfo());
producto1.toggleDisponibilidad();
console.log(producto1.disponible); // Debería mostrar false
```

### Ejercicio 2: Herencia y encapsulamiento 🔄

**Objetivo:** Practicar la creación de jerarquías de clases y protección de datos.

#### Instrucciones:

1. Crea una clase base `Vehiculo` con:

   - Propiedades: `marca`, `modelo`, `año`
   - Propiedad con convención de privada: `_kilometraje`
   - Constructor que inicialice estas propiedades
   - Método `mostrarDetalles()` que muestre la información básica
   - Getter para obtener el kilometraje
   - Método para aumentar el kilometraje (`agregarKilometraje(km)`)

2. Crea dos clases que hereden de `Vehiculo`:
   - `Automovil` con propiedad adicional `puertas` y método `encender()`
   - `Motocicleta` con propiedad adicional `tipo` y método `hacerAcrobacia()`

```javascript
// Plantilla inicial
class Vehiculo {
  constructor(marca, modelo, año, kilometraje = 0) {
    this.marca = marca;
    this.modelo = modelo;
    this.año = año;
    this._kilometraje = kilometraje; // Convención para "privado"
  }

  // Método getter
  get kilometraje() {
    return this._kilometraje;
  }

  // Añade los métodos necesarios
}

class Automovil extends Vehiculo {
  // Tu código aquí
}

class Motocicleta extends Vehiculo {
  // Tu código aquí
}

// Código de prueba
const auto = new Automovil("Toyota", "Corolla", 2023, 5000, 4);
auto.agregarKilometraje(200);
console.log(auto.mostrarDetalles());
console.log(`Kilometraje actual: ${auto.kilometraje} km`);
console.log(auto.encender());

const moto = new Motocicleta("Honda", "CBR", 2022, 3000, "Deportiva");
console.log(moto.mostrarDetalles());
console.log(moto.hacerAcrobacia());
```

### Ejercicio 3: Sistema de gestión de biblioteca 📚

**Objetivo:** Crear un sistema que integre múltiples clases con relaciones entre ellas.

#### Instrucciones:

Implementa un sistema de gestión de biblioteca con las siguientes clases:

1. `Libro`:

   - Propiedades: `titulo`, `autor`, `isbn`, `disponible` (booleano)
   - Métodos: `prestar()`, `devolver()`

2. `Usuario`:

   - Propiedades: `id`, `nombre`, `librosEnPrestamo` (array)
   - Métodos: `tomarPrestado(libro)`, `devolverLibro(libro)`

3. `Biblioteca`:
   - Propiedades: `nombre`, `libros` (array), `usuarios` (array)
   - Métodos: `agregarLibro(libro)`, `registrarUsuario(usuario)`, `prestarLibro(usuario, libro)`, `recibirLibro(usuario, libro)`

```javascript
// Plantilla inicial
class Libro {
  constructor(titulo, autor, isbn) {
    this.titulo = titulo;
    this.autor = autor;
    this.isbn = isbn;
    this.disponible = true;
  }

  // Añade los métodos necesarios
}

class Usuario {
  constructor(id, nombre) {
    this.id = id;
    this.nombre = nombre;
    this.librosEnPrestamo = [];
  }

  // Añade los métodos necesarios
}

class Biblioteca {
  constructor(nombre) {
    this.nombre = nombre;
    this.libros = [];
    this.usuarios = [];
  }

  // Añade los métodos necesarios
}

// Código para probar el sistema
const biblioteca = new Biblioteca("Biblioteca Central");

const libro1 = new Libro("El Quijote", "Miguel de Cervantes", "123456");
const libro2 = new Libro(
  "Cien años de soledad",
  "Gabriel García Márquez",
  "789012"
);

biblioteca.agregarLibro(libro1);
biblioteca.agregarLibro(libro2);

const usuario1 = new Usuario(1, "Ana Martínez");
biblioteca.registrarUsuario(usuario1);

// Préstamo y devolución
biblioteca.prestarLibro(usuario1, libro1);
console.log(usuario1.librosEnPrestamo); // Debería mostrar el libro1
console.log(libro1.disponible); // Debería ser false

biblioteca.recibirLibro(usuario1, libro1);
console.log(usuario1.librosEnPrestamo); // Debería estar vacío
console.log(libro1.disponible); // Debería ser true
```

### Ejercicio 4: Tienda Online Básica 🛍️

**Objetivo:** Aplicar los conceptos de POO en un sistema completo pero accesible.

#### Instrucciones:

Crea un sistema de tienda online simplificado con las siguientes clases:

1. `Producto`:

   - Propiedades: `id`, `nombre`, `precio`, `stock`
   - Métodos: `actualizarStock(cantidad)`, `mostrarDetalle()`

2. `Cliente`:

   - Propiedades: `id`, `nombre`, `email`, `direccion`
   - Métodos: `actualizarDireccion(nuevaDireccion)`

3. `Carrito`:
   - Propiedades: `cliente`, `productos` (array de objetos {producto, cantidad}), `fecha`
   - Métodos: `agregarProducto(producto, cantidad)`, `eliminarProducto(idProducto)`, `calcularTotal()`, `vaciar()`

```javascript
// Plantilla inicial
class Producto {
  constructor(id, nombre, precio, stock) {
    // Tu código aquí
  }

  // Métodos
}

class Cliente {
  constructor(id, nombre, email, direccion) {
    // Tu código aquí
  }

  // Métodos
}

class Carrito {
  constructor(cliente) {
    this.cliente = cliente;
    this.productos = [];
    this.fecha = new Date();
  }

  agregarProducto(producto, cantidad) {
    // Verificar que hay suficiente stock
    // Agregar al carrito
    // Actualizar stock
  }

  // Más métodos
}

// Código de prueba
const producto1 = new Producto(1, "Laptop Gaming", 1200, 10);
const producto2 = new Producto(2, "Mouse Inalámbrico", 45, 20);
const cliente1 = new Cliente(
  1,
  "Juan Pérez",
  "juan@ejemplo.com",
  "Av. Principal 123"
);

const carrito = new Carrito(cliente1);
carrito.agregarProducto(producto1, 1);
carrito.agregarProducto(producto2, 2);

console.log(`Total: $${carrito.calcularTotal()}`); // Debería ser 1290
console.log(`Stock restante de laptops: ${producto1.stock}`); // Debería ser 9

carrito.eliminarProducto(2); // Elimina el mouse
console.log(`Total actualizado: $${carrito.calcularTotal()}`); // Debería ser 1200
console.log(`Stock actualizado de mouse: ${producto2.stock}`); // Debería ser 20 de nuevo
```

### 🔍 Desafío Adicional: Sistema de Blog Simplificado 📝

Si completas los ejercicios anteriores y quieres un reto adicional, implementa un sistema de blog con:

```javascript
// Clases a implementar:
// 1. Autor (nombre, email, bio)
// 2. Artículo (título, contenido, fecha, autor, comentarios[])
// 3. Comentario (contenido, autor, fecha)
// 4. Blog (nombre, artículos[])

// Funcionalidades:
// - Crear y publicar artículos
// - Agregar comentarios a artículos
// - Listar artículos por autor
// - Búsqueda de artículos por palabra clave
```

### 💡 Consejos para resolver los ejercicios

1. **Lee las instrucciones completas** antes de empezar a programar
2. **Planifica tu enfoque**: Esboza las clases y sus relaciones
3. **Prueba constantemente**: No esperes a terminar todo para probar
4. **Usa la consola**: Agrega `console.log()` para verificar valores
5. **Implementa por partes**: Comienza con la estructura básica y ve añadiendo funcionalidades
6. **Aplica los 4 pilares de POO**: Encapsulamiento, herencia, polimorfismo y abstracción

¡Manos a la obra! 💪
