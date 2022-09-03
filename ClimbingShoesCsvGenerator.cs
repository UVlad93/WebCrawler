using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using CsvHelper;
using System.Globalization;

namespace WebCrawler
{
    public class ClimbingShoesCsvGenerator
    {
        public static List<ClimbingShoe> GetClimbingShoesInfo()
        {
            string fullUrl = "https://basecamp-shop.com/en/products/climb/climbing-shoes";
            List<string> shoesLinks = new List<string>();

            var options = new ChromeOptions()
            {
                BinaryLocation = "C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe",
            };

            options.AddArguments(new List<string>() { "headless", "disable-gpu" });

            var browser = new ChromeDriver(options);
            browser.Navigate().GoToUrl(fullUrl);

            var links = browser.FindElements(By.XPath("//div[@class='product-grid-item']"));

            List<ClimbingShoe> climbingShoes = new List<ClimbingShoe>();

            foreach (var link in links)
            {
                var shoe = new ClimbingShoe
                {
                    Brand = link.GetAttribute("data-brand").ToString(),
                    Model = link.GetAttribute("data-name").ToString(),
                    Price = link.GetAttribute("data-price").ToString(),
                    Link = "https://basecamp-shop.com" + link.GetAttribute("data-url").ToString(),
                };
                climbingShoes.Add(shoe);
            }

            return climbingShoes;
        }

        public static void GenerateCsv(List<ClimbingShoe> climbingShoes)
        {
            string outputPath = "C:\\Users\\vladu\\Documents\\C# stuff\\WebCrawler\\WebCrawler\\Results\\climbingShoes.csv";
            using (var writer = new StreamWriter(outputPath))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(climbingShoes);
                writer.Flush();
            }

            Console.WriteLine("CSV Generated!");
        }
    }
}
