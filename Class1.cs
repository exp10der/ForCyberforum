using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

class Class1
{
    static void Main()
    {
        MainAsync().Wait();
    }

    static async Task MainAsync()
    {
        const string add = "https://api.vk.com/method/likes.getList?type=post&owner_id=135215546&item_id=4377&filter=likes&frinds_only=0&extended=1&offset=1&count=1000&access_token=aa5d63a10ecf232dd20ae58f807722d5e5a8d553e88fb7a833c8622bfe558ef472191341e75ddbfb8d182&item_id=";

        string[] m = { "4377", "4247", "4390", "4241", "4275", "4274", "4392" };
        var task = m.Select(async url =>
        {
            using (var clien = new HttpClient())
            {
                return await clien.GetStringAsync(add + url);
            }

        }).ToList();

        var results = await Task.WhenAll(task);

        var query = results.SelectMany(n => JObject.Parse(n)["response"]["items"].Select(x => x["uid"].ToString()))
            .GroupBy(n => n, (k, n) => new {k, count = n.Count()}).OrderByDescending( n=>n.count);

        foreach (var q in query)
        {
            Console.WriteLine(q.k + " " + q.count);
        }

    }
}