using System.Text.Json;
using System.Net;
using System.IO.Compression;

public class JsonData
{
    public string Title { get; set; }
    public string Author { get; set; }
    public List<string> Datapacks { get; set; }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Path of .dppack file:");
        string filePath = Console.ReadLine();
        Console.WriteLine("World Path:");
        string worldPath = Console.ReadLine();

        try
        {
            string jsonString = File.ReadAllText(filePath);
            JsonData json = JsonSerializer.Deserialize<JsonData>(jsonString);
            Console.WriteLine($"Title: {json.Title}");
            Console.WriteLine($"Author: {json.Author}");

            foreach (string url in json.Datapacks)
            {
                // Also get rid of the %20 that is sometimes in URLs
                var name = Path.GetFileNameWithoutExtension(url).Replace("%20", " ");
                Console.WriteLine(name);
                using (var client = new WebClient())
                {
                    client.DownloadFile(url, worldPath + "/datapacks/datapack.zip");
                }
                
                ZipFile.ExtractToDirectory($"{worldPath}/datapacks/datapack.zip", $"{worldPath}/datapacks/{name}");
            }
            File.Delete(worldPath + "/datapacks/datapack.zip");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}