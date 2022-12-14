using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using CsvHelper;
using System.Globalization;

namespace WebCrawler
{
    public class ClimbingShoesCsvGenerator
    {
        public static List<ClimbingShoe> GetClimbingShoesInfo()
        {
            Console.WriteLine("Paste the location of your Chrome executable below: ");
            string chromePath = Console.ReadLine();
            var options = new ChromeOptions()
            {
                BinaryLocation = chromePath,
            };

            options.AddArguments(new List<string>() { "headless", "disable-gpu" });

            var browser = new ChromeDriver(options);
            string fullUrl = "https://basecamp-shop.com/en/products/climb/climbing-shoes?pa_manufacturer%5B%5D=638&filtered=1";
            browser.Navigate().GoToUrl(fullUrl);
            browser.Manage().Window.Maximize();
            
            List<ClimbingShoe> climbingShoes = new List<ClimbingShoe>();            
            var pages = browser.FindElements(By.XPath("//div[@class='paging']/ul/li"));
            int numOfPages = pages.Count();
            var wait = new WebDriverWait(browser, TimeSpan.FromSeconds(15));

            for (int i = 0; i < numOfPages; i++)
            {
                var links = wait.Until(browser => browser.FindElements(By.XPath("//div[@class='product-grid-item']")));
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
                IWebElement clickableNextBtn = wait.Until(browser => browser.FindElement(By.XPath("//a[contains(text(),'Next')]")));
                clickableNextBtn.Click();   
            }

            var lastPageLinks = wait.Until(browser => browser.FindElements(By.XPath("//div[@class='product-grid-item']")));
            foreach (var link in lastPageLinks)
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
            Console.WriteLine("Paste the path where you wish the report to go to: ");
            string outputPath = Console.ReadLine();
            string reportPath = outputPath + "\\climbingShoes.csv";

            using (var writer = new StreamWriter(reportPath))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(climbingShoes);
                writer.Flush();
            }

            Console.WriteLine("CSV Generated!");
        }
    }
}
