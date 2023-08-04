using System;
using System.Diagnostics;
using System.Management;

const string gameCenter = "GameCenter";
const string openGameInGameCenter = "hg64";
const string gameCenterAtomicHeartURI = "mailrugames://play/0.2015959";
const string wemodAtomicHeartURI = "wemod://play?titleId=26989&gameId=26989";

bool found = false;
bool childStarted = false;

Process gameCenterProcess = new();
gameCenterProcess.StartInfo.FileName = gameCenterAtomicHeartURI;
gameCenterProcess.StartInfo.UseShellExecute = true;
gameCenterProcess.Start();

while (true)
{
    var processes = Process.GetProcessesByName(gameCenter);
    foreach (var process in processes)
    {
        found = true;
        try
        {
            string query = $"Select * From Win32_Process Where ParentProcessID={process.Id}";
            var searcher = new ManagementObjectSearcher(query);
            var results = searcher.Get();
            foreach (var result in results)
            {
                string childProcess = result["Name"].ToString();
                if (string.Equals(childProcess, openGameInGameCenter + ".exe", StringComparison.OrdinalIgnoreCase))
                {
                    childStarted = true;
                    Process wemodProcess  = new();
                    gameCenterProcess.StartInfo.FileName = wemodAtomicHeartURI;
                    gameCenterProcess.StartInfo.UseShellExecute = true;
                    gameCenterProcess.Start();
                    break;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Ошибка: {e.Message}");
        }
        continue;
    }
    if (!found || childStarted)
    {
        break;
    }
    System.Threading.Thread.Sleep(50);
}





// // Ссылка на запуск игры через wemod
// string vkplay = "mailrugames://play/0.2015959";

// // Создаем процесс для запуска ссылки
// Process process = new();
// process.StartInfo.FileName = vkplay;
// process.StartInfo.UseShellExecute = true;

// // Запускаем процесс
// process.Start();
