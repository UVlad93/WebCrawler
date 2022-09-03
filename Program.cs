namespace WebCrawler
{
    public class Program
    {
        static void Main(string[] args)
        {
            List<ClimbingShoe> climbingShoes = ClimbingShoesCsvGenerator.GetClimbingShoesInfo();
            ClimbingShoesCsvGenerator.GenerateCsv(climbingShoes);
        } 
    }
}