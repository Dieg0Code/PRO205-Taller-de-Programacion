/**
 * Ejemplo: realizar una solicitud de chat (Chat Completions) usando el SDK oficial de OpenAI
 * con variables de entorno gestionadas por dotenv.
 *
 * Requisitos previos:
 * - Node.js 18 o superior (para contar con fetch y Web Streams nativos).
 * - Paquetes instalados: openai y dotenv
 *   > npm install openai dotenv
 * - Archivo .env en la raíz del proyecto con:
 *   OPENAI_API_KEY=tu_token
 *
 * Buenas prácticas:
 * - Nunca subas tu .env al repositorio (agrega ".env" a .gitignore).
 * - Mantén tu API key fuera del código fuente (usa process.env).
 */

// Carga automáticamente las variables definidas en el archivo .env hacia process.env.
// Debe ejecutarse lo antes posible en el ciclo de vida del programa, preferentemente en la primera línea,
// para que cualquier lectura posterior de process.env ya tenga los valores disponibles.
import "dotenv/config";

// Importa el cliente oficial de OpenAI. Este SDK expone clases y métodos para invocar modelos
// de lenguaje (chat/completions, embeddings, etc.) de forma tipada y sencilla.
import OpenAI from "openai";

// Variables de configuración:
// - token: se obtiene de la variable de entorno OPENAI_API_KEY. Si es undefined, la API rechazará la petición.
// - endpoint: URL base del proveedor que implementa el protocolo compatible (aquí, GitHub Models).
// - model: identificador del modelo a usar (en GitHub Models suele ser "<proveedor>/<modelo>").
const token = process.env.OPENAI_API_KEY; // Nunca escribas tu API key en el código; mantenla en .env
const endpoint = "https://models.github.ai/inference"; // Endpoint compatible con la API de OpenAI (GitHub Models)
const model = "openai/gpt-5-chat"; // Nombre del modelo (asegúrate de que exista/disponible en el proveedor)

// Función principal (asincrónica):
// - Crea el cliente de OpenAI apuntando al endpoint deseado.
// - Envía un mensaje de ejemplo en formato "chat" y muestra la respuesta por consola.
export async function main() {
  // Instanciamos el cliente. Nota:
  // - baseURL: permite redirigir el SDK al proveedor compatible (por defecto apuntaría a OpenAI).
  // - apiKey: credencial usada para autenticar la petición.
  const client = new OpenAI({
    baseURL: endpoint,
    apiKey: token,
  });

  // Realizamos la petición al modelo usando la API de chat.completions.
  // Este método recibe un objeto de configuración donde, típicamente, lo esencial es:
  // - messages: historial de la conversación en formato { role, content }.
  // - model: el identificador del modelo a consultar.
  // const ej = await client.responses.create({
  //   instructions:
  //     "Tu nombre es AIEP Bot, eres una inteligencia artificial diseñada para demostrar a los estudiantes el uso de la API de OpenAI. Todas tus respuestas deben tener un tono educativo, ser claras, concisas y terminar con la frase 'AIEP Osorno, 2025'",
  //   input: "Hola, me llamo Diego, tu quien eres?",
  //   model: model,
  // });

  const response = await client.chat.completions.create({
    /**
     * messages define la conversación que el modelo "ve".
     * Cada elemento tiene:
     * - role: "system" | "user" | "assistant"
     *   - system: establece reglas, tono y objetivos. El modelo tenderá a seguir estas instrucciones.
     *   - user: mensaje del usuario (preguntas, instrucciones, contexto).
     *   - assistant: mensajes previos del asistente (útil si recreas histórico).
     * - content: texto (u otros formatos si el proveedor lo soporta) que acompaña al rol.
     *
     * Consejos:
     * - Usa "system" para fijar identidad, estilo y límites del asistente.
     * - Mantén "user" claro y específico para respuestas más útiles.
     */
    messages: [
      {
        role: "system",
        content:
          "Tu nombre es AIEP Bot, eres una inteligencia artificial diseñada para demostrar a los estudiantes el uso de la API de OpenAI. Todas tus respuestas deben tener un tono educativo, ser claras, concisas y terminar con la frase 'AIEP Osorno, 2025'",
      },
      {
        role: "user",
        content: "Hola, me llamo Diego, tu quien eres?",
      },
    ],

    // model especifica el modelo a invocar. Debe coincidir con uno disponible en el endpoint configurado.
    model: model,

    /**
     * Parámetros opcionales útiles (no usados aquí, pero relevantes):
     * - temperature (0.0–2.0): mayor valor => respuestas más creativas/aleatorias; menor => más deterministas, mas robotico.
     * - max_tokens: tope de tokens en la respuesta. Útil para limitar longitud.
     * - top_p (nucleus sampling): alternativa a temperature para controlar diversidad.
     * - n: cantidad de respuestas a generar en paralelo (response.choices tendrá n elementos).
     * - tools / tool_choice: permite que el modelo invoque funciones/APIs externas (si el proveedor lo soporta).
     *
     * Nota sobre "tokens": no equivalen a caracteres ni palabras; son unidades de subtexto.
     * Controlar tokens ayuda a gestionar costos, latencia y longitud de salida.
     */
  });

  // Extraemos el contenido del primer candidato de respuesta.
  // La estructura típica es: response.choices[0].message.content
  // Si hubieras pedido n > 1, podrías iterar por response.choices para ver todas las variantes.
  const res = response.choices[0].message.content;
  console.log(res);
}

// Ejecutamos main() y registramos cualquier error de forma simple.
// Errores comunes:
// - API key inválida o ausente (verifica OPENAI_API_KEY en tu .env).
// - Endpoint incorrecto o conectividad de red.
// - Nombre de modelo inexistente o sin permisos.
main().catch((err) => {
  console.error("The sample encountered an error:", err);
});
