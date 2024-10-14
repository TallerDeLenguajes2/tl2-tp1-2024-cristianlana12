using System.IO.Compression;
using persistencia;

internal class Program
{
    private static Cadeteria cadeteria;
    private static void Main(string[] args)
    {
        try
        {
            AccesoDatos? acceso;
            System.Console.WriteLine("--- Seleccione el tipo de acceso a los datos ---");
            System.Console.WriteLine("\n\t1. CSV");
            System.Console.WriteLine("\n> Digite su opcion: ");
            string strOpcion = Console.ReadLine() ?? string.Empty;

            if (!int.TryParse(strOpcion, out int opcion))
            {
                
            }
        }
        catch (System.Exception)
        {
            
            throw;
        }        
    }

     private static void MostrarError(string error)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        System.Console.WriteLine($"\n[!] Error: {error}\n");
        Console.ResetColor();
    }

    private static void MostrarResultadoExitoso(string mensaje)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        System.Console.WriteLine($"\n[/] {mensaje}\n");
        Console.ResetColor();
    }

    private static Cliente SolicitarDatosCliente()
    {
        System.Console.Write("> Ingrese el DNI del cliente (sin puntos ni espacios): ");
        var dni = 0;
        var strDni = Console.ReadLine() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(strDni)) throw new Exception("El DNI no puede estar vacio");
        if (!int.TryParse(strDni, out dni)) throw new Exception("El DNI debe ser un número");

        System.Console.Write("> Ingrese el nombre del cliente: ");
        var nombre = Console.ReadLine() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(nombre)) throw new Exception("El nombre no puede estar vacío");

        System.Console.Write("> Ingrese el telefono del cliente: ");
        var telefono = Console.ReadLine() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(telefono)) throw new Exception("El teléfono no puede estar vacío");

        System.Console.Write("> Ingrese la dirección del cliente: ");
        var direccion = Console.ReadLine() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(direccion)) throw new Exception("La dirección no puede estar vacía");

        System.Console.Write("> Ingrese datos o referencias de la dirección del cliente (opcional): ");
        var datosRerencia = Console.ReadLine() ?? string.Empty;

        return new Cliente(dni, nombre, direccion, telefono, datosRerencia);
    }

    private static Pedido SolicitarDatosPedido(Cliente cliente)
    {
        System.Console.Write("> Ingrese los detalles del pedido (obligatorio): ");
        var detalles = Console.ReadLine() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(detalles)) throw new Exception("Debe incluir los detalles del pedido");

        return new Pedido(detalles, cliente);
    }

    private static Pedido SolicitarSeleccionPedido(List<Pedido> pedidos)
    {
        var detallesPedidos = pedidos.Select(pedido => pedido.ToString());
        foreach (var detallePedido in detallesPedidos)
        {
            Console.WriteLine($"\t* {detallePedido}");
        }

        System.Console.Write("\n> Ingrese el número del pedido a asignar: ");
        var strNro = Console.ReadLine() ?? string.Empty;
        var nroPedido = 0;

        if (!int.TryParse(strNro, out nroPedido))
            throw new Exception("Debe ingresar un número entero para seleccionar el pedido");

        var pedidoSeleccionado = pedidos.Where(p => p.Numero == nroPedido).FirstOrDefault();
        if (pedidoSeleccionado == null)
            throw new Exception($"El número de pedido ingresado es inválido ({nroPedido})");

        return pedidoSeleccionado;
    }

    private static Cadete SolicitarSeleccionCadete()
    {
        var detallesCadetes = cadeteria.ListadoCadetes.Select(cadete => cadete.ToString());
        foreach (var detalle in detallesCadetes)
        {
            System.Console.WriteLine($"\t* {detalle}");
        }

        System.Console.Write("\n> Ingrese el ID del cadete al cual asignarle el pedido: ");
        var strId = Console.ReadLine() ?? string.Empty;
        var id = 0;

        if (!int.TryParse(strId, out id))
            throw new Exception("El ID debe ser un número");

        var cadeteSeleccionado = cadeteria.ListadoCadetes.Where(cadete => cadete.Id == id).FirstOrDefault();
        if (cadeteSeleccionado == null)
            throw new Exception("$No existe ningún cadete con el ID {id}");

        return cadeteSeleccionado;
    }

    private static Estado SolicitarSeleccionEstado()
    {
        int contador = 0;
        foreach (var estado in Enum.GetValues(typeof(Estado)))
        {
            System.Console.WriteLine($"> ID {++contador}. {estado}");
        }

        System.Console.Write("> Selecciona el ID del nuevo estado para el pedido: ");
        var strOpcion = Console.ReadLine() ?? string.Empty;

        Estado nuevoEstado;
        if (!Enum.TryParse(strOpcion, out nuevoEstado))
            throw new Exception("Seleccione un ID válido");

        return nuevoEstado;
    }

}