using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

namespace MiAppWinForms
{
    /// <summary>
    /// ==================================================================================
    /// FORMULARIO PRINCIPAL DE LA APLICACIÓN
    /// ==================================================================================
    /// 
    /// Esta clase representa una versión SIMPLIFICADA de una aplicación WinForms para
    /// gestión de usuarios. Es perfecta para aprender los conceptos fundamentales:
    /// 
    /// CARACTERÍSTICAS PRINCIPALES:
    /// - Interfaz simple con posicionamiento absoluto (sin layouts complejos)
    /// - Validación básica de datos
    /// - Persistencia en archivo JSON
    /// - Manejo de eventos básico
    /// - Código directo y fácil de entender
    /// 
    /// DIFERENCIAS CON VERSIÓN AVANZADA:
    /// - No usa controles personalizados
    /// - Posicionamiento manual (coordenadas fijas)
    /// - Estilos estándar de Windows
    /// - Menos código pero misma funcionalidad
    /// </summary>
    public class FrmMain : Form
    {
        #region DECLARACIÓN DE CONTROLES
        // ==================================================================================
        // CONTROLES DE LA INTERFAZ DE USUARIO
        // ==================================================================================

        /// <summary>
        /// CAMPOS DE ENTRADA DE DATOS
        /// Estos TextBox son los controles estándar de Windows Forms para entrada de texto.
        /// Se declaran a nivel de clase para poder accederlos desde cualquier método.
        /// </summary>
        private TextBox txtName;        // Campo para ingresar el nombre del usuario
        private TextBox txtEmail;       // Campo para ingresar el email del usuario

        /// <summary>
        /// CONTROLES DE ACCIÓN E INFORMACIÓN
        /// </summary>
        private Button btnSubmit;       // Botón para agregar el usuario a la lista
        private ListBox lstUsers;       // Lista que muestra todos los usuarios registrados

        /// <summary>
        /// CONFIGURACIÓN DE PERSISTENCIA
        /// Constante que define dónde se guardarán los datos de los usuarios.
        /// </summary>
        private const string DataFile = "data/users.json";
        #endregion

        #region CONSTRUCTOR Y CONFIGURACIÓN INICIAL
        // ==================================================================================
        // INICIALIZACIÓN DEL FORMULARIO
        // ==================================================================================

        /// <summary>
        /// CONSTRUCTOR PRINCIPAL
        /// 
        /// Aquí se construye toda la interfaz de usuario y se configura el formulario.
        /// Este enfoque se llama "programación imperativa" - definimos paso a paso
        /// qué queremos que ocurra, sin usar diseñadores visuales.
        /// 
        /// VENTAJAS DE ESTE ENFOQUE:
        /// - Control total sobre la UI
        /// - Código versionable (no hay archivos .designer ocultos)
        /// - Fácil de entender y modificar
        /// - Rápido de escribir para UIs simples
        /// </summary>
        public FrmMain()
        {
            // PASO 1: CONFIGURACIÓN BÁSICA DEL FORMULARIO
            // ==================================================================================

            /// <summary>
            /// PROPIEDADES BÁSICAS DE LA VENTANA
            /// Estas propiedades definen cómo se ve y comporta la ventana principal
            /// </summary>
            Text = "Registro de Usuarios";      // Título en la barra de título
            Width = 420;                        // Ancho de la ventana en píxeles
            Height = 380;                       // Alto de la ventana en píxeles

            // PASO 2: CREACIÓN DE ETIQUETAS (LABELS)
            // ==================================================================================

            /// <summary>
            /// ETIQUETA PARA CAMPO NOMBRE
            /// Los Labels son controles de solo lectura que muestran texto descriptivo.
            /// Patrón: Text (qué dice) + Left/Top (dónde está) + Width (qué tan ancho)
            /// </summary>
            var lbl1 = new Label
            {
                Text = "Nombre:",           // Texto que muestra
                Left = 10,                  // Posición X (desde el borde izquierdo)
                Top = 15,                   // Posición Y (desde el borde superior)
                Width = 60                  // Ancho en píxeles
            };

            /// <summary>
            /// CAMPO DE TEXTO PARA NOMBRE
            /// TextBox estándar de Windows Forms. La posición se calcula para quedar
            /// alineado con la etiqueta pero más a la derecha.
            /// </summary>
            txtName = new TextBox
            {
                Left = 80,                  // 80px desde la izquierda (después de la etiqueta)
                Top = 12,                   // 12px desde arriba (3px más arriba para alineación visual)
                Width = 300                 // Ancho suficiente para nombres largos
            };

            /// <summary>
            /// ETIQUETA PARA CAMPO EMAIL
            /// Misma estructura que la etiqueta anterior, pero 30px más abajo
            /// </summary>
            var lbl2 = new Label
            {
                Text = "Email:",
                Left = 10,                  // Misma posición X que la etiqueta anterior
                Top = 45,                   // 30px más abajo que la etiqueta anterior
                Width = 60                  // Mismo ancho para consistencia visual
            };

            /// <summary>
            /// CAMPO DE TEXTO PARA EMAIL
            /// Mismo patrón que el campo de nombre, pero posicionado debajo
            /// </summary>
            txtEmail = new TextBox
            {
                Left = 80,                  // Alineado con el campo de nombre
                Top = 42,                   // Alineado con la etiqueta de email
                Width = 300                 // Mismo ancho que el campo de nombre
            };

            // PASO 3: CREACIÓN DEL BOTÓN DE ACCIÓN
            // ==================================================================================

            /// <summary>
            /// BOTÓN PARA AGREGAR USUARIO
            /// Button estándar con evento asociado para procesar los datos ingresados
            /// </summary>
            btnSubmit = new Button
            {
                Text = "Agregar",           // Texto del botón
                Left = 80,                  // Alineado con los campos de texto
                Top = 75,                   // 33px debajo del campo de email
                Width = 100                 // Ancho suficiente para el texto
            };

            /// <summary>
            /// ASIGNACIÓN DE EVENTO
            /// Conectamos el evento Click del botón con nuestro método manejador.
            /// Cuando el usuario haga clic, se ejecutará BtnSubmit_Click.
            /// </summary>
            btnSubmit.Click += BtnSubmit_Click;

            // PASO 4: CREACIÓN DE LA LISTA DE USUARIOS
            // ==================================================================================

            /// <summary>
            /// LISTA PARA MOSTRAR USUARIOS REGISTRADOS
            /// ListBox estándar que ocupará la mayor parte del espacio restante de la ventana
            /// </summary>
            lstUsers = new ListBox
            {
                Left = 10,                  // Alineado con las etiquetas
                Top = 110,                  // Debajo del botón con espacio suficiente
                Width = 370,                // Casi todo el ancho de la ventana
                Height = 210                // Altura suficiente para ver varios usuarios
            };

            // PASO 5: AGREGAR TODOS LOS CONTROLES AL FORMULARIO
            // ==================================================================================

            /// <summary>
            /// REGISTRO DE CONTROLES EN EL FORMULARIO
            /// AddRange permite agregar múltiples controles de una vez.
            /// El orden no importa para la funcionalidad, pero sí para el orden de tabulación.
            /// </summary>
            Controls.AddRange(new Control[]
            {
                lbl1, txtName,              // Etiqueta y campo de nombre
                lbl2, txtEmail,             // Etiqueta y campo de email
                btnSubmit,                  // Botón de agregar
                lstUsers                    // Lista de usuarios
            });

            // PASO 6: CARGAR DATOS EXISTENTES
            // ==================================================================================

            /// <summary>
            /// CARGA INICIAL DE DATOS
            /// Al final del constructor, intentamos cargar usuarios que puedan
            /// existir de ejecuciones anteriores de la aplicación.
            /// </summary>
            LoadUsers();
        }
        #endregion

        #region MANEJO DE EVENTOS
        // ==================================================================================
        // LÓGICA DE EVENTOS DEL USUARIO
        // ==================================================================================

        /// <summary>
        /// EVENTO: CLICK EN BOTÓN "AGREGAR"
        /// 
        /// Este método se ejecuta cada vez que el usuario hace clic en el botón Agregar.
        /// Implementa el flujo completo de procesamiento de un nuevo usuario:
        /// 
        /// FLUJO DEL MÉTODO:
        /// 1. Obtener datos de los campos de texto
        /// 2. Validar que los datos sean válidos
        /// 3. Crear objeto usuario
        /// 4. Agregar a la lista visual
        /// 5. Limpiar campos para siguiente entrada
        /// 6. Guardar datos en archivo
        /// </summary>
        /// <param name="sender">El control que disparó el evento (en este caso, btnSubmit)</param>
        /// <param name="e">Información adicional sobre el evento (no la usamos aquí)</param>
        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            // PASO 1: OBTENER Y LIMPIAR DATOS DE ENTRADA
            // ==================================================================================

            /// <summary>
            /// EXTRACCIÓN DE DATOS
            /// Trim() es crucial para eliminar espacios en blanco que el usuario
            /// puede haber ingresado accidentalmente al principio o final
            /// </summary>
            var name = txtName.Text.Trim();     // Obtener nombre sin espacios extra
            var email = txtEmail.Text.Trim();   // Obtener email sin espacios extra

            // PASO 2: VALIDACIÓN DE DATOS
            // ==================================================================================

            /// <summary>
            /// VALIDACIÓN SIMPLE PERO EFECTIVA
            /// Verificamos que ambos campos tengan contenido. Si no, mostramos un mensaje
            /// de error y salimos del método sin procesar nada.
            /// 
            /// NOTA: Esta es validación básica. En aplicaciones reales se podría:
            /// - Validar formato de email con regex o MailAddress
            /// - Verificar longitud mínima/máxima de campos  
            /// - Validar caracteres especiales
            /// - Verificar duplicados
            /// </summary>
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email))
            {
                MessageBox.Show(
                    "Completa nombre y email",              // Mensaje para el usuario
                    "Atención",                             // Título del diálogo
                    MessageBoxButtons.OK,                   // Solo botón OK
                    MessageBoxIcon.Warning                  // Icono de advertencia
                );
                return; // IMPORTANTE: Salir del método sin continuar el procesamiento
            }

            // PASO 3: CREAR Y PROCESAR USUARIO VÁLIDO
            // ==================================================================================

            /// <summary>
            /// CREACIÓN DEL OBJETO USUARIO
            /// Si llegamos aquí, los datos son válidos. Creamos un objeto User
            /// con la información proporcionada.
            /// </summary>
            var user = new User { Name = name, Email = email };

            /// <summary>
            /// AGREGAR A LA LISTA VISUAL
            /// FormatUser convierte el objeto User en una cadena legible para mostrar
            /// en el ListBox. El formato será: "Nombre <email@dominio.com>"
            /// </summary>
            lstUsers.Items.Add(FormatUser(user));

            // PASO 4: LIMPIEZA Y PREPARACIÓN PARA SIGUIENTE ENTRADA
            // ==================================================================================

            /// <summary>
            /// LIMPIAR CAMPOS DE ENTRADA
            /// Después de procesar exitosamente, limpiamos los campos para que el
            /// usuario pueda ingresar otro usuario fácilmente
            /// </summary>
            txtName.Clear();        // Limpiar campo de nombre
            txtEmail.Clear();       // Limpiar campo de email

            // PASO 5: PERSISTENCIA DE DATOS
            // ==================================================================================

            /// <summary>
            /// GUARDAR TODOS LOS USUARIOS ACTUALES
            /// Llamamos a SaveUsers() para asegurar que el nuevo usuario
            /// se persista en el archivo JSON
            /// </summary>
            SaveUsers();
        }
        #endregion

        #region MÉTODOS AUXILIARES
        // ==================================================================================
        // MÉTODOS DE APOYO Y UTILIDADES
        // ==================================================================================

        /// <summary>
        /// FORMATEAR USUARIO PARA VISUALIZACIÓN
        /// 
        /// Método simple que convierte un objeto User en una cadena de texto
        /// con formato estándar para mostrar en la lista.
        /// 
        /// FORMATO DE SALIDA: "Nombre Completo <email@ejemplo.com>"
        /// 
        /// VENTAJAS DE ESTE FORMATO:
        /// - Fácil de leer para el usuario
        /// - Fácil de parsear de vuelta (para SaveUsers)
        /// - Formato común en aplicaciones de email
        /// </summary>
        /// <param name="u">Objeto User a formatear</param>
        /// <returns>Cadena formateada para mostrar</returns>
        private string FormatUser(User u) => $"{u.Name} <{u.Email}>";
        #endregion

        #region PERSISTENCIA DE DATOS
        // ==================================================================================
        // CARGA Y GUARDADO DE DATOS EN ARCHIVO JSON
        // ==================================================================================

        /// <summary>
        /// CARGAR USUARIOS DESDE ARCHIVO JSON
        /// 
        /// Este método se ejecuta al iniciar la aplicación para recuperar usuarios
        /// que fueron guardados en sesiones anteriores.
        /// 
        /// PROCESO DE CARGA:
        /// 1. Verificar si existe el archivo
        /// 2. Leer contenido JSON
        /// 3. Deserializar a lista de objetos User
        /// 4. Agregar cada usuario a la lista visual
        /// 5. Manejar errores graciosamente
        /// </summary>
        private void LoadUsers()
        {
            try
            {
                // PASO 1: VERIFICAR EXISTENCIA DEL ARCHIVO
                // ==================================================================================

                /// <summary>
                /// VERIFICACIÓN DE ARCHIVO
                /// Si el archivo no existe, es la primera ejecución de la aplicación
                /// o se borró el archivo. En cualquier caso, no hay nada que cargar.
                /// </summary>
                if (!File.Exists(DataFile))
                    return; // Salir silenciosamente, no es un error

                // PASO 2: LEER CONTENIDO DEL ARCHIVO
                // ==================================================================================

                /// <summary>
                /// LECTURA DEL ARCHIVO JSON
                /// File.ReadAllText lee todo el contenido del archivo de una vez.
                /// Para archivos grandes, se usaría FileStream, pero para listas
                /// de usuarios típicas, este enfoque es más simple y eficiente.
                /// </summary>
                var json = File.ReadAllText(DataFile);

                // PASO 3: DESERIALIZAR JSON A OBJETOS
                // ==================================================================================

                /// <summary>
                /// CONVERSIÓN DE JSON A OBJETOS C#
                /// JsonSerializer.Deserialize convierte el texto JSON en una lista
                /// de objetos User que podemos manipular en C#.
                /// </summary>
                var users = JsonSerializer.Deserialize<List<User>>(json);

                /// <summary>
                /// VALIDACIÓN DE DESERIALIZACIÓN
                /// Aunque raro, la deserialización puede devolver null si el JSON
                /// está mal formado o vacío
                /// </summary>
                if (users == null)
                    return; // Salir si no se pudo deserializar

                // PASO 4: AGREGAR USUARIOS A LA LISTA VISUAL
                // ==================================================================================

                /// <summary>
                /// POBLACIÓN DE LA LISTA
                /// Iteramos sobre cada usuario deserializado y lo agregamos a la
                /// lista visual usando el mismo formato que cuando se agregan manualmente
                /// </summary>
                foreach (var u in users)
                    lstUsers.Items.Add(FormatUser(u));
            }
            catch (Exception ex)
            {
                // PASO 5: MANEJO DE ERRORES
                // ==================================================================================

                /// <summary>
                /// GESTIÓN DE ERRORES EN CARGA
                /// Si algo sale mal (archivo corrupto, permisos insuficientes, etc.),
                /// mostramos un mensaje de error pero permitimos que la aplicación continúe.
                /// 
                /// TIPOS DE ERRORES POSIBLES:
                /// - FileNotFoundException: Aunque verificamos existencia, el archivo podría borrarse
                /// - UnauthorizedAccessException: Sin permisos de lectura
                /// - JsonException: JSON mal formado
                /// - IOException: Problemas de E/S del sistema
                /// </summary>
                MessageBox.Show(
                    $"No se pudo cargar users: {ex.Message}",   // Mensaje descriptivo con detalle del error
                    "Error",                                    // Título del diálogo
                    MessageBoxButtons.OK,                       // Solo botón OK
                    MessageBoxIcon.Error                        // Icono de error
                );
            }
        }

        /// <summary>
        /// GUARDAR USUARIOS EN ARCHIVO JSON
        /// 
        /// Este método toma todos los usuarios actualmente en la lista visual
        /// y los guarda en un archivo JSON para persistencia entre sesiones.
        /// 
        /// PROCESO DE GUARDADO:
        /// 1. Extraer usuarios de la lista visual
        /// 2. Convertir strings de vuelta a objetos User
        /// 3. Crear directorio si no existe
        /// 4. Serializar lista a JSON
        /// 5. Escribir JSON al archivo
        /// 6. Manejar errores graciosamente
        /// </summary>
        private void SaveUsers()
        {
            try
            {
                // PASO 1: CREAR LISTA DE USUARIOS
                // ==================================================================================

                /// <summary>
                /// INICIALIZACIÓN DE LISTA DESTINO
                /// Creamos una nueva lista que contendrá todos los objetos User
                /// que extraigamos de la lista visual
                /// </summary>
                var users = new List<User>();

                // PASO 2: EXTRAER USUARIOS DE LA LISTA VISUAL
                // ==================================================================================

                /// <summary>
                /// PROCESAMIENTO DE ELEMENTOS DE LA LISTA
                /// Iteramos sobre cada elemento del ListBox y lo convertimos
                /// de vuelta de string a objeto User
                /// </summary>
                foreach (var item in lstUsers.Items)
                {
                    /// <summary>
                    /// CONVERSIÓN A STRING
                    /// Cada elemento del ListBox se convierte a string para procesamiento
                    /// </summary>
                    var s = item.ToString();

                    // PARSING DEL FORMATO "Nombre <email>"
                    // ==================================================================================

                    /// <summary>
                    /// EXTRACCIÓN DE NOMBRE Y EMAIL
                    /// Buscamos el último '<' en la cadena para separar nombre y email.
                    /// Usamos LastIndexOf por si el nombre contiene '<' (aunque sería raro).
                    /// 
                    /// EJEMPLO DE PARSING:
                    /// Input:  "Juan Pérez <juan@email.com>"
                    /// idx:    13 (posición del '<')
                    /// name:   "Juan Pérez" (substring de 0 a 13, sin espacios extra)
                    /// email:  "juan@email.com" (substring de 14 al final, sin '>')
                    /// </summary>
                    var idx = s.LastIndexOf('<');
                    if (idx > 0)    // Solo procesar si encontramos el delimitador
                    {
                        var name = s.Substring(0, idx).Trim();          // Nombre desde inicio hasta '<'
                        var email = s.Substring(idx + 1).TrimEnd('>');  // Email desde '<' hasta final, sin '>'
                        users.Add(new User { Name = name, Email = email });
                    }
                }

                // PASO 3: CREAR DIRECTORIO SI ES NECESARIO
                // ==================================================================================

                /// <summary>
                /// PREPARACIÓN DEL DIRECTORIO
                /// Path.GetDirectoryName extrae la carpeta de la ruta completa del archivo.
                /// Directory.CreateDirectory crea la carpeta si no existe (no falla si ya existe).
                /// El operador ?? proporciona "data" como fallback si GetDirectoryName devuelve null.
                /// </summary>
                Directory.CreateDirectory(Path.GetDirectoryName(DataFile) ?? "data");

                // PASO 4: SERIALIZAR A JSON
                // ==================================================================================

                /// <summary>
                /// CONVERSIÓN DE OBJETOS A JSON
                /// JsonSerializer.Serialize convierte la lista de objetos User a texto JSON.
                /// WriteIndented = true hace que el JSON sea legible para humanos (con indentación).
                /// 
                /// EJEMPLO DE SALIDA:
                /// [
                ///   {
                ///     "Name": "Juan Pérez",
                ///     "Email": "juan@email.com"
                ///   },
                ///   {
                ///     "Name": "María García", 
                ///     "Email": "maria@email.com"
                ///   }
                /// ]
                /// </summary>
                var json = JsonSerializer.Serialize(users, new JsonSerializerOptions
                {
                    WriteIndented = true    // JSON formateado para legibilidad
                });

                // PASO 5: ESCRIBIR AL ARCHIVO
                // ==================================================================================

                /// <summary>
                /// ESCRITURA DEL ARCHIVO
                /// File.WriteAllText escribe todo el contenido JSON al archivo,
                /// sobrescribiendo el contenido anterior (si existe)
                /// </summary>
                File.WriteAllText(DataFile, json);
            }
            catch (Exception ex)
            {
                // PASO 6: MANEJO DE ERRORES EN GUARDADO
                // ==================================================================================

                /// <summary>
                /// GESTIÓN DE ERRORES EN ESCRITURA
                /// 
                /// TIPOS DE ERRORES POSIBLES:
                /// - UnauthorizedAccessException: Sin permisos de escritura
                /// - DirectoryNotFoundException: Problemas creando el directorio
                /// - IOException: Disco lleno, archivo en uso, etc.
                /// - JsonException: Error en serialización (muy raro)
                /// </summary>
                MessageBox.Show(
                    $"No se pudo guardar users: {ex.Message}",  // Mensaje con detalle técnico
                    "Error",                                    // Título del diálogo
                    MessageBoxButtons.OK,                       // Solo botón OK
                    MessageBoxIcon.Error                        // Icono de error
                );
            }
        }
        #endregion

        #region MODELOS DE DATOS
        // ==================================================================================
        // DEFINICIÓN DE ESTRUCTURAS DE DATOS
        // ==================================================================================

        /// <summary>
        /// CLASE MODELO PARA USUARIO
        /// 
        /// Esta clase representa la estructura de datos de un usuario en nuestra aplicación.
        /// Es una clase muy simple que solo contiene datos (sin lógica de negocio compleja).
        /// 
        /// CARACTERÍSTICAS DE ESTA CLASE:
        /// - POCO (Plain Old CLR Object): Objeto simple sin dependencias
        /// - Propiedades públicas: Facilita serialización JSON
        /// - Valores por defecto: Evita valores null no deseados
        /// - Inmutable en la práctica: Una vez creado, generalmente no se modifica
        /// 
        /// USOS DE ESTA CLASE:
        /// - Transferencia de datos entre métodos
        /// - Serialización/deserialización JSON
        /// - Tipado fuerte (mejor que usar strings sueltos)
        /// </summary>
        private class User
        {
            /// <summary>
            /// NOMBRE COMPLETO DEL USUARIO
            /// Propiedad que almacena el nombre completo. 
            /// Valor por defecto "" evita problemas con valores null.
            /// </summary>
            public string Name { get; set; } = "";

            /// <summary>
            /// DIRECCIÓN DE EMAIL DEL USUARIO  
            /// Propiedad que almacena el email.
            /// En aplicaciones reales podría incluir validación en el setter.
            /// </summary>
            public string Email { get; set; } = "";
        }
        #endregion
    }
}

/*
==================================================================================
RESUMEN EDUCATIVO DE ESTA IMPLEMENTACIÓN
==================================================================================

CONCEPTOS FUNDAMENTALES DEMOSTRADOS:

1. PROGRAMACIÓN ORIENTADA A EVENTOS
   - El formulario responde a acciones del usuario (clicks, etc.)
   - Patrón evento -> validación -> procesamiento -> persistencia

2. INTERFAZ DE USUARIO IMPERATIVA
   - Creación de controles por código (no diseñador visual)
   - Posicionamiento absoluto con coordenadas fijas
   - Registro manual de controles en el formulario

3. PERSISTENCIA SIMPLE DE DATOS
   - Serialización JSON para almacenamiento
   - Manejo de archivos con System.IO
   - Patrón carga al inicio -> guardado después de cambios

4. VALIDACIÓN Y MANEJO DE ERRORES
   - Validación básica de campos requeridos
   - Try-catch para operaciones que pueden fallar
   - Mensajes de error amigables para el usuario

5. SEPARACIÓN DE RESPONSABILIDADES
   - Modelo de datos (User) separado de la UI
   - Métodos específicos para cada responsabilidad
   - Lógica de formateo encapsulada en métodos auxiliares

PATRONES DE DISEÑO UTILIZADOS:

- MODEL-VIEW: Separación entre datos (User) y presentación (Form)
- EVENT-DRIVEN: Toda la lógica se ejecuta en respuesta a eventos
- PERSISTENCE LAYER: Métodos dedicados a carga/guardado de datos

FORTALEZAS DE ESTA APROXIMACIÓN:
- Código simple y directo
- Fácil de entender para principiantes
- Mínimas dependencias externas
- Funcionalidad completa en pocas líneas

OPORTUNIDADES DE MEJORA:
- Validación de formato de email
- Manejo de duplicados
- Interfaz responsive (layouts automáticos)
- Controles personalizados para mejor UX
*/