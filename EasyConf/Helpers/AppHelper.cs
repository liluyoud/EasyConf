using EasyConf.Models;
using Microsoft.AspNetCore.Components.Forms;
using System.Text;

namespace EasyConf.Helpers;

public static class AppHelper
{
    public static TypeErrorFileList ValidateFileList(List<IBrowserFile> files)
    {
        // Verifica se há no máximo 2 arquivos
        if (files == null || files.Count == 0 || files.Count > 2)
            return TypeErrorFileList.FileCountError;

        // Se houver apenas 1 arquivo
        if (files.Count == 1)
        {
            // Verifica se a extensão é .json
            string file = files[0].Name;
            if (Path.GetExtension(file).ToLower() == ".json")
                return TypeErrorFileList.Ok;
            return TypeErrorFileList.OneJsonFileError;
        }

        // Se houver 2 arquivos
        if (files.Count == 2)
        {
            string file1 = files[0].Name;
            string file2 = files[1].Name;

            // Verifica se um tem extensão .json e o outro .env
            bool hasJson = Path.GetExtension(file1).ToLower() == ".json" || Path.GetExtension(file2).ToLower() == ".json";
            bool hasEnv = Path.GetExtension(file1).ToLower() == ".env" || Path.GetExtension(file2).ToLower() == ".env";

            if (!hasJson || !hasEnv)
                return TypeErrorFileList.OneJsonOneEnvFileError;

            // Verifica se ambos têm o mesmo nome (sem extensão)
            string baseName1 = Path.GetFileNameWithoutExtension(file1);
            string baseName2 = Path.GetFileNameWithoutExtension(file2);

            if (baseName1 == baseName2)
                return TypeErrorFileList.Ok;
            else
                return TypeErrorFileList.FileNameError;
        }

        return TypeErrorFileList.OtherError;
    }

    public static string GenerateKey(int tamanho)
    {
        if (tamanho <= 0)
        {
            throw new ArgumentException("O tamanho deve ser maior que zero.");
        }

        const string letrasMaiusculas = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string letrasMinusculas = "abcdefghijklmnopqrstuvwxyz";
        const string numeros = "0123456789";
        string caracteresPermitidos = letrasMaiusculas + letrasMinusculas + numeros;

        StringBuilder senha = new StringBuilder(tamanho);
        Random random = new Random();

        for (int i = 0; i < tamanho; i++)
        {
            int indice = random.Next(caracteresPermitidos.Length);
            senha.Append(caracteresPermitidos[indice]);
        }

        return senha.ToString();
    }
}
