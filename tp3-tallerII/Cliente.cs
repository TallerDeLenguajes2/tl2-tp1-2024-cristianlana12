public class Cliente
{
    private int dni;
    private string nombre;
    private string direccion;
    private string telefono;
    private string datosDireccion;

    public Cliente(int dni, string nombre, string direccion, string telefono, string datosDireccion)
    {
        this.dni = dni;
        this.Nombre = nombre;
        this.Direccion = direccion;
        this.telefono = telefono;
        this.datosDireccion = datosDireccion;
    }

    public string Nombre { get => nombre; set => nombre = value; }
    public string Direccion { get => direccion; set => direccion = value; }

    public override string ToString()
    {
        return $"CLIENTE: \n\t* dni: {dni} \n\t* nombre: {nombre} \n\t* teléfono: {telefono} \n\t* dirección: {direccion} ({datosDireccion})";
    }
}