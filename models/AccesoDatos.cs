using System.Text.Json;
using System.Text.Json.Serialization;
using cadeteria;

namespace persistencia;

public interface AccesoDatos
{
    List<string> LeerArchivo(string nombreArchivo);

    Cadeteria CrearCadeteria();

    List<Cadete> CrearCadetes();
}

public class AccesoCsv : AccesoDatos
{
    public Cadeteria CrearCadeteria()
    {
        var datosCsv = LeerArchivo("datosCadeteria.csv");
        var datos = datosCsv[0].Split(",");

        if (datos.Count() < 2) throw new Exception("No hay datos suficientes para instanciar la cadeteria");

        return new Cadeteria(datos[0], datos[1]);
    }

    public List<Cadete> CrearCadetes()
    {
        var datosCsv = LeerArchivo("datos_cadetes.csv");
        var cadetes = new List<Cadete>();

        foreach (var linea in datosCsv)
        {
            var datos = linea.Split(",");

            if (datos.Count() < 4)
            {
                System.Console.WriteLine($"\n[!] No se pudo cargar el cadete: {linea} - {datos}");
                continue;
            }
            if (!int.TryParse(datos[0], out int id))
                continue;

            cadetes.Add(new Cadete(id, datos[1], datos[2], datos[3]));
        }

        return cadetes;
    }


    public List<string> LeerArchivo(string nombreArchivo)
    {
        var lineas = new List<string>();

        using (FileStream archivoCsv = new FileStream(nombreArchivo, FileMode.Open))
        {
            using (StreamReader readerCsv = new StreamReader(archivoCsv))
            {
                // salto la cabecera
                readerCsv.ReadLine();

                while (readerCsv.Peek() != -1)
                {
                    var linea = readerCsv.ReadLine();
                    if (!string.IsNullOrWhiteSpace(linea)) lineas.Add(linea);
                }
            }
        }

        return lineas;
    }
}

public class AccesoJson : AccesoDatos
{
    public Cadeteria CrearCadeteria()
    {
        var datosCadeteriaJson = LeerArchivo("datosCadeteria.json").FirstOrDefault(); 
        
        if (string.IsNullOrWhiteSpace(datosCadeteriaJson))
            throw new Exception("No se pudieron leer los datos de la cadeteria del json");

        var cadeteria = JsonSerializer.Deserialize<Cadeteria>(datosCadeteriaJson);

        if (cadeteria == null)
            throw new Exception("Ha ocurrido un error deserealizando los datos de la cadeteria");

        return cadeteria;
    }

    public List<Cadete> CrearCadetes()
    {
        var datosCadetesJson = LeerArchivo("datosCadetes.json").FirstOrDefault();
        
        if (string.IsNullOrWhiteSpace(datosCadetesJson))
            return new List<Cadete>();

        var cadetes = JsonSerializer.Deserialize<List<Cadete>>(datosCadetesJson);

        if (cadetes == null)
            return new List<Cadete>();

        return cadetes;
    }

    public List<string> LeerArchivo(string nombreArchivo)
    {
        if (!File.Exists(nombreArchivo))
            throw new Exception($"El archivo {nombreArchivo} no existe");

        return new List<string>() { File.ReadAllText(nombreArchivo) };
    }
}
