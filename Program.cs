using System.Diagnostics;
using System.Text;

Console.InputEncoding = Encoding.UTF8;
Console.OutputEncoding = Encoding.UTF8;

Console.WriteLine("Привіт! Завантажуємо пісні за допомогою потоків");

Directory.CreateDirectory("songs");

//Масив посилань
string[] songUrls = new string[]
{
    "https://www.soundhelix.com/examples/mp3/SoundHelix-Song-1.mp3",
    "https://www.soundhelix.com/examples/mp3/SoundHelix-Song-2.mp3",
    "https://www.soundhelix.com/examples/mp3/SoundHelix-Song-3.mp3"
};

Stopwatch stopWatch = new Stopwatch();
stopWatch.Start();

// Створюємо масив потоків за кількістю пісень
Thread[] threads = new Thread[songUrls.Length];

for (int i = 0; i < songUrls.Length; i++)
{
    int index = i; // Фіксуємо індекс для потоку
    threads[i] = new Thread(() => DownloadSong(songUrls[index], index + 1));
    threads[i].Start();
}

// Чекаємо завершення всіх потоків
foreach (var thread in threads)
{
    thread.Join();
}

stopWatch.Stop();
Console.WriteLine("Усі пісні завантажено! Час виконання: " + stopWatch.ElapsedMilliseconds + " мс");

void DownloadSong(string url, int songNumber)
{
    try
    {
        using (HttpClient client = new HttpClient())
        {
            var bytes = client.GetByteArrayAsync(url).Result;
            string fileName = Path.Combine("songs", $"song_{songNumber}_{Guid.NewGuid()}.mp3");
            File.WriteAllBytes(fileName, bytes);
            Console.WriteLine($"Завантажено пісню №{songNumber}");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Помилка завантаження пісні №{songNumber}: {ex.Message}");
    }
}

Console.WriteLine("Позитивчику та гарного настрою! :)");