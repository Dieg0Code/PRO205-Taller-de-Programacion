import { Empleado } from "./Empleado.js";

export class Gerente extends Empleado {

    public equipoAcargo: number;
    public bonoGestion: number;

    constructor(
        nombre: string,
        email: string,
        salarioBase: number,
        añosServicio: number,
        equipoAcargo: number,
        bonoGestion: number
    ) {
        super(
            nombre,
            email,
            salarioBase,
            añosServicio
        );
        this.equipoAcargo = equipoAcargo;
        this.bonoGestion = bonoGestion;
    }

    public trabajar(): void {
        console.log(`${this.nombre} está gestionando un equipo de ${this.equipoAcargo} personas.`);
    }
    public calcularSalario(): number {
        return this.salarioBase + (this.añosServicio * 3000) + this.bonoGestion;
    }
    public obtenerInfo(): string {
        return `Gerente: ${this.nombre}, Email: ${this.email}, Salario Base: ${this.salarioBase}, Años de Servicio: ${this.añosServicio}, Equipo a Cargo: ${this.equipoAcargo}, Bono de Gestión: ${this.bonoGestion}`;
    }

}