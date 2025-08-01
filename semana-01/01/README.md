# 📚 Clase 01: Presentación del Curso y Configuración de Herramientas

- Unidad 1: **Introducción a la Programación Orientada a Objetos**
- Fecha: Lunes 11 de agosto, 2025
- Horario: 10:50 - 13:00
- Docente: Diego Obando

## 🎯 Objetivos de la Clase

Al finalizar esta clase, serás capaz de:

- Comprender el contexto y objetivos del módulo **Taller de Programación**.
- Identificar tu nivel actual de conocimientos mediante el diagnóstico.
- Configurar tu entorno de desarrollo con **Git** y **GitHub**.

## 🚀 Presentación del Curso

¿Qué veremos en este curso?

En este módulo exploraremos los fundamentos de la programación orientada a objetos, mediante el uso de lenguajes como **JavaScript**, **TypeScript** y **C#**. El diseño de software orientado a objetos se abordará mediante el uso de **UML** con diagramas de clases. Veremos también el diseño de interfaces gráficas de usuario (GUI) de escritorio y web usando **Windows Forms** y **React**, persistencia de datos usando **MSSQL Server** y **PostgreSQL**, y la creación de APIs RESTful con **ASP.NET Core** y **Node.js**.

Mi objetivo como docente y programador es introducirlos al desarrollo de software full stack, brindándoles las herramientas y conocimientos necesarios para que puedan crear aplicaciones completas, desde el backend hasta el frontend, además del análisis y diseño de sistemas usando UML.

> 💡 **Importante**: Este módulo incluye preparación para la **Evaluación Nacional de Especialidad (ENE)**

🎓 ¿Qué lograrás al finalizar?

- Crear aplicaciones web y de escritorio utilizando los principios de la programación orientada a objetos.
- Modelar sistemas utilizando UML.
- Aplicar principios de diseño SOLID.
- Conectar aplicaciones con bases de datos relacionales.
- Desarrollar un portafolio en GitHub.

## 🔍 Diagnóstico de Conocimientos

📝 Cuestionario de Evaluación Inicial

Responde honestamente para que pueda conocer tu nivel actual de conocimientos y adaptar el curso a tus necesidades.

### Sección A: Experiencia en Programación

1. ¿Has programado antes?

   - Nunca
   - Muy poco (solo ejercicios básicos)
   - Moderado (algunos proyectos pequeños)
   - Avanzado (proyectos complejos)

2. ¿Qué lenguajes conoces? (marca todos los que apliquen)

   - JavaScript
   - TypeScript
   - C#
   - Java
   - Python
   - Otros:

### Sección B: Conceptos Técnicos

3. ¿Conoces estos conceptos? (marca todos lo sepas explicar)

   - Variable
   - Función
   - Bucle (for, while)
   - Condicional (if/else)
   - Array/Lista

4. ¿Has trabajado con estas herramientas? (marca las que hayas usado)

   - Git/GitHub
   - Visual Studio Code
   - Terminal/Línea de comandos
   - Windows PowerShell
   - Ninguna

### Sección C: Programación Orientada a Objetos

5. ¿Has escuchado sobre "programación orientada a objetos"?

   - Nunca
   - He escuchado el término
   - Conozco los conceptos básicos (clases, objetos, herencia)
   - He programado usando POO

6. ¿Puedes explicar qué es una clase?

   - No tengo idea
   - Una idea vaga
   - Sí, puedo explicarlo básicamente
   - Sí, domino el concepto

### Sección D: UML y Diseño de Software

7. ¿Conoces UML?

   - Nunca
   - He oído hablar de él
   - Conozco algunos diagramas (clases, secuencia)
   - He usado UML en proyectos

8. ¿Has diseñado interfaces gráficas de usuario (GUI)?

   - Nunca
   - He oído hablar de ellas
   - He diseñado algunas interfaces simples
   - Tengo experiencia diseñando interfaces complejas

> Tiempo estimado para completar el cuestionario: **10 minutos**.

## 🛠️ Introducción y Configuración de Git y GitHub

¿Por qué Git y GitHub?

### 🌟 Git: Herramienta de Control de Versiones

**Definición Formal**: Git es un sistema de control de versiones distribuido que permite a los desarrolladores rastrear cambios en el código fuente a lo largo del tiempo. Facilita la colaboración entre múltiples desarrolladores y la gestión de versiones de un proyecto.

- **Historial completo** de todos los cambios realizados en el código.
- **Backups automáticos** de tu trabajo.
- **Colaboración** con otros desarrolladores a través de ramas y fusiones (merge).
- **Deshacer cambios** fácilmente si algo sale mal.
- **Rastrear errores** y revertir a versiones anteriores.
- **Control de versiones** profesional y organizado.

### 🌐 GitHub: Plataforma de Colaboración

**Definición Formal**: GitHub es una plataforma de desarrollo colaborativo que utiliza Git como sistema de control de versiones. Permite a los desarrolladores alojar sus proyectos, colaborar con otros y gestionar el ciclo de vida del desarrollo de software.

- **95% de las empresas** revisan perfiles de GitHub al contratar.
- **Repositorio centralizado** accesible desde cualquier lugar.
- **Colaboración** con desarrolladores de todo el mundo.
- **Issues y Pull Requests** para gestionar tareas y revisiones de código.
- **Empresas como Microsoft, Google y Facebook** utilizan GitHub para sus proyectos.

### Breve introducción a la Terminal y línea de comandos en Windows

Para usar Git y GitHub, es fundamental familiarizarse con la línea de comandos en Windows. Aquí tienes algunos conceptos básicos:

- **Win + R**, escribe `cmd` y presiona Enter para abrir la línea de comandos.
- También puedes buscar "Símbolo del sistema" o "Command Prompt" en el menú de inicio.
- Navega entre carpetas con el comando `cd` (change directory) más el nombre de la carpeta.
- El comando `dir` muestra el contenido de la carpeta actual.
- Para crear una nueva carpeta, usa `mkdir nombre_de_la_carpeta`.
- Para volver a la carpeta anterior, usa `cd..` (sin espacio después de cd).

```cmd
# 1. Crear carpeta del proyecto
mkdir taller-programacion-2025

# 2. Navegar a la carpeta del proyecto
cd taller-programacion-2025
```

### 📦 Instalación de Git en Windows

1. **Descargar Git**: Ve al sitio web oficial de Git [git-scm.com](https://git-scm.com/) y descarga la versión para Windows.

2. **Instalar Git**: Ejecuta el instalador descargado y sigue estos pasos:

   - Acepta la licencia
   - Selecciona la ubicación de instalación (usa la predeterminada)
   - Selecciona componentes (deja las opciones predeterminadas)
   - Elige el editor predeterminado (recomendado: VS Code)
   - Selecciona "Git from the command line and also from 3rd-party software"
   - Usa OpenSSL para HTTPS (opción predeterminada)
   - Selecciona "Checkout Windows-style, commit Unix-style line endings"
   - Usa MinTTY como terminal
   - Habilita el caché de credenciales
   - Finaliza la instalación

3. **Verificar la instalación**: Abre una nueva ventana de CMD y ejecuta:

   ```cmd
   git --version
   ```

   Deberías ver la versión de Git instalada (por ejemplo, "git version 2.35.1.windows.1").

4. **Configurar Git**: Es importante configurar tu nombre de usuario y correo electrónico. Ejecuta:

   ```cmd
   git config --global user.name "Tu Nombre"
   git config --global user.email "tu.email@estudiante.aiep.cl"
   ```

5. **Crear un repositorio**: Para crear un nuevo repositorio, navega a la carpeta donde deseas crear el proyecto y ejecuta:

   ```cmd
   git init
   ```

   Esto inicializará un nuevo repositorio Git en esa carpeta.

6. **Crear un archivo README**: En Windows, puedes usar el comando:

   ```cmd
   echo # Mi Taller de Programacion > README.md
   ```

7. **Agregar archivos al repositorio**: Para agregar archivos, usa:

   ```cmd
   git add README.md
   ```

   O para agregar todos los archivos:

   ```cmd
   git add .
   ```

8. **Hacer un commit**: Después de agregar archivos, debes hacer un commit:

   ```cmd
   git commit -m "Primer commit: Configuración inicial"
   ```

9. **Subir cambios a GitHub**: Para subir tus cambios, usa:

   ```cmd
   git push origin main
   ```

### 🌐 GitHub

1. **Crear una cuenta**: Ve a [github.com](https://github.com/) y haz clic en "Sign up" para crear una nueva cuenta.

2. **Crear un nuevo repositorio**: Una vez que hayas iniciado sesión, haz clic en el botón "New" para crear un nuevo repositorio. Dale un nombre y una descripción, y selecciona si deseas que sea público o privado.

3. **Una vez creado el repositorio**, GitHub te indicará una serie de pasos que te permitirán vincular tu repositorio local con el remoto:

   ```cmd
   git branch -M main
   git remote add origin https://github.com/tu-usuario/taller-programacion-2025.git
   git push -u origin main
   ```

### 🎯 Comandos Git Esenciales para Windows

```cmd
# Ver estado actual
git status

# Agregar archivos al staging
git add archivo.txt          # Agregar archivo específico
git add .                    # Agregar todos los archivos

# Hacer commit
git commit -m "Descripción del cambio"

# Ver historial
git log --oneline

# Subir cambios a GitHub
git push

# Descargar cambios desde GitHub
git pull
```

## 📅 Actividades de la Clase

### Instrucciones

1. **Crear repositorio** siguiendo los pasos anteriores.
2. **Agregar archivo README.md** con tu información:

```markdown
# 👋 ¡Hola! Soy [Tu Nombre]

## 🎓 Sobre mí

- **Carrera**: Programación y Análisis de Sistemas
- **Año**: 2025
- **Semestre**: 2° Semestre
- **Sede**: [Tu sede]

## 💻 Experiencia en Programación

[Describe brevemente tu experiencia]

## 🎯 Objetivos en este Módulo

- Dominar POO
- Crear aplicaciones profesionales
- Construir mi portafolio en GitHub

## 🚀 Mi Primera Clase

Hoy aprendí:

- Configuración de Git y GitHub
- Conceptos básicos del módulo
- [Agrega lo que más te llamó la atención]
```

3. **Subir el archivo README.md** a tu repositorio en GitHub.
4. **Verificar** que aparezca en tu perfil de GitHub.

## 🎯 Para la próxima clase:

- **Tema**: Historia y evolución de los paradigmas de programación.
- **Herramientas**: Seguiremos usando Git para versionar nuestro trabajo. Además veremos VS Code, terminal y aprenderemos más sobre Git.

## 📝 Trabajo Autónomo

1. **Completar la configuración de Git y GitHub**.
2. **Personalizar tu perfil de GitHub** (foto, README de perfil).
3. **Explorar repositorios** de otros usuarios para inspirarte.

## 🔗 Enlaces Útiles

- [Git - Documentación Oficial](https://docs.github.com/es/get-started/using-git/about-git)
- [GitHub - Guía de Inicio Rápido](https://docs.github.com/es/get-started/start-your-journey/hello-world)
- [Guía de Markdown](https://www.markdownguide.org/)
- [Git para Windows - Guía de instalación](https://gitforwindows.org/)
