using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using IronWebScraper;
using System.Drawing;
using System.Net;

namespace ShopApp.Models
{
    public class DownloadedNotebook : WebScraper
    {
        public List<Notebook> notebookList = new List<Notebook>();
        private int countFeatures = 0;
        private Notebook notebook = new Notebook();

        public override void Init()
        {
            this.LoggingLevel = WebScraper.LogLevel.All;
            this.Request("https://www.morele.net/laptopy/laptopy/notebooki-laptopy-ultrabooki-31/,,,,,,,,0,,,,/1/", Parse);
        }

        public override void Parse(Response response)
        {
            var urlFromString = new Regex(@"\b(?:https?://|www\.)\S+\b"); //Get url from string
            var lastPartOfUrl = new Regex(@"[^/]+(?=/$|$)"); // Get last part of url, in this case "name_of_image.jpg"

            //string appPath = AppDomain.CurrentDomain.BaseDirectory; // Another method to obtain directory
            string appPath = System.Environment.CurrentDirectory;  // Get current directory
            string imageFolderPath = System.IO.Path.Combine(appPath, "wwwroot", "images"); // Create path to the folder with images

            DirectoryInfo di = Directory.CreateDirectory(imageFolderPath); // Create folder

            List<string> listOfImageDirectories = new List<string>();
            List<string> listOfImageURLs = new List<string>();
            List<string> listOfNames = new List<string>();



            var webClient = new WebClient();


            foreach (var searched_link in response.Css("div > a[title]")) // select category-image from every div, and then contains of a[]
            {
                string notebookName = searched_link.Attributes["title"];
                listOfNames.Add(notebookName);
            }

            foreach (var searched_link in response.Css("div.category-image > a[style*=background-image:url]")) // select category-image from every div, and then contains of a[]
            {
                string strTitle = searched_link.Attributes["style"];
                Match matchedURLToImage = urlFromString.Match(strTitle); // slice to final url
                Match matchedImageName = lastPartOfUrl.Match(matchedURLToImage.Value); // slice to get image name

                string finalPathToImage = System.IO.Path.Combine(imageFolderPath, matchedImageName.Value);


                if (matchedURLToImage.Success && matchedImageName.Success)
                {
                    Console.WriteLine(matchedURLToImage.Value);
                    listOfImageDirectories.Add(matchedImageName.Value);
                    listOfImageURLs.Add(matchedURLToImage.Value);
                    //DownloadImage(matchedURLToImage.Value, imageFolderPath, 0, 0, false);   // for some reason, it's not working, although the same code in console app works perfectly?
                }
            }
            
            int notebooksCounter = 1001;
            int howManyNotebooks = 1001;
            decimal tempInch = 0;
            foreach (var searched_link in response.Css("div.feature-item")) // get every feature of the notebook
            {
                string strFeature = searched_link.Attributes["title"];           
                Scrape(new ScrapedData() { { "Feature", strFeature } });                  
                
                switch (countFeatures) // this is not the pretties way to do that, but it's pretty clean looking and self-explainatory
                {
                    case 0:
                        notebook.GPU = strFeature;
                        countFeatures++;
                        break;
                    case 1:
                        notebook.RAM = (int)Char.GetNumericValue(strFeature[0]);
                        countFeatures++;
                        break;
                    case 2:
                        notebook.Processor = strFeature;
                        countFeatures++;
                        break;
                    case 3:
                        if (Decimal.TryParse(strFeature, out tempInch)) //try if this is a correct form of decimal number, or has it only on number after dot
                            notebook.ScreenSizeInch = decimal.Parse(strFeature);
                        else
                        {
                            strFeature = strFeature.Substring(0, 2);
                            notebook.ScreenSizeInch = decimal.Parse(strFeature);
                        }
                        countFeatures++;
                        break;
                    case 4:
                        notebook.Name = listOfNames[notebooksCounter - howManyNotebooks];
                        notebook.RouteToImage = listOfImageDirectories[notebooksCounter - howManyNotebooks];
                        byte[] imageBytes = webClient.DownloadData(listOfImageURLs[notebooksCounter - howManyNotebooks]); // Download image directly to the byte array
                        notebook.ImageBytes = imageBytes;
                        notebooksCounter++;
                        countFeatures = 0; // break cycle every 4th feature, becouse we are scraping next object in next iteration
                        notebookList.Add(notebook);
                        notebook = new Notebook();
                        break;
                    default:
                        break;
                }
            }

            using (MemoryStream mStream = new MemoryStream())
            {
                
            }
        }
    }
}
