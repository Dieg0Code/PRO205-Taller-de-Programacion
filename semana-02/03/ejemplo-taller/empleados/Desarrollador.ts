import { Empleado } from "./Empleado.js";

export class Desarrollador extends Empleado {

    public lenguajePrincipal: string;
    public nivel: string;

    constructor(
        nombre: string,
        email: string,
        salarioBase: number,
        añosServicio: number,
        lenguajePrincipal: string,
        nivel: string
    ) {
        super(nombre, email, salarioBase, añosServicio);
        this.lenguajePrincipal = lenguajePrincipal;
        this.nivel = nivel;
    }

    public trabajar(): void {
        console.log(`${this.nombre} está programando en ${this.lenguajePrincipal}.`);
    }
    public calcularSalario(): number {
        return this.salarioBase + (this.añosServicio * 2000);
    }
    public obtenerInfo(): string {
        return `Desarrollador: ${this.nombre}, Email: ${this.email}, Salario Base: ${this.salarioBase}, Años de Servicio: ${this.añosServicio}, Lenguaje Principal: ${this.lenguajePrincipal}, Nivel: ${this.nivel}`;
    }
}