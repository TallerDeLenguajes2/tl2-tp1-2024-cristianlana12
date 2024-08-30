namespace cadeteria
{
    public class Cliente
    {
        public int Dni {get; set;}
        public string? Nombre {get;set;}
        public string? Direccion {get;set;}
        public string? Telefono {get;set;}
        public string? DatosReferenciaDireccion {get;set;}

        public Cliente(int dni,string Nombre, string direccion, string telefono, string datosReferenciaDireccion){
            this.Dni = dni;
            this.Nombre = Nombre;
            this.Direccion = direccion;
            this.Telefono = telefono;
            this.DatosReferenciaDireccion = datosReferenciaDireccion;
        }

        public override string ToString()
    {
        return $"CLIENTE: \n\t* dni: {Dni} \n\t* nombre: {Nombre} \n\t* teléfono: {Telefono} \n\t* dirección: {Direccion} ({DatosReferenciaDireccion})";
    }
    }
}