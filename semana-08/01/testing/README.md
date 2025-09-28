# Clase 01 - Semana 08 - Testing

- Unidad 03: Programación de interfaces gráficas
- Fecha: Lunes 29 de septiembre, 2025
- Horario: 10:50 - 13:30
- Docente: Diego Obando

## 🎯 Objetivos de la Clase

- Introducir el concepto de pruebas unitarias y su importancia en el desarrollo de software.
- Aprender a escribir y ejecutar pruebas unitarias en C# utilizando xUnit.
- Entender cómo las pruebas unitarias mejoran la calidad del código y facilitan el mantenimiento.
- Configurar un entorno de desarrollo para pruebas unitarias en Visual Studio Code.
- Familiarizarse con las mejores prácticas para escribir pruebas efectivas y mantenibles.
- Introducir brevemente el concepto de pruebas de integración y su relación con las pruebas unitarias.

---

## 📚 1. ¿Qué son las Pruebas Unitarias?

### 1.1 Definición y Conceptos Fundamentales

Una **prueba unitaria** es un fragmento de código que verifica el comportamiento de una pequeña parte específica del código (generalmente un método o función) de manera aislada.

#### Características principales:

- **Automatizadas**: Se ejecutan sin intervención manual
- **Rápidas**: Deben ejecutarse en milisegundos
- **Independientes**: No dependen de otras pruebas ni de recursos externos
- **Repetibles**: Producen el mismo resultado cada vez que se ejecutan
- **Auto-verificables**: Determinan automáticamente si pasaron o fallaron

### 1.2 Importancia de las Pruebas Unitarias

```csharp
// ❌ Sin pruebas: ¿Cómo sabemos si este método funciona correctamente?
public static string FormatUser(User user)
{
    return $"{user.Name} <{user.Email}>";
}

// ✅ Con pruebas: Verificamos todos los escenarios posibles
[Fact]
public void FormatUser_WithValidUser_ReturnsCorrectFormat()
{
    // Arrange
    var user = new User { Name = "Juan Pérez", Email = "juan@example.com" };

    // Act
    var result = UserFormatter.FormatUser(user);

    // Assert
    Assert.Equal("Juan Pérez <juan@example.com>", result);
}
```

#### Beneficios clave:

1. **Detección temprana de errores**: Encuentran bugs antes de que lleguen a producción
2. **Documentación viva**: Las pruebas documentan cómo debería comportarse el código
3. **Refactoring seguro**: Permiten cambiar código con confianza
4. **Diseño mejorado**: Fuerzan a escribir código más modular y testeable

### 1.3 El patrón AAA (Arrange-Act-Assert)

Todas las pruebas unitarias siguen este patrón:

```csharp
[Fact]
public void ExampleTest()
{
    // ARRANGE: Preparar los datos y el entorno de prueba
    var user = new User { Name = "Ana", Email = "ana@test.com" };
    var expectedFormat = "Ana <ana@test.com>";

    // ACT: Ejecutar la acción que queremos probar
    var result = UserFormatter.FormatUser(user);

    // ASSERT: Verificar que el resultado es el esperado
    Assert.Equal(expectedFormat, result);
}
```

---

## 🛠️ 2. Configurando el Entorno de Testing

### 2.1 Estructura de Proyecto Recomendada

```
MiProyecto/
├── src/
│   └── MiAppWinFormsSimple/
│       ├── Models/
│       │   └── User.cs
│       ├── Services/
│       │   └── UserStorage.cs
│       └── MiAppWinFormsSimple.csproj
└── tests/
    └── MiApp.Tests/
        ├── Models/
        │   └── UserTests.cs
        ├── Services/
        │   └── UserStorageTests.cs
        └── MiApp.Tests.csproj
```

### 2.2 Configuración del Proyecto de Tests

#### Archivo `.csproj` para el proyecto de pruebas:

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net9.0-windows</TargetFramework>
    <IsPackable>false</IsPackable>
    <UseWindowsForms>true</UseWindowsForms>
    <!-- Configurar para que se reconozcan las pruebas -->
    <IsTestProject>true</IsTestProject>
  </PropertyGroup>

  <!-- Paquetes necesarios para testing -->
  <ItemGroup>
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.10.0" />
    <PackageReference Include="xunit" Version="2.5.3" />
    <PackageReference Include="xunit.runner.visualstudio" Version="2.5.3" />
    <PackageReference Include="coverlet.collector" Version="3.2.0" />
    <!-- Para mocking (opcional) -->
    <PackageReference Include="Moq" Version="4.20.69" />
  </ItemGroup>

  <!-- Referencia al proyecto principal -->
  <ItemGroup>
    <ProjectReference Include="..\..\src\MiAppWinFormsSimple\MiAppWinFormsSimple.csproj" />
  </ItemGroup>

</Project>
```

### 2.3 Comandos Útiles en Terminal

```bash
# Crear proyecto de pruebas
dotnet new xunit -n MiApp.Tests

# Agregar referencia al proyecto principal
dotnet add reference ../src/MiAppWinFormsSimple/MiAppWinFormsSimple.csproj

# Ejecutar todas las pruebas
dotnet test

# Ejecutar pruebas con detalles
dotnet test --verbosity normal

# Ejecutar pruebas con cobertura de código
dotnet test --collect:"XPlat Code Coverage"
```

---

## 🔬 3. Escribiendo Pruebas con xUnit

### 3.1 Fundamentos de xUnit

xUnit es el framework de testing más moderno para .NET. Sus principales características:

```csharp
using Xunit;

public class BasicXUnitExamples
{
    // [Fact]: Prueba simple que siempre debe pasar
    [Fact]
    public void SimpleTest_ShouldPass()
    {
        Assert.True(true);
    }

    // [Theory]: Prueba parametrizada con múltiples valores
    [Theory]
    [InlineData(1, 1, 2)]
    [InlineData(2, 3, 5)]
    [InlineData(-1, 1, 0)]
    public void Add_WithDifferentNumbers_ReturnsCorrectSum(int a, int b, int expected)
    {
        var result = a + b;
        Assert.Equal(expected, result);
    }
}
```

### 3.2 Principales Métodos de Assert

```csharp
public class AssertExamples
{
    [Fact]
    public void Assert_Examples()
    {
        // Igualdad
        Assert.Equal("expected", "expected");
        Assert.NotEqual("different", "values");

        // Booleanos
        Assert.True(true);
        Assert.False(false);

        // Nulos
        Assert.Null(null);
        Assert.NotNull("not null");

        // Colecciones
        var list = new List<int> { 1, 2, 3 };
        Assert.Contains(2, list);
        Assert.DoesNotContain(4, list);
        Assert.Equal(3, list.Count);
        Assert.Empty(new List<int>());

        // Excepciones
        Assert.Throws<ArgumentNullException>(() =>
            throw new ArgumentNullException("param"));

        // Strings
        Assert.StartsWith("Hello", "Hello World");
        Assert.EndsWith("World", "Hello World");
        Assert.Contains("lo W", "Hello World");
    }
}
```

---

## 📝 4. Ejemplo Práctico: Testing de UserStorage

### 4.1 El Código a Probar

Primero, necesitamos refactorizar nuestro código para hacerlo más testeable:

```csharp
// Models/User.cs
namespace MiAppWinFormsSimple
{
    public class User
    {
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";

        public override bool Equals(object obj)
        {
            return obj is User user &&
                   Name == user.Name &&
                   Email == user.Email;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Email);
        }
    }
}
```

```csharp
// Services/UserStorage.cs
using System.Text.Json;

namespace MiAppWinFormsSimple
{
    public static class UserStorage
    {
        /// <summary>
        /// Serializa una lista de usuarios a JSON
        /// </summary>
        public static string Serialize(List<User> users)
        {
            if (users == null)
                throw new ArgumentNullException(nameof(users));

            return JsonSerializer.Serialize(users, new JsonSerializerOptions
            {
                WriteIndented = true
            });
        }

        /// <summary>
        /// Deserializa JSON a una lista de usuarios
        /// </summary>
        public static List<User> Deserialize(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return new List<User>();

            return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
        }

        /// <summary>
        /// Guarda usuarios en un archivo
        /// </summary>
        public static void SaveToFile(List<User> users, string filePath)
        {
            if (users == null)
                throw new ArgumentNullException(nameof(users));
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("File path cannot be empty", nameof(filePath));

            var directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var json = Serialize(users);
            File.WriteAllText(filePath, json);
        }

        /// <summary>
        /// Carga usuarios desde un archivo
        /// </summary>
        public static List<User> LoadFromFile(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("File path cannot be empty", nameof(filePath));

            if (!File.Exists(filePath))
                return new List<User>();

            var json = File.ReadAllText(filePath);
            return Deserialize(json);
        }
    }
}
```

### 4.2 Las Pruebas Completas

```csharp
// Tests/UserStorageTests.cs
using System;
using System.Collections.Generic;
using System.IO;
using MiAppWinFormsSimple;
using Xunit;

namespace MiApp.Tests
{
    /// <summary>
    /// Pruebas para la clase UserStorage
    /// Implementa IDisposable para limpiar archivos temporales después de cada prueba
    /// </summary>
    public class UserStorageTests : IDisposable
    {
        private readonly string _tempDir;
        private readonly string _tempFile;

        public UserStorageTests()
        {
            // Crear directorio temporal único para cada ejecución de prueba
            _tempDir = Path.Combine(Path.GetTempPath(), "MiAppTests", Guid.NewGuid().ToString());
            Directory.CreateDirectory(_tempDir);
            _tempFile = Path.Combine(_tempDir, "users.json");
        }

        #region Pruebas de Serialización

        [Fact]
        public void Serialize_WithValidUsers_ReturnsValidJson()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Name = "Ana García", Email = "ana@example.com" },
                new User { Name = "Luis Pérez", Email = "luis@example.com" }
            };

            // Act
            var json = UserStorage.Serialize(users);

            // Assert
            Assert.NotNull(json);
            Assert.NotEmpty(json);
            Assert.Contains("Ana García", json);
            Assert.Contains("ana@example.com", json);
        }

        [Fact]
        public void Serialize_WithEmptyList_ReturnsValidJson()
        {
            // Arrange
            var users = new List<User>();

            // Act
            var json = UserStorage.Serialize(users);

            // Assert
            Assert.Equal("[]", json.Trim());
        }

        [Fact]
        public void Serialize_WithNullList_ThrowsArgumentNullException()
        {
            // Arrange
            List<User> users = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => UserStorage.Serialize(users));
        }

        #endregion

        #region Pruebas de Deserialización

        [Fact]
        public void Deserialize_WithValidJson_ReturnsCorrectUsers()
        {
            // Arrange
            var json = """
                [
                  {
                    "Name": "María López",
                    "Email": "maria@example.com"
                  }
                ]
                """;

            // Act
            var users = UserStorage.Deserialize(json);

            // Assert
            Assert.Single(users);
            Assert.Equal("María López", users[0].Name);
            Assert.Equal("maria@example.com", users[0].Email);
        }

        [Fact]
        public void Deserialize_WithEmptyJson_ReturnsEmptyList()
        {
            // Arrange & Act
            var users = UserStorage.Deserialize("[]");

            // Assert
            Assert.Empty(users);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Deserialize_WithInvalidJson_ReturnsEmptyList(string invalidJson)
        {
            // Act
            var users = UserStorage.Deserialize(invalidJson);

            // Assert
            Assert.Empty(users);
        }

        #endregion

        #region Pruebas de Serialización/Deserialización Round-Trip

        [Fact]
        public void SerializeDeserialize_PreservesDataIntegrity()
        {
            // Arrange
            var originalUsers = new List<User>
            {
                new User { Name = "Carlos Ruiz", Email = "carlos@example.com" },
                new User { Name = "Elena Vásquez", Email = "elena@example.com" },
                new User { Name = "José María de la Cruz y Fernández", Email = "jose.maria@example.com" }
            };

            // Act
            var json = UserStorage.Serialize(originalUsers);
            var deserializedUsers = UserStorage.Deserialize(json);

            // Assert
            Assert.Equal(originalUsers.Count, deserializedUsers.Count);

            for (int i = 0; i < originalUsers.Count; i++)
            {
                Assert.Equal(originalUsers[i].Name, deserializedUsers[i].Name);
                Assert.Equal(originalUsers[i].Email, deserializedUsers[i].Email);
            }
        }

        #endregion

        #region Pruebas de Archivos

        [Fact]
        public void SaveToFile_CreatesFileWithCorrectContent()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Name = "Roberto Silva", Email = "roberto@example.com" }
            };

            // Act
            UserStorage.SaveToFile(users, _tempFile);

            // Assert
            Assert.True(File.Exists(_tempFile));

            var savedContent = File.ReadAllText(_tempFile);
            Assert.Contains("Roberto Silva", savedContent);
            Assert.Contains("roberto@example.com", savedContent);
        }

        [Fact]
        public void SaveToFile_CreatesDirectoryIfNotExists()
        {
            // Arrange
            var users = new List<User> { new User { Name = "Test", Email = "test@example.com" } };
            var nestedPath = Path.Combine(_tempDir, "subdir1", "subdir2", "users.json");

            // Act
            UserStorage.SaveToFile(users, nestedPath);

            // Assert
            Assert.True(File.Exists(nestedPath));
            Assert.True(Directory.Exists(Path.GetDirectoryName(nestedPath)));
        }

        [Fact]
        public void SaveToFile_WithNullUsers_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                UserStorage.SaveToFile(null, _tempFile));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void SaveToFile_WithInvalidPath_ThrowsArgumentException(string invalidPath)
        {
            // Arrange
            var users = new List<User>();

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                UserStorage.SaveToFile(users, invalidPath));
        }

        [Fact]
        public void LoadFromFile_WithExistingFile_ReturnsCorrectUsers()
        {
            // Arrange
            var originalUsers = new List<User>
            {
                new User { Name = "Andrea Torres", Email = "andrea@example.com" },
                new User { Name = "Sebastián Morales", Email = "sebastian@example.com" }
            };

            UserStorage.SaveToFile(originalUsers, _tempFile);

            // Act
            var loadedUsers = UserStorage.LoadFromFile(_tempFile);

            // Assert
            Assert.Equal(2, loadedUsers.Count);
            Assert.Contains(loadedUsers, u => u.Name == "Andrea Torres" && u.Email == "andrea@example.com");
            Assert.Contains(loadedUsers, u => u.Name == "Sebastián Morales" && u.Email == "sebastian@example.com");
        }

        [Fact]
        public void LoadFromFile_WithNonExistentFile_ReturnsEmptyList()
        {
            // Arrange
            var nonExistentFile = Path.Combine(_tempDir, "nonexistent.json");

            // Act
            var users = UserStorage.LoadFromFile(nonExistentFile);

            // Assert
            Assert.Empty(users);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void LoadFromFile_WithInvalidPath_ThrowsArgumentException(string invalidPath)
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                UserStorage.LoadFromFile(invalidPath));
        }

        #endregion

        #region Pruebas de Integración Completa

        [Fact]
        public void SaveAndLoadFile_CompleteWorkflow_PreservesAllData()
        {
            // Arrange
            var originalUsers = new List<User>
            {
                new User { Name = "Patricia González", Email = "patricia@example.com" },
                new User { Name = "Fernando Ramírez", Email = "fernando@example.com" },
                new User { Name = "Claudia Herrera", Email = "claudia@example.com" }
            };

            // Act - Save
            UserStorage.SaveToFile(originalUsers, _tempFile);

            // Act - Load
            var loadedUsers = UserStorage.LoadFromFile(_tempFile);

            // Assert
            Assert.Equal(originalUsers.Count, loadedUsers.Count);

            // Verificar que cada usuario se preservó correctamente
            foreach (var originalUser in originalUsers)
            {
                Assert.Contains(loadedUsers, u =>
                    u.Name == originalUser.Name &&
                    u.Email == originalUser.Email);
            }
        }

        #endregion

        /// <summary>
        /// Limpia los archivos temporales después de cada prueba
        /// </summary>
        public void Dispose()
        {
            try
            {
                if (Directory.Exists(_tempDir))
                {
                    Directory.Delete(_tempDir, true);
                }
            }
            catch
            {
                // Ignorar errores de limpieza
            }
        }
    }
}
```

---

## 🏆 5. Mejores Prácticas para Testing

### 5.1 Nomenclatura de Pruebas

Usa nombres descriptivos que expliquen qué estás probando:

```csharp
// ❌ Mal: Nombres poco descriptivos
[Fact]
public void Test1() { }

[Fact]
public void UserTest() { }

// ✅ Bien: Nombres que explican el escenario
[Fact]
public void Serialize_WithValidUsers_ReturnsValidJson() { }

[Fact]
public void LoadFromFile_WithNonExistentFile_ReturnsEmptyList() { }

[Fact]
public void SaveToFile_WithNullUsers_ThrowsArgumentNullException() { }
```

### 5.2 Patrón AAA Bien Implementado

```csharp
[Fact]
public void WellStructuredTest()
{
    // ARRANGE: Una sola responsabilidad, datos claros
    var user = new User { Name = "Test User", Email = "test@example.com" };
    var expectedResult = "Test User <test@example.com>";

    // ACT: Una sola acción
    var result = UserFormatter.FormatUser(user);

    // ASSERT: Verificación específica y clara
    Assert.Equal(expectedResult, result);
}
```

### 5.3 Una Prueba, Una Responsabilidad

```csharp
// ❌ Mal: Probando múltiples cosas en una prueba
[Fact]
public void BadTest_TestingTooManyThings()
{
    var users = new List<User>();
    var json = UserStorage.Serialize(users); // Testing serialización
    var loaded = UserStorage.Deserialize(json); // Testing deserialización
    UserStorage.SaveToFile(loaded, "test.json"); // Testing guardado
    Assert.True(File.Exists("test.json")); // Testing existencia de archivo
}

// ✅ Bien: Una prueba por responsabilidad
[Fact]
public void Serialize_WithEmptyList_ReturnsEmptyJsonArray()
{
    var users = new List<User>();
    var json = UserStorage.Serialize(users);
    Assert.Equal("[]", json.Trim());
}

[Fact]
public void SaveToFile_WithValidData_CreatesFile()
{
    var users = new List<User>();
    UserStorage.SaveToFile(users, _tempFile);
    Assert.True(File.Exists(_tempFile));
}
```

### 5.4 Manejo de Recursos (IDisposable)

```csharp
public class ResourceManagementTests : IDisposable
{
    private readonly string _testDirectory;

    public ResourceManagementTests()
    {
        // Setup: Crear recursos para cada prueba
        _testDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(_testDirectory);
    }

    [Fact]
    public void SomeTest()
    {
        // La prueba usa _testDirectory
        var testFile = Path.Combine(_testDirectory, "test.txt");
        File.WriteAllText(testFile, "test content");
        Assert.True(File.Exists(testFile));
    }

    public void Dispose()
    {
        // Cleanup: Limpiar después de cada prueba
        try
        {
            if (Directory.Exists(_testDirectory))
                Directory.Delete(_testDirectory, true);
        }
        catch { }
    }
}
```

---

## 🚀 **Proyecto para casa**:

- Implementar pruebas para toda la lógica de negocio de tu proyecto WinForms
- Investigar sobre Code Coverage y configurar reportes
- Practicar TDD con una nueva funcionalidad

---

## 🔧 10. Herramientas Adicionales y Extensiones

### 10.1 Extensiones Útiles para VS Code

```json
// settings.json - Configuraciones recomendadas para testing
{
  "dotnet.unitTests.runSettingsPath": "./test.runsettings",
  "files.exclude": {
    "**/bin": true,
    "**/obj": true,
    "**/.vs": true
  },
  "dotnet.completion.showCompletionItemsFromUnimportedNamespaces": true,
  "omnisharp.enableEditorConfigSupport": true,
  "omnisharp.enableRoslynAnalyzers": true
}
```

**Extensiones recomendadas:**

- **C# Dev Kit**: Testing integrado y debugging
- **Coverage Gutters**: Visualización de cobertura de código
- **Test Explorer UI**: Interfaz gráfica para pruebas
- **Live Unit Testing**: Ejecución automática de pruebas

### 10.2 Configuración Avanzada de Pruebas

#### Archivo `test.runsettings`:

```xml
<?xml version="1.0" encoding="utf-8"?>
<RunSettings>
  <DataCollectionRunSettings>
    <DataCollectors>
      <DataCollector friendlyName="XPlat code coverage">
        <Configuration>
          <Format>cobertura,opencover</Format>
          <Exclude>[*.Tests]*</Exclude>
          <IncludeTestAssembly>false</IncludeTestAssembly>
        </Configuration>
      </DataCollector>
    </DataCollectors>
  </DataCollectionRunSettings>

  <RunConfiguration>
    <MaxCpuCount>0</MaxCpuCount>
    <ResultsDirectory>.\TestResults</ResultsDirectory>
  </RunConfiguration>
</RunSettings>
```

### 10.3 Scripts Útiles

#### `run-tests.ps1`:

```powershell
# Script PowerShell para ejecutar pruebas con reportes
Write-Host "🧪 Ejecutando pruebas unitarias..." -ForegroundColor Green

# Limpiar resultados anteriores
Remove-Item -Path "TestResults" -Recurse -Force -ErrorAction SilentlyContinue

# Ejecutar pruebas con cobertura
dotnet test --collect:"XPlat Code Coverage" --results-directory:"TestResults" --verbosity:normal

# Generar reporte HTML de cobertura (requiere reportgenerator)
$coverageFile = Get-ChildItem -Path "TestResults" -Filter "coverage.cobertura.xml" -Recurse | Select-Object -First 1

if ($coverageFile) {
    Write-Host "📊 Generando reporte de cobertura..." -ForegroundColor Yellow
    reportgenerator "-reports:$($coverageFile.FullName)" "-targetdir:TestResults/CoverageReport" "-reporttypes:Html"

    # Abrir reporte en navegador
    Start-Process "TestResults/CoverageReport/index.html"
} else {
    Write-Host "⚠️ No se encontró archivo de cobertura" -ForegroundColor Yellow
}
```

---

## 🧪 11. Técnicas Avanzadas de Testing

### 11.1 Mocking con Moq

Para casos donde necesitamos aislar dependencias:

```csharp
// Interfaz que podemos mockear
public interface IFileService
{
    string ReadAllText(string path);
    void WriteAllText(string path, string content);
    bool Exists(string path);
}

// Clase que depende de IFileService
public class UserRepository
{
    private readonly IFileService _fileService;

    public UserRepository(IFileService fileService)
    {
        _fileService = fileService;
    }

    public List<User> LoadUsers(string filePath)
    {
        if (!_fileService.Exists(filePath))
            return new List<User>();

        var json = _fileService.ReadAllText(filePath);
        return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
    }
}

// Prueba usando Moq
[Fact]
public void LoadUsers_WithExistingFile_ReturnsUsers()
{
    // Arrange
    var mockFileService = new Mock<IFileService>();
    var testJson = """[{"Name":"Test","Email":"test@example.com"}]""";

    mockFileService.Setup(x => x.Exists("test.json")).Returns(true);
    mockFileService.Setup(x => x.ReadAllText("test.json")).Returns(testJson);

    var repository = new UserRepository(mockFileService.Object);

    // Act
    var users = repository.LoadUsers("test.json");

    // Assert
    Assert.Single(users);
    Assert.Equal("Test", users[0].Name);

    // Verificar que se llamaron los métodos esperados
    mockFileService.Verify(x => x.Exists("test.json"), Times.Once);
    mockFileService.Verify(x => x.ReadAllText("test.json"), Times.Once);
}
```

### 11.2 Pruebas Parametrizadas Avanzadas

```csharp
public class EmailValidationTests
{
    public static IEnumerable<object[]> ValidEmails()
    {
        yield return new object[] { "test@example.com", true };
        yield return new object[] { "user.name@domain.co.uk", true };
        yield return new object[] { "firstname.lastname@subdomain.domain.com", true };
        yield return new object[] { "email@domain-name.com", true };
    }

    public static IEnumerable<object[]> InvalidEmails()
    {
        yield return new object[] { "", false };
        yield return new object[] { "   ", false };
        yield return new object[] { "invalid-email", false };
        yield return new object[] { "@domain.com", false };
        yield return new object[] { "email@", false };
        yield return new object[] { "email.domain.com", false };
    }

    [Theory]
    [MemberData(nameof(ValidEmails))]
    public void IsValidEmail_WithValidFormats_ReturnsTrue(string email, bool expected)
    {
        var result = UserValidator.IsValidEmail(email);
        Assert.Equal(expected, result);
    }

    [Theory]
    [MemberData(nameof(InvalidEmails))]
    public void IsValidEmail_WithInvalidFormats_ReturnsFalse(string email, bool expected)
    {
        var result = UserValidator.IsValidEmail(email);
        Assert.Equal(expected, result);
    }
}
```

### 11.3 Testing de Excepciones

```csharp
[Fact]
public void ProcessUser_WithInvalidData_ThrowsSpecificException()
{
    // Arrange
    var processor = new UserProcessor();

    // Act & Assert - Verificar tipo específico de excepción
    var exception = Assert.Throws<ArgumentException>(() =>
        processor.ProcessUser("", "invalid-email"));

    // Verificar mensaje de la excepción
    Assert.Contains("Invalid user data", exception.Message);
    Assert.Equal("userData", exception.ParamName);
}

[Fact]
public async Task AsyncMethod_WithError_ThrowsCorrectException()
{
    // Arrange
    var service = new UserService();

    // Act & Assert - Para métodos async
    var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
        service.SaveUserAsync(null));

    Assert.Contains("User cannot be null", exception.Message);
}
```

---

## 📈 12. Cobertura de Código (Code Coverage)

### 12.1 ¿Qué es la Cobertura de Código?

La cobertura de código mide qué porcentaje del código fuente es ejecutado por las pruebas.

**Tipos de cobertura:**

- **Líneas**: % de líneas ejecutadas
- **Ramas**: % de condiciones (if, switch) evaluadas
- **Métodos**: % de métodos llamados

### 12.2 Configurando Cobertura

```bash
# Instalar herramienta de reportes (una sola vez)
dotnet tool install -g dotnet-reportgenerator-globaltool

# Ejecutar pruebas con cobertura
dotnet test --collect:"XPlat Code Coverage" --settings test.runsettings

# Generar reporte HTML
reportgenerator -reports:"TestResults/**/coverage.cobertura.xml" -targetdir:"TestResults/html" -reporttypes:Html

# Ver reporte
start TestResults/html/index.html  # Windows
open TestResults/html/index.html   # macOS
```

### 12.3 Interpretando Métricas de Cobertura

```
Class Coverage Summary:
┌─────────────────────────┬─────────┬─────────┬─────────┐
│ Class                   │ Line    │ Branch  │ Method  │
├─────────────────────────┼─────────┼─────────┼─────────┤
│ UserStorage             │ 95.2%   │ 87.5%   │ 100%    │
│ UserValidator           │ 88.9%   │ 75.0%   │ 100%    │
│ User                    │ 100%    │ N/A     │ 100%    │
└─────────────────────────┴─────────┴─────────┴─────────┘
Total Coverage: 92.1%
```

**¿Qué buscar?**

- **85%+ líneas**: Buena cobertura general
- **80%+ ramas**: Casos edge cubiertos
- **100% métodos críticos**: Lógica importante probada

**⚠️ Advertencia**: 100% cobertura ≠ 100% calidad. Es mejor tener 70% de cobertura con pruebas de calidad que 100% con pruebas superficiales.

---

## 🚨 13. Debugging de Pruebas

### 13.1 Debugging en VS Code

1. **Punto de interrupción**: Clic en el margen izquierdo
2. **Ejecutar con debug**: `Ctrl+F5` o usar el panel de debugging
3. **Inspeccionar variables**: Pasar el mouse sobre variables
4. **Evaluar expresiones**: En la consola de debug

### 13.2 Técnicas de Debugging

```csharp
[Fact]
public void DebuggingExample()
{
    // Arrange
    var user = new User { Name = "Debug Test", Email = "debug@test.com" };

    // Para debugging: usar System.Diagnostics.Debugger
    if (System.Diagnostics.Debugger.IsAttached)
    {
        System.Diagnostics.Debugger.Break(); // Pausa automática en debug
    }

    // Act
    var result = UserStorage.Serialize(new List<User> { user });

    // Debug output para ver valores
    System.Diagnostics.Debug.WriteLine($"Serialized result: {result}");

    // Assert
    Assert.Contains("Debug Test", result);
}
```

### 13.3 Output de Debugging

```csharp
using Xunit.Abstractions;

public class DebuggableTests
{
    private readonly ITestOutputHelper _output;

    public DebuggableTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void TestWithOutput()
    {
        var user = new User { Name = "Test", Email = "test@example.com" };

        _output.WriteLine($"Testing user: {user.Name}");
        _output.WriteLine($"Email: {user.Email}");

        var json = UserStorage.Serialize(new List<User> { user });
        _output.WriteLine($"Generated JSON: {json}");

        Assert.NotEmpty(json);
    }
}
```

---

## 🎯 14. Casos de Uso Reales en WinForms

### 14.1 Testing de Validación en Formularios

```csharp
public class FormValidationTests
{
    [Theory]
    [InlineData("", "test@email.com", false)] // Nombre vacío
    [InlineData("Test User", "", false)]      // Email vacío
    [InlineData("Test User", "invalid-email", false)] // Email inválido
    [InlineData("Test User", "test@email.com", true)]  // Datos válidos
    public void ValidateUserInput_WithVariousInputs_ReturnsExpectedResult(
        string name, string email, bool expected)
    {
        // Act
        var result = FormValidator.IsValidUserInput(name, email);

        // Assert
        Assert.Equal(expected, result);
    }
}
```

### 14.2 Testing de Lógica de Negocio Separada de UI

```csharp
// Separar lógica de UI para mejor testabilidad
public class UserManagementService
{
    private readonly IUserRepository _repository;

    public UserManagementService(IUserRepository repository)
    {
        _repository = repository;
    }

    public ValidationResult AddUser(string name, string email)
    {
        // Validación
        if (string.IsNullOrWhiteSpace(name))
            return ValidationResult.Error("Name is required");

        if (!IsValidEmail(email))
            return ValidationResult.Error("Invalid email format");

        // Verificar duplicados
        var existingUsers = _repository.GetAll();
        if (existingUsers.Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)))
            return ValidationResult.Error("Email already exists");

        // Agregar usuario
        var user = new User { Name = name, Email = email };
        _repository.Add(user);

        return ValidationResult.Success($"User {name} added successfully");
    }
}

// Pruebas para el servicio
public class UserManagementServiceTests
{
    [Fact]
    public void AddUser_WithValidData_ReturnsSuccessResult()
    {
        // Arrange
        var mockRepo = new Mock<IUserRepository>();
        mockRepo.Setup(x => x.GetAll()).Returns(new List<User>());

        var service = new UserManagementService(mockRepo.Object);

        // Act
        var result = service.AddUser("John Doe", "john@example.com");

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Contains("John Doe added successfully", result.Message);
        mockRepo.Verify(x => x.Add(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public void AddUser_WithDuplicateEmail_ReturnsErrorResult()
    {
        // Arrange
        var existingUsers = new List<User>
        {
            new User { Name = "Jane", Email = "john@example.com" }
        };

        var mockRepo = new Mock<IUserRepository>();
        mockRepo.Setup(x => x.GetAll()).Returns(existingUsers);

        var service = new UserManagementService(mockRepo.Object);

        // Act
        var result = service.AddUser("John Doe", "john@example.com");

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("Email already exists", result.Message);
        mockRepo.Verify(x => x.Add(It.IsAny<User>()), Times.Never);
    }
}
```

### 14.3 Testing de Persistencia con Archivos

```csharp
public class FileBasedUserRepositoryTests : IDisposable
{
    private readonly string _testDirectory;
    private readonly FileBasedUserRepository _repository;

    public FileBasedUserRepositoryTests()
    {
        _testDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(_testDirectory);

        var testFilePath = Path.Combine(_testDirectory, "users.json");
        _repository = new FileBasedUserRepository(testFilePath);
    }

    [Fact]
    public void Add_NewUser_SavesCorrectly()
    {
        // Arrange
        var user = new User { Name = "Test User", Email = "test@example.com" };

        // Act
        _repository.Add(user);

        // Assert
        var allUsers = _repository.GetAll();
        Assert.Single(allUsers);
        Assert.Equal("Test User", allUsers[0].Name);
    }

    [Fact]
    public void GetAll_AfterMultipleAdds_ReturnsAllUsers()
    {
        // Arrange
        var users = new[]
        {
            new User { Name = "User 1", Email = "user1@example.com" },
            new User { Name = "User 2", Email = "user2@example.com" },
            new User { Name = "User 3", Email = "user3@example.com" }
        };

        // Act
        foreach (var user in users)
            _repository.Add(user);

        var result = _repository.GetAll();

        // Assert
        Assert.Equal(3, result.Count);
        Assert.All(users, user =>
            Assert.Contains(result, r => r.Name == user.Name && r.Email == user.Email));
    }

    public void Dispose()
    {
        try
        {
            if (Directory.Exists(_testDirectory))
                Directory.Delete(_testDirectory, true);
        }
        catch { }
    }
}
```

---

## 🏁 15. Conclusiones y Recursos Adicionales

### 15.1 Puntos Clave para Recordar

✅ **Testing es una inversión, no un costo**:

- Las pruebas previenen bugs costosos
- Facilitan refactoring y mantenimiento
- Documentan el comportamiento esperado

✅ **Calidad sobre cantidad**:

- Mejor pocas pruebas bien escritas que muchas superficiales
- Focus en lógica crítica de negocio
- Prueba casos edge y escenarios de error

✅ **Testabilidad guía buen diseño**:

- Código testeable es código modular
- Separar lógica de negocio de UI
- Usar dependency injection cuando sea necesario

### 15.2 Recursos para Profundizar

#### 📚 Libros Recomendados:

- **"The Art of Unit Testing" by Roy Osherove**
- **"Test Driven Development: By Example" by Kent Beck**
- **"Working Effectively with Legacy Code" by Michael Feathers**

#### 🌐 Recursos Online:

- [xUnit.net Documentation](https://xunit.net/)
- [Microsoft Testing Guide](https://docs.microsoft.com/en-us/dotnet/core/testing/)
- [Moq Documentation](https://github.com/moq/moq4)
- [FluentAssertions](https://fluentassertions.com/) - Alternativa más legible a Assert

#### 🎥 Videos y Cursos:

- Pluralsight: "Unit Testing in C#"
- YouTube: "Clean Code: Unit Testing" by Uncle Bob
- Microsoft Learn: "Test your .NET apps"

### 15.3 Checklist para Evaluación

Usa este checklist para evaluar la calidad de tus pruebas:

```
□ ¿Los nombres de las pruebas explican claramente qué se está probando?
□ ¿Cada prueba tiene una sola responsabilidad?
□ ¿Las pruebas siguen el patrón AAA (Arrange-Act-Assert)?
□ ¿Las pruebas son independientes entre sí?
□ ¿Se manejan correctamente los recursos (IDisposable)?
□ ¿Se prueban los casos edge y escenarios de error?
□ ¿La cobertura de código es razonable (>80% para lógica crítica)?
□ ¿Las pruebas se ejecutan rápidamente (<1 segundo cada una)?
□ ¿El código de producción es testeable (sin dependencias hard-coded)?
```

### 15.4 Próximos Temas Sugeridos

Para continuar tu aprendizaje en testing:

🔄 **Integration Testing**: Pruebas que involucran múltiples componentes
🎭 **Mocking y Test Doubles**: Simular dependencias externas
🏗️ **Test-Driven Development (TDD)**: Red-Green-Refactor cycle
📊 **Mutation Testing**: Verificar la calidad de las pruebas
🚀 **Performance Testing**: Probar rendimiento y carga
🔒 **Security Testing**: Probar vulnerabilidades de seguridad

---

## 📝 Ejercicios para Entregar

### Ejercicio 1: Implementar UserValidator con Pruebas

**Objetivo**: Practicar TDD y pruebas parametrizadas
**Deadline**: Próxima clase
**Entregables**: Código fuente y archivo de pruebas

### Ejercicio 2: Refactoring para Testabilidad

**Objetivo**: Mejorar la testabilidad del código existente
**Deadline**: Fin de semana
**Entregables**: Código refactorizado con suite completa de pruebas

### Ejercicio 3: Proyecto de Cobertura

**Objetivo**: Alcanzar 90%+ cobertura en tu proyecto WinForms
**Deadline**: Próxima semana
**Entregables**: Reporte de cobertura y análisis de métricas

---

_¿Preguntas? ¡No dudes en consultarme durante o después de la clase!_

**Contacto**: diego.obando@universidad.edu  
**Office Hours**: Martes y Jueves 2:00-4:00 PM 6. Ejecutando las Pruebas

### 6.1 En Visual Studio Code

1. **Instalar la extensión C# Dev Kit**
2. **Usar la paleta de comandos** (`Ctrl+Shift+P`):
   - `.NET: Test All` - Ejecutar todas las pruebas
   - `.NET: Test Current` - Ejecutar prueba actual

### 6.2 Desde Terminal

```bash
# Ejecutar todas las pruebas
dotnet test

# Ejecutar con más detalles
dotnet test --verbosity normal

# Ejecutar solo pruebas que contengan "Serialize"
dotnet test --filter "DisplayName~Serialize"

# Ejecutar pruebas de una clase específica
dotnet test --filter "ClassName~UserStorageTests"

# Generar reporte de cobertura
dotnet test --collect:"XPlat Code Coverage"
```

### 6.3 Interpretando los Resultados

```
Determining projects to restore...
  All projects are up-to-date for restore.
  MiApp.Tests -> /path/to/bin/Debug/net9.0-windows/MiApp.Tests.dll
Test run for /path/to/bin/Debug/net9.0-windows/MiApp.Tests.dll (.NETCoreApp,Version=v9.0)
Microsoft (R) Test Execution Command Line Tool Version 17.8.0 (x64)

Starting test execution, please wait...
A total of 1 test files matched the specified pattern.

Passed!  - Failed:     0, Passed:    15, Skipped:     0, Total:    15, Duration: 45 ms
```

---

## 🔄 7. Pruebas de Integración vs Unitarias

### 7.1 Diferencias Clave

| Aspecto          | Pruebas Unitarias | Pruebas de Integración  |
| ---------------- | ----------------- | ----------------------- |
| **Alcance**      | Un método/clase   | Múltiples componentes   |
| **Velocidad**    | Muy rápidas (ms)  | Más lentas (segundos)   |
| **Dependencias** | Aisladas (mocks)  | Reales (BD, archivos)   |
| **Propósito**    | Verificar lógica  | Verificar interacciones |

### 7.2 Ejemplo de Prueba de Integración

```csharp
[Fact]
public void ApplicationWorkflow_SaveAndReloadUsers_WorksEndToEnd()
{
    // Esta sería una prueba de integración más completa
    // que involucra el formulario, el storage y el sistema de archivos

    // Arrange
    var form = new FrmMain();
    var testUsers = new List<User>
    {
        new User { Name = "Integration Test User", Email = "integration@test.com" }
    };

    // Act - Simular el flujo completo de la aplicación
    // 1. Agregar usuarios al formulario
    // 2. Guardar usando UserStorage
    // 3. Crear nueva instancia del formulario
    // 4. Verificar que los datos se cargaron correctamente

    // Assert
    // Verificar que el flujo completo funciona
}
```

---

## 📋 8. Actividades Prácticas

### 8.1 Ejercicio 1: Escribir Pruebas Básicas

Escribe pruebas para esta clase:

```csharp
public static class UserValidator
{
    public static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    public static bool IsValidName(string name)
    {
        return !string.IsNullOrWhiteSpace(name) &&
               name.Length >= 2 &&
               name.Length <= 100;
    }
}
```

**Pruebas requeridas:**

- Emails válidos e inválidos
- Nombres válidos e inválidos
- Casos edge (null, empty, whitespace)

### 8.2 Ejercicio 2: Refactoring para Testabilidad

Refactoriza este código para hacerlo más testeable:

```csharp
// Código actual (difícil de testear)
public class UserManager
{
    public void ProcessUser(string name, string email)
    {
        // Lógica mezclada con dependencies externas
        if (IsValid(name, email))
        {
            var user = new User { Name = name, Email = email };
            File.WriteAllText("users.json", JsonSerializer.Serialize(new[] { user }));
            MessageBox.Show("Usuario agregado");
        }
        else
        {
            MessageBox.Show("Datos inválidos");
        }
    }
}
```

### 8.3 Ejercicio 3: TDD (Test-Driven Development)

Implementa una clase `UserFormatter` siguiendo TDD:

1. **Red**: Escribe una prueba que falle
2. **Green**: Escribe el mínimo código para que pase
3. **Refactor**: Mejora el código manteniendo las pruebas verdes

```csharp
// Requisitos para UserFormatter:
// - FormatUser(User user) -> "Name <email>"
// - FormatUsers(List<User> users) -> "Name1 <email1>, Name2 <email2>"
// - ParseUser(string formatted) -> User object
// - Manejar casos null y empty correctamente
```

---

## 🎓 9. Resumen y Próximos Pasos

### 9.1 Lo que Aprendimos Hoy

✅ **Conceptos fundamentales**:

- Qué son las pruebas unitarias y por qué son importantes
- Patrón AAA (Arrange-Act-Assert)
- Framework xUnit y sus principales características

✅ **Configuración práctica**:

- Estructura de proyecto con tests
- Configuración de .csproj para testing
- Comandos básicos de dotnet test

✅ **Implementación real**:

- Refactoring de código para testabilidad
- Escritura de pruebas completas con xUnit
- Manejo de recursos y archivos temporales

✅ **Mejores prácticas**:

- Nomenclatura clara y descriptiva
- Una responsabilidad por prueba
- Manejo correcto de setup/cleanup

### 9.2 Para la Próxima Clase

📚 **Preparación**:

- Completar los ejercicios prácticos
- Experimentar con más métodos de Assert
- Leer sobre mocking y test doubles

🚀
