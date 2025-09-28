using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Text.Json;
using System.Net.Mail;
using System.Windows.Forms;

namespace MiAppWinForms.src
{
    /// <summary>
    /// CLASE PRINCIPAL DEL FORMULARIO
    /// Esta clase hereda de Form y representa la ventana principal de nuestra aplicación.
    /// Aquí se construye toda la interfaz de usuario (UI), se manejan los eventos del usuario
    /// y se implementa la lógica para agregar, mostrar y guardar usuarios.
    /// 
    /// PATRÓN DE DISEÑO: Esta clase sigue el patrón "Code-First" donde toda la UI
    /// se crea mediante código en lugar de usar el diseñador visual de Visual Studio.
    /// </summary>
    public class FrmMain : Form
    {
        #region DECLARACIÓN DE CONTROLES
        // ==================================================================================
        // CONTROLES DE LA INTERFAZ DE USUARIO
        // ==================================================================================
        // Declaramos todos los controles a nivel de clase para que puedan ser accedidos
        // desde cualquier método de la clase. Esto es importante para manejar eventos
        // y actualizar la UI desde diferentes partes del código.

        private ModernTextBox txtName;      // Campo de texto para el nombre del usuario
        private ModernTextBox txtEmail;     // Campo de texto para el email del usuario
        private ModernButton btnSubmit;     // Botón para agregar un nuevo usuario
        private ModernListBox lstUsers;     // Lista que muestra todos los usuarios registrados

        // Paneles para organizar la UI en secciones lógicas
        private Panel headerPanel;          // Panel superior con título y subtítulo
        private Panel inputPanel;           // Panel medio con campos de entrada y botón
        private Panel listPanel;            // Panel inferior con la lista de usuarios
        #endregion

        #region CONFIGURACIÓN DE DATOS
        // ==================================================================================
        // CONFIGURACIÓN DE PERSISTENCIA DE DATOS
        // ==================================================================================

        /// <summary>
        /// RUTA DEL ARCHIVO DE DATOS
        /// Aquí se define dónde se guardarán los usuarios en formato JSON.
        /// Se usa una ruta relativa para que el archivo se guarde junto al ejecutable.
        /// El directorio 'data' se creará automáticamente si no existe.
        /// </summary>
        private const string DataFile = "data/users.json";
        #endregion

        #region PALETA DE COLORES
        // ==================================================================================
        // SISTEMA DE DISEÑO - PALETA DE COLORES MODERNA
        // ==================================================================================
        // Definimos todos los colores como constantes para mantener un diseño consistente
        // y facilitar futuros cambios de tema. Esta paleta está inspirada en diseños web modernos.

        // Color principal - Usado para botones y elementos destacados (Indigo)
        private static readonly Color PrimaryColor = Color.FromArgb(79, 70, 229);

        // Color de fondo secundario - Usado para áreas de contenido (Gris claro)
        private static readonly Color SecondaryColor = Color.FromArgb(249, 250, 251);

        // Color de acento - Para elementos interactivos y estados de éxito (Verde esmeralda)
        private static readonly Color AccentColor = Color.FromArgb(16, 185, 129);

        // Color de texto principal - Para títulos y texto importante (Gris oscuro)
        private static readonly Color TextPrimary = Color.FromArgb(17, 24, 39);

        // Color de texto secundario - Para subtítulos y texto menos importante
        private static readonly Color TextSecondary = Color.FromArgb(107, 114, 128);

        // Color de bordes - Para separar elementos visualmente
        private static readonly Color BorderColor = Color.FromArgb(229, 231, 235);

        // Color de hover - Para efectos cuando el mouse pasa sobre elementos
        private static readonly Color HoverColor = Color.FromArgb(67, 56, 202);
        #endregion

        #region CONSTRUCTOR Y INICIALIZACIÓN
        // ==================================================================================
        // CONSTRUCTOR PRINCIPAL
        // ==================================================================================

        /// <summary>
        /// Constructor de la clase FrmMain
        /// Este es el punto de entrada donde se inicializa toda la aplicación.
        /// Se ejecuta en este orden específico para asegurar que todo se configure correctamente.
        /// </summary>
        public FrmMain()
        {
            // PASO 1: Configurar las propiedades básicas del formulario
            // (tamaño, posición, fuentes, colores, etc.)
            InitializeForm();

            // PASO 2: Crear y posicionar todos los controles de la interfaz
            // (paneles, campos de texto, botones, lista)
            CreateControls();

            // PASO 3: Cargar datos existentes desde el archivo JSON
            // (si existe, mostrar usuarios previamente guardados)
            LoadUsers();
        }
        #endregion

        #region INICIALIZACIÓN DEL FORMULARIO
        // ==================================================================================
        // CONFIGURACIÓN BÁSICA DEL FORMULARIO
        // ==================================================================================

        /// <summary>
        /// Configura las propiedades fundamentales del formulario
        /// Aquí se establecen características como tamaño, posición, fuente y apariencia general.
        /// </summary>
        private void InitializeForm()
        {
            // CONFIGURACIÓN DE VENTANA
            Text = "Gestión de Usuarios";                    // Título que aparece en la barra de título
            Size = new Size(420, 580);                       // Tamaño inicial: 420px ancho, 580px alto
            MinimumSize = new Size(400, 500);               // Tamaño mínimo para evitar que se vea mal
            MaximumSize = new Size(500, 700);               // Tamaño máximo para mantener proporciones
            StartPosition = FormStartPosition.CenterScreen;  // Centrar en pantalla al abrir
            FormBorderStyle = FormBorderStyle.FixedSingle;   // Borde fijo, no redimensionable con mouse
            MaximizeBox = false;                            // Desactivar botón de maximizar
            BackColor = Color.White;                        // Color de fondo blanco

            // CONFIGURACIÓN DE FUENTE
            // Intentamos usar la fuente moderna "Inter", si no está disponible usamos "Segoe UI"
            try
            {
                Font = new Font("Inter", 9F, FontStyle.Regular);
            }
            catch
            {
                // Fallback: Si Inter no está instalada, usar Segoe UI (estándar en Windows)
                Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            }

            // ELIMINACIÓN DE PADDING
            // Quitamos el padding interno para que nuestro TableLayoutPanel controle completamente el espaciado
            Padding = new Padding(0);
        }
        #endregion

        #region CREACIÓN DE CONTROLES
        // ==================================================================================
        // CONSTRUCCIÓN DE LA INTERFAZ DE USUARIO
        // ==================================================================================

        /// <summary>
        /// Método principal que orquesta la creación de todos los controles
        /// Utiliza TableLayoutPanel como contenedor principal para organizar la UI en secciones
        /// </summary>
        private void CreateControls()
        {
            // CONTENEDOR PRINCIPAL - TableLayoutPanel
            // Este control nos permite dividir la ventana en filas con diferentes comportamientos:
            // - Filas de altura fija para header e inputs
            // - Fila expansible para la lista (se adapta al tamaño de la ventana)
            var mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,          // Ocupa toda el área del formulario
                ColumnCount = 1,                // Solo una columna (layout vertical)
                RowCount = 3,                   // Tres filas (header, inputs, lista)
                BackColor = Color.White,        // Fondo blanco
                Margin = new Padding(0),        // Sin margen externo
                Padding = new Padding(0)        // Sin padding interno
            };

            // CONFIGURACIÓN DE FILAS
            // Definimos cómo se comporta cada fila en cuanto a tamaño:
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 80));   // Header: 80px fijos
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 160));  // Inputs: 160px fijos  
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));   // Lista: ocupa espacio restante

            // CREAR CADA SECCIÓN DE LA UI
            CreateHeaderPanel(mainLayout);     // Crear y agregar panel de encabezado
            CreateInputPanel(mainLayout);      // Crear y agregar panel de campos de entrada
            CreateListPanel(mainLayout);       // Crear y agregar panel de lista

            // Agregar el layout principal al formulario
            Controls.Add(mainLayout);
        }
        #endregion

        #region PANEL DE ENCABEZADO
        // ==================================================================================
        // SECCIÓN SUPERIOR - TÍTULO Y SUBTÍTULO
        // ==================================================================================

        /// <summary>
        /// Crea el panel superior con el título principal y subtítulo
        /// Esta sección es puramente informativa y no contiene elementos interactivos
        /// </summary>
        /// <param name="mainLayout">El contenedor principal donde se agregará este panel</param>
        private void CreateHeaderPanel(TableLayoutPanel mainLayout)
        {
            // CONTENEDOR DEL HEADER
            headerPanel = new Panel
            {
                Dock = DockStyle.Fill,                      // Ocupa todo el espacio asignado por TableLayoutPanel
                BackColor = Color.White,                    // Fondo blanco
                Margin = new Padding(0),                    // Sin margen externo
                Padding = new Padding(24, 24, 24, 16)      // Padding interno: 24px laterales, 24px arriba, 16px abajo
            };

            // TÍTULO PRINCIPAL
            var titleLabel = new Label
            {
                Text = "Usuarios",                          // Texto del título
                Font = new Font(Font.FontFamily, 16F, FontStyle.Bold),  // Fuente grande y en negritas
                ForeColor = TextPrimary,                    // Color de texto principal (gris oscuro)
                AutoSize = true,                            // Ajustar tamaño automáticamente al contenido
                Location = new Point(0, 0)                  // Posición dentro del panel
            };

            // SUBTÍTULO EXPLICATIVO
            var subtitleLabel = new Label
            {
                Text = "Gestiona los usuarios registrados", // Texto explicativo
                Font = new Font(Font.FontFamily, 9F, FontStyle.Regular),    // Fuente normal, tamaño estándar
                ForeColor = TextSecondary,                  // Color de texto secundario (gris medio)
                AutoSize = true,                            // Ajustar tamaño automáticamente
                Location = new Point(0, 32)                 // Posición 32px debajo del título
            };

            // AGREGAR CONTROLES AL PANEL
            headerPanel.Controls.Add(titleLabel);
            headerPanel.Controls.Add(subtitleLabel);

            // AGREGAR PANEL AL LAYOUT PRINCIPAL
            // Posición: columna 0, fila 0 (primera fila)
            mainLayout.Controls.Add(headerPanel, 0, 0);
        }
        #endregion

        #region PANEL DE ENTRADA DE DATOS
        // ==================================================================================
        // SECCIÓN MEDIA - CAMPOS DE ENTRADA Y BOTÓN
        // ==================================================================================

        /// <summary>
        /// Crea el panel con los campos de entrada (nombre y email) y el botón de agregar
        /// Esta es la sección interactiva donde el usuario ingresa nuevos datos
        /// </summary>
        /// <param name="mainLayout">El contenedor principal donde se agregará este panel</param>
        private void CreateInputPanel(TableLayoutPanel mainLayout)
        {
            // CONTENEDOR DE INPUTS
            inputPanel = new Panel
            {
                Dock = DockStyle.Fill,                      // Ocupa todo el espacio asignado
                BackColor = SecondaryColor,                 // Fondo gris claro para diferenciarlo del header
                Margin = new Padding(0),                    // Sin margen externo
                Padding = new Padding(24, 24, 24, 24)      // Padding uniforme de 24px en todos los lados
            };

            // VARIABLES DE LAYOUT
            // Centralizamos estos valores para facilitar ajustes futuros de la UI
            var leftMargin = 12;            // Margen izquierdo para todos los elementos
            var inputHeight = 42;           // Altura estándar para campos y botones

            // === CAMPO DE NOMBRE ===

            // Etiqueta del campo nombre
            var nameLabel = new Label
            {
                Text = "Nombre completo",                   // Texto descriptivo del campo
                Font = new Font(Font.FontFamily, 8.5F, FontStyle.Regular),  // Fuente pequeña para etiquetas
                ForeColor = TextPrimary,                    // Color de texto principal
                AutoSize = true,                            // Ajustar tamaño al contenido
                Location = new Point(leftMargin, 0)        // Posición con margen izquierdo
            };

            // Campo de texto personalizado para nombre
            txtName = new ModernTextBox
            {
                Size = new Size(348, inputHeight),          // Ancho: casi todo el panel, alto: estándar
                Location = new Point(leftMargin, 20),       // 20px debajo de la etiqueta
                Font = new Font(Font.FontFamily, 9F),       // Fuente estándar
                PlaceholderText = "Ingresa el nombre completo"  // Texto de ayuda (placeholder)
            };

            // === CAMPO DE EMAIL ===

            // Etiqueta del campo email
            var emailLabel = new Label
            {
                Text = "Correo electrónico",                // Texto descriptivo
                Font = new Font(Font.FontFamily, 8.5F, FontStyle.Regular),  // Misma fuente que otras etiquetas
                ForeColor = TextPrimary,                    // Color consistente
                AutoSize = true,                            // Tamaño automático
                Location = new Point(leftMargin, 80)        // Posición debajo del campo de nombre
            };

            // Campo de texto para email (más angosto para dejar espacio al botón)
            txtEmail = new ModernTextBox
            {
                Size = new Size(244, inputHeight),          // Ancho reducido para dejar espacio al botón
                Location = new Point(leftMargin, 100),      // 20px debajo de la etiqueta
                Font = new Font(Font.FontFamily, 9F),       // Fuente consistente
                PlaceholderText = "ejemplo@correo.com"      // Ejemplo de formato de email
            };

            // === BOTÓN DE AGREGAR ===

            // Botón personalizado para agregar usuarios
            btnSubmit = new ModernButton
            {
                Text = "Agregar",                           // Texto del botón
                Size = new Size(96, inputHeight),           // Tamaño del botón
                Location = new Point(leftMargin + 244 + 8, 100),  // Al lado del campo email con 8px de separación
                Font = new Font(Font.FontFamily, 8.5F, FontStyle.Regular)  // Fuente del botón
            };

            // ASIGNAR EVENTO DEL BOTÓN
            // Cuando se hace clic en el botón, se ejecutará el método BtnSubmit_Click
            btnSubmit.Click += BtnSubmit_Click;

            // AGREGAR TODOS LOS CONTROLES AL PANEL
            // Usamos AddRange para agregar múltiples controles de una vez
            inputPanel.Controls.AddRange(new Control[]
            {
                nameLabel, txtName,         // Campo de nombre con su etiqueta
                emailLabel, txtEmail,       // Campo de email con su etiqueta
                btnSubmit                   // Botón de agregar
            });

            // AGREGAR PANEL AL LAYOUT PRINCIPAL
            // Posición: columna 0, fila 1 (segunda fila)
            mainLayout.Controls.Add(inputPanel, 0, 1);
        }
        #endregion

        #region PANEL DE LISTA
        // ==================================================================================
        // SECCIÓN INFERIOR - LISTA DE USUARIOS
        // ==================================================================================

        /// <summary>
        /// Crea el panel inferior que contiene la lista de usuarios registrados
        /// Esta sección muestra todos los usuarios que han sido agregados
        /// </summary>
        /// <param name="mainLayout">El contenedor principal donde se agregará este panel</param>
        private void CreateListPanel(TableLayoutPanel mainLayout)
        {
            // CONTENEDOR DE LA LISTA
            listPanel = new Panel
            {
                Dock = DockStyle.Fill,                      // Ocupa todo el espacio disponible
                BackColor = Color.White,                    // Fondo blanco (igual al header)
                Margin = new Padding(0),                    // Sin margen externo
                Padding = new Padding(24, 24, 24, 32)      // Padding interno con más espacio abajo
            };

            // ETIQUETA DE LA LISTA
            var listLabel = new Label
            {
                Text = "Usuarios registrados",              // Título de la sección
                Font = new Font(Font.FontFamily, 9F, FontStyle.Regular),    // Fuente estándar
                ForeColor = TextPrimary,                    // Color de texto principal
                AutoSize = true,                            // Tamaño automático
                Location = new Point(0, 0)                  // Posición en la esquina superior
            };

            // LISTA PERSONALIZADA DE USUARIOS
            lstUsers = new ModernListBox
            {
                Size = new Size(348, 200),                  // Tamaño inicial (se redimensionará automáticamente)
                Location = new Point(0, 24),                // 24px debajo de la etiqueta
                // ANCLAJE IMPORTANTE: permite que la lista se redimensione con la ventana
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                Font = new Font(Font.FontFamily, 9F)        // Fuente para los elementos de la lista
            };

            // AGREGAR CONTROLES AL PANEL
            listPanel.Controls.Add(listLabel);             // Etiqueta
            listPanel.Controls.Add(lstUsers);              // Lista

            // AGREGAR PANEL AL LAYOUT PRINCIPAL  
            // Posición: columna 0, fila 2 (tercera fila - la que se expande)
            mainLayout.Controls.Add(listPanel, 0, 2);
        }
        #endregion

        #region EVENTOS Y LÓGICA DE NEGOCIO
        // ==================================================================================
        // MANEJO DE EVENTOS Y LÓGICA DE LA APLICACIÓN
        // ==================================================================================

        /// <summary>
        /// EVENTO DEL BOTÓN "AGREGAR"
        /// Este método se ejecuta cuando el usuario hace clic en el botón Agregar.
        /// Realiza todo el proceso de validación, agregado y persistencia de un nuevo usuario.
        /// </summary>
        /// <param name="sender">El control que generó el evento (el botón)</param>
        /// <param name="e">Información adicional sobre el evento</param>
        private void BtnSubmit_Click(object? sender, EventArgs e)
        {
            // PASO 1: OBTENER Y LIMPIAR LOS DATOS DE ENTRADA
            // Trim() elimina espacios en blanco al inicio y final
            var name = txtName.Text.Trim();
            var email = txtEmail.Text.Trim();

            // PASO 2: VALIDACIÓN BÁSICA - CAMPOS REQUERIDOS
            // Verificamos que ambos campos tengan contenido
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email))
            {
                ShowModernMessage("Por favor completa ambos campos", "Campos requeridos", MessageBoxIcon.Warning);
                return; // Salir del método sin procesar
            }

            // PASO 3: VALIDACIÓN DE FORMATO DE EMAIL
            // Verificamos que el email tenga un formato válido
            if (!IsValidEmail(email))
            {
                ShowModernMessage("El formato del correo electrónico no es válido", "Email inválido", MessageBoxIcon.Warning);
                txtEmail.Focus(); // Poner el cursor en el campo de email para corrección
                return; // Salir del método sin procesar
            }

            // PASO 4: CREAR OBJETO USUARIO Y AGREGARLO A LA LISTA
            var user = new User { Name = name, Email = email };     // Crear objeto usuario
            lstUsers.Items.Add(FormatUser(user));                   // Agregar a la lista visual

            // PASO 5: LIMPIAR CAMPOS Y PREPARAR PARA SIGUIENTE ENTRADA
            txtName.Clear();        // Limpiar campo de nombre
            txtEmail.Clear();       // Limpiar campo de email
            txtName.Focus();        // Poner cursor en campo de nombre para siguiente usuario

            // PASO 6: GUARDAR DATOS EN ARCHIVO
            // Persistir todos los usuarios actuales en el archivo JSON
            SaveUsers();
        }

        /// <summary>
        /// MOSTRAR MENSAJES AL USUARIO
        /// Wrapper sobre MessageBox para mostrar mensajes con estilo consistente.
        /// En el futuro, esto podría cambiarse por un diálogo personalizado sin afectar el resto del código.
        /// </summary>
        /// <param name="message">El mensaje a mostrar</param>
        /// <param name="title">El título del diálogo</param>
        /// <param name="icon">El tipo de icono (información, advertencia, error)</param>
        private void ShowModernMessage(string message, string title, MessageBoxIcon icon)
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, icon);
        }

        /// <summary>
        /// FORMATEAR USUARIO PARA MOSTRAR EN LISTA
        /// Convierte un objeto User en una cadena de texto con formato "Nombre <email>"
        /// Este formato facilita luego extraer los datos cuando necesitemos guardar
        /// </summary>
        /// <param name="u">El objeto usuario a formatear</param>
        /// <returns>Cadena formateada para mostrar en la lista</returns>
        private string FormatUser(User u) => $"{u.Name} <{u.Email}>";

        /// <summary>
        /// VALIDADOR DE EMAIL
        /// Utiliza la clase MailAddress de .NET para validar que el email tenga formato correcto.
        /// Esta validación es más robusta que usar expresiones regulares.
        /// </summary>
        /// <param name="email">El email a validar</param>
        /// <returns>true si el email es válido, false en caso contrario</returns>
        private bool IsValidEmail(string email)
        {
            try
            {
                // MailAddress lanza excepción si el formato es inválido
                var addr = new MailAddress(email);
                // Verificamos que no se haya modificado el email durante la validación
                return addr.Address == email;
            }
            catch
            {
                // Si hay cualquier excepción, el email no es válido
                return false;
            }
        }
        #endregion

        #region PERSISTENCIA DE DATOS
        // ==================================================================================
        // CARGA Y GUARDADO DE DATOS EN ARCHIVO JSON
        // ==================================================================================

        /// <summary>
        /// CARGAR USUARIOS DESDE ARCHIVO
        /// Lee el archivo JSON (si existe) y carga los usuarios en la lista visual.
        /// Este método se ejecuta al inicio de la aplicación para recuperar datos previamente guardados.
        /// </summary>
        private void LoadUsers()
        {
            try
            {
                // VERIFICAR SI EL ARCHIVO EXISTE
                // Si no existe, no hay nada que cargar (primera ejecución de la app)
                if (!File.Exists(DataFile)) return;

                // LEER Y DESERIALIZAR EL ARCHIVO JSON
                var json = File.ReadAllText(DataFile);                          // Leer todo el contenido
                var users = JsonSerializer.Deserialize<List<User>>(json);       // Convertir JSON a objetos
                if (users == null) return; // Si la deserialización falla, salir

                // AGREGAR CADA USUARIO A LA LISTA VISUAL
                foreach (var u in users)
                    lstUsers.Items.Add(FormatUser(u));
            }
            catch (Exception ex)
            {
                // MANEJO DE ERRORES
                // Si algo sale mal (archivo corrupto, permisos, etc.), mostrar mensaje de error
                ShowModernMessage($"No se pudo cargar los usuarios: {ex.Message}", "Error de carga", MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// GUARDAR USUARIOS EN ARCHIVO
        /// Toma todos los usuarios actuales de la lista visual, los convierte a objetos
        /// y los guarda en formato JSON para persistencia.
        /// </summary>
        private void SaveUsers()
        {
            try
            {
                // PASO 1: CREAR LISTA DE OBJETOS USER
                var users = new List<User>();

                // PASO 2: PROCESAR CADA ELEMENTO DE LA LISTA VISUAL
                foreach (var item in lstUsers.Items)
                {
                    var s = item.ToString();
                    if (!string.IsNullOrEmpty(s))
                    {
                        // EXTRAER NOMBRE Y EMAIL DEL FORMATO "Nombre <email>"
                        // Buscamos el último '<' para separar correctamente
                        var idx = s.LastIndexOf('<');
                        if (idx > 0)
                        {
                            var name = s.Substring(0, idx).Trim();          // Parte antes de '<'
                            var email = s.Substring(idx + 1).TrimEnd('>');  // Parte después de '<', sin '>'
                            users.Add(new User { Name = name, Email = email });
                        }
                    }
                }

                // PASO 3: CREAR DIRECTORIO SI NO EXISTE
                // Path.GetDirectoryName extrae la carpeta de la ruta del archivo
                Directory.CreateDirectory(Path.GetDirectoryName(DataFile) ?? "data");

                // PASO 4: SERIALIZAR Y GUARDAR
                var json = JsonSerializer.Serialize(users, new JsonSerializerOptions
                {
                    WriteIndented = true    // JSON formateado (bonito) para legibilidad humana
                });
                File.WriteAllText(DataFile, json);
            }
            catch (Exception ex)
            {
                // MANEJO DE ERRORES
                // Si hay problemas de permisos, espacio en disco, etc.
                ShowModernMessage($"No se pudo guardar los usuarios: {ex.Message}", "Error de guardado", MessageBoxIcon.Error);
            }
        }
        #endregion

        #region MODELO DE DATOS
        // ==================================================================================
        // CLASE DE MODELO DE DATOS
        // ==================================================================================

        /// <summary>
        /// CLASE USUARIO
        /// Representa la estructura de datos de un usuario en la aplicación.
        /// Esta clase se usa tanto para almacenamiento en memoria como para serialización JSON.
        /// 
        /// PATRÓN: Esta es una clase POCO (Plain Old CLR Object) - simple contenedor de datos
        /// sin lógica de negocio compleja.
        /// </summary>
        private class User
        {
            /// <summary>Nombre completo del usuario</summary>
            public string Name { get; set; } = "";

            /// <summary>Dirección de correo electrónico del usuario</summary>
            public string Email { get; set; } = "";
        }
        #endregion
    }

    #region CONTROLES PERSONALIZADOS
    // ==================================================================================
    // CONTROLES PERSONALIZADOS CON ESTILO MODERNO
    // ==================================================================================
    // Las siguientes clases extienden controles estándar de WinForms para darles
    // un aspecto más moderno y consistente con diseños web actuales.

    /// <summary>
    /// TEXTBOX PERSONALIZADO CON PLACEHOLDER
    /// Extiende TextBox para agregar funcionalidad de texto de placeholder (como en HTML5)
    /// y aplicar estilos modernos con bordes sutiles.
    /// </summary>
    public class ModernTextBox : TextBox
    {
        #region CAMPOS PRIVADOS
        // Campo para almacenar el texto del placeholder
        private string placeholderText = "";
        // Flag para saber si actualmente se está mostrando el placeholder
        private bool isPlaceholderVisible = false;
        #endregion

        #region PROPIEDADES PÚBLICAS
        /// <summary>
        /// PROPIEDAD PLACEHOLDER TEXT
        /// Permite establecer y obtener el texto que se muestra cuando el campo está vacío.
        /// Redefine la propiedad heredada para agregar la lógica del placeholder.
        /// </summary>
        public new string PlaceholderText
        {
            get => placeholderText;
            set
            {
                placeholderText = value;
                UpdatePlaceholder(); // Actualizar inmediatamente la visualización
            }
        }
        #endregion

        #region CONSTRUCTOR
        /// <summary>
        /// Constructor del TextBox moderno
        /// Configura la apariencia inicial y registra los eventos necesarios para el placeholder
        /// </summary>
        public ModernTextBox()
        {
            // CONFIGURACIÓN DE APARIENCIA
            BorderStyle = BorderStyle.None;                 // Sin borde estándar (lo dibujaremos custom)
            BackColor = Color.White;                        // Fondo blanco
            ForeColor = Color.FromArgb(17, 24, 39);        // Texto gris oscuro
            Padding = new Padding(14, 8, 14, 8);           // Espaciado interno cómodo
            Multiline = true;                              // Permite líneas múltiples para evitar cortes de texto

            // REGISTRO DE EVENTOS PARA FUNCIONALIDAD DE PLACEHOLDER
            Enter += OnEnter;           // Cuando recibe el foco
            Leave += OnLeave;           // Cuando pierde el foco  
            TextChanged += OnTextChanged; // Cuando cambia el texto

            // Mostrar placeholder inicial si está configurado
            UpdatePlaceholder();
        }
        #endregion

        #region DIBUJO PERSONALIZADO
        /// <summary>
        /// DIBUJO DEL BORDE PERSONALIZADO
        /// Override del método OnPaint para dibujar un borde sutil en lugar del borde estándar de Windows
        /// </summary>
        /// <param name="e">Argumentos del evento de pintura con el contexto gráfico</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            // Llamar al método base para que se dibuje el contenido normal del TextBox
            base.OnPaint(e);

            // DIBUJAR BORDE PERSONALIZADO
            // Crear un pen (lápiz) con color gris claro y grosor de 1 pixel
            using (var pen = new Pen(Color.FromArgb(229, 231, 235), 1))
            {
                // Dibujar rectángulo alrededor del control (borde completo)
                // Width-1 y Height-1 porque el rectángulo incluye el borde en sus medidas
                e.Graphics.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);
            }
        }
        #endregion

        #region LÓGICA DEL PLACEHOLDER
        /// <summary>
        /// ACTUALIZAR VISUALIZACIÓN DEL PLACEHOLDER
        /// Muestra el texto del placeholder cuando el campo está vacío y no tiene foco
        /// </summary>
        private void UpdatePlaceholder()
        {
            // CONDICIONES PARA MOSTRAR PLACEHOLDER:
            // 1. El texto actual está vacío
            // 2. El control no tiene el foco (no está siendo editado)
            // 3. Hay un placeholder configurado
            if (string.IsNullOrEmpty(Text) && !Focused && !string.IsNullOrEmpty(placeholderText))
            {
                Text = placeholderText;                         // Mostrar texto del placeholder
                ForeColor = Color.FromArgb(156, 163, 175);     // Color gris claro (más sutil)
                isPlaceholderVisible = true;                    // Marcar que el placeholder está visible
            }
        }

        /// <summary>
        /// EVENTO: CUANDO EL CONTROL RECIBE EL FOCO
        /// Si se está mostrando el placeholder, lo ocultamos para que el usuario pueda escribir
        /// </summary>
        /// <param name="sender">El control que generó el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void OnEnter(object? sender, EventArgs e)
        {
            if (isPlaceholderVisible)
            {
                Text = "";                                      // Limpiar el texto del placeholder
                ForeColor = Color.FromArgb(17, 24, 39);        // Restaurar color normal del texto
                isPlaceholderVisible = false;                   // Marcar que el placeholder ya no está visible
            }
        }

        /// <summary>
        /// EVENTO: CUANDO EL CONTROL PIERDE EL FOCO
        /// Si el campo quedó vacío, mostramos nuevamente el placeholder
        /// </summary>
        /// <param name="sender">El control que generó el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void OnLeave(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Text))
            {
                UpdatePlaceholder(); // Mostrar placeholder si está vacío
            }
        }

        /// <summary>
        /// EVENTO: CUANDO CAMBIA EL TEXTO
        /// Si el usuario empieza a escribir mientras el placeholder está visible, lo ocultamos
        /// </summary>
        /// <param name="sender">El control que generó el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void OnTextChanged(object? sender, EventArgs e)
        {
            // Si el placeholder está visible y el texto cambió (usuario escribió algo)
            if (isPlaceholderVisible && Text != placeholderText)
            {
                ForeColor = Color.FromArgb(17, 24, 39);        // Cambiar a color normal
                isPlaceholderVisible = false;                   // El placeholder ya no está visible
            }
        }
        #endregion

        #region PROPIEDAD TEXT OVERRIDE
        /// <summary>
        /// OVERRIDE DE LA PROPIEDAD TEXT
        /// Personaliza el comportamiento de la propiedad Text para manejar correctamente el placeholder.
        /// Cuando se obtiene el Text y el placeholder está visible, devuelve cadena vacía.
        /// Cuando se establece el Text, usa el comportamiento normal.
        /// </summary>
        public new string Text
        {
            get => isPlaceholderVisible ? "" : base.Text;  // Si hay placeholder visible, devolver vacío
            set => base.Text = value;                       // Setter normal
        }
        #endregion
    }

    /// <summary>
    /// BOTÓN PERSONALIZADO CON EFECTOS HOVER
    /// Extiende Button para crear un botón con diseño moderno, bordes redondeados
    /// y efectos visuales cuando el mouse pasa sobre él.
    /// </summary>
    public class ModernButton : Button
    {
        #region CAMPOS PRIVADOS
        // Flag para rastrear si el mouse está sobre el botón
        private bool isHovered = false;
        #endregion

        #region CONSTRUCTOR
        /// <summary>
        /// Constructor del botón moderno
        /// Configura la apariencia inicial y registra eventos para efectos hover
        /// </summary>
        public ModernButton()
        {
            // CONFIGURACIÓN DE APARIENCIA BÁSICA
            FlatStyle = FlatStyle.Flat;                     // Estilo plano (sin efectos 3D)
            BackColor = Color.FromArgb(79, 70, 229);       // Color de fondo principal (indigo)
            ForeColor = Color.White;                        // Texto blanco
            FlatAppearance.BorderSize = 0;                 // Sin borde estándar
            Cursor = Cursors.Hand;                         // Cursor de mano al pasar sobre el botón

            // REGISTRO DE EVENTOS PARA EFECTOS HOVER
            MouseEnter += OnMouseEnter;    // Cuando el mouse entra al área del botón
            MouseLeave += OnMouseLeave;    // Cuando el mouse sale del área del botón
        }
        #endregion

        #region EVENTOS DE HOVER
        /// <summary>
        /// EVENTO: MOUSE ENTRA AL BOTÓN
        /// Cambia el color de fondo para dar feedback visual al usuario
        /// </summary>
        /// <param name="sender">El control que generó el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void OnMouseEnter(object? sender, EventArgs e)
        {
            isHovered = true;
            BackColor = Color.FromArgb(67, 56, 202);       // Color más oscuro para hover
        }

        /// <summary>
        /// EVENTO: MOUSE SALE DEL BOTÓN
        /// Restaura el color original del botón
        /// </summary>
        /// <param name="sender">El control que generó el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void OnMouseLeave(object? sender, EventArgs e)
        {
            isHovered = false;
            BackColor = Color.FromArgb(79, 70, 229);       // Color original
        }
        #endregion

        #region DIBUJO PERSONALIZADO
        /// <summary>
        /// DIBUJO PERSONALIZADO DEL BOTÓN
        /// Override del OnPaint para dibujar el botón con bordes redondeados
        /// y centrar el texto manualmente
        /// </summary>
        /// <param name="pevent">Argumentos del evento de pintura</param>
        protected override void OnPaint(PaintEventArgs pevent)
        {
            var rect = ClientRectangle;         // Área disponible para dibujar
            var g = pevent.Graphics;            // Contexto gráfico
            g.SmoothingMode = SmoothingMode.AntiAlias;  // Activar suavizado de bordes

            // DIBUJAR FONDO CON BORDES REDONDEADOS
            using (var brush = new SolidBrush(BackColor))       // Pincel con color de fondo actual
            using (var path = GetRoundedRect(rect, 6))          // Crear path de rectángulo redondeado
            {
                g.FillPath(brush, path);        // Rellenar el path con el color
            }

            // DIBUJAR TEXTO CENTRADO
            var textRect = rect;                // Copiar el rectángulo principal
            textRect.Inflate(-Padding.Horizontal / 2, -Padding.Vertical / 2);  // Aplicar padding

            // Usar TextRenderer para dibujar texto centrado
            TextRenderer.DrawText(g, Text, Font, textRect, ForeColor,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        /// <summary>
        /// CREAR RECTÁNGULO CON BORDES REDONDEADOS
        /// Método auxiliar que crea un GraphicsPath representando un rectángulo con esquinas redondeadas
        /// </summary>
        /// <param name="rect">Rectángulo base</param>
        /// <param name="radius">Radio de las esquinas redondeadas</param>
        /// <returns>Path gráfico del rectángulo redondeado</returns>
        private GraphicsPath GetRoundedRect(Rectangle rect, int radius)
        {
            var path = new GraphicsPath();

            // CREAR ARCOS PARA CADA ESQUINA
            // Esquina superior izquierda (180° a 270° = 90°)
            path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
            // Esquina superior derecha (270° a 360° = 90°)
            path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
            // Esquina inferior derecha (0° a 90° = 90°)
            path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
            // Esquina inferior izquierda (90° a 180° = 90°)
            path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);

            // Cerrar la figura para formar un rectángulo completo
            path.CloseAllFigures();
            return path;
        }
        #endregion
    }

    /// <summary>
    /// LISTBOX PERSONALIZADO CON ELEMENTOS ESTILO TARJETA
    /// Extiende ListBox para mostrar cada elemento como una tarjeta moderna
    /// con avatar circular, nombre en negritas y email en color secundario.
    /// </summary>
    public class ModernListBox : ListBox
    {
        #region CONSTRUCTOR
        /// <summary>
        /// Constructor del ListBox moderno
        /// Configura el modo de dibujo personalizado y propiedades visuales básicas
        /// </summary>
        public ModernListBox()
        {
            // CONFIGURACIÓN BÁSICA
            DrawMode = DrawMode.OwnerDrawFixed;     // Modo de dibujo personalizado con altura fija
            BorderStyle = BorderStyle.None;         // Sin borde estándar (lo dibujaremos nosotros)
            BackColor = Color.White;                // Fondo blanco
            ItemHeight = 60;                        // Altura de cada elemento (suficiente para avatar + texto)
            Font = new Font("Segoe UI", 9F);       // Fuente por defecto
        }
        #endregion

        #region DIBUJO DE ELEMENTOS
        /// <summary>
        /// DIBUJO PERSONALIZADO DE CADA ELEMENTO
        /// Este método se ejecuta para cada elemento visible en la lista.
        /// Crea un diseño tipo "tarjeta de contacto" con avatar, nombre y email.
        /// </summary>
        /// <param name="e">Argumentos del evento de dibujo con información del elemento</param>
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            // VALIDACIONES BÁSICAS
            // Salir si el índice no es válido (puede ocurrir durante redimensionamiento)
            if (e.Index < 0 || e.Index >= Items.Count) return;

            var g = e.Graphics;                     // Contexto gráfico
            g.SmoothingMode = SmoothingMode.AntiAlias;  // Suavizado para elementos circulares

            // OBTENER DATOS DEL ELEMENTO
            var itemText = Items[e.Index].ToString() ?? "";                    // Texto del elemento
            var isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;  // ¿Está seleccionado?

            // DIBUJAR FONDO DEL ELEMENTO
            // Color diferente si está seleccionado
            var bgColor = isSelected ? Color.FromArgb(243, 244, 246) : Color.White;  // Gris muy claro si está seleccionado
            using (var brush = new SolidBrush(bgColor))
            {
                g.FillRectangle(brush, e.Bounds);   // Rellenar toda el área del elemento
            }

            // EXTRAER NOMBRE Y EMAIL DEL TEXTO
            // El formato esperado es: "Nombre Completo <email@dominio.com>"
            var idx = itemText.LastIndexOf('<');    // Buscar el último '<'
            var name = itemText;                    // Por defecto, todo el texto es el nombre
            var email = "";                         // Email vacío por defecto

            if (idx > 0)  // Si se encontró el delimitador
            {
                name = itemText.Substring(0, idx).Trim();           // Parte antes de '<'
                email = itemText.Substring(idx + 1).TrimEnd('>');   // Parte después de '<', sin '>'
            }

            // DIBUJAR AVATAR CIRCULAR
            // Rectángulo para el avatar: margen izq. 16px, arriba 12px, tamaño 36x36px
            var avatarRect = new Rectangle(e.Bounds.X + 16, e.Bounds.Y + 12, 36, 36);
            using (var brush = new SolidBrush(Color.FromArgb(79, 70, 229)))  // Color principal (indigo)
            {
                g.FillEllipse(brush, avatarRect);   // Dibujar círculo relleno
            }

            // DIBUJAR INICIAL EN EL AVATAR
            if (!string.IsNullOrEmpty(name))
            {
                var initial = name[0].ToString().ToUpper();        // Primera letra en mayúsculas
                var avatarFont = new Font(Font.FontFamily, 12F, FontStyle.Bold);  // Fuente más grande y bold
                var textRect = avatarRect;                         // Usar el mismo rectángulo del avatar

                // Dibujar la inicial centrada en el avatar
                TextRenderer.DrawText(g, initial, avatarFont, textRect, Color.White,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            }

            // DIBUJAR NOMBRE DEL USUARIO
            // Rectángulo para el nombre: después del avatar, primera línea de texto
            var nameRect = new Rectangle(e.Bounds.X + 64, e.Bounds.Y + 12, e.Bounds.Width - 80, 20);
            var nameFont = new Font(Font, FontStyle.Bold);         // Fuente en negritas
            TextRenderer.DrawText(g, name, nameFont, nameRect, Color.FromArgb(17, 24, 39),  // Color de texto principal
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter);

            // DIBUJAR EMAIL DEL USUARIO
            // Rectángulo para el email: debajo del nombre, segunda línea de texto
            var emailRect = new Rectangle(e.Bounds.X + 64, e.Bounds.Y + 32, e.Bounds.Width - 80, 16);
            TextRenderer.DrawText(g, email, Font, emailRect, Color.FromArgb(107, 114, 128),  // Color secundario (gris)
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter);

            // DIBUJAR LÍNEA SEPARADORA ENTRE ELEMENTOS
            // Solo dibujar si no es el último elemento de la lista
            if (e.Index < Items.Count - 1)
            {
                using (var pen = new Pen(Color.FromArgb(243, 244, 246), 1))  // Línea muy sutil
                {
                    // Línea desde después del avatar hasta el borde derecho con margen
                    g.DrawLine(pen, e.Bounds.X + 64, e.Bounds.Bottom - 1, e.Bounds.Right - 16, e.Bounds.Bottom - 1);
                }
            }
        }
        #endregion

        #region DIBUJO DEL CONTENEDOR
        /// <summary>
        /// DIBUJO DEL BORDE DEL LISTBOX
        /// Override del OnPaint para dibujar un borde sutil alrededor de toda la lista
        /// </summary>
        /// <param name="e">Argumentos del evento de pintura</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            // Llamar al método base para que se dibuje el contenido normal
            base.OnPaint(e);

            // DIBUJAR BORDE EXTERIOR SUTIL
            using (var pen = new Pen(Color.FromArgb(229, 231, 235), 1))  // Borde gris claro
            {
                // Dibujar rectángulo alrededor de todo el control
                e.Graphics.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);
            }
        }
        #endregion
    }
    #endregion
}