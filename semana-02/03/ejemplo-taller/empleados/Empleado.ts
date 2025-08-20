export abstract class Empleado {
    public nombre: string;
    public email: string;
    public salarioBase: number;
    public añosServicio: number;

    constructor(
        nombre: string,
        email: string,
        salarioBase: number,
        añosServicio: number
    ) {
        this.nombre = nombre;
        this.email = email;
        this.salarioBase = salarioBase;
        this.añosServicio = añosServicio;
    }

    public abstract trabajar(): void;
    public abstract calcularSalario(): number;
    public abstract obtenerInfo(): string;
}