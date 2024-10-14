using System.Text.Json;
using System.Text.Json.Serialization;

namespace persistencia;

public interface AccesoDatos
{
    List<string> leerArchivo(string nombreArchivo);
    Cadeteria crearCadeteria();
    List<Cadete> crearCadetes();
}

public class AccesoCsv : AccesoDatos{
    public List<string> leerArchivo(string nombreArchivo){
        var lineas = new List<string>();

        using (FileStream archivoCsv = new FileStream(nombreArchivo, FileMode.Open))    
        {
            using (StreamReader readerCsv = new StreamReader(archivoCsv))
            {
                readerCsv.ReadLine();
                while (readerCsv.Peek() != -1)
                {
                    var linea = readerCsv.ReadLine();
                    if (!string.IsNullOrWhiteSpace(linea))
                    {
                        lineas.Add(linea);
                    }
                }
            }
        }

        return lineas;
    }

    public List<Cadete> crearCadetes(){
        var csv =  leerArchivo("datosCadetes.csv");
        var cadetes = new List<Cadete>();

        foreach (var linea in csv)
        {
            var datos = linea.Split(";");

            if (datos.Count() < 4)
            {
                System.Console.WriteLine($"\n[!] No se pudo cargar el cadete: {linea} - {datos}");
                continue;
            }
            if (!int.TryParse(datos[0], out int id))
            {
                continue;
            }
            cadetes.Add(new Cadete(id,datos[1], datos[2], datos[3]));
        }
        return cadetes;
    }

    public Cadeteria crearCadeteria(){
        var csv = leerArchivo("datosCadeteria.csv");
        var datos = csv[0].Split(";");

        if (datos.Count() < 2)
        {
            throw new Exception("No hay datos Suficientes para instanciar la cadeteria");
        }

        return new Cadeteria(datos[0], datos[1]);
    }
}

public class AccesoJson : AccesoDatos
{
    public List<string> leerArchivo(string nombreArchivo){
        if (!File.Exists(nombreArchivo))
        {
            throw new Exception($"El archivo {nombreArchivo} no existe");
        }
        return new List<string>(){File.ReadAllText(nombreArchivo)};
    }

    public List<Cadete> crearCadetes(){
        var datosCadetesJson = leerArchivo("datosCadetes.json").FirstOrDefault();
        if (string.IsNullOrWhiteSpace(datosCadetesJson))
        {
            return new List<Cadete>();
        }

        var cadetes = JsonSerializer.Deserialize<List<Cadete>>(datosCadetesJson);

        if (cadetes == null)
        {
            return new List<Cadete>();
        }

        return cadetes;

    }

    public Cadeteria crearCadeteria(){
        var datosCadeteriaJson = leerArchivo("datosCadeteria.json").FirstOrDefault();

        if (string.IsNullOrWhiteSpace(datosCadeteriaJson))
        {
            throw new Exception("no se pudieron leer los datos de la cadeteria del json");
        }

        var cadeteria = JsonSerializer.Deserialize<Cadeteria>(datosCadeteriaJson);

        if (cadeteria == null)
        {
            throw new Exception("Error deserealizando los datos de la cadeteria");
        }

        return cadeteria;
    }
}