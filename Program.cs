using cadeteria;
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
            System.Console.WriteLine("\t2. JSON");
            System.Console.Write("\n> Digite su opcion: ");
            string strOpcion = Console.ReadLine() ?? string.Empty;

            if (!int.TryParse(strOpcion, out int opcion))
            {
                mostrarError("Opción inválida. No se puede proceder con el cargado de datos.");
                return;
            }

            acceso = opcion switch
            {
                1 => new AccesoCsv(),
                2 => new AccesoJson(),
                _ => null
            };

            if (acceso == null)
            {
                MostrarError("Opción inválida. No se puede proceder con el cargado de datos.");
                return;
            }

            cadeteria = CrearCadeteria();
            if (cadeteria == null) throw new Exception("No se pudo crear la cadeteria.");
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
                            if (pedidoNew == null) throw new Exception("No se pudo crear el pedido.");
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
                        case 3:
                            var pedidos = cadeteria.ObtenerTodosLosPedidos();
                            if (!pedidos.Any()) throw new Exception("No hay pedidos a los cuales modificarles el estado");

                            System.Console.WriteLine("\n\n*** MODIFICANDO ESTADO DE UN PEDIDO ***\n");

                            var pedidoC = SolicitarSeleccionPedidos(pedidos);
                            System.Console.WriteLine();
                            var nuevoEstado = SolicitarSeleccionEstados();
                            pedidoC.Estado = nuevoEstado;

                            MostrarResultadoExitoso($"El estado del pedido nro. {pedidoC.Nro} ha sido modificado a: {nuevoEstado}");
                            break;

                        case 4:
                            var pedidosAsignados = cadeteria.ObtenerPedidosAsignados();
                            if (!pedidosAsignados.Any()) throw new Exception("No hay pedidos para reasignar");

                            System.Console.WriteLine("\n\n*** REASIGNANDO UN PEDIDO ***\n");

                            var pedidoD = SolicitarSeleccionPedidos(pedidosAsignados);
                            System.Console.WriteLine();
                            var cadeteB = SolicitarSeleccionCadete();
                            cadeteria.AsignarCadete(cadeteB, pedidoD);
                            MostrarResultadoExitoso($"El pedido nro. {pedidoD.Nro} ha sido re-asignado al cadete {cadeteB.Nombre} ({cadeteB.Id})");
                            break;

                        default:
                            Console.WriteLine("\nSaliendo...");
                            break;
                    }
                }
            }
            catch (System.Exception e)
            {
                mostrarError(e.Message);
            }

        } while (opcionSeleccionado != opcionSalida);

        Console.WriteLine("\n\n ### Informe ###");
        System.Console.WriteLine("* Envíos de cada cadete:");
        int totalEnvios = 0;
        foreach (var cadete in cadeteria.ListadoCadetes)
        {
            System.Console.WriteLine($"\t> CADETE ID {cadete.Id} ({cadete.Nombre}) - Envíos terminados: {cadete.buscarPedidos(Estado.COMPLETADO).Count()} - Envíos pendientes: {cadete.buscarPedidos(Estado.PENDIENTE).Count()}");
            totalEnvios += cadete.ListadoPedidos.Count();
        }
        System.Console.WriteLine($"\n* Envíos totales del día: {totalEnvios}");

        System.Console.WriteLine($"* Promedio de envíos por cadete: {cadeteria.ListadoCadetes.Select(c => c.ListadoPedidos.Count()).Average()}");
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

    private static Estado SolicitarSeleccionEstados()
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