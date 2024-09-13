using cadeteria;

internal class Program
{
    private static Cadeteria cadeteria;
    private static void Main(string[] args)
    {
        try
        {
            cadeteria = CrearCadeteria();
            cargarCadetes(cadeteria);
        }
        catch (System.Exception e)
        {
            mostrarError(e.Message);
        }

        int opcionSeleccionado = 0;
        int opcionSalida = 5;

        do
        {
            Console.WriteLine("### MENU PRINCIPAL ###\n");
            Console.WriteLine("\t1- Dar de alta un pedido");
            Console.WriteLine("\t2- Asignar un pedido a un cadete");
            Console.WriteLine("\t3- Cambiar el estado de un pedido");
            Console.WriteLine("\t4- Reasignar el cadete en un pedido");
            Console.WriteLine("\t5- Salir del programa");
            Console.Write("\n>Seleccione una opcion: ");

            var strSeleccion = Console.ReadLine() ?? string.Empty;

            try
            {
                if (!int.TryParse(strSeleccion, out opcionSeleccionado))
                {
                    throw new Exception("debe ingresar un numero entero");
                }
                else if (opcionSeleccionado < 1 || opcionSeleccionado > opcionSalida)
                {
                    throw new Exception("Debe ingresar una opcion valida");
                }
                else
                {
                    switch (opcionSeleccionado)
                    {
                        case 1:
                            Console.WriteLine("\n\n ### INGRESAR PEDIDO ###\n");
                            var cliente = SolicitarDatosCliente();
                            var pedidoNew = SolicitarDatosPedido(cliente);
                            cadeteria.tomarPedido(pedidoNew);
                            MostrarResultadoExitoso($"Nuevo pedido generado con exito NRO: {pedidoNew.Nro} --- CLIENTE: {cliente.Nombre}");
                            break;
                        case 2:
                            if (!cadeteria.ListadoCadetes.Any()) throw new Exception("No hay cadetes para asignarle este pedidio");
                            if (!cadeteria.ListadoPedidos.Any()) throw new Exception("No hay pedidos para asignarle a los cadetes");

                            Console.WriteLine("\n\n### ASIGNANDO PEDIDO ###\n");
                            var pedidoNew2 = SolicitarSeleccionPedidos(cadeteria.ListadoPedidos);
                            Console.WriteLine();
                            var cadete = SolicitarSeleccionCadete();

                            cadeteria.AsignarCadete(cadete, pedidoNew2);
                            MostrarResultadoExitoso($"El pedido nro. {pedidoNew2.Nro} ha sido asignado al cadete {cadete.Nombre} ({cadete.Id})");

                            break;
                    }
                }
            }
            catch (System.Exception)
            {

                throw;
            }

        } while (opcionSeleccionado != opcionSalida);
    }


    private static void MostrarResultadoExitoso(string mensaje)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        System.Console.WriteLine($"\n[/] {mensaje}\n");
        Console.ResetColor();
    }
    private static Cadeteria CrearCadeteria()
    {
        var Csv = LeerCsv("datosCadeteria.csv");

        var datos = Csv[0].Split(",");

        if (datos.Count() < 2) throw new Exception("No hay datos suficientes para instanciar la cadeteria");

        return new Cadeteria(datos[0], datos[1]);
    }

    private static void cargarCadetes(Cadeteria cadeteria)
    {
        var Csv = LeerCsv("DatosCadetes.csv");

        foreach (var linea in Csv)
        {
            var datos = linea.Split(",");
            if (datos.Count() < 4)
            {
                System.Console.WriteLine($"\n[!] No se pudo cargar el cadete: {linea} - {datos}");
                continue;
            }

            cadeteria.AltaCadete(new Cadete(int.Parse(datos[0]), datos[1], datos[2], datos[3]));
        }
    }

    private static List<string> LeerCsv(string archivo, bool cabecera = true)
    {
        var lineas = new List<string>();

        using (FileStream archivoCsv = new FileStream(archivo, FileMode.Open))
        {
            using (StreamReader readerCsv = new StreamReader(archivoCsv))
            {
                if (cabecera) readerCsv.ReadLine();

                while (readerCsv.Peek() != -1)
                {
                    var linea = readerCsv.ReadLine();
                    if (!string.IsNullOrWhiteSpace(linea)) lineas.Add(linea);
                }
            }
        }

        return lineas;
    }

    private static void mostrarError(string error)
    {
        Console.WriteLine($"\n[!] ERROR: {error}\n");
    }

    private static Cliente SolicitarDatosCliente()
    {
        Console.WriteLine("> Ingresar DNI del CLIENTE (SIN PUNTOS NI ESPACIOS) <");
        var DNI = 0;
        var stringDni = Console.ReadLine() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(stringDni)) throw new Exception("El DNI no debe estar vacio");
        if (!int.TryParse(stringDni, out DNI)) throw new Exception("El DNI debe ser un numero");

        Console.WriteLine("> Ingrese el NOMBRE del CLIENTE <");
        var stringNombreCliente = Console.ReadLine() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(stringNombreCliente)) throw new Exception("El NOMBRE no puede estar vacio");

        System.Console.Write("> Ingrese el telefono del cliente: ");
        var telefono = Console.ReadLine() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(telefono)) throw new Exception("El teléfono no puede estar vacío");

        System.Console.Write("> Ingrese la dirección del cliente: ");
        var direccion = Console.ReadLine() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(direccion)) throw new Exception("La dirección no puede estar vacía");

        System.Console.Write("> Ingrese datos o referencias de la dirección del cliente (opcional): ");
        var datosRerencia = Console.ReadLine() ?? string.Empty;

        return new Cliente(DNI, stringNombreCliente, direccion, telefono, datosRerencia);
    }
    private static Pedido SolicitarDatosPedido(Cliente cliente)
    {
        Console.WriteLine("> Ingresar los detalles del pedido (OBLIGATORIO) <");
        var detalles = Console.ReadLine() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(detalles)) throw new Exception("Los Detalles no pueden estar vacios");

        return new Pedido(detalles, cliente);
    }
    private static Pedido SolicitarSeleccionPedidos(List<Pedido> listadoPedidos)
    {
        var detallePedidos = listadoPedidos.Select(pedido => pedido.ToString());
        foreach (var detallePedido in detallePedidos)
        {
            Console.WriteLine($"\t* {detallePedido}");
        }
        Console.WriteLine("\n> Ingrese el numero del pedido a asignar: <");
        var stringNumero = Console.ReadLine() ?? string.Empty;
        var nroPedido = 0;

        if (!int.TryParse(stringNumero, out nroPedido)) throw new Exception("Debe ingresar un numero entero para seleccionar el pedido ");

        var pedidoSeleccionado = listadoPedidos.Where(p => p.Nro == nroPedido).FirstOrDefault();
        if (pedidoSeleccionado == null) throw new Exception($"El numero de pedido es invalido ({nroPedido})");

        return pedidoSeleccionado;
    }
    private static Cadete SolicitarSeleccionCadete()
    {
        var detallesCadetes = cadeteria.ListadoCadetes.Select(cadete => cadete.ToString());
        foreach (var detalle in detallesCadetes)
        {
            Console.WriteLine($"\t- {detalle}");
        }

        Console.WriteLine("\n> Ingrese el ID del cadete al cual asignarle el pedido:  <");
        var stringId = Console.ReadLine() ?? string.Empty;
        var id = 0;

        if (!int.TryParse(stringId, out id)) throw new Exception("El ID debe ser un numero");

        var cadeteSeleccionado = cadeteria.ListadoCadetes.Where(cadete => cadete.Id == id).FirstOrDefault();

        if (cadeteSeleccionado == null) throw new Exception($"No existe ningun cadete con el ID: {id}");

        return cadeteSeleccionado;
    }
}