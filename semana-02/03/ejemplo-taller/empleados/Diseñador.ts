import { Empleado } from "./Empleado.js";

export class Diseñador extends Empleado {

    public herramientaPrincipal: string;
    public portafolio: string;

    constructor(
        nombre: string,
        email: string,
        salarioBase: number,
        añosServicio: number,
        herramientaPrincipal: string,
        portafolio: string
    ) {
        super(nombre, email, salarioBase, añosServicio);
        this.herramientaPrincipal = herramientaPrincipal;
        this.portafolio = portafolio;
    }

    public trabajar(): void {
        console.log(`${this.nombre} está diseñando con ${this.herramientaPrincipal}.`);
    }

    public calcularSalario(): number {
        return this.salarioBase + (this.añosServicio * 1000);
    }

    public obtenerInfo(): string {
        return `Diseñador: ${this.nombre}, Email: ${this.email}, Salario Base: ${this.salarioBase}, Años de Servicio: ${this.añosServicio}, Herramienta Principal: ${this.herramientaPrincipal}, Portafolio: ${this.portafolio}`;
    }
}