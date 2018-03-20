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
            // todo: extend to the form with changable number of site "0,,,,/{i}/," to download from more pages
        }

        public override void Parse(Response response)
        {   
            //Get url from string
            var urlFromString = new Regex(@"\b(?:https?://|www\.)\S+\b"); 

            // Get last part of url, in this case "name_of_image.jpg"
            var lastPartOfUrl = new Regex(@"[^/]+(?=/$|$)"); 

            // Get current directory
            string appPath = System.Environment.CurrentDirectory;  
            // Create path to the folder with images
            string imageFolderPath = System.IO.Path.Combine(appPath, "wwwroot", "images"); 

            // Create folder for our pictures downloaded (only necessary if we are actually downloading them)
            DirectoryInfo di = Directory.CreateDirectory(imageFolderPath); 

            List<string> listOfImageDirectories = new List<string>();
            List<string> listOfImageURLs = new List<string>();
            List<string> listOfNames = new List<string>();



            var webClient = new WebClient();

            // select category-image from every div, and then contains of a[]
            foreach (var searched_link in response.Css("div > a[title]"))
            {
                string notebookName = searched_link.Attributes["title"];
                listOfNames.Add(notebookName);
            }

            // select category-image from every div, and then contains of a[]
            foreach (var searched_link in response.Css("div.category-image > a[style*=background-image:url]")) 
            {
                string strTitle = searched_link.Attributes["style"];
                Match matchedURLToImage = urlFromString.Match(strTitle); // slice to final url
                Match matchedImageName = lastPartOfUrl.Match(matchedURLToImage.Value); // slice to get image name

                string finalPathToImage = System.IO.Path.Combine(imageFolderPath, matchedImageName.Value);


                if (matchedURLToImage.Success && matchedImageName.Success)
                {
                    // if we have correct URL and image name, we can assign it to the list, to later put into the database
                    Console.WriteLine(matchedURLToImage.Value);
                    listOfImageDirectories.Add(matchedImageName.Value);
                    listOfImageURLs.Add(matchedURLToImage.Value);
                }
            }
            
            int notebooksCounter = 1001;
            int howManyNotebooks = 1001;
            decimal tempInch = 0;

            // get every feature of the notebook
            foreach (var searched_link in response.Css("div.feature-item"))
            {
                // get the "title" subdivision from the searched link
                string strFeature = searched_link.Attributes["title"];           
                Scrape(new ScrapedData() { { "Feature", strFeature } });

                //assign every feature to the given variable in the object created
                switch (countFeatures) 
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
                        //try if this is a correct form of decimal number, or has it only one number after dot
                        if (Decimal.TryParse(strFeature, out tempInch)) 
                            notebook.ScreenSizeInch = tempInch;
                        else // if it's not correct, create a substring to correctly parse the number
                        {
                            strFeature = strFeature.Substring(0, 2);
                            notebook.ScreenSizeInch = decimal.Parse(strFeature);
                        }
                        countFeatures++;
                        break;
                    case 4:
                        notebook.Name = listOfNames[notebooksCounter - howManyNotebooks];
                        notebook.RouteToImage = listOfImageDirectories[notebooksCounter - howManyNotebooks];

                        // Download image directly to the byte array
                        byte[] imageBytes = webClient.DownloadData(listOfImageURLs[notebooksCounter - howManyNotebooks]); 
                        notebook.ImageBytes = imageBytes;
                        notebooksCounter++;

                        // break cycle every 4th feature, becouse we are scraping next object in next iteration
                        countFeatures = 0; 
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
