namespace EasyConf.Extensions;

public static class AppExtension
{
    public static string RemoveArray(this string template)
    {
        if (template[0] == '[')
        {
            template = template.Substring(1);
            if (template[template.Length - 1] == ']') 
                template = template.Remove(template.Length - 1);
            else if (template[template.Length - 2] == ']')
                template = template.Remove(template.Length - 2);
            else if (template[template.Length - 3] == ']')
                template = template.Remove(template.Length - 3);
        }
        return template;
    }

    public static string RemoveComma(this string template)
    {
        if (template[0] == ',')
        {
            template = template.Substring(1);
            if (template[template.Length - 1] == ',')
                template = template.Remove(template.Length - 1);
            else if (template[template.Length - 2] == ',')
                template = template.Remove(template.Length - 2);
            else if (template[template.Length - 3] == ',')
                template = template.Remove(template.Length - 3);
        }
        return template;
    }

    public static string ToEnv(this string name)
    {
        return name + ".env";
    }

    public static string ToJson(this string name)
    {
        return name + ".json";
    }

}
