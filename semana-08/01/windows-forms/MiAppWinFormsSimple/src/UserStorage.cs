using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace MiAppWinFormsSimple
{
    // Clase utilitaria para serializar/deserializar y persistir usuarios.
    public static class UserStorage
    {
        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions { WriteIndented = true };

        // Serializa una lista de usuarios a JSON
        public static string Serialize(List<User> users)
        {
            return JsonSerializer.Serialize(users, JsonOptions);
        }

        // Deserializa JSON a lista de usuarios (devuelve vacío si null)
        public static List<User> Deserialize(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return new List<User>();
            return JsonSerializer.Deserialize<List<User>>(json, JsonOptions) ?? new List<User>();
        }

        // Guarda la lista a un archivo (crea carpeta si es necesario)
        public static void SaveToFile(List<User> users, string path)
        {
            var dir = Path.GetDirectoryName(path) ?? "data";
            Directory.CreateDirectory(dir);
            var json = Serialize(users);
            File.WriteAllText(path, json);
        }

        // Carga la lista desde un archivo (si no existe devuelve lista vacía)
        public static List<User> LoadFromFile(string path)
        {
            if (!File.Exists(path)) return new List<User>();
            var json = File.ReadAllText(path);
            return Deserialize(json);
        }
    }

    // DTO público para usuarios (usado por la app y por los tests)
    public class User
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
