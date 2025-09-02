# 🚀 Guía Completa: API de OpenAI con TypeScript

_Tu guía definitiva para dominar los LLMs en producción_

---

## 📋 Tabla de Contenidos

1. [🛠️ Configuración Inicial](#configuración-inicial)
2. [🧠 Conceptos Fundamentales](#conceptos-fundamentales)
3. [💬 Chat Completions Básico](#chat-completions-básico)
4. [💰 Tokens y Pricing](#tokens-y-pricing)
5. [🔍 Embeddings](#embeddings)
6. [🔗 RAG (Retrieval Augmented Generation)](#rag-retrieval-augmented-generation)
7. [🛠️ Function/Tool Calling](#functiontool-calling)
8. [⚡ Streaming](#streaming)
9. [🖼️ Modelos Multimodales](#modelos-multimodales)
10. [⚡ Best Practices y Optimización](#best-practices-y-optimización)

---

## 🛠️ Configuración Inicial

### Instalación

Primero, instala las dependencias necesarias:

```bash
npm install openai
npm install -D @types/node typescript

# Para proyectos con embeddings y RAG
npm install @pinecone-database/pinecone
npm install chromadb

# Para procesamiento de documentos
npm install pdf-parse
npm install mammoth  # Para documentos Word
```

### Setup Básico

Crea tu archivo de configuración:

```typescript
import OpenAI from "openai";
import { config } from "dotenv";

// Cargar variables de entorno
config();

const openai = new OpenAI({
  apiKey: process.env.OPENAI_API_KEY,
  organization: process.env.OPENAI_ORG_ID, // Opcional
});

// Función para verificar la conexión
async function testConnection(): Promise<boolean> {
  try {
    const models = await openai.models.list();
    console.log("✅ Conexión exitosa a OpenAI");
    console.log(`📋 Modelos disponibles: ${models.data.length}`);
    return true;
  } catch (error) {
    console.error("❌ Error de conexión:", error);
    return false;
  }
}

// Exportar la instancia configurada
export { openai, testConnection };
```

### Variables de Entorno (.env)

Crea un archivo `.env` en la raíz de tu proyecto:

```env
OPENAI_API_KEY=sk-tu-api-key-aqui
OPENAI_ORG_ID=org-tu-org-id-aqui
```

### Verificación de Setup

```typescript
// test-setup.ts
import { testConnection } from "./config";

async function main() {
  const isConnected = await testConnection();
  if (isConnected) {
    console.log("🎉 ¡Todo configurado correctamente!");
  } else {
    console.log("❌ Revisa tu configuración");
  }
}

main().catch(console.error);
```

---

## 🧠 Conceptos Fundamentales

### ¿Qué son los Tokens?

Los **tokens** son la unidad básica de procesamiento de texto en los LLMs. No son exactamente palabras, sino fragmentos más pequeños:

```typescript
// Aproximaciones para entender tokens (no exactas)
// 1 token ≈ 4 caracteres en inglés
// 1 token ≈ 3/4 de una palabra en promedio
// 100 tokens ≈ 75 palabras aproximadamente

const ejemploTexto = "Hola mundo desde TypeScript!";
// Esto serían aproximadamente 6-7 tokens:
// ["Hola", " mundo", " desde", " Type", "Script", "!"]
```

### ¿Por qué importan los tokens?

1. **Costo**: Se cobra por token (entrada + salida)
2. **Límites**: Cada modelo tiene un límite máximo de tokens
3. **Performance**: Menos tokens = respuestas más rápidas

### Tipos de Modelos OpenAI (2025)

```typescript
// Modelos principales disponibles
type OpenAIModel =
  | "gpt-4o" // Multimodal, más rápido y económico
  | "gpt-4o-mini" // Pequeño, muy económico
  | "gpt-4-turbo" // Ventana de contexto grande (128k)
  | "gpt-3.5-turbo" // Clásico, económico
  | "o1-preview" // Modelo de razonamiento avanzado
  | "o1-mini" // Razonamiento económico
  | "text-embedding-3-large" // Embeddings de alta calidad
  | "text-embedding-3-small" // Embeddings económicos
  | "dall-e-3" // Generación de imágenes
  | "tts-1" // Text-to-speech
  | "whisper-1"; // Speech-to-text

// Límites de contexto aproximados (tokens)
const modelLimits: Record<string, number> = {
  "gpt-4o": 128000,
  "gpt-4o-mini": 128000,
  "gpt-4-turbo": 128000,
  "gpt-3.5-turbo": 4096,
  "o1-preview": 128000,
  "o1-mini": 128000,
};
```

### Roles en los Mensajes

```typescript
interface ChatMessage {
  role: "system" | "user" | "assistant" | "tool";
  content: string;
  name?: string; // Para identificar tools
  tool_calls?: any; // Para function calling
}

// Ejemplos de uso de cada rol:

// SYSTEM: Define el comportamiento del asistente
const systemMessage: ChatMessage = {
  role: "system",
  content:
    "Eres un experto desarrollador TypeScript que explica conceptos de forma clara y práctica.",
};

// USER: Mensaje del usuario
const userMessage: ChatMessage = {
  role: "user",
  content: "¿Cómo implemento autenticación JWT en Node.js?",
};

// ASSISTANT: Respuesta del modelo
const assistantMessage: ChatMessage = {
  role: "assistant",
  content: "Te explico paso a paso cómo implementar JWT...",
};

// TOOL: Resultado de una función ejecutada
const toolMessage: ChatMessage = {
  role: "tool",
  name: "search_database",
  content: "Encontré 5 usuarios activos en la base de datos...",
};
```

### Parámetros Importantes

```typescript
interface ChatCompletionOptions {
  model: string;
  messages: ChatMessage[];

  // Creatividad: 0 = determinístico, 2 = muy creativo
  temperature?: number; // Default: 1

  // Máximo tokens en la respuesta
  max_tokens?: number; // Default: inf

  // Penalización por repetir palabras
  frequency_penalty?: number; // -2.0 a 2.0, default: 0

  // Penalización por usar palabras comunes
  presence_penalty?: number; // -2.0 a 2.0, default: 0

  // Candidatos alternativos (usa solo 1 en producción)
  n?: number; // Default: 1

  // Streaming de respuesta
  stream?: boolean; // Default: false

  // Parar generación en estas secuencias
  stop?: string | string[];

  // ID para tracking
  user?: string;
}

// Ejemplo de configuración típica para diferentes casos:

// Para respuestas determinísticas (documentación, análisis)
const deterministicConfig = {
  temperature: 0.1,
  max_tokens: 1000,
  frequency_penalty: 0.1,
};

// Para contenido creativo (writing, brainstorming)
const creativeConfig = {
  temperature: 0.8,
  max_tokens: 2000,
  presence_penalty: 0.2,
};

// Para código (preciso pero con algo de flexibilidad)
const codeConfig = {
  temperature: 0.3,
  max_tokens: 1500,
  frequency_penalty: 0.1,
};
```

---

## 💬 Chat Completions Básico

### Tu Primera Llamada a la API

Empezemos con el ejemplo más simple:

```typescript
import { openai } from "./config";

async function simpleChat(prompt: string): Promise<string> {
  try {
    const completion = await openai.chat.completions.create({
      model: "gpt-4o-mini",
      messages: [{ role: "user", content: prompt }],
      max_tokens: 500,
      temperature: 0.7,
    });

    return completion.choices[0]?.message?.content || "Sin respuesta";
  } catch (error) {
    console.error("Error en chat:", error);
    throw error;
  }
}

// Ejemplo de uso
async function ejemploBasico() {
  const pregunta = "Explica qué es TypeScript en 2 párrafos";
  const respuesta = await simpleChat(pregunta);
  console.log("🤖 Respuesta:", respuesta);
}

// ejecutar: ejemploBasico().catch(console.error);
```

### Añadir Context con Mensaje System

El mensaje `system` define cómo debe comportarse el asistente:

```typescript
async function chatConContexto(
  prompt: string,
  contexto?: string
): Promise<string> {
  const messages: ChatMessage[] = [
    {
      role: "system",
      content:
        contexto ||
        "Eres un asistente experto en programación que da respuestas claras y prácticas.",
    },
    {
      role: "user",
      content: prompt,
    },
  ];

  const completion = await openai.chat.completions.create({
    model: "gpt-4o-mini",
    messages,
    temperature: 0.3,
    max_tokens: 800,
  });

  return completion.choices[0]?.message?.content || "Sin respuesta";
}

// Ejemplos con diferentes contextos
async function ejemplosConContexto() {
  // Asistente de código
  const respuestaCodigo = await chatConContexto(
    "¿Cómo creo una API REST con Express?",
    "Eres un senior backend developer especializado en Node.js. Proporciona código completo y explicaciones detalladas."
  );

  // Asistente para principiantes
  const respuestaPrincipiante = await chatConContexto(
    "¿Qué es una API REST?",
    "Eres un profesor que explica conceptos técnicos de forma simple y didáctica para estudiantes principiantes."
  );

  // Asistente formal
  const respuestaFormal = await chatConContexto(
    "Análisis de arquitectura de microservicios",
    "Eres un arquitecto de software senior. Responde de forma técnica y profesional, incluyendo pros, contras y mejores prácticas."
  );

  console.log("💻 Código:", respuestaCodigo);
  console.log("🎓 Principiante:", respuestaPrincipiante);
  console.log("🏢 Formal:", respuestaFormal);
}
```

### Sistema de Conversación con Historial

Para mantener contexto entre múltiples intercambios:

```typescript
class ChatSession {
  private messages: ChatMessage[] = [];
  private model: string;
  private defaultConfig: any;

  constructor(
    systemPrompt: string = "Eres un asistente útil y conocedor",
    model: string = "gpt-4o-mini"
  ) {
    this.model = model;
    this.messages.push({
      role: "system",
      content: systemPrompt,
    });

    this.defaultConfig = {
      temperature: 0.7,
      max_tokens: 1000,
      frequency_penalty: 0.1,
    };
  }

  async sendMessage(content: string): Promise<string> {
    // Agregar mensaje del usuario al historial
    this.messages.push({
      role: "user",
      content,
    });

    try {
      const completion = await openai.chat.completions.create({
        model: this.model,
        messages: this.messages,
        ...this.defaultConfig,
      });

      const response =
        completion.choices[0]?.message?.content || "Sin respuesta";

      // Agregar respuesta al historial
      this.messages.push({
        role: "assistant",
        content: response,
      });

      return response;
    } catch (error) {
      console.error("Error en conversación:", error);
      // Remover el último mensaje del usuario si hay error
      this.messages.pop();
      throw error;
    }
  }

  // Obtener historial completo
  getHistory(): ChatMessage[] {
    return [...this.messages]; // Copia para evitar mutaciones
  }

  // Limpiar historial pero mantener system prompt
  clearHistory(): void {
    const systemMessage = this.messages.find((m) => m.role === "system");
    this.messages = systemMessage ? [systemMessage] : [];
  }

  // Obtener estadísticas de la conversación
  getStats(): {
    totalMessages: number;
    userMessages: number;
    assistantMessages: number;
    estimatedTokens: number;
  } {
    const userMessages = this.messages.filter((m) => m.role === "user").length;
    const assistantMessages = this.messages.filter(
      (m) => m.role === "assistant"
    ).length;
    const estimatedTokens = this.messages
      .map((m) => Math.ceil(m.content.length / 4))
      .reduce((a, b) => a + b, 0);

    return {
      totalMessages: this.messages.length,
      userMessages,
      assistantMessages,
      estimatedTokens,
    };
  }

  // Cambiar configuración
  updateConfig(newConfig: Partial<typeof this.defaultConfig>): void {
    this.defaultConfig = { ...this.defaultConfig, ...newConfig };
  }
}
```

### Manejo de Errores Robusto

```typescript
class RobustChatClient {
  private maxRetries = 3;
  private baseDelay = 1000; // 1 segundo

  async chatWithRetry(
    messages: ChatMessage[],
    options: any = {},
    retries = this.maxRetries
  ): Promise<string> {
    try {
      const completion = await openai.chat.completions.create({
        model: "gpt-4o-mini",
        messages,
        ...options,
      });

      return completion.choices[0]?.message?.content || "Sin respuesta";
    } catch (error: any) {
      console.log(
        `❌ Error en intento ${this.maxRetries - retries + 1}:`,
        error.message
      );

      if (retries <= 0) {
        throw new Error(
          `Error después de ${this.maxRetries} intentos: ${error.message}`
        );
      }

      // Diferentes estrategias según el error
      if (error.status === 429) {
        // Rate limit - esperar más tiempo
        const delay = this.baseDelay * (this.maxRetries - retries + 1) * 2;
        console.log(`⏳ Rate limit alcanzado. Esperando ${delay}ms...`);
        await this.sleep(delay);
      } else if (error.status >= 500) {
        // Error del servidor - reintentar con backoff exponencial
        const delay = this.baseDelay * Math.pow(2, this.maxRetries - retries);
        console.log(`🔄 Error del servidor. Reintentando en ${delay}ms...`);
        await this.sleep(delay);
      } else {
        // Error del cliente (400-499) - no reintentar
        throw error;
      }

      return this.chatWithRetry(messages, options, retries - 1);
    }
  }

  private sleep(ms: number): Promise<void> {
    return new Promise((resolve) => setTimeout(resolve, ms));
  }

  // Wrapper fácil de usar
  async safeChat(prompt: string, systemPrompt?: string): Promise<string> {
    const messages: ChatMessage[] = [];

    if (systemPrompt) {
      messages.push({ role: "system", content: systemPrompt });
    }

    messages.push({ role: "user", content: prompt });

    return this.chatWithRetry(messages, {
      temperature: 0.7,
      max_tokens: 1000,
    });
  }
}
```

### Ejemplos Prácticos Completos

```typescript
// Ejemplo de uso de ChatSession
async function ejemploConversacion() {
  const chat = new ChatSession(
    "Eres un experto en TypeScript que ayuda a developers. Sé práctico y proporciona ejemplos de código.",
    "gpt-4o-mini"
  );

  console.log("🤖 Iniciando conversación...\n");

  // Primera pregunta
  const respuesta1 = await chat.sendMessage(
    "¿Cómo defino una interfaz en TypeScript?"
  );
  console.log("👤 Usuario: ¿Cómo defino una interfaz en TypeScript?");
  console.log("🤖 Asistente:", respuesta1, "\n");

  // Pregunta de seguimiento (mantiene contexto)
  const respuesta2 = await chat.sendMessage(
    "Dame un ejemplo más complejo con herencia de interfaces"
  );
  console.log(
    "👤 Usuario: Dame un ejemplo más complejo con herencia de interfaces"
  );
  console.log("🤖 Asistente:", respuesta2, "\n");

  // Mostrar estadísticas
  const stats = chat.getStats();
  console.log("📊 Estadísticas de la conversación:");
  console.log(`- Mensajes totales: ${stats.totalMessages}`);
  console.log(`- Mensajes del usuario: ${stats.userMessages}`);
  console.log(`- Respuestas del asistente: ${stats.assistantMessages}`);
  console.log(`- Tokens estimados: ${stats.estimatedTokens}`);
}

// Asistente especializado en código
async function asistenteCodigoChat() {
  const session = new ChatSession(
    `Eres un senior developer especializado en TypeScript, Node.js y React. 
    Cuando te pidan código, proporciona ejemplos completos, bien comentados y siguiendo mejores prácticas.
    Explica brevemente cada parte importante.`,
    "gpt-4o"
  );

  const respuesta = await session.sendMessage(`
    Necesito crear un middleware de autenticación JWT para Express que:
    1. Verifique el token en el header Authorization
    2. Decodifique el payload
    3. Añada la info del usuario a req.user
    4. Maneje errores apropiadamente
  `);

  console.log("💻 Código generado:", respuesta);
}

// Ejecutar todos los ejemplos
async function ejecutarEjemplos() {
  console.log("=".repeat(50));
  console.log("🚀 Ejecutando ejemplos de Chat Completions");
  console.log("=".repeat(50));

  try {
    await ejemploBasico();
    console.log("\n" + "-".repeat(30) + "\n");

    await ejemploConversacion();
    console.log("\n" + "-".repeat(30) + "\n");

    const robustClient = new RobustChatClient();
    const respuesta = await robustClient.safeChat(
      "Explica el patrón Observer en JavaScript",
      "Eres un experto en patrones de diseño. Proporciona ejemplos prácticos."
    );
    console.log("✅ Respuesta robusta:", respuesta);
  } catch (error) {
    console.error("❌ Error ejecutando ejemplos:", error);
  }
}

// Descomenta para probar:
// ejecutarEjemplos().catch(console.error);
```

---

## 💰 Tokens y Pricing

### Entendiendo los Tokens

Los tokens son fragmentos de texto que el modelo procesa. No equivalen exactamente a palabras:

```typescript
// Ejemplos de tokenización aproximada:
const ejemplos = {
  Hola: 1, // ["Hola"]
  "Hello world": 2, // ["Hello", " world"]
  TypeScript: 2, // ["Type", "Script"]
  "JavaScript rocks!": 3, // ["JavaScript", " rocks", "!"]
  "console.log()": 4, // ["console", ".", "log", "()"]
  función: 2, // ["fun", "ción"] (caracteres especiales)
  "🚀": 1, // ["🚀"] (emojis = 1 token)
  "   espacios   ": 3, // ["   ", "espacios", "   "]
  "123456": 1, // ["123456"] (números)
  "<div>Hello</div>": 5, // ["<", "div", ">Hello</", "div", ">"]
};
```

### Conteo Preciso de Tokens

Para conteo exacto, usa la librería `tiktoken`:

```bash
npm install tiktoken
```

```typescript
import { encoding_for_model } from "tiktoken";

class TokenCounter {
  private encoder: any;

  constructor(model: string = "gpt-4o") {
    this.encoder = encoding_for_model(model);
  }

  // Contar tokens de un texto
  count(text: string): number {
    const tokens = this.encoder.encode(text);
    return tokens.length;
  }

  // Contar tokens de una conversación completa
  countConversation(messages: ChatMessage[]): number {
    let totalTokens = 0;

    // Cada mensaje tiene overhead de tokens por metadatos
    for (const message of messages) {
      totalTokens += 3; // Overhead por mensaje (role, content, etc)
      totalTokens += this.count(message.content);

      if (message.role === "system") {
        totalTokens += 1; // Overhead adicional para system
      }
    }

    totalTokens += 3; // Overhead final para la respuesta

    return totalTokens;
  }

  // Estimar tokens para respuesta esperada
  estimateResponse(
    promptTokens: number,
    targetLength: "short" | "medium" | "long"
  ): number {
    const estimates = {
      short: Math.min(promptTokens * 0.5, 150), // ~100-150 tokens
      medium: Math.min(promptTokens * 1.0, 500), // ~300-500 tokens
      long: Math.min(promptTokens * 2.0, 1500), // ~800-1500 tokens
    };

    return Math.ceil(estimates[targetLength]);
  }

  // Truncar texto para que no exceda límite de tokens
  truncate(text: string, maxTokens: number): string {
    const tokens = this.encoder.encode(text);

    if (tokens.length <= maxTokens) {
      return text;
    }

    const truncatedTokens = tokens.slice(0, maxTokens - 3); // -3 para "..."
    const truncatedText = this.encoder.decode(truncatedTokens);

    return truncatedText + "...";
  }

  // Dividir texto en chunks que no excedan límite
  chunk(
    text: string,
    maxTokensPerChunk: number,
    overlap: number = 50
  ): string[] {
    const tokens = this.encoder.encode(text);
    const chunks: string[] = [];

    let start = 0;
    while (start < tokens.length) {
      const end = Math.min(start + maxTokensPerChunk, tokens.length);
      const chunkTokens = tokens.slice(start, end);
      chunks.push(this.encoder.decode(chunkTokens));

      start = end - overlap; // Overlap para mantener contexto
    }

    return chunks;
  }

  // Limpiar recursos
  free(): void {
    this.encoder.free();
  }
}

// Función auxiliar más simple para uso rápido
function countTokens(text: string, model: string = "gpt-4o"): number {
  const counter = new TokenCounter(model);
  const count = counter.count(text);
  counter.free();
  return count;
}

// Ejemplos de uso
async function ejemplosConteoTokens() {
  const counter = new TokenCounter("gpt-4o");

  const texto = `
    Desarrollar una aplicación web moderna con TypeScript, React y Node.js 
    requiere entender varios conceptos fundamentales y mejores prácticas.
  `;

  console.log(`📊 Tokens en el texto: ${counter.count(texto)}`);

  const conversacion: ChatMessage[] = [
    { role: "system", content: "Eres un experto en desarrollo web." },
    { role: "user", content: "Explica qué es React" },
    { role: "assistant", content: "React es una librería de JavaScript..." },
  ];

  console.log(
    `💬 Tokens en conversación: ${counter.countConversation(conversacion)}`
  );

  // Truncar texto largo
  const textoLargo = "Lorem ipsum ".repeat(100);
  const textoTruncado = counter.truncate(textoLargo, 50);
  console.log(`✂️ Texto truncado: ${textoTruncado}`);

  // Dividir en chunks
  const chunks = counter.chunk(textoLargo, 30, 5);
  console.log(`📦 Chunks creados: ${chunks.length}`);

  counter.free();
}
```

### Precios Actualizados (2025)

```typescript
// Precios por 1K tokens (USD) - Actualizado para 2025
const PRICING_2025 = {
  // Modelos de Chat
  "gpt-4o": {
    input: 0.005, // $5 por 1M tokens input
    output: 0.015, // $15 por 1M tokens output
  },
  "gpt-4o-mini": {
    input: 0.00015, // $0.15 por 1M tokens input
    output: 0.0006, // $0.60 por 1M tokens output
  },
  "gpt-4-turbo": {
    input: 0.01, // $10 por 1M tokens input
    output: 0.03, // $30 por 1M tokens output
  },
  "gpt-3.5-turbo": {
    input: 0.0005, // $0.50 por 1M tokens input
    output: 0.0015, // $1.50 por 1M tokens output
  },
  "o1-preview": {
    input: 0.015, // $15 por 1M tokens input
    output: 0.06, // $60 por 1M tokens output
  },
  "o1-mini": {
    input: 0.003, // $3 por 1M tokens input
    output: 0.012, // $12 por 1M tokens output
  },

  // Modelos de Embedding
  "text-embedding-3-large": {
    input: 0.00013, // $0.13 por 1M tokens
    output: 0, // No output cost
  },
  "text-embedding-3-small": {
    input: 0.00002, // $0.02 por 1M tokens
    output: 0,
  },

  // Otros modelos
  "dall-e-3": {
    "1024x1024": 0.04, // $0.040 por imagen
    "1024x1792": 0.08, // $0.080 por imagen
    "1792x1024": 0.08, // $0.080 por imagen
  },
  "tts-1": {
    input: 0.015, // $15 por 1M caracteres
    output: 0,
  },
  "whisper-1": {
    input: 0.006, // $0.006 por minuto
    output: 0,
  },
} as const;

class CostCalculator {
  private pricing = PRICING_2025;

  // Calcular costo de chat completion
  calculateChatCost(
    model: keyof typeof PRICING_2025,
    inputTokens: number,
    outputTokens: number
  ): number {
    const modelPricing = this.pricing[model];

    if (
      !modelPricing ||
      typeof modelPricing !== "object" ||
      !("input" in modelPricing)
    ) {
      throw new Error(`Modelo no soportado: ${String(model)}`);
    }

    const inputCost = (inputTokens / 1000) * modelPricing.input;
    const outputCost = (outputTokens / 1000) * modelPricing.output;

    return inputCost + outputCost;
  }

  // Calcular costo de embeddings
  calculateEmbeddingCost(
    model: "text-embedding-3-large" | "text-embedding-3-small",
    tokens: number
  ): number {
    const modelPricing = this.pricing[model];
    return (tokens / 1000) * modelPricing.input;
  }

  // Comparar costos entre modelos
  compareModels(
    inputTokens: number,
    outputTokens: number
  ): Array<{ model: string; cost: number; costPer1k: number }> {
    const chatModels = [
      "gpt-4o",
      "gpt-4o-mini",
      "gpt-4-turbo",
      "gpt-3.5-turbo",
      "o1-preview",
      "o1-mini",
    ] as const;

    return chatModels
      .map((model) => {
        const cost = this.calculateChatCost(model, inputTokens, outputTokens);
        const costPer1k = (cost / (inputTokens + outputTokens)) * 1000;

        return {
          model,
          cost: Math.round(cost * 100000) / 100000, // 5 decimales
          costPer1k: Math.round(costPer1k * 100000) / 100000,
        };
      })
      .sort((a, b) => a.cost - b.cost);
  }

  // Estimar costo mensual
  estimateMonthlyCost(
    dailyRequests: number,
    avgInputTokens: number,
    avgOutputTokens: number,
    model: keyof typeof PRICING_2025
  ): {
    daily: number;
    monthly: number;
    yearly: number;
  } {
    const costPerRequest = this.calculateChatCost(
      model,
      avgInputTokens,
      avgOutputTokens
    );
    const dailyCost = dailyRequests * costPerRequest;

    return {
      daily: Math.round(dailyCost * 100) / 100,
      monthly: Math.round(dailyCost * 30 * 100) / 100,
      yearly: Math.round(dailyCost * 365 * 100) / 100,
    };
  }

  // Recomendar modelo basado en presupuesto
  recommendModel(
    maxMonthlyCost: number,
    dailyRequests: number,
    avgInputTokens: number,
    avgOutputTokens: number
  ): Array<{
    model: string;
    monthlyCost: number;
    withinBudget: boolean;
  }> {
    const chatModels = [
      "gpt-4o-mini",
      "gpt-3.5-turbo",
      "gpt-4o",
      "gpt-4-turbo",
      "o1-mini",
      "o1-preview",
    ] as const;

    return chatModels
      .map((model) => {
        const estimates = this.estimateMonthlyCost(
          dailyRequests,
          avgInputTokens,
          avgOutputTokens,
          model
        );

        return {
          model,
          monthlyCost: estimates.monthly,
          withinBudget: estimates.monthly <= maxMonthlyCost,
        };
      })
      .sort((a, b) => a.monthlyCost - b.monthlyCost);
  }
}
```

### Sistema de Tracking de Costos

```typescript
interface UsageStats {
  totalRequests: number;
  totalInputTokens: number;
  totalOutputTokens: number;
  totalCost: number;
  modelUsage: Record<
    string,
    {
      requests: number;
      inputTokens: number;
      outputTokens: number;
      cost: number;
    }
  >;
}

class CostTracker {
  private stats: UsageStats;
  private calculator: CostCalculator;

  constructor() {
    this.stats = {
      totalRequests: 0,
      totalInputTokens: 0,
      totalOutputTokens: 0,
      totalCost: 0,
      modelUsage: {},
    };
    this.calculator = new CostCalculator();
  }

  // Registrar uso de un request
  trackRequest(model: string, inputTokens: number, outputTokens: number): void {
    const cost = this.calculator.calculateChatCost(
      model as any,
      inputTokens,
      outputTokens
    );

    // Actualizar stats globales
    this.stats.totalRequests++;
    this.stats.totalInputTokens += inputTokens;
    this.stats.totalOutputTokens += outputTokens;
    this.stats.totalCost += cost;

    // Actualizar stats por modelo
    if (!this.stats.modelUsage[model]) {
      this.stats.modelUsage[model] = {
        requests: 0,
        inputTokens: 0,
        outputTokens: 0,
        cost: 0,
      };
    }

    this.stats.modelUsage[model].requests++;
    this.stats.modelUsage[model].inputTokens += inputTokens;
    this.stats.modelUsage[model].outputTokens += outputTokens;
    this.stats.modelUsage[model].cost += cost;
  }

  // Obtener estadísticas
  getStats(): UsageStats & {
    avgCostPerRequest: number;
    avgTokensPerRequest: number;
    mostUsedModel: string;
    mostExpensiveModel: string;
  } {
    const avgCostPerRequest =
      this.stats.totalRequests > 0
        ? this.stats.totalCost / this.stats.totalRequests
        : 0;

    const avgTokensPerRequest =
      this.stats.totalRequests > 0
        ? (this.stats.totalInputTokens + this.stats.totalOutputTokens) /
          this.stats.totalRequests
        : 0;

    const models = Object.entries(this.stats.modelUsage);
    const mostUsedModel =
      models.reduce(
        (a, b) => (a[1].requests > b[1].requests ? a : b),
        models[0]
      )?.[0] || "ninguno";

    const mostExpensiveModel =
      models.reduce(
        (a, b) => (a[1].cost > b[1].cost ? a : b),
        models[0]
      )?.[0] || "ninguno";

    return {
      ...this.stats,
      avgCostPerRequest: Math.round(avgCostPerRequest * 10000) / 10000,
      avgTokensPerRequest: Math.round(avgTokensPerRequest),
      mostUsedModel,
      mostExpensiveModel,
    };
  }

  // Generar reporte detallado
  generateReport(): string {
    const stats = this.getStats();

    let report = `
# 📊 Reporte de Uso de OpenAI

## Resumen General
- **Total de requests:** ${stats.totalRequests.toLocaleString()}
- **Tokens de input:** ${stats.totalInputTokens.toLocaleString()}
- **Tokens de output:** ${stats.totalOutputTokens.toLocaleString()}
- **Costo total:** $${stats.totalCost.toFixed(4)}
- **Costo promedio por request:** $${stats.avgCostPerRequest.toFixed(4)}
- **Tokens promedio por request:** ${stats.avgTokensPerRequest}

## Uso por Modelo
`;

    Object.entries(stats.modelUsage)
      .sort((a, b) => b[1].cost - a[1].cost)
      .forEach(([model, usage]) => {
        report += `
### ${model}
- Requests: ${usage.requests.toLocaleString()}
- Input tokens: ${usage.inputTokens.toLocaleString()}
- Output tokens: ${usage.outputTokens.toLocaleString()}
- Costo: $${usage.cost.toFixed(4)}
- % del costo total: ${((usage.cost / stats.totalCost) * 100).toFixed(1)}%
`;
      });

    report += `
## Recomendaciones
- **Modelo más usado:** ${stats.mostUsedModel}
- **Modelo más caro:** ${stats.mostExpensiveModel}
- **Costo proyectado mensual:** $${(stats.totalCost * 30).toFixed(2)}
`;

    return report;
  }

  // Limpiar estadísticas
  reset(): void {
    this.stats = {
      totalRequests: 0,
      totalInputTokens: 0,
      totalOutputTokens: 0,
      totalCost: 0,
      modelUsage: {},
    };
  }

  // Guardar stats en archivo JSON
  saveToFile(filename: string = "openai-usage-stats.json"): void {
    const fs = require("fs");
    fs.writeFileSync(filename, JSON.stringify(this.getStats(), null, 2));
    console.log(`💾 Estadísticas guardadas en ${filename}`);
  }

  // Cargar stats desde archivo
  loadFromFile(filename: string = "openai-usage-stats.json"): void {
    const fs = require("fs");
    if (fs.existsSync(filename)) {
      this.stats = JSON.parse(fs.readFileSync(filename, "utf8"));
      console.log(`📂 Estadísticas cargadas desde ${filename}`);
    }
  }
}
```

### Cliente OpenAI con Tracking Integrado

```typescript
class TrackedOpenAIClient {
  private client: OpenAI;
  private tracker: CostTracker;
  private tokenCounter: TokenCounter;

  constructor(apiKey: string) {
    this.client = new OpenAI({ apiKey });
    this.tracker = new CostTracker();
    this.tokenCounter = new TokenCounter();
  }

  async chat(
    messages: ChatMessage[],
    options: {
      model?: string;
      temperature?: number;
      max_tokens?: number;
    } = {}
  ): Promise<{
    response: string;
    usage: {
      inputTokens: number;
      outputTokens: number;
      cost: number;
    };
  }> {
    const model = options.model || "gpt-4o-mini";

    // Contar tokens de input
    const inputTokens = this.tokenCounter.countConversation(messages);

    try {
      const completion = await this.client.chat.completions.create({
        model,
        messages,
        temperature: options.temperature || 0.7,
        max_tokens: options.max_tokens || 1000,
      });

      const response = completion.choices[0]?.message?.content || "";
      const outputTokens = this.tokenCounter.count(response);

      // Registrar uso
      this.tracker.trackRequest(model, inputTokens, outputTokens);

      const calculator = new CostCalculator();
      const cost = calculator.calculateChatCost(
        model as any,
        inputTokens,
        outputTokens
      );

      return {
        response,
        usage: {
          inputTokens,
          outputTokens,
          cost,
        },
      };
    } catch (error) {
      console.error("Error en chat con tracking:", error);
      throw error;
    }
  }

  // Obtener estadísticas del tracker
  getUsageStats() {
    return this.tracker.getStats();
  }

  // Generar reporte
  generateUsageReport(): string {
    return this.tracker.generateReport();
  }

  // Verificar si se está dentro del presupuesto
  checkBudget(monthlyBudget: number): {
    withinBudget: boolean;
    currentSpend: number;
    projectedMonthly: number;
    daysRemaining: number;
  } {
    const stats = this.tracker.getStats();
    const now = new Date();
    const daysInMonth = new Date(
      now.getFullYear(),
      now.getMonth() + 1,
      0
    ).getDate();
    const currentDay = now.getDate();
    const daysRemaining = daysInMonth - currentDay;

    const dailyAverage = stats.totalCost / currentDay;
    const projectedMonthly = dailyAverage * daysInMonth;

    return {
      withinBudget: projectedMonthly <= monthlyBudget,
      currentSpend: stats.totalCost,
      projectedMonthly,
      daysRemaining,
    };
  }

  // Limpiar recursos
  cleanup(): void {
    this.tokenCounter.free();
  }
}
```

### Ejemplos Prácticos

```typescript
async function ejemplosCostosCompletos() {
  const calculator = new CostCalculator();

  console.log("💰 Calculadora de Costos - Ejemplos\n");

  // 1. Comparar modelos para una tarea típica
  console.log("📊 Comparación de modelos (1000 input, 500 output tokens):");
  const comparison = calculator.compareModels(1000, 500);
  comparison.forEach(({ model, cost, costPer1k }) => {
    console.log(
      `- ${model}: $${cost.toFixed(4)} ($${costPer1k.toFixed(4)}/1k tokens)`
    );
  });

  // 2. Estimación mensual
  console.log("\n📅 Estimación mensual (100 requests/día):");
  const monthlyEstimate = calculator.estimateMonthlyCost(
    100,
    800,
    400,
    "gpt-4o-mini"
  );
  console.log(`- Diario: $${monthlyEstimate.daily}`);
  console.log(`- Mensual: $${monthlyEstimate.monthly}`);
  console.log(`- Anual: $${monthlyEstimate.yearly}`);

  // 3. Recomendación de modelo
  console.log("\n🎯 Modelos recomendados para presupuesto $50/mes:");
  const recommendations = calculator.recommendModel(50, 100, 800, 400);
  recommendations.forEach(({ model, monthlyCost, withinBudget }) => {
    const status = withinBudget ? "✅" : "❌";
    console.log(`${status} ${model}: $${monthlyCost}/mes`);
  });

  // 4. Ejemplo con cliente tracked
  console.log("\n🔍 Ejemplo con tracking:");
  const trackedClient = new TrackedOpenAIClient(process.env.OPENAI_API_KEY!);

  try {
    const result = await trackedClient.chat([
      { role: "user", content: "Explica qué es TypeScript en una oración." },
    ]);

    console.log(`- Respuesta: ${result.response}`);
    console.log(`- Tokens input: ${result.usage.inputTokens}`);
    console.log(`- Tokens output: ${result.usage.outputTokens}`);
    console.log(`- Costo: $${result.usage.cost.toFixed(6)}`);

    // Ver estadísticas acumuladas
    const stats = trackedClient.getUsageStats();
    console.log(`- Total gastado hasta ahora: $${stats.totalCost.toFixed(4)}`);
  } catch (error) {
    console.log("Error:", error);
  }

  trackedClient.cleanup();
}

// Ejecutar ejemplos
// ejemplosCostosCompletos().catch(console.error);
```

### Consejos para Optimizar Costos

```typescript
const OPTIMIZATION_TIPS = {
  // 1. Elegir el modelo correcto
  modelSelection: {
    simple: "gpt-4o-mini", // Tareas simples, Q&A básico
    balanced: "gpt-4o", // Balance calidad/precio
    complex: "gpt-4-turbo", // Tareas complejas, análisis profundo
    reasoning: "o1-mini", // Razonamiento matemático/lógico
    creative: "gpt-4o", // Contenido creativo
  },

  // 2. Optimización de tokens
  tokenOptimization: [
    "Usar prompts concisos y específicos",
    "Eliminar texto innecesario del contexto",
    "Truncar conversaciones largas manteniendo contexto relevante",
    "Usar stop sequences para evitar respuestas largas innecesarias",
    "Cachear respuestas comunes",
    "Usar temperature baja (0.1-0.3) para respuestas determinísticas",
  ],

  // 3. Estrategias de arquitectura
  architecture: [
    "Implementar cache de respuestas",
    "Usar rate limiting para controlar gastos",
    "Batch de requests cuando sea posible",
    "Implementar fallbacks a modelos más baratos",
    "Monitorear y alertar por gastos inusuales",
  ],
};

// Función para optimizar automáticamente una conversación
function optimizeConversation(
  messages: ChatMessage[],
  maxTokens: number = 8000
): ChatMessage[] {
  const counter = new TokenCounter();
  let currentTokens = counter.countConversation(messages);

  if (currentTokens <= maxTokens) {
    counter.free();
    return messages;
  }

  // Mantener siempre el system message
  const systemMessage = messages.find((m) => m.role === "system");
  let optimized: ChatMessage[] = systemMessage ? [systemMessage] : [];

  // Tomar los mensajes más recientes hasta llegar al límite
  const otherMessages = messages.filter((m) => m.role !== "system").reverse();

  for (const message of otherMessages) {
    const testMessages = [message, ...optimized];
    if (counter.countConversation(testMessages) <= maxTokens) {
      optimized.unshift(message);
    } else {
      break;
    }
  }

  counter.free();
  return optimized;
}

console.log("💡 Tips de optimización cargados");
console.log("🛠️ Función de optimización de conversaciones disponible");
```

---

## 🛠️ Function/Tool Calling

Function calling permite que los modelos ejecuten funciones específicas cuando lo necesiten. Es fundamental para crear agentes inteligentes que pueden interactuar con APIs, bases de datos y sistemas externos.

### Conceptos Fundamentales

Function calling funciona en 3 pasos:

1. **Definir** las funciones disponibles con JSON Schema
2. **El modelo decide** cuándo y cómo llamar las funciones
3. **Ejecutar** las funciones y devolver resultados al modelo

### Definiendo Funciones con JSON Schema

```typescript
// Esquemas de funciones disponibles
const toolSchemas = [
  {
    type: "function" as const,
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
          units: {
            type: "string",
            enum: ["celsius", "fahrenheit"],
            description: "Unidades de temperatura",
            default: "celsius",
          },
        },
        required: ["city"],
      },
    },
  },
  {
    type: "function" as const,
    function: {
      name: "calculate_math",
      description: "Realiza cálculos matemáticos básicos",
      parameters: {
        type: "object",
        properties: {
          expression: {
            type: "string",
            description:
              "Expresión matemática a evaluar (ej: '2 + 2', '10 * 5')",
          },
        },
        required: ["expression"],
      },
    },
  },
  {
    type: "function" as const,
    function: {
      name: "search_database",
      description: "Busca información en la base de datos de usuarios",
      parameters: {
        type: "object",
        properties: {
          query: {
            type: "string",
            description: "Término de búsqueda",
          },
          table: {
            type: "string",
            enum: ["users", "products", "orders"],
            description: "Tabla donde buscar",
          },
          limit: {
            type: "number",
            description: "Número máximo de resultados",
            minimum: 1,
            maximum: 100,
            default: 10,
          },
        },
        required: ["query", "table"],
      },
    },
  },
  {
    type: "function" as const,
    function: {
      name: "send_email",
      description: "Envía un email a un destinatario",
      parameters: {
        type: "object",
        properties: {
          to: {
            type: "string",
            description: "Email del destinatario",
          },
          subject: {
            type: "string",
            description: "Asunto del email",
          },
          body: {
            type: "string",
            description: "Contenido del email",
          },
          priority: {
            type: "string",
            enum: ["low", "normal", "high"],
            description: "Prioridad del email",
            default: "normal",
          },
        },
        required: ["to", "subject", "body"],
      },
    },
  },
];
```

### Implementando las Funciones

```typescript
// Implementaciones de las funciones
async function getWeather(
  city: string,
  units: string = "celsius"
): Promise<string> {
  // En producción, conectarías con una API real como OpenWeatherMap
  const mockWeatherData = {
    madrid: { temp: units === "celsius" ? 22 : 72, condition: "soleado" },
    barcelona: { temp: units === "celsius" ? 24 : 75, condition: "nublado" },
    valencia: { temp: units === "celsius" ? 26 : 79, condition: "lluvioso" },
    sevilla: { temp: units === "celsius" ? 28 : 82, condition: "soleado" },
  };

  const weather =
    mockWeatherData[city.toLowerCase() as keyof typeof mockWeatherData];

  if (!weather) {
    return `No tengo información del clima para ${city}. Ciudades disponibles: ${Object.keys(
      mockWeatherData
    ).join(", ")}`;
  }

  const unit = units === "celsius" ? "°C" : "°F";
  return `El clima en ${city} es ${weather.condition} con una temperatura de ${weather.temp}${unit}`;
}

function calculateMath(expression: string): string {
  try {
    // ⚠️ ADVERTENCIA: eval() es peligroso en producción
    // En un entorno real, usa una librería como math.js
    const sanitizedExpression = expression.replace(/[^0-9+\-*/.() ]/g, "");

    if (sanitizedExpression !== expression) {
      return `Expresión no válida: ${expression}. Solo se permiten números y operadores básicos (+, -, *, /, (), .)`;
    }

    const result = eval(sanitizedExpression);

    if (typeof result !== "number" || !isFinite(result)) {
      return `Error: El resultado no es un número válido`;
    }

    return `El resultado de ${expression} es ${result}`;
  } catch (error) {
    return `Error calculando ${expression}: ${
      error instanceof Error ? error.message : "Error desconocido"
    }`;
  }
}

async function searchDatabase(
  query: string,
  table: string,
  limit: number = 10
): Promise<string> {
  // Mock de base de datos
  const mockDatabase = {
    users: [
      { id: 1, name: "Juan Pérez", email: "juan@email.com", role: "developer" },
      {
        id: 2,
        name: "María García",
        email: "maria@email.com",
        role: "designer",
      },
      {
        id: 3,
        name: "Carlos López",
        email: "carlos@email.com",
        role: "manager",
      },
      { id: 4, name: "Ana Martín", email: "ana@email.com", role: "developer" },
      {
        id: 5,
        name: "Luis Rodríguez",
        email: "luis@email.com",
        role: "analyst",
      },
    ],
    products: [
      { id: 1, name: "Laptop HP", category: "electronics", price: 899 },
      { id: 2, name: "iPhone 15", category: "electronics", price: 1199 },
      { id: 3, name: "Escritorio", category: "furniture", price: 299 },
    ],
    orders: [
      { id: 1, user_id: 1, product_id: 1, status: "completed", total: 899 },
      { id: 2, user_id: 2, product_id: 2, status: "pending", total: 1199 },
    ],
  };

  const data = mockDatabase[table as keyof typeof mockDatabase] || [];

  const results = data
    .filter((item: any) =>
      Object.values(item).some((value) =>
        String(value).toLowerCase().includes(query.toLowerCase())
      )
    )
    .slice(0, limit);

  if (results.length === 0) {
    return `No se encontraron resultados para "${query}" en la tabla ${table}`;
  }

  return (
    `Encontrados ${results.length} resultados en ${table}:\n` +
    results
      .map(
        (item: any, index) => `${index + 1}. ${JSON.stringify(item, null, 2)}`
      )
      .join("\n")
  );
}

async function sendEmail(
  to: string,
  subject: string,
  body: string,
  priority: string = "normal"
): Promise<string> {
  // En producción, usarías un servicio real como SendGrid, AWS SES, etc.
  console.log(`📧 Enviando email:
  Para: ${to}
  Asunto: ${subject}
  Prioridad: ${priority}
  Contenido: ${body}`);

  // Simular envío
  await new Promise((resolve) => setTimeout(resolve, 1000));

  return `Email enviado exitosamente a ${to} con asunto "${subject}" (prioridad: ${priority})`;
}

// Mapa de funciones disponibles para ejecución
const availableFunctions: Record<string, Function> = {
  get_weather: getWeather,
  calculate_math: calculateMath,
  search_database: searchDatabase,
  send_email: sendEmail,
};
```

### Sistema de Function Calling Completo

```typescript
class FunctionCallingAgent {
  private messages: ChatMessage[] = [];
  private tools: any[];
  private availableFunctions: Record<string, Function>;

  constructor(
    systemPrompt: string = "Eres un asistente útil que puede usar herramientas cuando sea necesario.",
    tools: any[] = toolSchemas
  ) {
    this.messages = [{ role: "system", content: systemPrompt }];
    this.tools = tools;
    this.availableFunctions = availableFunctions;
  }

  async chat(userMessage: string): Promise<string> {
    // Agregar mensaje del usuario
    this.messages.push({ role: "user", content: userMessage });

    try {
      // Llamada inicial con tools disponibles
      const completion = await openai.chat.completions.create({
        model: "gpt-4o",
        messages: this.messages,
        tools: this.tools,
        tool_choice: "auto", // El modelo decide si usar tools
      });

      const assistantMessage = completion.choices[0]?.message;

      if (!assistantMessage) {
        throw new Error("No se recibió respuesta del modelo");
      }

      // Agregar mensaje del asistente
      this.messages.push(assistantMessage);

      // Verificar si el modelo quiere usar tools
      if (
        assistantMessage.tool_calls &&
        assistantMessage.tool_calls.length > 0
      ) {
        console.log(
          `🔧 El modelo quiere usar ${assistantMessage.tool_calls.length} herramienta(s)`
        );

        // Ejecutar cada tool call
        for (const toolCall of assistantMessage.tool_calls) {
          const functionName = toolCall.function.name;
          const functionArgs = JSON.parse(toolCall.function.arguments);

          console.log(
            `⚙️ Ejecutando ${functionName} con argumentos:`,
            functionArgs
          );

          try {
            const functionResult = await this.executeTool(
              functionName,
              functionArgs
            );

            // Agregar resultado del tool al historial
            this.messages.push({
              role: "tool",
              tool_call_id: toolCall.id,
              content: functionResult,
            });
          } catch (error) {
            console.error(`❌ Error ejecutando ${functionName}:`, error);

            this.messages.push({
              role: "tool",
              tool_call_id: toolCall.id,
              content: `Error ejecutando ${functionName}: ${
                error instanceof Error ? error.message : "Error desconocido"
              }`,
            });
          }
        }

        // Segunda llamada para que el modelo procese los resultados
        const finalCompletion = await openai.chat.completions.create({
          model: "gpt-4o",
          messages: this.messages,
        });

        const finalResponse =
          finalCompletion.choices[0]?.message?.content ||
          "No se pudo procesar los resultados";

        // Agregar respuesta final
        this.messages.push({
          role: "assistant",
          content: finalResponse,
        });

        return finalResponse;
      } else {
        // Respuesta directa sin tools
        return assistantMessage.content || "Sin respuesta";
      }
    } catch (error) {
      console.error("Error en function calling:", error);
      throw error;
    }
  }

  private async executeTool(functionName: string, args: any): Promise<string> {
    const fn = this.availableFunctions[functionName];

    if (!fn) {
      throw new Error(`Función no implementada: ${functionName}`);
    }

    // Ejecutar función con argumentos
    if (typeof fn !== "function") {
      throw new Error(`${functionName} no es una función válida`);
    }

    // Extraer argumentos en el orden correcto según la función
    switch (functionName) {
      case "get_weather":
        return await fn(args.city, args.units);

      case "calculate_math":
        return fn(args.expression);

      case "search_database":
        return await fn(args.query, args.table, args.limit);

      case "send_email":
        return await fn(args.to, args.subject, args.body, args.priority);

      default:
        throw new Error(`Handler no implementado para ${functionName}`);
    }
  }

  // Obtener historial de conversación
  getHistory(): ChatMessage[] {
    return [...this.messages];
  }

  // Limpiar historial pero mantener system prompt
  clearHistory(): void {
    const systemMessage = this.messages.find((m) => m.role === "system");
    this.messages = systemMessage ? [systemMessage] : [];
  }

  // Agregar nueva función disponible
  addFunction(schema: any, implementation: Function): void {
    this.tools.push(schema);
    this.availableFunctions[schema.function.name] = implementation;
  }

  // Obtener estadísticas de uso de tools
  getToolUsageStats(): Record<string, number> {
    const toolUsage: Record<string, number> = {};

    this.messages
      .filter((m) => m.role === "assistant" && m.tool_calls)
      .forEach((message) => {
        message.tool_calls?.forEach((toolCall) => {
          const functionName = toolCall.function.name;
          toolUsage[functionName] = (toolUsage[functionName] || 0) + 1;
        });
      });

    return toolUsage;
  }
}
```

### Structured Output con JSON Schema

Para obtener respuestas en formato JSON estructurado:

```typescript
// Definir schema para respuesta estructurada
const userProfileSchema = {
  name: "user_profile_analysis",
  description: "Analiza información de usuario y devuelve perfil estructurado",
  parameters: {
    type: "object",
    properties: {
      name: {
        type: "string",
        description: "Nombre completo del usuario",
      },
      age: {
        type: "number",
        description: "Edad estimada",
        minimum: 0,
        maximum: 120,
      },
      location: {
        type: "object",
        properties: {
          city: { type: "string" },
          country: { type: "string" },
        },
        required: ["city", "country"],
      },
      interests: {
        type: "array",
        items: { type: "string" },
        description: "Lista de intereses identificados",
      },
      profession: {
        type: "string",
        description: "Profesión o área de trabajo",
      },
      skillLevel: {
        type: "string",
        enum: ["beginner", "intermediate", "advanced", "expert"],
        description: "Nivel de habilidad en su área",
      },
      isActive: {
        type: "boolean",
        description: "Si parece ser un usuario activo",
      },
      contactPreference: {
        type: "string",
        enum: ["email", "phone", "chat", "none"],
        description: "Preferencia de contacto sugerida",
      },
    },
    required: ["name", "location", "interests", "isActive"],
  },
};

interface UserProfile {
  name: string;
  age?: number;
  location: {
    city: string;
    country: string;
  };
  interests: string[];
  profession?: string;
  skillLevel?: "beginner" | "intermediate" | "advanced" | "expert";
  isActive: boolean;
  contactPreference?: "email" | "phone" | "chat" | "none";
}

async function analyzeUserProfile(userText: string): Promise<UserProfile> {
  const completion = await openai.chat.completions.create({
    model: "gpt-4o",
    messages: [
      {
        role: "system",
        content:
          "Eres un experto analizador de perfiles de usuario. Extrae y estructura la información proporcionada.",
      },
      {
        role: "user",
        content: `Analiza este texto sobre un usuario y extrae la información relevante: "${userText}"`,
      },
    ],
    tools: [
      {
        type: "function",
        function: userProfileSchema,
      },
    ],
    tool_choice: {
      type: "function",
      function: { name: "user_profile_analysis" },
    },
  });

  const toolCall = completion.choices[0]?.message?.tool_calls?.[0];

  if (!toolCall) {
    throw new Error("No se pudo extraer el perfil del usuario");
  }

  const profileData = JSON.parse(toolCall.function.arguments);
  return profileData as UserProfile;
}

// Función para validar schema JSON
function validateJsonSchema(
  data: any,
  schema: any
): { valid: boolean; errors: string[] } {
  const errors: string[] = [];

  // Validación básica de required fields
  if (schema.required) {
    for (const field of schema.required) {
      if (!(field in data)) {
        errors.push(`Campo requerido faltante: ${field}`);
      }
    }
  }

  // Validación de tipos básicos
  for (const [key, value] of Object.entries(data)) {
    const fieldSchema = schema.properties?.[key];
    if (fieldSchema) {
      const actualType = typeof value;
      const expectedType = fieldSchema.type;

      if (expectedType === "array" && !Array.isArray(value)) {
        errors.push(`${key} debe ser un array`);
      } else if (expectedType !== "array" && actualType !== expectedType) {
        errors.push(
          `${key} debe ser de tipo ${expectedType}, recibido ${actualType}`
        );
      }
    }
  }

  return {
    valid: errors.length === 0,
    errors,
  };
}
```

### Ejemplos Prácticos Avanzados

```typescript
// 1. Agente de Análisis de Datos
async function dataAnalysisAgent() {
  const agent = new FunctionCallingAgent(
    `Eres un analista de datos experto. Puedes buscar información en bases de datos, 
    realizar cálculos y enviar reportes por email. Siempre analiza los datos antes de hacer conclusiones.`,
    toolSchemas
  );

  console.log("📊 Agente de Análisis de Datos iniciado\n");

  try {
    const response1 = await agent.chat(
      "Busca todos los desarrolladores en la base de datos y calcula cuántos son"
    );
    console.log("🔍 Búsqueda de desarrolladores:", response1, "\n");

    const response2 = await agent.chat(
      "Ahora busca los productos de electrónicos y calcula el precio promedio"
    );
    console.log("💻 Análisis de productos:", response2, "\n");

    const response3 = await agent.chat(
      "Envía un reporte por email a admin@company.com con un resumen de los hallazgos"
    );
    console.log("📧 Envío de reporte:", response3, "\n");
  } catch (error) {
    console.error("Error en análisis:", error);
  }
}

// 2. Asistente Personal Inteligente
async function personalAssistant() {
  const assistant = new FunctionCallingAgent(
    `Eres un asistente personal eficiente. Ayudas con tareas como consultar el clima, 
    hacer cálculos, buscar información y gestionar comunicaciones. Siempre confirma antes de enviar emails.`,
    toolSchemas
  );

  console.log("🤖 Asistente Personal iniciado\n");

  const response = await assistant.chat(
    `Hola! Necesito que me ayudes con varias cosas:
    1. Consulta el clima en Madrid
    2. Calcula cuánto es 15% de 2500
    3. Busca si tenemos usuarios con rol de manager
    4. Si encuentras managers, prepara un email para contactarlos sobre una reunión mañana a las 10am`
  );

  console.log("✅ Respuesta del asistente:", response);

  // Ver estadísticas de uso de herramientas
  const stats = assistant.getToolUsageStats();
  console.log("\n📈 Uso de herramientas:", stats);
}

// 3. Agente de Atención al Cliente
class CustomerServiceAgent extends FunctionCallingAgent {
  constructor() {
    super(
      `Eres un agente de atención al cliente profesional y empático. 
      Puedes buscar información de usuarios y pedidos, hacer cálculos para descuentos,
      y enviar emails de seguimiento. Siempre mantén un tono amable y profesional.`,
      toolSchemas
    );
  }

  async handleCustomerInquiry(
    customerMessage: string,
    customerId?: string
  ): Promise<string> {
    let contextualMessage = customerMessage;

    if (customerId) {
      contextualMessage = `Consulta del cliente ID ${customerId}: ${customerMessage}`;
    }

    return await this.chat(contextualMessage);
  }

  async escalateToHuman(reason: string): Promise<string> {
    return `🚨 ESCALADO A HUMANO: ${reason}. Un representante se pondrá en contacto pronto.`;
  }
}

// 4. Sistema de Análisis de Perfiles
async function profileAnalysisSystem() {
  const profiles = [
    "Juan tiene 28 años, vive en Barcelona, España. Es desarrollador senior especializado en React y Node.js. Le gusta el fútbol y la fotografía. Muy activo en GitHub y prefiere comunicación por email.",

    "María García, 34 años, diseñadora UX/UI en Madrid. Experta en Figma y Adobe Creative Suite. Intereses: arte, viajes, yoga. Freelancer activa, prefiere chat para comunicación rápida.",

    "Carlos López, 45 años, manager de proyecto en Valencia. 15 años de experiencia en gestión de equipos. Intereses: golf, lectura, mentoring. Prefiere comunicación telefónica.",
  ];

  console.log("👥 Sistema de Análisis de Perfiles\n");

  for (const [index, profileText] of profiles.entries()) {
    try {
      console.log(`Analizando perfil ${index + 1}:`);
      const profile = await analyzeUserProfile(profileText);

      console.log(`✅ Perfil estructurado:`, JSON.stringify(profile, null, 2));

      // Validar schema
      const validation = validateJsonSchema(
        profile,
        userProfileSchema.parameters
      );
      console.log(
        `📋 Validación: ${
          validation.valid
            ? "✅ Válido"
            : "❌ Errores: " + validation.errors.join(", ")
        }`
      );
      console.log("\n" + "-".repeat(50) + "\n");
    } catch (error) {
      console.error(`❌ Error analizando perfil ${index + 1}:`, error);
    }
  }
}
```

### Mejores Prácticas y Consejos

```typescript
const FUNCTION_CALLING_BEST_PRACTICES = {
  // 1. Diseño de schemas
  schemaDesign: [
    "Usar descripciones claras y específicas",
    "Incluir ejemplos en las descripciones cuando sea útil",
    "Definir enums para valores limitados",
    "Usar tipos apropiados (string, number, boolean, array, object)",
    "Marcar campos como required solo cuando sea necesario",
    "Agregar validaciones (minimum, maximum, pattern)",
  ],

  // 2. Manejo de errores
  errorHandling: [
    "Siempre validar argumentos antes de ejecutar funciones",
    "Proporcionar mensajes de error informativos",
    "Implementar timeouts para funciones que pueden tardar",
    "Log detallado para debugging",
    "Graceful fallbacks cuando las funciones fallan",
  ],

  // 3. Seguridad
  security: [
    "Nunca usar eval() en producción para funciones matemáticas",
    "Validar y sanitizar todos los inputs",
    "Implementar rate limiting para funciones costosas",
    "Usar principio de menor privilegio",
    "Auditar y log todas las llamadas a funciones sensibles",
  ],

  // 4. Performance
  performance: [
    "Cachear resultados de funciones cuando sea apropiado",
    "Implementar timeouts para evitar funciones colgadas",
    "Usar async/await apropiadamente",
    "Limitar el número de tool calls concurrentes",
    "Optimizar funciones que serán llamadas frecuentemente",
  ],

  // 5. Testing
  testing: [
    "Testear cada función independientemente",
    "Crear mocks para APIs externas",
    "Testear casos edge y manejo de errores",
    "Validar que los schemas JSON son correctos",
    "Testear la integración completa del agent",
  ],
};

// Utilidad para crear schemas más fácilmente
class SchemaBuilder {
  static createFunctionSchema(
    name: string,
    description: string,
    parameters: Record<string, any>,
    required: string[] = []
  ) {
    return {
      type: "function" as const,
      function: {
        name,
        description,
        parameters: {
          type: "object",
          properties: parameters,
          required,
        },
      },
    };
  }

  static stringParam(description: string, enumValues?: string[]): any {
    const param: any = {
      type: "string",
      description,
    };

    if (enumValues) {
      param.enum = enumValues;
    }

    return param;
  }

  static numberParam(description: string, min?: number, max?: number): any {
    const param: any = {
      type: "number",
      description,
    };

    if (min !== undefined) param.minimum = min;
    if (max !== undefined) param.maximum = max;

    return param;
  }

  static arrayParam(description: string, itemType: string = "string"): any {
    return {
      type: "array",
      description,
      items: { type: itemType },
    };
  }

  static objectParam(
    description: string,
    properties: Record<string, any>
  ): any {
    return {
      type: "object",
      description,
      properties,
    };
  }
}

// Ejemplo de uso del SchemaBuilder
const weatherSchema = SchemaBuilder.createFunctionSchema(
  "get_weather_v2",
  "Obtiene información meteorológica detallada",
  {
    location: SchemaBuilder.stringParam("Ciudad o coordenadas"),
    units: SchemaBuilder.stringParam("Unidades de temperatura", [
      "celsius",
      "fahrenheit",
      "kelvin",
    ]),
    details: SchemaBuilder.arrayParam(
      "Detalles específicos a incluir",
      "string"
    ),
    forecast_days: SchemaBuilder.numberParam("Días de pronóstico", 1, 7),
  },
  ["location"]
);

console.log("🛠️ Function Calling configurado con mejores prácticas");
console.log("📋 SchemaBuilder disponible para crear schemas fácilmente");
```

### Ejecutar Ejemplos

```typescript
async function runAllFunctionCallingExamples() {
  console.log("=".repeat(60));
  console.log("🚀 EJEMPLOS DE FUNCTION CALLING");
  console.log("=".repeat(60));

  try {
    // Ejemplo básico
    console.log("\n1. 🤖 Agente Básico");
    const basicAgent = new FunctionCallingAgent();
    const basicResponse = await basicAgent.chat(
      "¿Cuál es el clima en Madrid y cuánto es 25 * 4?"
    );
    console.log("Respuesta:", basicResponse);

    // Análisis de datos
    console.log("\n2. 📊 Agente de Análisis");
    await dataAnalysisAgent();

    // Asistente personal
    console.log("\n3. 🤖 Asistente Personal");
    await personalAssistant();

    // Análisis de perfiles
    console.log("\n4. 👥 Análisis de Perfiles");
    await profileAnalysisSystem();

    console.log("\n✅ Todos los ejemplos ejecutados correctamente");
  } catch (error) {
    console.error("❌ Error en ejemplos:", error);
  }
}

// Descomenta para ejecutar:
// runAllFunctionCallingExamples().catch(console.error);
```

---

## 🔍 Embeddings

Los **embeddings** son representaciones vectoriales de texto que capturan el significado semántico. Son la base de la búsqueda semántica y sistemas RAG (Retrieval Augmented Generation).

### ¿Qué son los Embeddings?

Los embeddings convierten texto en vectores de números que representan el significado:

```typescript
// Conceptualmente:
"perro" → [0.2, -0.1, 0.8, 0.3, ...]  // Vector de 1536 dimensiones
"gato"  → [0.1, -0.2, 0.7, 0.4, ...]  // Vector similar por ser ambos animales
"auto"  → [-0.3, 0.9, -0.1, 0.2, ...] // Vector diferente por ser vehículo

// Textos similares tienen vectores similares
"El perro ladra" ≈ "El can hace ruido"
"JavaScript es genial" ≈ "JS es increíble"
```

### Modelos de Embedding (2025)

```typescript
// Modelos disponibles y sus características
const EMBEDDING_MODELS = {
  "text-embedding-3-large": {
    dimensions: 3072, // Vectores más grandes = más precisos
    maxTokens: 8191, // Texto máximo por request
    cost: 0.00013, // $0.13 por 1M tokens
    performance: "best", // Mejor calidad
    useCase: "production",
  },

  "text-embedding-3-small": {
    dimensions: 1536, // Vectores más pequeños = más rápidos
    maxTokens: 8191,
    cost: 0.00002, // $0.02 por 1M tokens (muy económico)
    performance: "good", // Buena calidad
    useCase: "development",
  },

  // Modelo legacy (no recomendado para proyectos nuevos)
  "text-embedding-ada-002": {
    dimensions: 1536,
    maxTokens: 8191,
    cost: 0.0001,
    performance: "legacy",
    useCase: "compatibility",
  },
} as const;

type EmbeddingModel = keyof typeof EMBEDDING_MODELS;
```

### Creando Embeddings Básicos

```typescript
// Crear embedding simple
async function createEmbedding(
  text: string,
  model: EmbeddingModel = "text-embedding-3-small"
): Promise<number[]> {
  try {
    const response = await openai.embeddings.create({
      model,
      input: text,
      encoding_format: "float", // Formato de los números
    });

    return response.data[0].embedding;
  } catch (error) {
    console.error("Error creando embedding:", error);
    throw error;
  }
}

// Crear múltiples embeddings (más eficiente)
async function createMultipleEmbeddings(
  texts: string[],
  model: EmbeddingModel = "text-embedding-3-small"
): Promise<number[][]> {
  if (texts.length === 0) {
    return [];
  }

  // OpenAI permite hasta 2048 inputs por request
  const batchSize = 100; // Procesamos de a 100 para ser conservadores
  const allEmbeddings: number[][] = [];

  for (let i = 0; i < texts.length; i += batchSize) {
    const batch = texts.slice(i, i + batchSize);

    try {
      const response = await openai.embeddings.create({
        model,
        input: batch,
        encoding_format: "float",
      });

      const embeddings = response.data
        .sort((a, b) => a.index - b.index) // Mantener orden
        .map((item) => item.embedding);

      allEmbeddings.push(...embeddings);

      console.log(
        `✅ Procesado batch ${Math.floor(i / batchSize) + 1}: ${
          batch.length
        } embeddings`
      );

      // Pausa pequeña para evitar rate limits
      if (i + batchSize < texts.length) {
        await new Promise((resolve) => setTimeout(resolve, 100));
      }
    } catch (error) {
      console.error(
        `❌ Error en batch ${Math.floor(i / batchSize) + 1}:`,
        error
      );
      throw error;
    }
  }

  return allEmbeddings;
}

// Función auxiliar para limpiar texto antes de crear embedding
function cleanTextForEmbedding(text: string): string {
  return text
    .trim() // Quitar espacios
    .replace(/\s+/g, " ") // Normalizar espacios
    .replace(/[^\w\sáéíóúüñ.,!?-]/gi, "") // Quitar caracteres especiales
    .substring(0, 8000); // Limitar longitud (considerando tokens)
}

// Ejemplo de uso básico
async function ejemploEmbeddingsBasico() {
  console.log("🔍 Creando embeddings básicos\n");

  // Textos de ejemplo
  const textos = [
    "TypeScript es un lenguaje de programación",
    "JavaScript es la base de TypeScript",
    "Los gatos son animales domésticos",
    "El clima está soleado hoy",
  ];

  // Crear embeddings individuales
  for (const texto of textos) {
    const embedding = await createEmbedding(texto);
    console.log(`📝 "${texto}"`);
    console.log(
      `🔢 Embedding: [${embedding
        .slice(0, 5)
        .map((n) => n.toFixed(3))
        .join(", ")}...]`
    );
    console.log(`📏 Dimensiones: ${embedding.length}\n`);
  }

  // Crear múltiples embeddings de una vez
  console.log("📦 Creando embeddings en lote...");
  const embeddings = await createMultipleEmbeddings(textos);
  console.log(`✅ ${embeddings.length} embeddings creados\n`);
}
```

### Similitud entre Embeddings

```typescript
// Calcular similitud coseno (la más usada)
function cosineSimilarity(a: number[], b: number[]): number {
  if (a.length !== b.length) {
    throw new Error("Los vectores deben tener la misma longitud");
  }

  const dotProduct = a.reduce((sum, ai, i) => sum + ai * b[i], 0);
  const magnitudeA = Math.sqrt(a.reduce((sum, ai) => sum + ai * ai, 0));
  const magnitudeB = Math.sqrt(b.reduce((sum, bi) => sum + bi * bi, 0));

  if (magnitudeA === 0 || magnitudeB === 0) {
    return 0;
  }

  return dotProduct / (magnitudeA * magnitudeB);
}

// Calcular similitud euclidiana (distancia)
function euclideanDistance(a: number[], b: number[]): number {
  if (a.length !== b.length) {
    throw new Error("Los vectores deben tener la misma longitud");
  }

  const sumSquaredDiff = a.reduce((sum, ai, i) => {
    const diff = ai - b[i];
    return sum + diff * diff;
  }, 0);

  return Math.sqrt(sumSquaredDiff);
}

// Calcular similitud dot product
function dotProduct(a: number[], b: number[]): number {
  if (a.length !== b.length) {
    throw new Error("Los vectores deben tener la misma longitud");
  }

  return a.reduce((sum, ai, i) => sum + ai * b[i], 0);
}

// Utilidad para encontrar los más similares
function findMostSimilar(
  queryEmbedding: number[],
  embeddings: number[][],
  texts: string[],
  topK: number = 5,
  similarityFn: (a: number[], b: number[]) => number = cosineSimilarity
): Array<{ text: string; similarity: number; index: number }> {
  if (embeddings.length !== texts.length) {
    throw new Error(
      "Los arrays de embeddings y textos deben tener la misma longitud"
    );
  }

  const similarities = embeddings.map((embedding, index) => ({
    text: texts[index],
    similarity: similarityFn(queryEmbedding, embedding),
    index,
  }));

  // Ordenar por similitud descendente
  return similarities
    .sort((a, b) => b.similarity - a.similarity)
    .slice(0, topK);
}

// Ejemplo práctico de búsqueda
async function ejemploBusquedaSemantica() {
  console.log("🎯 Ejemplo de búsqueda semántica\n");

  // Base de conocimiento
  const documentos = [
    "TypeScript es un superset de JavaScript que añade tipos estáticos",
    "React es una librería para construir interfaces de usuario",
    "Node.js permite ejecutar JavaScript en el servidor",
    "Python es un lenguaje de programación interpretado",
    "Los gatos son animales felinos domésticos",
    "Docker facilita la containerización de aplicaciones",
    "Git es un sistema de control de versiones distribuido",
    "MongoDB es una base de datos NoSQL orientada a documentos",
  ];

  console.log("📚 Creando base de conocimiento...");
  const embeddingsDocumentos = await createMultipleEmbeddings(documentos);

  // Consulta del usuario
  const consulta = "¿Cómo funciona JavaScript en el backend?";
  console.log(`❓ Consulta: "${consulta}"`);

  const embeddingConsulta = await createEmbedding(consulta);

  // Encontrar documentos más relevantes
  const resultados = findMostSimilar(
    embeddingConsulta,
    embeddingsDocumentos,
    documentos,
    3
  );

  console.log("\n🔍 Resultados más relevantes:");
  resultados.forEach((resultado, i) => {
    console.log(
      `${i + 1}. [${(resultado.similarity * 100).toFixed(1)}%] ${
        resultado.text
      }`
    );
  });
}
```

### Sistema de Embeddings Avanzado

```typescript
interface Document {
  id: string;
  content: string;
  metadata?: Record<string, any>;
  embedding?: number[];
  createdAt?: Date;
}

interface SearchResult extends Document {
  similarity: number;
  rank: number;
}

class EmbeddingStore {
  private documents: Document[] = [];
  private model: EmbeddingModel;
  private dimensions: number;

  constructor(model: EmbeddingModel = "text-embedding-3-small") {
    this.model = model;
    this.dimensions = EMBEDDING_MODELS[model].dimensions;
  }

  // Agregar un documento
  async addDocument(
    doc: Omit<Document, "embedding" | "createdAt">
  ): Promise<void> {
    const cleanContent = cleanTextForEmbedding(doc.content);

    if (!cleanContent.trim()) {
      throw new Error(
        "El contenido del documento está vacío después de limpiar"
      );
    }

    const embedding = await createEmbedding(cleanContent, this.model);

    const document: Document = {
      ...doc,
      embedding,
      createdAt: new Date(),
    };

    // Verificar si ya existe (por ID)
    const existingIndex = this.documents.findIndex((d) => d.id === doc.id);
    if (existingIndex >= 0) {
      this.documents[existingIndex] = document;
      console.log(`🔄 Documento actualizado: ${doc.id}`);
    } else {
      this.documents.push(document);
      console.log(`➕ Documento agregado: ${doc.id}`);
    }
  }

  // Agregar múltiples documentos (más eficiente)
  async addDocuments(
    docs: Omit<Document, "embedding" | "createdAt">[]
  ): Promise<void> {
    if (docs.length === 0) return;

    console.log(`📦 Procesando ${docs.length} documentos...`);

    // Limpiar contenido
    const cleanedDocs = docs
      .map((doc) => ({
        ...doc,
        content: cleanTextForEmbedding(doc.content),
      }))
      .filter((doc) => doc.content.trim());

    if (cleanedDocs.length === 0) {
      throw new Error("Ningún documento tiene contenido válido");
    }

    // Crear embeddings en lote
    const contents = cleanedDocs.map((doc) => doc.content);
    const embeddings = await createMultipleEmbeddings(contents, this.model);

    // Crear documentos con embeddings
    const documentsWithEmbeddings: Document[] = cleanedDocs.map(
      (doc, index) => ({
        ...doc,
        embedding: embeddings[index],
        createdAt: new Date(),
      })
    );

    // Agregar al store (verificar duplicados)
    for (const doc of documentsWithEmbeddings) {
      const existingIndex = this.documents.findIndex((d) => d.id === doc.id);
      if (existingIndex >= 0) {
        this.documents[existingIndex] = doc;
      } else {
        this.documents.push(doc);
      }
    }

    console.log(`✅ ${cleanedDocs.length} documentos procesados`);
  }

  // Búsqueda semántica
  async search(
    query: string,
    options: {
      topK?: number;
      threshold?: number;
      filter?: (doc: Document) => boolean;
      includeMetadata?: boolean;
    } = {}
  ): Promise<SearchResult[]> {
    const {
      topK = 10,
      threshold = 0.1,
      filter = () => true,
      includeMetadata = true,
    } = options;

    if (this.documents.length === 0) {
      throw new Error("No hay documentos en el store");
    }

    // Crear embedding de la consulta
    const queryEmbedding = await createEmbedding(query, this.model);

    // Filtrar documentos
    const validDocuments = this.documents.filter(
      (doc) => doc.embedding && filter(doc)
    );

    if (validDocuments.length === 0) {
      return [];
    }

    // Calcular similitudes
    const results = validDocuments.map((doc) => {
      const similarity = cosineSimilarity(queryEmbedding, doc.embedding!);
      return {
        ...doc,
        similarity,
        rank: 0, // Se asignará después del ordenamiento
        metadata: includeMetadata ? doc.metadata : undefined,
      } as SearchResult;
    });

    // Filtrar por threshold y ordenar
    const filteredResults = results
      .filter((result) => result.similarity >= threshold)
      .sort((a, b) => b.similarity - a.similarity)
      .slice(0, topK);

    // Asignar rankings
    filteredResults.forEach((result, index) => {
      result.rank = index + 1;
    });

    return filteredResults;
  }

  // Búsqueda híbrida (semántica + palabras clave)
  async hybridSearch(
    query: string,
    options: {
      topK?: number;
      semanticWeight?: number; // 0-1, peso de búsqueda semántica vs keywords
      keywordBoost?: number;
    } = {}
  ): Promise<SearchResult[]> {
    const { topK = 10, semanticWeight = 0.7, keywordBoost = 0.3 } = options;

    // Búsqueda semántica
    const semanticResults = await this.search(query, { topK: topK * 2 });

    // Búsqueda por palabras clave (simple)
    const queryWords = query.toLowerCase().split(/\s+/);
    const keywordResults = this.documents.map((doc) => {
      const content = doc.content.toLowerCase();
      const matchCount = queryWords.reduce((count, word) => {
        return count + (content.includes(word) ? 1 : 0);
      }, 0);

      const keywordScore = matchCount / queryWords.length;

      return {
        ...doc,
        similarity: 0, // Se calculará después
        rank: 0,
        keywordScore,
      } as SearchResult & { keywordScore: number };
    });

    // Combinar scores
    const combinedResults = semanticResults.map((semanticResult) => {
      const keywordResult = keywordResults.find(
        (kr) => kr.id === semanticResult.id
      );
      const keywordScore = keywordResult?.keywordScore || 0;

      const combinedSimilarity =
        semanticResult.similarity * semanticWeight +
        keywordScore * keywordBoost;

      return {
        ...semanticResult,
        similarity: combinedSimilarity,
      };
    });

    // Reordenar y limitar
    const finalResults = combinedResults
      .sort((a, b) => b.similarity - a.similarity)
      .slice(0, topK);

    // Reasignar rankings
    finalResults.forEach((result, index) => {
      result.rank = index + 1;
    });

    return finalResults;
  }

  // Encontrar documentos similares a uno dado
  async findSimilarDocuments(
    documentId: string,
    topK: number = 5
  ): Promise<SearchResult[]> {
    const sourceDoc = this.documents.find((doc) => doc.id === documentId);
    if (!sourceDoc || !sourceDoc.embedding) {
      throw new Error(`Documento no encontrado: ${documentId}`);
    }

    const otherDocs = this.documents.filter(
      (doc) => doc.id !== documentId && doc.embedding
    );

    const similarities = otherDocs.map((doc) => ({
      ...doc,
      similarity: cosineSimilarity(sourceDoc.embedding!, doc.embedding!),
      rank: 0,
    }));

    const results = similarities
      .sort((a, b) => b.similarity - a.similarity)
      .slice(0, topK);

    results.forEach((result, index) => {
      result.rank = index + 1;
    });

    return results as SearchResult[];
  }

  // Clustering simple de documentos
  async clusterDocuments(numClusters: number = 3): Promise<{
    clusters: Array<{
      id: number;
      centroid: number[];
      documents: Document[];
      avgSimilarity: number;
    }>;
    totalDocuments: number;
  }> {
    if (this.documents.length < numClusters) {
      throw new Error("No hay suficientes documentos para crear clusters");
    }

    const docsWithEmbeddings = this.documents.filter((doc) => doc.embedding);

    // K-means simplificado
    // 1. Inicializar centroides aleatoriamente
    const centroids: number[][] = [];
    for (let i = 0; i < numClusters; i++) {
      const randomDoc =
        docsWithEmbeddings[
          Math.floor(Math.random() * docsWithEmbeddings.length)
        ];
      centroids.push([...randomDoc.embedding!]);
    }

    let iterations = 0;
    const maxIterations = 50;
    let changed = true;

    const clusters: Document[][] = Array(numClusters)
      .fill(null)
      .map(() => []);

    while (changed && iterations < maxIterations) {
      changed = false;

      // Limpiar clusters
      clusters.forEach((cluster) => (cluster.length = 0));

      // Asignar cada documento al cluster más cercano
      for (const doc of docsWithEmbeddings) {
        let bestCluster = 0;
        let bestSimilarity = -1;

        for (let i = 0; i < centroids.length; i++) {
          const similarity = cosineSimilarity(doc.embedding!, centroids[i]);
          if (similarity > bestSimilarity) {
            bestSimilarity = similarity;
            bestCluster = i;
          }
        }

        clusters[bestCluster].push(doc);
      }

      // Recalcular centroides
      for (let i = 0; i < centroids.length; i++) {
        if (clusters[i].length > 0) {
          const newCentroid = new Array(this.dimensions).fill(0);

          for (const doc of clusters[i]) {
            for (let j = 0; j < this.dimensions; j++) {
              newCentroid[j] += doc.embedding![j];
            }
          }

          for (let j = 0; j < this.dimensions; j++) {
            newCentroid[j] /= clusters[i].length;
          }

          // Verificar si cambió
          const oldCentroid = centroids[i];
          const diff = newCentroid.some(
            (val, idx) => Math.abs(val - oldCentroid[idx]) > 0.001
          );
          if (diff) {
            changed = true;
            centroids[i] = newCentroid;
          }
        }
      }

      iterations++;
    }

    // Calcular estadísticas de clusters
    const clusterStats = clusters.map((cluster, id) => {
      if (cluster.length === 0) {
        return {
          id,
          centroid: centroids[id],
          documents: [],
          avgSimilarity: 0,
        };
      }

      // Calcular similitud promedio dentro del cluster
      let totalSimilarity = 0;
      let comparisons = 0;

      for (let i = 0; i < cluster.length; i++) {
        for (let j = i + 1; j < cluster.length; j++) {
          totalSimilarity += cosineSimilarity(
            cluster[i].embedding!,
            cluster[j].embedding!
          );
          comparisons++;
        }
      }

      const avgSimilarity = comparisons > 0 ? totalSimilarity / comparisons : 1;

      return {
        id,
        centroid: centroids[id],
        documents: cluster,
        avgSimilarity,
      };
    });

    return {
      clusters: clusterStats,
      totalDocuments: docsWithEmbeddings.length,
    };
  }

  // Estadísticas del store
  getStats(): {
    totalDocuments: number;
    documentsWithEmbeddings: number;
    model: string;
    dimensions: number;
    estimatedSize: string;
  } {
    const documentsWithEmbeddings = this.documents.filter(
      (doc) => doc.embedding
    ).length;

    // Estimar tamaño en memoria (muy aproximado)
    const embeddingSize = documentsWithEmbeddings * this.dimensions * 4; // 4 bytes por float
    const textSize = this.documents.reduce(
      (size, doc) => size + doc.content.length,
      0
    );
    const totalSizeBytes = embeddingSize + textSize;

    let estimatedSize: string;
    if (totalSizeBytes < 1024) {
      estimatedSize = `${totalSizeBytes} bytes`;
    } else if (totalSizeBytes < 1024 * 1024) {
      estimatedSize = `${(totalSizeBytes / 1024).toFixed(1)} KB`;
    } else {
      estimatedSize = `${(totalSizeBytes / (1024 * 1024)).toFixed(1)} MB`;
    }

    return {
      totalDocuments: this.documents.length,
      documentsWithEmbeddings,
      model: this.model,
      dimensions: this.dimensions,
      estimatedSize,
    };
  }

  // Guardar/cargar desde archivo (para persistencia simple)
  saveToFile(filename: string = "embedding-store.json"): void {
    const data = {
      model: this.model,
      dimensions: this.dimensions,
      documents: this.documents,
      savedAt: new Date().toISOString(),
    };

    const fs = require("fs");
    fs.writeFileSync(filename, JSON.stringify(data, null, 2));
    console.log(
      `💾 Store guardado: ${filename} (${this.documents.length} documentos)`
    );
  }

  loadFromFile(filename: string = "embedding-store.json"): void {
    const fs = require("fs");

    if (!fs.existsSync(filename)) {
      throw new Error(`Archivo no encontrado: ${filename}`);
    }

    const data = JSON.parse(fs.readFileSync(filename, "utf8"));

    this.model = data.model;
    this.dimensions = data.dimensions;
    this.documents = data.documents;

    console.log(
      `📂 Store cargado: ${filename} (${this.documents.length} documentos)`
    );
  }

  // Limpiar store
  clear(): void {
    this.documents = [];
    console.log("🧹 Store limpiado");
  }

  // Obtener documento por ID
  getDocument(id: string): Document | undefined {
    return this.documents.find((doc) => doc.id === id);
  }

  // Obtener todos los documentos
  getAllDocuments(): Document[] {
    return [...this.documents];
  }

  // Eliminar documento
  removeDocument(id: string): boolean {
    const index = this.documents.findIndex((doc) => doc.id === id);
    if (index >= 0) {
      this.documents.splice(index, 1);
      console.log(`🗑️ Documento eliminado: ${id}`);
      return true;
    }
    return false;
  }
}
```

### Ejemplos Prácticos Completos

```typescript
// 1. Sistema de FAQ inteligente
async function sistemaFAQInteligente() {
  console.log("❓ Creando sistema de FAQ inteligente\n");

  const store = new EmbeddingStore("text-embedding-3-small");

  // Base de conocimiento FAQ
  const faqs = [
    {
      id: "faq-1",
      content:
        "¿Cómo instalar Node.js en Windows? Descarga el instalador desde nodejs.org, ejecútalo y sigue las instrucciones. Verifica la instalación con 'node --version'",
      metadata: { category: "instalacion", difficulty: "beginner" },
    },
    {
      id: "faq-2",
      content:
        "¿Qué es TypeScript? TypeScript es un superset de JavaScript que añade tipos estáticos opcionales. Compila a JavaScript puro y mejora la experiencia de desarrollo",
      metadata: { category: "conceptos", difficulty: "beginner" },
    },
    {
      id: "faq-3",
      content:
        "¿Cómo manejar errores async/await en JavaScript? Usa try-catch blocks o .catch() en las promesas. También puedes crear un wrapper para manejar errores globalmente",
      metadata: { category: "javascript", difficulty: "intermediate" },
    },
    {
      id: "faq-4",
      content:
        "¿Cuál es la diferencia entre let, const y var en JavaScript? var tiene scope de función, let y const tienen scope de bloque. const no puede reasignarse, let sí",
      metadata: { category: "javascript", difficulty: "beginner" },
    },
    {
      id: "faq-5",
      content:
        "¿Cómo optimizar el rendimiento de una aplicación React? Usa React.memo, useMemo, useCallback, lazy loading, y evita re-renders innecesarios",
      metadata: { category: "react", difficulty: "advanced" },
    },
  ];

  // Agregar FAQs al store
  await store.addDocuments(faqs);

  // Simular consultas de usuarios
  const consultas = [
    "¿Cómo instalo Node en mi PC?",
    "Diferencias entre let y var",
    "Optimizar React performance",
    "Manejo de errores en async",
    "¿Qué es TS?",
  ];

  console.log("🔍 Procesando consultas de usuarios:\n");

  for (const consulta of consultas) {
    console.log(`👤 Usuario pregunta: "${consulta}"`);

    const resultados = await store.search(consulta, {
      topK: 2,
      threshold: 0.3,
    });

    if (resultados.length > 0) {
      const mejor = resultados[0];
      console.log(
        `🎯 Mejor respuesta (${(mejor.similarity * 100).toFixed(
          1
        )}%): ${mejor.content.substring(0, 100)}...`
      );
      console.log(`📂 Categoría: ${mejor.metadata?.category}\n`);
    } else {
      console.log("❌ No se encontraron respuestas relevantes\n");
    }
  }

  // Mostrar estadísticas
  const stats = store.getStats();
  console.log("📊 Estadísticas del sistema FAQ:");
  console.log(`- Documentos: ${stats.totalDocuments}`);
  console.log(`- Modelo: ${stats.model}`);
  console.log(`- Tamaño estimado: ${stats.estimatedSize}\n`);
}

// 2. Motor de recomendaciones de contenido
async function motorRecomendaciones() {
  console.log("🎯 Creando motor de recomendaciones\n");

  const store = new EmbeddingStore("text-embedding-3-small");

  // Contenido de ejemplo (artículos de blog)
  const articulos = [
    {
      id: "art-1",
      content:
        "Guía completa de TypeScript para principiantes. Aprende tipos, interfaces, clases y más",
      metadata: {
        author: "Juan",
        tags: ["typescript", "beginner"],
        views: 1500,
      },
    },
    {
      id: "art-2",
      content:
        "Patrones avanzados en React: HOCs, Render Props y Hooks personalizados",
      metadata: { author: "María", tags: ["react", "advanced"], views: 890 },
    },
    {
      id: "art-3",
      content:
        "Node.js y Express: Construyendo APIs RESTful robustas y escalables",
      metadata: { author: "Carlos", tags: ["nodejs", "api"], views: 2100 },
    },
    {
      id: "art-4",
      content:
        "Testing en JavaScript: Jest, Mocha y mejores prácticas de testing",
      metadata: { author: "Ana", tags: ["testing", "javascript"], views: 750 },
    },
    {
      id: "art-5",
      content: "Deploy y DevOps: Docker, CI/CD y automatización de despliegues",
      metadata: { author: "Luis", tags: ["devops", "docker"], views: 1200 },
    },
    {
      id: "art-6",
      content:
        "Bases de datos NoSQL: MongoDB, diseño de esquemas y optimización",
      metadata: { author: "Sofia", tags: ["database", "mongodb"], views: 980 },
    },
  ];

  await store.addDocuments(articulos);

  // Simular historial de lectura del usuario
  const historialUsuario = ["art-1", "art-3"]; // Usuario leyó sobre TypeScript y Node.js

  console.log("📚 Historial del usuario:", historialUsuario);
  console.log("🤖 Generando recomendaciones...\n");

  // Encontrar artículos similares basado en historial
  const recomendaciones = new Set<string>();

  for (const articuloId of historialUsuario) {
    const similares = await store.findSimilarDocuments(articuloId, 3);
    similares.forEach((similar) => {
      if (!historialUsuario.includes(similar.id)) {
        recomendaciones.add(similar.id);
      }
    });
  }

  // Obtener detalles de recomendaciones
  const recomendacionesDetalle = Array.from(recomendaciones)
    .map((id) => store.getDocument(id))
    .filter((doc) => doc)
    .slice(0, 3);

  console.log("📝 Artículos recomendados:");
  recomendacionesDetalle.forEach((articulo, index) => {
    console.log(`${index + 1}. ${articulo!.content.substring(0, 60)}...`);
    console.log(`   👤 Autor: ${articulo!.metadata?.author}`);
    console.log(`   🏷️ Tags: ${articulo!.metadata?.tags?.join(", ")}`);
    console.log(`   👁️ Views: ${articulo!.metadata?.views}\n`);
  });
}

// 3. Detector de contenido duplicado
async function detectorContenidoDuplicado() {
  console.log("🔍 Detector de contenido duplicado\n");

  const store = new EmbeddingStore("text-embedding-3-small");

  // Contenido con algunos duplicados/similares
  const contenidos = [
    {
      id: "doc-1",
      content:
        "JavaScript es un lenguaje de programación dinámico usado principalmente para desarrollo web",
    },
    {
      id: "doc-2",
      content:
        "TypeScript extiende JavaScript agregando tipos estáticos opcionales para mejorar el desarrollo",
    },
    {
      id: "doc-3",
      content:
        "JS es un lenguaje dinámico de programación utilizado especialmente en desarrollo web", // Similar a doc-1
    },
    {
      id: "doc-4",
      content:
        "React es una biblioteca de JavaScript para construir interfaces de usuario interactivas",
    },
    {
      id: "doc-5",
      content:
        "TypeScript es un superset de JavaScript que añade tipado estático opcional", // Similar a doc-2
    },
  ];

  await store.addDocuments(contenidos);

  console.log("📊 Analizando similitudes...\n");

  // Detectar duplicados usando threshold alto de similitud
  const umbralDuplicado = 0.85; // 85% de similitud
  const duplicadosDetectados: Array<{
    doc1: string;
    doc2: string;
    similarity: number;
  }> = [];

  const documentos = store.getAllDocuments();

  for (let i = 0; i < documentos.length; i++) {
    for (let j = i + 1; j < documentos.length; j++) {
      const doc1 = documentos[i];
      const doc2 = documentos[j];

      if (doc1.embedding && doc2.embedding) {
        const similarity = cosineSimilarity(doc1.embedding, doc2.embedding);

        if (similarity >= umbralDuplicado) {
          duplicadosDetectados.push({
            doc1: doc1.id,
            doc2: doc2.id,
            similarity,
          });
        }
      }
    }
  }

  if (duplicadosDetectados.length > 0) {
    console.log("⚠️ Contenido duplicado detectado:");
    duplicadosDetectados.forEach((dup) => {
      const doc1 = store.getDocument(dup.doc1);
      const doc2 = store.getDocument(dup.doc2);

      console.log(`\n🔄 Similitud: ${(dup.similarity * 100).toFixed(1)}%`);
      console.log(`📄 ${dup.doc1}: ${doc1?.content.substring(0, 60)}...`);
      console.log(`📄 ${dup.doc2}: ${doc2?.content.substring(0, 60)}...`);
    });
  } else {
    console.log("✅ No se detectó contenido duplicado");
  }
}

// 4. Análisis de clusters temáticos
async function analisisClusters() {
  console.log("🎯 Análisis de clusters temáticos\n");

  const store = new EmbeddingStore("text-embedding-3-small");

  // Contenido diverso para clustering
  const documentos = [
    // Grupo 1: Frontend
    {
      id: "1",
      content: "React hooks useState useEffect desarrollo frontend componentes",
    },
    {
      id: "2",
      content: "Vue.js componentes reactivos frontend JavaScript framework",
    },
    {
      id: "3",
      content: "Angular TypeScript frontend framework componentes servicios",
    },

    // Grupo 2: Backend
    { id: "4", content: "Node.js Express servidor backend API REST endpoints" },
    { id: "5", content: "Python Django Flask backend desarrollo web servidor" },
    { id: "6", content: "Java Spring Boot backend microservicios API" },

    // Grupo 3: Bases de datos
    { id: "7", content: "MongoDB NoSQL base datos documentos colecciones" },
    { id: "8", content: "PostgreSQL SQL base datos relacional consultas" },
    { id: "9", content: "Redis cache memoria base datos clave valor" },

    // Grupo 4: DevOps
    { id: "10", content: "Docker contenedores deployment infraestructura" },
    {
      id: "11",
      content: "Kubernetes orchestration contenedores escalabilidad",
    },
    { id: "12", content: "AWS cloud servicios infraestructura deployment" },
  ];

  await store.addDocuments(documentos);

  console.log("🧮 Realizando clustering...\n");

  const resultadoClustering = await store.clusterDocuments(4);

  console.log(
    `📊 Clusters encontrados: ${resultadoClustering.clusters.length}`
  );
  console.log(`📄 Total documentos: ${resultadoClustering.totalDocuments}\n`);

  resultadoClustering.clusters.forEach((cluster, index) => {
    if (cluster.documents.length > 0) {
      console.log(
        `🎯 Cluster ${index + 1} (${
          cluster.documents.length
        } docs, similitud promedio: ${(cluster.avgSimilarity * 100).toFixed(
          1
        )}%)`
      );
      cluster.documents.forEach((doc) => {
        console.log(`   📄 ${doc.id}: ${doc.content.substring(0, 50)}...`);
      });
      console.log();
    }
  });
}

// Ejecutar todos los ejemplos
async function ejecutarEjemplosEmbeddings() {
  console.log("=".repeat(60));
  console.log("🚀 EJEMPLOS DE EMBEDDINGS");
  console.log("=".repeat(60));

  try {
    await ejemploEmbeddingsBasico();
    console.log("=".repeat(60));

    await ejemploBusquedaSemantica();
    console.log("=".repeat(60));

    await sistemaFAQInteligente();
    console.log("=".repeat(60));

    await motorRecomendaciones();
    console.log("=".repeat(60));

    await detectorContenidoDuplicado();
    console.log("=".repeat(60));

    await analisisClusters();

    console.log("✅ Todos los ejemplos de embeddings ejecutados correctamente");
  } catch (error) {
    console.error("❌ Error en ejemplos de embeddings:", error);
  }
}

// Descomenta para ejecutar:
// ejecutarEjemplosEmbeddings().catch(console.error);
```

### Best Practices para Embeddings

```typescript
const EMBEDDING_BEST_PRACTICES = {
  // 1. Preparación de datos
  dataPreparation: [
    "Limpiar y normalizar texto antes de crear embeddings",
    "Dividir documentos largos en chunks de tamaño apropiado",
    "Mantener contexto relevante en cada chunk",
    "Usar títulos y metadatos como contexto adicional",
    "Eliminar contenido irrelevante (HTML, formatting, etc.)",
  ],

  // 2. Selección de modelo
  modelSelection: [
    "text-embedding-3-small para desarrollo y prototipado",
    "text-embedding-3-large para producción que requiere máxima precisión",
    "Considerar el balance costo/performance según caso de uso",
    "Evaluar dimensiones necesarias según tamaño del dataset",
  ],

  // 3. Optimización de búsqueda
  searchOptimization: [
    "Usar threshold apropiado para filtrar resultados irrelevantes",
    "Implementar re-ranking con múltiples factores (recency, popularity, etc.)",
    "Combinar búsqueda semántica con keywords para mejor cobertura",
    "Cachear embeddings para queries frecuentes",
    "Implementar paginación para grandes datasets",
  ],

  // 4. Escalabilidad
  scalability: [
    "Usar bases de datos vectoriales para grandes volúmenes (Pinecone, Weaviate)",
    "Implementar indexing apropiado (FAISS, Annoy)",
    "Procesar embeddings en lotes para eficiencia",
    "Considerar compresión de vectores para reducir memoria",
    "Implementar clustering para acelerar búsquedas",
  ],

  // 5. Monitoreo y mantenimiento
  monitoring: [
    "Monitorear calidad de resultados regularmente",
    "Actualizar embeddings cuando cambie el contenido",
    "A/B test diferentes modelos y configuraciones",
    "Trackear métricas de relevancia y satisfacción del usuario",
    "Mantener logs de queries para análisis y mejora",
  ],
};

// Utilidad para evaluar calidad de embeddings
class EmbeddingEvaluator {
  static async evaluateSearchQuality(
    store: EmbeddingStore,
    testCases: Array<{
      query: string;
      expectedResults: string[];
      description: string;
    }>
  ): Promise<{
    averagePrecision: number;
    averageRecall: number;
    testResults: Array<{
      query: string;
      precision: number;
      recall: number;
      passed: boolean;
    }>;
  }> {
    const testResults = [];
    let totalPrecision = 0;
    let totalRecall = 0;

    for (const testCase of testCases) {
      const results = await store.search(testCase.query, { topK: 10 });
      const foundIds = results.map((r) => r.id);

      // Calcular precision y recall
      const relevantFound = testCase.expectedResults.filter((id) =>
        foundIds.includes(id)
      );
      const precision =
        foundIds.length > 0 ? relevantFound.length / foundIds.length : 0;
      const recall =
        testCase.expectedResults.length > 0
          ? relevantFound.length / testCase.expectedResults.length
          : 0;

      const passed = precision >= 0.5 && recall >= 0.5; // Thresholds mínimos

      testResults.push({
        query: testCase.query,
        precision,
        recall,
        passed,
      });

      totalPrecision += precision;
      totalRecall += recall;

      console.log(`📊 ${testCase.description}`);
      console.log(`   Query: "${testCase.query}"`);
      console.log(`   Precision: ${(precision * 100).toFixed(1)}%`);
      console.log(`   Recall: ${(recall * 100).toFixed(1)}%`);
      console.log(`   Status: ${passed ? "✅ PASS" : "❌ FAIL"}\n`);
    }

    return {
      averagePrecision: totalPrecision / testCases.length,
      averageRecall: totalRecall / testCases.length,
      testResults,
    };
  }

  static calculateCoverage(embeddings: number[][]): {
    avgMagnitude: number;
    dimensionVariance: number[];
    sparsity: number;
  } {
    if (embeddings.length === 0) {
      return { avgMagnitude: 0, dimensionVariance: [], sparsity: 0 };
    }

    const dimensions = embeddings[0].length;

    // Magnitud promedio
    const magnitudes = embeddings.map((emb) =>
      Math.sqrt(emb.reduce((sum, val) => sum + val * val, 0))
    );
    const avgMagnitude =
      magnitudes.reduce((sum, mag) => sum + mag, 0) / magnitudes.length;

    // Varianza por dimensión
    const dimensionVariance = [];
    for (let dim = 0; dim < dimensions; dim++) {
      const values = embeddings.map((emb) => emb[dim]);
      const mean = values.reduce((sum, val) => sum + val, 0) / values.length;
      const variance =
        values.reduce((sum, val) => sum + Math.pow(val - mean, 2), 0) /
        values.length;
      dimensionVariance.push(variance);
    }

    // Sparsity (porcentaje de valores cerca de cero)
    const threshold = 0.01;
    let sparseCount = 0;
    let totalValues = 0;

    embeddings.forEach((emb) => {
      emb.forEach((val) => {
        if (Math.abs(val) < threshold) sparseCount++;
        totalValues++;
      });
    });

    const sparsity = sparseCount / totalValues;

    return {
      avgMagnitude,
      dimensionVariance,
      sparsity,
    };
  }
}

console.log("🔍 Sistema de embeddings configurado");
console.log("📊 Evaluador de calidad disponible");
console.log("💡 Best practices cargadas");
```

---

## 🔗 RAG (Retrieval Augmented Generation)

**RAG** combina la búsqueda de información relevante (Retrieval) con la generación de respuestas por LLMs (Generation). Es fundamental para crear sistemas que pueden acceder y utilizar conocimiento actualizado y específico.

### ¿Cómo Funciona RAG?

El proceso RAG tiene 4 etapas principales:

```typescript
// Flujo típico de RAG:
// 1. INDEXACIÓN (offline): Procesar documentos → Crear embeddings → Almacenar
// 2. CONSULTA: Usuario hace pregunta
// 3. RECUPERACIÓN: Buscar documentos relevantes usando similitud semántica
// 4. GENERACIÓN: LLM genera respuesta usando documentos como contexto

interface RAGPipeline {
  index: (documents: Document[]) => Promise<void>; // Paso 1
  retrieve: (query: string, k?: number) => Promise<Document[]>; // Paso 3
  generate: (query: string, context: Document[]) => Promise<string>; // Paso 4
}
```

### Arquitectura RAG Básica

```typescript
interface RAGDocument {
  id: string;
  content: string;
  metadata?: {
    title?: string;
    source?: string;
    author?: string;
    date?: string;
    tags?: string[];
    url?: string;
  };
  chunks?: DocumentChunk[];
}

interface DocumentChunk {
  id: string;
  content: string;
  startIndex: number;
  endIndex: number;
  embedding?: number[];
  metadata?: Record<string, any>;
}

interface RetrievalResult {
  document: RAGDocument;
  chunk: DocumentChunk;
  similarity: number;
  rank: number;
}

interface RAGResponse {
  answer: string;
  sources: RetrievalResult[];
  confidence: number;
  tokensUsed: {
    input: number;
    output: number;
  };
}
```

### Sistema RAG Completo

```typescript
class RAGSystem {
  private embeddingStore: EmbeddingStore;
  private chunkSize: number;
  private chunkOverlap: number;
  private maxContextTokens: number;

  constructor(
    options: {
      embeddingModel?: EmbeddingModel;
      chunkSize?: number;
      chunkOverlap?: number;
      maxContextTokens?: number;
    } = {}
  ) {
    const {
      embeddingModel = "text-embedding-3-small",
      chunkSize = 1000,
      chunkOverlap = 200,
      maxContextTokens = 8000,
    } = options;

    this.embeddingStore = new EmbeddingStore(embeddingModel);
    this.chunkSize = chunkSize;
    this.chunkOverlap = chunkOverlap;
    this.maxContextTokens = maxContextTokens;
  }

  // 1. CHUNKING: Dividir documento en fragmentos manejables
  private chunkDocument(document: RAGDocument): DocumentChunk[] {
    const text = document.content;
    const chunks: DocumentChunk[] = [];

    // Dividir por párrafos primero (mejor contexto)
    const paragraphs = text.split(/\n\s*\n/);
    let currentChunk = "";
    let chunkStart = 0;
    let chunkId = 0;

    for (let i = 0; i < paragraphs.length; i++) {
      const paragraph = paragraphs[i].trim();

      // Si agregar este párrafo excede el tamaño, crear chunk
      if (
        currentChunk.length + paragraph.length > this.chunkSize &&
        currentChunk.length > 0
      ) {
        chunks.push({
          id: `${document.id}-chunk-${chunkId++}`,
          content: currentChunk.trim(),
          startIndex: chunkStart,
          endIndex: chunkStart + currentChunk.length,
          metadata: {
            ...document.metadata,
            chunkIndex: chunkId - 1,
            totalChunks: 0, // Se actualizará al final
          },
        });

        // Overlap: mantener parte del chunk anterior
        const overlapText = this.getOverlapText(
          currentChunk,
          this.chunkOverlap
        );
        currentChunk = overlapText + paragraph;
        chunkStart = chunkStart + currentChunk.length - overlapText.length;
      } else {
        currentChunk += (currentChunk ? "\n\n" : "") + paragraph;
      }
    }

    // Último chunk
    if (currentChunk.trim()) {
      chunks.push({
        id: `${document.id}-chunk-${chunkId}`,
        content: currentChunk.trim(),
        startIndex: chunkStart,
        endIndex: chunkStart + currentChunk.length,
        metadata: {
          ...document.metadata,
          chunkIndex: chunkId,
          totalChunks: chunkId + 1,
        },
      });
    }

    // Actualizar totalChunks
    chunks.forEach((chunk) => {
      chunk.metadata!.totalChunks = chunks.length;
    });

    return chunks;
  }

  private getOverlapText(text: string, overlapSize: number): string {
    if (text.length <= overlapSize) return text;

    // Intentar cortar en límite de palabra
    const overlap = text.slice(-overlapSize);
    const lastSpaceIndex = overlap.lastIndexOf(" ");

    return lastSpaceIndex > overlapSize * 0.5
      ? overlap.slice(lastSpaceIndex + 1)
      : overlap;
  }

  // 2. INDEXACIÓN: Procesar y almacenar documentos
  async indexDocument(document: RAGDocument): Promise<void> {
    console.log(`📄 Indexando documento: ${document.id}`);

    // Dividir en chunks
    const chunks = this.chunkDocument(document);
    document.chunks = chunks;

    console.log(`📦 Creados ${chunks.length} chunks`);

    // Crear embeddings para cada chunk
    const chunkContents = chunks.map((chunk) => {
      // Agregar contexto del título si existe
      const title = document.metadata?.title;
      return title ? `${title}\n\n${chunk.content}` : chunk.content;
    });

    const embeddings = await createMultipleEmbeddings(chunkContents);

    // Almacenar chunks con embeddings
    for (let i = 0; i < chunks.length; i++) {
      await this.embeddingStore.addDocument({
        id: chunks[i].id,
        content: chunks[i].content,
        metadata: {
          ...chunks[i].metadata,
          originalDocumentId: document.id,
          isChunk: true,
        },
      });
    }

    console.log(`✅ Documento ${document.id} indexado correctamente`);
  }

  async indexDocuments(documents: RAGDocument[]): Promise<void> {
    console.log(`📚 Indexando ${documents.length} documentos...`);

    for (const document of documents) {
      await this.indexDocument(document);
    }

    console.log("✅ Todos los documentos indexados");
  }

  // 3. RECUPERACIÓN: Buscar chunks relevantes
  async retrieve(
    query: string,
    options: {
      topK?: number;
      threshold?: number;
      includeMetadata?: boolean;
      rerank?: boolean;
    } = {}
  ): Promise<RetrievalResult[]> {
    const {
      topK = 5,
      threshold = 0.3,
      includeMetadata = true,
      rerank = true,
    } = options;

    console.log(`🔍 Buscando información para: "${query}"`);

    // Buscar chunks relevantes
    const searchResults = await this.embeddingStore.search(query, {
      topK: rerank ? topK * 2 : topK, // Obtener más si vamos a rerank
      threshold,
      includeMetadata,
      filter: (doc) => doc.metadata?.isChunk === true,
    });

    if (searchResults.length === 0) {
      console.log("❌ No se encontraron chunks relevantes");
      return [];
    }

    // Convertir a RetrievalResult
    let retrievalResults = searchResults.map((result, index) => ({
      document: {
        id: result.metadata?.originalDocumentId || result.id,
        content: result.content,
        metadata: result.metadata,
      } as RAGDocument,
      chunk: {
        id: result.id,
        content: result.content,
        startIndex: 0, // Se podría calcular si es necesario
        endIndex: result.content.length,
        metadata: result.metadata,
      } as DocumentChunk,
      similarity: result.similarity,
      rank: index + 1,
    }));

    // Re-ranking opcional usando diversidad
    if (rerank && retrievalResults.length > topK) {
      retrievalResults = this.rerankResults(retrievalResults, topK);
    }

    console.log(`📊 Encontrados ${retrievalResults.length} chunks relevantes`);

    return retrievalResults;
  }

  private rerankResults(
    results: RetrievalResult[],
    topK: number
  ): RetrievalResult[] {
    // Algoritmo simple de re-ranking por diversidad
    const selected: RetrievalResult[] = [];
    const remaining = [...results];

    // Siempre incluir el mejor resultado
    if (remaining.length > 0) {
      selected.push(remaining.shift()!);
    }

    // Seleccionar resultados diversos
    while (selected.length < topK && remaining.length > 0) {
      let bestIndex = 0;
      let bestScore = -1;

      for (let i = 0; i < remaining.length; i++) {
        const candidate = remaining[i];

        // Score base de similitud
        let score = candidate.similarity;

        // Penalizar por similitud con resultados ya seleccionados
        for (const selectedResult of selected) {
          const diversity =
            1 -
            cosineSimilarity(
              selectedResult.chunk.embedding || [],
              candidate.chunk.embedding || []
            );
          score *= 0.7 + 0.3 * diversity; // 70% similitud + 30% diversidad
        }

        if (score > bestScore) {
          bestScore = score;
          bestIndex = i;
        }
      }

      selected.push(remaining.splice(bestIndex, 1)[0]);
    }

    // Actualizar rankings
    selected.forEach((result, index) => {
      result.rank = index + 1;
    });

    return selected;
  }

  // 4. GENERACIÓN: Crear respuesta usando contexto
  async generate(
    query: string,
    context: RetrievalResult[],
    options: {
      model?: string;
      temperature?: number;
      includeSource?: boolean;
      maxTokens?: number;
    } = {}
  ): Promise<RAGResponse> {
    const {
      model = "gpt-4o",
      temperature = 0.3,
      includeSource = true,
      maxTokens = 1500,
    } = options;

    if (context.length === 0) {
      throw new Error("No hay contexto disponible para generar respuesta");
    }

    // Construir contexto limitado por tokens
    const tokenCounter = new TokenCounter(model);
    let contextText = "";
    const usedSources: RetrievalResult[] = [];

    const basePrompt = `Eres un asistente experto que responde preguntas basándose únicamente en el contexto proporcionado.

INSTRUCCIONES:
- Responde SOLO basándote en la información del contexto
- Si la información no está en el contexto, di "No tengo información suficiente"
- Cita las fuentes cuando sea apropiado
- Sé preciso y conciso
- Si hay información contradictoria, menciónalo

CONTEXTO:
`;

    const responseTemplate = `
PREGUNTA: ${query}

RESPUESTA:`;

    const baseTokens = tokenCounter.count(basePrompt + responseTemplate);
    const availableTokens =
      this.maxContextTokens - baseTokens - maxTokens - 200; // Buffer

    for (const result of context) {
      const sourceInfo = includeSource
        ? `[Fuente: ${result.document.metadata?.title || result.document.id}]\n`
        : "";

      const chunkText = `${sourceInfo}${result.chunk.content}\n\n---\n\n`;

      if (tokenCounter.count(contextText + chunkText) <= availableTokens) {
        contextText += chunkText;
        usedSources.push(result);
      } else {
        break;
      }
    }

    const fullPrompt = basePrompt + contextText + responseTemplate;

    console.log(`🤖 Generando respuesta usando ${usedSources.length} fuentes`);

    try {
      const completion = await openai.chat.completions.create({
        model,
        messages: [{ role: "user", content: fullPrompt }],
        temperature,
        max_tokens: maxTokens,
      });

      const answer =
        completion.choices[0]?.message?.content || "Error generando respuesta";

      // Calcular tokens usados
      const inputTokens = tokenCounter.count(fullPrompt);
      const outputTokens = tokenCounter.count(answer);

      tokenCounter.free();

      // Calcular confidence basado en similitud promedio
      const avgSimilarity =
        usedSources.reduce((sum, source) => sum + source.similarity, 0) /
        usedSources.length;
      const confidence = Math.min(avgSimilarity * 1.2, 1.0); // Boost ligero, max 1.0

      return {
        answer,
        sources: usedSources,
        confidence,
        tokensUsed: {
          input: inputTokens,
          output: outputTokens,
        },
      };
    } catch (error) {
      console.error("Error generando respuesta:", error);
      throw error;
    }
  }

  // 5. PIPELINE COMPLETO: Query → Retrieve → Generate
  async query(
    question: string,
    options: {
      topK?: number;
      threshold?: number;
      model?: string;
      temperature?: number;
      includeSource?: boolean;
      rerank?: boolean;
    } = {}
  ): Promise<RAGResponse> {
    console.log(`❓ Procesando consulta: "${question}"`);

    // Paso 1: Recuperar contexto relevante
    const retrievalResults = await this.retrieve(question, {
      topK: options.topK,
      threshold: options.threshold,
      rerank: options.rerank,
    });

    if (retrievalResults.length === 0) {
      return {
        answer:
          "Lo siento, no encontré información relevante para responder tu pregunta en la base de conocimiento disponible.",
        sources: [],
        confidence: 0,
        tokensUsed: { input: 0, output: 0 },
      };
    }

    // Paso 2: Generar respuesta
    const response = await this.generate(question, retrievalResults, {
      model: options.model,
      temperature: options.temperature,
      includeSource: options.includeSource,
    });

    console.log(
      `✅ Respuesta generada con confianza: ${(
        response.confidence * 100
      ).toFixed(1)}%`
    );

    return response;
  }

  // Utilidades adicionales
  async getStats(): Promise<{
    totalDocuments: number;
    totalChunks: number;
    avgChunksPerDocument: number;
    embeddingStats: any;
  }> {
    const embeddingStats = this.embeddingStore.getStats();
    const totalChunks = embeddingStats.documentsWithEmbeddings;

    // Contar documentos originales
    const allDocs = this.embeddingStore.getAllDocuments();
    const originalDocIds = new Set(
      allDocs
        .filter((doc) => doc.metadata?.isChunk)
        .map((doc) => doc.metadata?.originalDocumentId)
        .filter(Boolean)
    );

    const totalDocuments = originalDocIds.size;
    const avgChunksPerDocument =
      totalDocuments > 0 ? totalChunks / totalDocuments : 0;

    return {
      totalDocuments,
      totalChunks,
      avgChunksPerDocument: Math.round(avgChunksPerDocument * 10) / 10,
      embeddingStats,
    };
  }

  async searchSimilarChunks(
    chunkId: string,
    topK: number = 5
  ): Promise<RetrievalResult[]> {
    const similarDocs = await this.embeddingStore.findSimilarDocuments(
      chunkId,
      topK
    );

    return similarDocs.map((doc, index) => ({
      document: {
        id: doc.metadata?.originalDocumentId || doc.id,
        content: doc.content,
        metadata: doc.metadata,
      } as RAGDocument,
      chunk: {
        id: doc.id,
        content: doc.content,
        startIndex: 0,
        endIndex: doc.content.length,
        metadata: doc.metadata,
      } as DocumentChunk,
      similarity: doc.similarity,
      rank: index + 1,
    }));
  }

  clear(): void {
    this.embeddingStore.clear();
  }

  saveIndex(filename: string = "rag-index.json"): void {
    this.embeddingStore.saveToFile(filename);
  }

  loadIndex(filename: string = "rag-index.json"): void {
    this.embeddingStore.loadFromFile(filename);
  }
}
```

### Document Loaders

```typescript
// Diferentes tipos de cargadores de documentos
interface DocumentLoader {
  load(source: string): Promise<RAGDocument[]>;
}

// Cargador de archivos de texto
class TextFileLoader implements DocumentLoader {
  async load(filepath: string): Promise<RAGDocument[]> {
    const fs = require("fs");
    const path = require("path");

    if (!fs.existsSync(filepath)) {
      throw new Error(`Archivo no encontrado: ${filepath}`);
    }

    const content = fs.readFileSync(filepath, "utf8");
    const filename = path.basename(filepath, path.extname(filepath));

    return [
      {
        id: `file-${filename}-${Date.now()}`,
        content: content.trim(),
        metadata: {
          title: filename,
          source: filepath,
          type: "text",
          loadedAt: new Date().toISOString(),
        },
      },
    ];
  }
}

// Cargador de múltiples archivos de texto
class DirectoryLoader implements DocumentLoader {
  private extensions: string[];

  constructor(extensions: string[] = [".txt", ".md", ".mdx"]) {
    this.extensions = extensions;
  }

  async load(dirPath: string): Promise<RAGDocument[]> {
    const fs = require("fs");
    const path = require("path");

    if (!fs.existsSync(dirPath)) {
      throw new Error(`Directorio no encontrado: ${dirPath}`);
    }

    const documents: RAGDocument[] = [];
    const files = fs.readdirSync(dirPath);

    for (const file of files) {
      const fullPath = path.join(dirPath, file);
      const ext = path.extname(file).toLowerCase();

      if (this.extensions.includes(ext) && fs.statSync(fullPath).isFile()) {
        try {
          const content = fs.readFileSync(fullPath, "utf8");
          const filename = path.basename(file, ext);

          documents.push({
            id: `file-${filename}-${Date.now()}-${Math.random()
              .toString(36)
              .substr(2, 9)}`,
            content: content.trim(),
            metadata: {
              title: filename,
              source: fullPath,
              type: "text",
              extension: ext,
              loadedAt: new Date().toISOString(),
            },
          });
        } catch (error) {
          console.error(`Error cargando archivo ${file}:`, error);
        }
      }
    }

    return documents;
  }
}

// Cargador de URLs/sitios web (básico)
class WebLoader implements DocumentLoader {
  async load(url: string): Promise<RAGDocument[]> {
    try {
      // En un entorno real usarías librerías como cheerio, jsdom, etc.
      console.log(
        "⚠️ WebLoader es una implementación básica. Para producción usar librerías especializadas."
      );

      const response = await fetch(url);
      if (!response.ok) {
        throw new Error(`HTTP ${response.status}: ${response.statusText}`);
      }

      let content = await response.text();

      // Extracción básica de contenido HTML
      content = content
        .replace(/<script[^>]*>.*?<\/script>/gis, "")
        .replace(/<style[^>]*>.*?<\/style>/gis, "")
        .replace(/<[^>]*>/g, " ")
        .replace(/\s+/g, " ")
        .trim();

      const title =
        this.extractTitle(await response.text()) || new URL(url).hostname;

      return [
        {
          id: `web-${Date.now()}-${Math.random().toString(36).substr(2, 9)}`,
          content,
          metadata: {
            title,
            source: url,
            type: "web",
            loadedAt: new Date().toISOString(),
          },
        },
      ];
    } catch (error) {
      console.error(`Error cargando URL ${url}:`, error);
      throw error;
    }
  }

  private extractTitle(html: string): string | null {
    const titleMatch = html.match(/<title[^>]*>([^<]*)<\/title>/i);
    return titleMatch ? titleMatch[1].trim() : null;
  }
}

// Cargador de JSON estructurado
class JSONLoader implements DocumentLoader {
  private contentField: string;
  private titleField?: string;

  constructor(contentField: string = "content", titleField?: string) {
    this.contentField = contentField;
    this.titleField = titleField;
  }

  async load(filepath: string): Promise<RAGDocument[]> {
    const fs = require("fs");

    if (!fs.existsSync(filepath)) {
      throw new Error(`Archivo no encontrado: ${filepath}`);
    }

    const data = JSON.parse(fs.readFileSync(filepath, "utf8"));
    const documents: RAGDocument[] = [];

    // Determinar si es un array o un objeto single
    const items = Array.isArray(data) ? data : [data];

    items.forEach((item, index) => {
      const content = item[this.contentField];
      if (!content || typeof content !== "string") {
        console.warn(
          `Item ${index} no tiene campo de contenido válido: ${this.contentField}`
        );
        return;
      }

      const title = this.titleField
        ? item[this.titleField]
        : `Documento ${index + 1}`;

      documents.push({
        id: `json-${index}-${Date.now()}`,
        content: content.trim(),
        metadata: {
          title,
          source: filepath,
          type: "json",
          index,
          ...item, // Incluir todos los campos como metadata
          loadedAt: new Date().toISOString(),
        },
      });
    });

    return documents;
  }
}
```

### RAG Avanzado con Citation

```typescript
// Sistema RAG con capacidades de citación y verificación
class AdvancedRAGSystem extends RAGSystem {
  // Generar respuesta con citas detalladas
  async generateWithCitations(
    query: string,
    context: RetrievalResult[],
    options: {
      model?: string;
      temperature?: number;
      citationStyle?: "numbered" | "parenthetical" | "footnote";
    } = {}
  ): Promise<RAGResponse & { citations: string[] }> {
    const { citationStyle = "numbered" } = options;

    if (context.length === 0) {
      throw new Error("No hay contexto disponible para generar respuesta");
    }

    // Preparar contexto con referencias numeradas
    let contextText = "";
    const citations: string[] = [];

    context.forEach((result, index) => {
      const citationNum = index + 1;
      const title =
        result.document.metadata?.title || `Documento ${citationNum}`;
      const source = result.document.metadata?.source || "Fuente desconocida";

      contextText += `[${citationNum}] ${result.chunk.content}\n\n`;

      // Formato de citación
      switch (citationStyle) {
        case "numbered":
          citations.push(`[${citationNum}] ${title} - ${source}`);
          break;
        case "parenthetical":
          citations.push(`(${title}, ${source})`);
          break;
        case "footnote":
          citations.push(`${citationNum}. ${title}. ${source}`);
          break;
      }
    });

    const citationInstruction = {
      numbered:
        "Usa referencias numeradas [1], [2], etc. para citar información específica.",
      parenthetical:
        "Usa citas parentéticas (Fuente, año) cuando referencias información.",
      footnote: "Usa números de pie de página¹, ², etc. para las citas.",
    };

    const prompt = `Eres un asistente experto que responde preguntas basándose únicamente en el contexto proporcionado.

INSTRUCCIONES IMPORTANTES:
- Responde SOLO basándote en la información del contexto
- ${citationInstruction[citationStyle]}
- Si la información no está en el contexto, di "No tengo información suficiente"
- Sé preciso, detallado y académico en tu respuesta
- Si hay información contradictoria, menciónalo y cita ambas fuentes

CONTEXTO CON REFERENCIAS:
${contextText}

PREGUNTA: ${query}

RESPUESTA (con citas apropiadas):`;

    try {
      const completion = await openai.chat.completions.create({
        model: options.model || "gpt-4o",
        messages: [{ role: "user", content: prompt }],
        temperature: options.temperature || 0.2,
        max_tokens: 2000,
      });

      const answer =
        completion.choices[0]?.message?.content || "Error generando respuesta";

      // Estadísticas de tokens
      const tokenCounter = new TokenCounter();
      const inputTokens = tokenCounter.count(prompt);
      const outputTokens = tokenCounter.count(answer);
      tokenCounter.free();

      const avgSimilarity =
        context.reduce((sum, source) => sum + source.similarity, 0) /
        context.length;

      return {
        answer,
        sources: context,
        confidence: Math.min(avgSimilarity * 1.1, 1.0),
        tokensUsed: {
          input: inputTokens,
          output: outputTokens,
        },
        citations,
      };
    } catch (error) {
      console.error("Error generando respuesta con citas:", error);
      throw error;
    }
  }

  // Verificar claims en la respuesta contra el contexto
  async verifyClaims(
    answer: string,
    context: RetrievalResult[]
  ): Promise<{
    verifiedClaims: Array<{
      claim: string;
      supported: boolean;
      confidence: number;
      sources: string[];
    }>;
    overallTrustScore: number;
  }> {
    // Extraer claims principales (simplificado)
    const prompt = `Extrae las principales afirmaciones factibles de este texto. Devuelve una lista JSON de las afirmaciones:

TEXTO:
${answer}

Formato: ["afirmación 1", "afirmación 2", ...]`;

    try {
      const completion = await openai.chat.completions.create({
        model: "gpt-4o",
        messages: [{ role: "user", content: prompt }],
        temperature: 0.1,
      });

      const response = completion.choices[0]?.message?.content || "[]";
      let claims: string[] = [];

      try {
        claims = JSON.parse(response);
      } catch {
        // Fallback: extraer claims manualmente
        claims = answer
          .split(".")
          .slice(0, 5)
          .map((s) => s.trim())
          .filter((s) => s.length > 10);
      }

      const verifiedClaims = [];
      const contextTexts = context.map((c) => c.chunk.content);

      for (const claim of claims) {
        // Verificar cada claim contra el contexto
        const verification = await this.verifySingleClaim(claim, contextTexts);
        verifiedClaims.push(verification);
      }

      // Calcular trust score general
      const supportedClaims = verifiedClaims.filter((c) => c.supported).length;
      const overallTrustScore =
        claims.length > 0 ? supportedClaims / claims.length : 0;

      return {
        verifiedClaims,
        overallTrustScore,
      };
    } catch (error) {
      console.error("Error verificando claims:", error);
      return {
        verifiedClaims: [],
        overallTrustScore: 0,
      };
    }
  }

  private async verifySingleClaim(
    claim: string,
    contextTexts: string[]
  ): Promise<{
    claim: string;
    supported: boolean;
    confidence: number;
    sources: string[];
  }> {
    const prompt = `Verifica si la siguiente afirmación está respaldada por el contexto proporcionado.

AFIRMACIÓN: ${claim}

CONTEXTO:
${contextTexts.join("\n\n---\n\n")}

¿La afirmación está respaldada por el contexto? Responde con JSON:
{
  "supported": true/false,
  "confidence": 0.0-1.0,
  "explanation": "breve explicación",
  "relevant_context_indices": [0, 1, 2...]
}`;

    try {
      const completion = await openai.chat.completions.create({
        model: "gpt-4o",
        messages: [{ role: "user", content: prompt }],
        temperature: 0.1,
      });

      const response = completion.choices[0]?.message?.content || "{}";
      const result = JSON.parse(response);

      return {
        claim,
        supported: result.supported || false,
        confidence: result.confidence || 0,
        sources: (result.relevant_context_indices || []).map(
          (i: number) => `Contexto ${i + 1}`
        ),
      };
    } catch (error) {
      return {
        claim,
        supported: false,
        confidence: 0,
        sources: [],
      };
    }
  }

  // Query con verificación automática
  async queryWithVerification(
    question: string,
    options: {
      topK?: number;
      threshold?: number;
      model?: string;
      citationStyle?: "numbered" | "parenthetical" | "footnote";
      verifyClaims?: boolean;
    } = {}
  ): Promise<
    RAGResponse & {
      citations?: string[];
      verification?: {
        verifiedClaims: Array<{
          claim: string;
          supported: boolean;
          confidence: number;
          sources: string[];
        }>;
        overallTrustScore: number;
      };
    }
  > {
    console.log(`❓ Procesando consulta con verificación: "${question}"`);

    // Recuperar contexto
    const retrievalResults = await this.retrieve(question, {
      topK: options.topK,
      threshold: options.threshold,
      rerank: true,
    });

    if (retrievalResults.length === 0) {
      return {
        answer:
          "Lo siento, no encontré información relevante para responder tu pregunta en la base de conocimiento disponible.",
        sources: [],
        confidence: 0,
        tokensUsed: { input: 0, output: 0 },
      };
    }

    // Generar respuesta con citas
    const response = await this.generateWithCitations(
      question,
      retrievalResults,
      options
    );

    // Verificar claims si se solicita
    let verification;
    if (options.verifyClaims) {
      verification = await this.verifyClaims(response.answer, retrievalResults);
    }

    console.log(
      `✅ Respuesta verificada generada (confianza: ${(
        response.confidence * 100
      ).toFixed(1)}%)`
    );

    return {
      ...response,
      verification,
    };
  }
}
```

### Ejemplos Prácticos Completos

```typescript
// 1. RAG para documentación técnica
async function ragDocumentacionTecnica() {
  console.log("📚 Sistema RAG para Documentación Técnica\n");

  const ragSystem = new AdvancedRAGSystem({
    chunkSize: 800,
    chunkOverlap: 100,
    maxContextTokens: 6000,
  });

  // Documentos de ejemplo (documentación de TypeScript/JavaScript)
  const docs: RAGDocument[] = [
    {
      id: "ts-basics",
      content: `
# TypeScript Básico

TypeScript es un superset de JavaScript desarrollado por Microsoft que añade tipos estáticos opcionales al lenguaje. Compila a JavaScript puro y puede ejecutarse en cualquier lugar donde JavaScript se ejecute.

## Características Principales

- **Tipado estático**: Permite detectar errores en tiempo de compilación
- **Inferencia de tipos**: TypeScript puede inferir tipos automáticamente
- **Compatibilidad con JavaScript**: Todo código JavaScript válido es código TypeScript válido
- **Tooling avanzado**: Mejor autocompletado, refactoring y navegación de código

## Instalación

Para instalar TypeScript globalmente:
\`\`\`bash
npm install -g typescript
\`\`\`

Para compilar un archivo TypeScript:
\`\`\`bash
tsc archivo.ts
\`\`\`
      `,
      metadata: {
        title: "TypeScript Básico",
        source: "docs/typescript/basics.md",
        author: "Documentación Oficial",
        tags: ["typescript", "basics", "installation"],
      },
    },
    {
      id: "ts-interfaces",
      content: `
# Interfaces en TypeScript

Las interfaces en TypeScript definen la forma que debe tener un objeto. Son una forma poderosa de definir contratos dentro de tu código.

## Sintaxis Básica

\`\`\`typescript
interface User {
  name: string;
  age: number;
  email?: string; // Propiedad opcional
}

const user: User = {
  name: "Juan",
  age: 30
};
\`\`\`

## Interfaces Extendibles

Las interfaces pueden extender otras interfaces:

\`\`\`typescript
interface Person {
  name: string;
  age: number;
}

interface Employee extends Person {
  employeeId: string;
  department: string;
}
\`\`\`

## Interfaces vs Types

- Las interfaces son más extensibles
- Los types son más flexibles para uniones y tipos complejos
- Para objetos simples, usa interfaces
      `,
      metadata: {
        title: "Interfaces en TypeScript",
        source: "docs/typescript/interfaces.md",
        author: "Documentación Oficial",
        tags: ["typescript", "interfaces", "types"],
      },
    },
    {
      id: "js-async",
      content: `
# JavaScript Asíncrono

JavaScript es un lenguaje de un solo hilo, pero puede manejar operaciones asíncronas mediante callbacks, promesas y async/await.

## Promesas

Las promesas representan un valor que puede estar disponible ahora, en el futuro, o nunca.

\`\`\`javascript
const fetchData = () => {
  return new Promise((resolve, reject) => {
    setTimeout(() => {
      resolve("Datos obtenidos");
    }, 1000);
  });
};

fetchData()
  .then(data => console.log(data))
  .catch(error => console.error(error));
\`\`\`

## Async/Await

Sintaxis más limpia para trabajar con promesas:

\`\`\`javascript
async function getData() {
  try {
    const data = await fetchData();
    console.log(data);
  } catch (error) {
    console.error(error);
  }
}
\`\`\`

## Manejo de Errores

Siempre usa try-catch con async/await para manejar errores apropiadamente.
      `,
      metadata: {
        title: "JavaScript Asíncrono",
        source: "docs/javascript/async.md",
        author: "Guía JavaScript",
        tags: ["javascript", "async", "promises"],
      },
    },
  ];

  // Indexar documentos
  await ragSystem.indexDocuments(docs);

  // Mostrar estadísticas
  const stats = await ragSystem.getStats();
  console.log("📊 Estadísticas del sistema:");
  console.log(`- Documentos: ${stats.totalDocuments}`);
  console.log(`- Chunks: ${stats.totalChunks}`);
  console.log(`- Promedio chunks/doc: ${stats.avgChunksPerDocument}\n`);

  // Consultas de prueba
  const consultas = [
    "¿Cómo instalo TypeScript?",
    "¿Cuál es la diferencia entre interfaces y types?",
    "¿Cómo manejo errores con async/await?",
    "¿Qué es la inferencia de tipos en TypeScript?",
  ];

  for (const consulta of consultas) {
    console.log(`❓ Pregunta: "${consulta}"`);

    const response = await ragSystem.queryWithVerification(consulta, {
      topK: 3,
      citationStyle: "numbered",
      verifyClaims: true,
    });

    console.log(`🤖 Respuesta:`);
    console.log(response.answer);
    console.log(`\n📚 Fuentes utilizadas:`);
    response.citations?.forEach((citation) => console.log(`  ${citation}`));

    if (response.verification) {
      console.log(
        `\n🔍 Verificación (confianza: ${(
          response.verification.overallTrustScore * 100
        ).toFixed(1)}%):`
      );
      response.verification.verifiedClaims.forEach((claim) => {
        const status = claim.supported ? "✅" : "❌";
        console.log(`  ${status} ${claim.claim.substring(0, 60)}...`);
      });
    }

    console.log(
      `\n💰 Tokens usados: ${
        response.tokensUsed.input + response.tokensUsed.output
      }`
    );
    console.log("\n" + "=".repeat(80) + "\n");
  }
}

// 2. Sistema RAG para base de conocimiento empresarial
async function ragBaseConocimientoEmpresarial() {
  console.log("🏢 Sistema RAG para Base de Conocimiento Empresarial\n");

  const ragSystem = new RAGSystem({
    chunkSize: 600,
    chunkOverlap: 150,
  });

  // Simular documentos empresariales
  const empresaDocs: RAGDocument[] = [
    {
      id: "politica-vacaciones",
      content: `
# Política de Vacaciones

## Días de Vacaciones

- Empleados con menos de 2 años: 15 días hábiles
- Empleados con 2-5 años: 20 días hábiles  
- Empleados con más de 5 años: 25 días hábiles

## Proceso de Solicitud

1. Solicitar vacaciones con al menos 15 días de anticipación
2. Obtener aprobación del supervisor directo
3. Registrar en el sistema HR antes de tomar las vacaciones
4. Las vacaciones no utilizadas no se acumulan al siguiente año

## Vacaciones de Emergencia

En casos de emergencia familiar, se pueden aprobar vacaciones con menos anticipación, sujeto a disponibilidad del equipo.
      `,
      metadata: {
        title: "Política de Vacaciones",
        source: "hr/policies/vacation.md",
        department: "HR",
        lastUpdated: "2024-01-15",
      },
    },
    {
      id: "proceso-reembolsos",
      content: `
# Proceso de Reembolsos

## Gastos Reembolsables

- Comidas de trabajo (máximo $50 por día)
- Transporte público para trabajo
- Materiales de oficina autorizados
- Gastos de capacitación aprobados

## Documentación Requerida

- Recibos originales
- Formulario de solicitud de reembolso completado
- Justificación del gasto relacionado con el trabajo
- Aprobación del manager (gastos >$100)

## Tiempos de Procesamiento

- Solicitudes completas: 5-7 días hábiles
- Solicitudes incompletas: se devuelven para corrección
- Pagos se realizan los viernes de cada semana
      `,
      metadata: {
        title: "Proceso de Reembolsos",
        source: "finance/procedures/reimbursements.md",
        department: "Finance",
        lastUpdated: "2024-02-01",
      },
    },
    {
      id: "trabajo-remoto",
      content: `
# Política de Trabajo Remoto

## Elegibilidad

- Empleados con más de 6 meses en la empresa
- Roles que permiten trabajo independiente
- Evaluación de desempeño satisfactoria o superior

## Modalidades Disponibles

- **Híbrido**: 2-3 días remotos por semana
- **Completamente remoto**: Para roles específicos
- **Remoto temporal**: Circunstancias especiales

## Requisitos

- Conexión de internet estable (mín. 50 Mbps)
- Espacio de trabajo dedicado
- Participación en reuniones de equipo obligatorias
- Disponibilidad durante horario laboral (9 AM - 5 PM)

## Equipamiento

La empresa proporciona laptop y accesorios básicos. Equipamiento adicional debe ser aprobado por IT.
      `,
      metadata: {
        title: "Política de Trabajo Remoto",
        source: "hr/policies/remote-work.md",
        department: "HR",
        lastUpdated: "2024-03-10",
      },
    },
  ];

  await ragSystem.indexDocuments(empresaDocs);

  // Simular consultas de empleados
  const consultasEmpleados = [
    "¿Cuántos días de vacaciones tengo si llevo 3 años en la empresa?",
    "¿Qué necesito para solicitar reembolso de una cena de trabajo?",
    "¿Puedo trabajar completamente remoto?",
    "¿Cuánto tiempo toma procesar un reembolso?",
    "¿Necesito aprobación para gastos de $75 en capacitación?",
  ];

  for (const consulta of consultasEmpleados) {
    console.log(`👤 Empleado pregunta: "${consulta}"`);

    const response = await ragSystem.query(consulta, {
      topK: 2,
      threshold: 0.4,
    });

    console.log(`🏢 Respuesta HR:`);
    console.log(response.answer);

    console.log(`\n📋 Fuentes consultadas:`);
    response.sources.forEach((source, index) => {
      console.log(
        `  ${index + 1}. ${source.document.metadata?.title} (${
          source.document.metadata?.department
        })`
      );
    });

    console.log(`\n📊 Confianza: ${(response.confidence * 100).toFixed(1)}%`);
    console.log("\n" + "-".repeat(60) + "\n");
  }
}

// 3. RAG con múltiples fuentes de datos
async function ragMultipleFuentes() {
  console.log("🗂️ Sistema RAG con Múltiples Fuentes de Datos\n");

  const ragSystem = new RAGSystem();

  // Cargar datos de diferentes fuentes
  console.log("📂 Cargando datos de diferentes fuentes...\n");

  // 1. Texto plano
  const textDocs: RAGDocument[] = [
    {
      id: "manual-api",
      content: `
# Manual de API REST

Nuestra API REST utiliza autenticación JWT y sigue las convenciones REST estándar.

## Endpoints Principales

- GET /api/users - Obtener lista de usuarios
- POST /api/users - Crear nuevo usuario
- PUT /api/users/:id - Actualizar usuario
- DELETE /api/users/:id - Eliminar usuario

## Autenticación

Incluir token JWT en header: Authorization: Bearer <token>

Tokens expiran en 24 horas y deben renovarse.
      `,
      metadata: {
        title: "Manual API REST",
        source: "manual.txt",
        type: "manual",
      },
    },
  ];

  // 2. FAQ estructurado como JSON
  const faqData = [
    {
      question: "¿Cómo reseteo mi contraseña?",
      answer:
        "Ve a la página de login, haz clic en 'Olvidé mi contraseña', ingresa tu email y sigue las instrucciones del email que recibirás.",
      category: "account",
      priority: "high",
    },
    {
      question: "¿Puedo cambiar mi plan de suscripción?",
      answer:
        "Sí, puedes cambiar tu plan en cualquier momento desde la configuración de tu cuenta. Los cambios se aplican inmediatamente y la facturación se ajusta pro-rata.",
      category: "billing",
      priority: "medium",
    },
    {
      question: "¿Cómo exporto mis datos?",
      answer:
        "Ve a Configuración > Privacidad > Exportar Datos. Recibirás un archivo ZIP con todos tus datos en formato JSON en tu email en 24-48 horas.",
      category: "data",
      priority: "medium",
    },
  ];

  const faqDocs: RAGDocument[] = faqData.map((item, index) => ({
    id: `faq-${index}`,
    content: `Pregunta: ${item.question}\n\nRespuesta: ${item.answer}`,
    metadata: {
      title: item.question,
      source: "faq.json",
      type: "faq",
      category: item.category,
      priority: item.priority,
    },
  }));

  // 3. Datos de configuración
  const configDocs: RAGDocument[] = [
    {
      id: "config-database",
      content: `
# Configuración de Base de Datos

## Conexión Principal
- Host: db.empresa.com
- Puerto: 5432
- Database: produccion_db
- Usuario: app_user

## Pool de Conexiones
- Máximo: 20 conexiones
- Mínimo: 2 conexiones  
- Timeout: 30 segundos

## Backups
- Backups diarios a las 2:00 AM
- Retención: 30 días
- Storage: AWS S3 bucket backup-db-prod
      `,
      metadata: {
        title: "Configuración Database",
        source: "config/database.md",
        type: "config",
      },
    },
  ];

  // Indexar todas las fuentes
  await ragSystem.indexDocuments([...textDocs, ...faqDocs, ...configDocs]);

  const stats = await ragSystem.getStats();
  console.log(
    `✅ Indexadas ${stats.totalDocuments} fuentes con ${stats.totalChunks} chunks\n`
  );

  // Consultas que requieren información de múltiples fuentes
  const consultasComplejas = [
    "¿Cómo autentico las llamadas a la API y qué hago si olvido mi contraseña?",
    "¿Dónde están configurados los backups de la base de datos?",
    "¿Puedo exportar mis datos y cambiar mi plan al mismo tiempo?",
    "¿Cuáles son los endpoints principales de la API?",
  ];

  for (const consulta of consultasComplejas) {
    console.log(`❓ Consulta compleja: "${consulta}"`);

    const response = await ragSystem.query(consulta, {
      topK: 4,
      threshold: 0.3,
      rerank: true,
    });

    console.log(`🤖 Respuesta integrada:`);
    console.log(response.answer);

    console.log(`\n📚 Fuentes utilizadas (${response.sources.length}):`);
    response.sources.forEach((source) => {
      const type = source.document.metadata?.type || "desconocido";
      const title = source.document.metadata?.title || source.document.id;
      console.log(
        `  • [${type.toUpperCase()}] ${title} (similitud: ${(
          source.similarity * 100
        ).toFixed(1)}%)`
      );
    });

    console.log("\n" + "=".repeat(80) + "\n");
  }
}

// Ejecutar todos los ejemplos
async function ejecutarEjemplosRAG() {
  console.log("🚀".repeat(20));
  console.log("🔗 EJEMPLOS DE RAG (RETRIEVAL AUGMENTED GENERATION)");
  console.log("🚀".repeat(20));

  try {
    await ragDocumentacionTecnica();
    console.log("\n" + "🏢".repeat(40) + "\n");

    await ragBaseConocimientoEmpresarial();
    console.log("\n" + "🗂️".repeat(40) + "\n");

    await ragMultipleFuentes();

    console.log("✅ Todos los ejemplos de RAG ejecutados correctamente");
  } catch (error) {
    console.error("❌ Error en ejemplos RAG:", error);
  }
}

// Descomenta para ejecutar:
// ejecutarEjemplosRAG().catch(console.error);
```

### Best Practices para RAG

```typescript
const RAG_BEST_PRACTICES = {
  // 1. Chunking estratégico
  chunkingStrategy: [
    "Usar tamaños de chunk apropiados (500-1500 tokens)",
    "Mantener overlap del 10-20% para preservar contexto",
    "Dividir en límites semánticos (párrafos, secciones)",
    "Incluir títulos y headers como contexto",
    "Considerar estructura del documento (listas, tablas, código)",
  ],

  // 2. Calidad de embeddings
  embeddingQuality: [
    "Limpiar y normalizar texto antes de crear embeddings",
    "Usar modelos de embedding apropiados para el dominio",
    "Experimentar con diferentes estrategias de chunking",
    "Evaluar calidad con métricas como precision/recall",
    "Actualizar embeddings cuando cambie el contenido",
  ],

  // 3. Recuperación efectiva
  retrieval: [
    "Ajustar threshold de similitud según el caso de uso",
    "Implementar re-ranking para mejor relevancia",
    "Combinar búsqueda semántica con keyword matching",
    "Filtrar por metadatos relevantes (fecha, tipo, etc.)",
    "Limitar contexto para evitar confundir al modelo",
  ],

  // 4. Generación optimizada
  generation: [
    "Usar prompts específicos para el dominio",
    "Instruir claramente sobre cómo usar el contexto",
    "Implementar verificación de claims cuando sea crítico",
    "Incluir sistemas de citación para transparencia",
    "Manejar casos donde no hay información suficiente",
  ],

  // 5. Monitoreo y evaluación
  monitoring: [
    "Implementar logging detallado de queries y respuestas",
    "Trackear métricas de relevancia y satisfacción",
    "Evaluar regularmente con datasets de test",
    "Monitorear costos de tokens y embeddings",
    "A/B test diferentes configuraciones",
  ],
};

// Sistema de evaluación para RAG
class RAGEvaluator {
  static async evaluateRAGSystem(
    ragSystem: RAGSystem,
    testCases: Array<{
      question: string;
      expectedAnswer?: string;
      expectedSources?: string[];
      category?: string;
    }>
  ): Promise<{
    overallScore: number;
    categoryScores: Record<string, number>;
    detailedResults: Array<{
      question: string;
      answer: string;
      sources: number;
      relevanceScore: number;
      completenessScore: number;
      category?: string;
    }>;
  }> {
    const detailedResults = [];
    const categoryScores: Record<string, number[]> = {};

    for (const testCase of testCases) {
      console.log(`🧪 Evaluando: "${testCase.question}"`);

      const response = await ragSystem.query(testCase.question);

      // Evaluar relevancia (simplificado)
      const relevanceScore = response.confidence;

      // Evaluar completeness basado en longitud de respuesta y fuentes
      const completenessScore = Math.min(
        (response.answer.length / 200) * 0.5 +
          (response.sources.length / 3) * 0.5,
        1.0
      );

      const result = {
        question: testCase.question,
        answer: response.answer,
        sources: response.sources.length,
        relevanceScore,
        completenessScore,
        category: testCase.category,
      };

      detailedResults.push(result);

      // Agrupar por categoría
      if (testCase.category) {
        if (!categoryScores[testCase.category]) {
          categoryScores[testCase.category] = [];
        }
        categoryScores[testCase.category].push(
          (relevanceScore + completenessScore) / 2
        );
      }
    }

    // Calcular scores promedio por categoría
    const avgCategoryScores: Record<string, number> = {};
    for (const [category, scores] of Object.entries(categoryScores)) {
      avgCategoryScores[category] =
        scores.reduce((sum, score) => sum + score, 0) / scores.length;
    }

    // Score general
    const overallScore =
      detailedResults.reduce(
        (sum, result) =>
          sum + (result.relevanceScore + result.completenessScore) / 2,
        0
      ) / detailedResults.length;

    return {
      overallScore,
      categoryScores: avgCategoryScores,
      detailedResults,
    };
  }

  static generateEvaluationReport(evaluation: any): string {
    let report = `
# 📊 Reporte de Evaluación RAG

## Resultados Generales
- **Score Overall**: ${(evaluation.overallScore * 100).toFixed(1)}%

## Scores por Categoría
`;

    for (const [category, score] of Object.entries(evaluation.categoryScores)) {
      report += `- **${category}**: ${((score as number) * 100).toFixed(1)}%\n`;
    }

    report += `
## Resultados Detallados

`;

    evaluation.detailedResults.forEach((result: any, index: number) => {
      report += `### Pregunta ${index + 1}
**Q**: ${result.question}
**A**: ${result.answer.substring(0, 100)}...
**Fuentes**: ${result.sources}
**Relevancia**: ${(result.relevanceScore * 100).toFixed(1)}%
**Completitud**: ${(result.completenessScore * 100).toFixed(1)}%

`;
    });

    return report;
  }
}

console.log("🔗 Sistema RAG avanzado configurado");
console.log("📊 Evaluador RAG disponible");
console.log("💡 Best practices para RAG cargadas");
```

---
