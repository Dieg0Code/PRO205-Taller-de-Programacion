import { Desarrollador } from "./Desarrollador.js";
import { Diseñador } from "./Diseñador.js";
import { Gerente } from "./Gerente.js";

function testearSistema(): void {
    console.log("=== TESTING: Sistema de Empleados ===\n");

    // Crear empleados de prueba
    const dev = new Desarrollador(
        "Ana García",
        "ana@tech.com",
        50000,
        3,
        "TypeScript",
        "Mid"
    );
    const diseñador = new Diseñador(
        "Carlos López",
        "carlos@tech.com",
        45000,
        2,
        "Figma",
        "portfolio.com"
    );
    const gerente = new Gerente(
        "María Silva",
        "maria@tech.com",
        80000,
        8,
        15,
        20000
    );

    // Probar funcionalidades
    console.log("--- Test 1: Trabajar (Polimorfismo) ---");
    [dev, diseñador, gerente].forEach((emp) => console.log(emp.trabajar()));

    console.log("\n--- Test 2: Cálculo de Salarios ---");
    console.log(`Dev: $${dev.calcularSalario()}`);
    console.log(`Diseñador: $${diseñador.calcularSalario()}`);
    console.log(`Gerente: $${gerente.calcularSalario()}`); // ¿Incluye el bono?

    console.log("\n--- Test 3: Información General ---");
    console.log(dev.obtenerInfo());
    console.log(
        `Lenguaje: ${dev.lenguajePrincipal}, Nivel: ${dev.nivel}`
    );
}

testearSistema();