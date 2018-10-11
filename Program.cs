using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using HtmlAgilityPack;
using System.Net.Http;
using System.Collections;
using System.IO;

namespace WebScrapper
{
    class Program
    {
        static void Main(string[] args)
        {
            GetHtmlAsync();
            Console.ReadLine();
        }

        static async void GetHtmlAsync()
        {
            StreamWriter outFile = new StreamWriter("C:/Users/scale/OneDrive/Desktop/VS Studios Code/WScrapper/WScrapper/ScrapperData.txt");

            var url = "https://www.apartments.com/frisco-tx-75034/"; // Starting url
            do
            {
                outFile.WriteLine("\n");

                var httpClient = new HttpClient();

                var html = await httpClient.GetStringAsync(url);

                var htmlDocument = new HtmlDocument(); // Document object is made
                htmlDocument.LoadHtml(html); // html is loaded into the dom.
                                             // parses 

                // Get the unordered list of apartments
                var apartmentListing = htmlDocument.DocumentNode.Descendants("div") // html tag
                                   .Where(node => node.GetAttributeValue("id", "") // var type
                                   .Equals("placardContainer")).ToList(); // var Name

                // Gets each apartment in the list instead of its url for some reason
                var apartments = apartmentListing[0].Descendants("article")
                                    .Where(node => node.GetAttributeValue("data-url", "")
                                    .Contains("www.apartments.com")).ToList();


                // Gets the last element in the list of apartments, A.K.A the arrow bar
                var nextPage = apartmentListing[0].Descendants("a")
                                    .Where(node => node.GetAttributeValue("class", "")
                                    .Equals("next ")).ToList();

                // Gets link to the next page.
                string nextPageUrl = "";
                if (nextPage.Count() == 0)
                {
                    nextPage = null;
                }
                else
                {
                    nextPageUrl = nextPage[0].Attributes["href"].Value;
                }

                ArrayList urls = new ArrayList(); 
                // Need to make because once we go through the apartments list and scrape data
                // the apartments variable get scrambled for some reason.

                for(int i = 0; i < apartments.Count(); i++)
                {
                    var apartmentUrl = apartments[i].Descendants("a")
                                        .Where(node => node.GetAttributeValue("href", "")
                                        .Contains("www.apartments.com")).ToList();
                    urls.Add(apartmentUrl[0].GetAttributeValue("href", ""));
                }
                
                // Allows access into each apartment.
                for(int i = 0; i < apartments.Count(); i ++)
                        /// change back to 0
                {
                    //ApartmentUrl

                    // gets the link to apartment page.
                    var aP = urls[i].ToString();

                    // Goes inside the apartments webpage.
                    if (urls[i] != null)
                    {
                        var urlNew = aP;

                        var htmlNew = await httpClient.GetStringAsync(urlNew);

                        //var htmlDocumentNew = new HtmlDocument(); // Document object is made

                        htmlDocument.LoadHtml(htmlNew);

                        var apartmentTable = htmlDocument.DocumentNode.Descendants("div")
                                             .Where(node => node.GetAttributeValue("class", "")
                                             .Equals("tabContent active")).ToList();

                        if (apartmentTable.Count() == 0) // For basic apartments
                        {
                            apartmentTable = htmlDocument.DocumentNode.Descendants("table")
                                             .Where(node => node.GetAttributeValue("class", "")
                                             .Equals("availabilityTable basic oneRental")).ToList();
                        }
                        if (apartmentTable.Count() == 0)
                        {
                            apartmentTable = htmlDocument.DocumentNode.Descendants("table")
                                             .Where(node => node.GetAttributeValue("class", "")
                                             .Equals("availabilityTable tiertwo")).ToList();
                        }

                        var apartmentModelList = apartmentTable[0].Descendants("tr")
                                             .Where(node => node.GetAttributeValue("class", "")
                                             .Contains("rentalGridRow")).ToList();

                        outFile.WriteLine("Models of the Apartment : \n");

                        foreach (var model in apartmentModelList)
                        {
                            var bedrooms = (model.Descendants("td")
                                .Where(node => node.GetAttributeValue("class", "")
                                .Contains("beds")).ToList())[0].Descendants("span")
                                .Where(node => node.GetAttributeValue("class", "")
                                .Equals("shortText")).ToList();

                            var baths = (model.Descendants("td")
                                .Where(node => node.GetAttributeValue("class", "")
                                .Contains("baths")).ToList())[0].Descendants("span")
                                .Where(node => node.GetAttributeValue("class", "")
                                .Equals("shortText")).ToList();

                            var rent = model.Descendants("td")
                                .Where(node => node.GetAttributeValue("class", "")
                                .Contains("rent")).ToList();

                            var deposit = model.Descendants("td")
                                .Where(node => node.GetAttributeValue("class", "")
                                .Contains("deposit")).ToList();

                            var modelName = model.Descendants("td")
                               .Where(node => node.GetAttributeValue("class", "")
                               .Contains("name")).ToList(); //Gives model name as 1A1 for some reason. Webscrapping has been detected by website?
                                // Actually its giving back the data-model as the name for some reason?

                            var area = model.Descendants("td")
                               .Where(node => node.GetAttributeValue("class", "")
                               .Contains("sqft")).ToList();

                            var availability = model.Descendants("td")
                               .Where(node => node.GetAttributeValue("class", "")
                               .Contains("availabile")).ToList();

                            if (modelName.Count() != 0 && modelName[0] != null) // going out of bounds?
                                outFile.WriteLine("ModelName: " + modelName[0].InnerText.Trim());
                            if (bedrooms.Count() != 0 && bedrooms[0] != null)
                                outFile.WriteLine("Bedroom: " + bedrooms[0].InnerText.Trim());
                            if (baths.Count() != 0 && baths[0] != null)
                                outFile.WriteLine("Bathroom: " + baths[0].InnerText.Trim());
                            if (rent.Count() != 0 && rent[0] != null)
                                outFile.WriteLine("Rent: " + rent[0].InnerText.Trim());
                            if (deposit.Count() != 0 && deposit[0] != null)
                                outFile.WriteLine("Deposit: " + deposit[0].InnerText.Trim());
                            if (area.Count() != 0 && area[0] != null)
                                outFile.WriteLine("Area: " + area[0].InnerText.Trim());

                            outFile.WriteLine();

                        }
                        outFile.WriteLine("Page Finish");

                        var apartmentDescription = htmlDocument.DocumentNode.Descendants("section")
                                             .Where(node => node.GetAttributeValue("class", "")
                                             .Contains("descriptionSection")).ToList();

                        outFile.WriteLine("\nDescription: " + apartmentDescription[0].InnerText.Trim());
                        outFile.WriteLine();

                    }
                    outFile.WriteLine();
                }

                outFile.WriteLine();

                outFile.WriteLine(nextPageUrl);


               url = nextPageUrl;
                if(url == null)
                {
                    break;
                }
            } while (url != "");

            outFile.Close();

            return;
        }
    }
}

