using System.IO;
using System.Text;
using UnityEngine;

public static class SaveSystem
{
    private static readonly string ChaveCriptografia = "ProjetoBolinhasChave2026";

    public static void SalvarSlot(int slot, SaveData data)
    {
        string path = ObterCaminho(slot);
        string json = JsonUtility.ToJson(data, true);
        string jsonEncriptado = CriptografarXOR(json, ChaveCriptografia);
        File.WriteAllText(path, jsonEncriptado);
    }

    public static SaveData CarregarSlot(int slot)
    {
        string path = ObterCaminho(slot);
        if (!ExisteSlot(slot)) return null;

        try
        {
            string jsonEncriptado = File.ReadAllText(path);
            string jsonDecriptado = CriptografarXOR(jsonEncriptado, ChaveCriptografia);
            return JsonUtility.FromJson<SaveData>(jsonDecriptado);
        }
        catch
        {
            return null;
        }
    }

    public static bool ExisteSlot(int slot)
    {
        string path = ObterCaminho(slot);
        return File.Exists(path) && new FileInfo(path).Length > 0;
    }

    private static string ObterCaminho(int slot)
    {
        return Path.Combine(Application.persistentDataPath, $"save_slot_{slot}.dat");
    }

    private static string CriptografarXOR(string input, string key)
    {
        StringBuilder result = new StringBuilder();
        for (int i = 0; i < input.Length; i++)
        {
            result.Append((char)(input[i] ^ key[i % key.Length]));
        }
        return result.ToString();
    }
}