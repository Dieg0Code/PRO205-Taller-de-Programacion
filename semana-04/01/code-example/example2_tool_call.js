/**
 * Ejemplo 2: realizar una solicitud de chat con Tool Calls usando el SDK oficial de OpenAI
 * con variables de entorno gestionadas por dotenv.
 *
 * Requisitos previos:
 * - Node.js 18 o superior (para contar con fetch y Web Streams nativos).
 * - Paquetes instalados: openai y dotenv
 *   > npm install openai dotenv
 * - Archivo .env en la raíz del proyecto con:
 *   OPENAI_API_KEY=tu_token
 *
 * Diferencias con Ejemplo 1:
 * - Definimos herramientas (tools) que el modelo puede invocar
 * - Implementamos las funciones correspondientes
 * - Manejamos el flujo de tool calls y respuestas
 */

import "dotenv/config";
import OpenAI from "openai";

// Configuración idéntica al Ejemplo 1
const token = process.env.OPENAI_API_KEY;
const endpoint = "https://models.github.ai/inference";
const model = "openai/gpt-5-chat";

/**
 * FUNCIONES DE HERRAMIENTAS
 * Estas son las funciones reales que el modelo puede invocar.
 * Importante: el modelo NO ejecuta estas funciones directamente,
 * nosotros debemos interceptar sus "tool calls" y ejecutarlas.
 */

// Función 1: Calculadora básica
function calculator(operation, a, b) {
  const operations = {
    add: (x, y) => x + y,
    subtract: (x, y) => x - y,
    multiply: (x, y) => x * y,
    divide: (x, y) => (y !== 0 ? x / y : "Error: División por cero"),
  };

  return operations[operation]
    ? operations[operation](a, b)
    : "Operación no válida";
}

// Función 2: Obtener información del clima (simulada)
function getWeather(city) {
  // En un caso real, aquí harías una llamada a una API de clima real
  const weatherData = {
    santiago: { temp: 18, condition: "nublado", humidity: 65 },
    osorno: { temp: 12, condition: "lluvioso", humidity: 80 },
    madrid: { temp: 25, condition: "soleado", humidity: 40 },
    default: { temp: 20, condition: "parcialmente nublado", humidity: 55 },
  };

  const data = weatherData[city.toLowerCase()] || weatherData.default;
  return `En ${city}: ${data.temp}°C, ${data.condition}, humedad ${data.humidity}%`;
}

// Función auxiliar: ejecuta la función correspondiente basándose en el tool call
function executeFunction(functionName, args) {
  switch (functionName) {
    case "calculator":
      return calculator(args.operation, args.a, args.b);
    case "get_weather":
      return getWeather(args.city);
    default:
      return "Función no encontrada";
  }
}

export async function main() {
  const client = new OpenAI({
    baseURL: endpoint,
    apiKey: token,
  });

  // Definición de herramientas disponibles para el modelo
  const tools = [
    {
      type: "function",
      function: {
        name: "calculator",
        description: "Realiza operaciones matemáticas básicas",
        parameters: {
          type: "object",
          properties: {
            operation: {
              type: "string",
              enum: ["add", "subtract", "multiply", "divide"],
              description: "Tipo de operación matemática",
            },
            a: {
              type: "number",
              description: "Primer número",
            },
            b: {
              type: "number",
              description: "Segundo número",
            },
          },
          required: ["operation", "a", "b"],
        },
      },
    },
    {
      type: "function",
      function: {
        name: "get_weather",
        description: "Obtiene información del clima para una ciudad específica",
        parameters: {
          type: "object",
          properties: {
            city: {
              type: "string",
              description: "Nombre de la ciudad",
            },
          },
          required: ["city"],
        },
      },
    },
  ];

  // Historial de mensajes (se irá construyendo durante la conversación)
  let messages = [
    {
      role: "system",
      content:
        "Tu nombre es AIEP Bot, eres una inteligencia artificial diseñada para demostrar a los estudiantes el uso de Tool Calls con la API de OpenAI. Puedes realizar cálculos matemáticos y consultar el clima. Todas tus respuestas deben tener un tono educativo, ser claras, concisas y terminar con la frase 'AIEP Osorno, 2025'",
    },
    {
      role: "user",
      content:
        "Hola AIEP Bot, ¿cuánto es 25 por 8 y cómo está el clima en Osorno?",
    },
  ];

  console.log("=== SOLICITUD INICIAL ===");
  console.log("Usuario:", messages[1].content);

  // Primera llamada al modelo con las herramientas disponibles
  let response = await client.chat.completions.create({
    messages: messages,
    model: model,
    tools: tools,
    tool_choice: "auto", // El modelo decide si usar herramientas o no
  });

  const responseMessage = response.choices[0].message;
  console.log("\n=== RESPUESTA DEL MODELO ===");

  // Verificamos si el modelo quiere usar herramientas
  if (responseMessage.tool_calls) {
    console.log("El modelo solicita ejecutar herramientas:");

    // Agregamos la respuesta del modelo al historial
    messages.push(responseMessage);

    // Procesamos cada tool call
    for (const toolCall of responseMessage.tool_calls) {
      console.log(`\n- Tool Call ID: ${toolCall.id}`);
      console.log(`  Función: ${toolCall.function.name}`);
      console.log(`  Argumentos: ${toolCall.function.arguments}`);

      // Parseamos los argumentos JSON
      const args = JSON.parse(toolCall.function.arguments);

      // Ejecutamos la función correspondiente
      const functionResult = executeFunction(toolCall.function.name, args);
      console.log(`  Resultado: ${functionResult}`);

      // Agregamos el resultado de la herramienta al historial
      messages.push({
        role: "tool",
        tool_call_id: toolCall.id,
        content: String(functionResult),
      });
    }

    console.log("\n=== SEGUNDA LLAMADA AL MODELO ===");
    console.log("Enviando resultados de las herramientas al modelo...");

    // Segunda llamada al modelo con los resultados de las herramientas
    response = await client.chat.completions.create({
      messages: messages,
      model: model,
      tools: tools,
      tool_choice: "auto",
    });

    // Verificamos si hay más tool calls o una respuesta final
    const finalMessage = response.choices[0].message;

    if (finalMessage.tool_calls) {
      console.log("\n=== EL MODELO SOLICITA MÁS HERRAMIENTAS ===");
      console.log("Procesando herramientas adicionales...");

      // Agregamos este mensaje al historial
      messages.push(finalMessage);

      // Procesamos las nuevas tool calls
      for (const toolCall of finalMessage.tool_calls) {
        console.log(`\n- Tool Call ID: ${toolCall.id}`);
        console.log(`  Función: ${toolCall.function.name}`);
        console.log(`  Argumentos: ${toolCall.function.arguments}`);

        const args = JSON.parse(toolCall.function.arguments);
        const functionResult = executeFunction(toolCall.function.name, args);
        console.log(`  Resultado: ${functionResult}`);

        messages.push({
          role: "tool",
          tool_call_id: toolCall.id,
          content: String(functionResult),
        });
      }

      // Tercera llamada final
      console.log("\n=== TERCERA LLAMADA AL MODELO ===");
      response = await client.chat.completions.create({
        messages: messages,
        model: model,
        tools: tools,
        tool_choice: "auto",
      });
    }

    console.log("\n=== RESPUESTA FINAL ===");
    const finalContent = response.choices[0].message.content;
    console.log(finalContent || "El modelo no generó respuesta de texto final");
  } else {
    // Si el modelo no usó herramientas, mostramos su respuesta directa
    console.log("El modelo respondió sin usar herramientas:");
    console.log(responseMessage.content);
  }

  // Mostrar el historial completo para fines educativos
  console.log("\n=== HISTORIAL COMPLETO DE MENSAJES ===");
  messages.forEach((msg, index) => {
    console.log(`${index + 1}. Role: ${msg.role}`);
    if (msg.content) console.log(`   Content: ${msg.content}`);
    if (msg.tool_calls)
      console.log(
        `   Tool Calls: ${msg.tool_calls.length} herramientas solicitadas`
      );
    if (msg.tool_call_id) console.log(`   Tool Call ID: ${msg.tool_call_id}`);
    console.log("---");
  });
}

/**
 * CONCEPTOS CLAVE DEMOSTRADOS:
 *
 * 1. **Definición de Tools**: Especificamos qué funciones están disponibles y sus parámetros.
 *
 * 2. **Tool Choice**: "auto" permite al modelo decidir si usar herramientas o no.
 *
 * 3. **Flujo de Tool Calls**:
 *    - Primera llamada: el modelo puede solicitar usar herramientas
 *    - Ejecutamos las funciones solicitadas
 *    - Segunda llamada: enviamos los resultados al modelo
 *    - El modelo genera una respuesta final incorporando los resultados
 *
 * 4. **Roles de Mensaje**:
 *    - "system": instrucciones para el modelo
 *    - "user": mensaje del usuario
 *    - "assistant": respuesta del modelo (puede incluir tool_calls)
 *    - "tool": resultado de una herramienta ejecutada
 *
 * 5. **Manejo de Errores**: En producción, siempre valida argumentos y maneja excepciones.
 */

main().catch((err) => {
  console.error("El ejemplo encontró un error:", err);
});
