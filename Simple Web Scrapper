using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using HtmlAgilityPack;
using System.Net.Http;

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
            /// Need to implement null checks and exceptions.
            
            var url = "https://www.apartments.com/frisco-tx-75035/";
            var httpClient = new HttpClient();

            var html = await httpClient.GetStringAsync(url);
            
            var htmlDocument = new HtmlDocument(); // Document object is made
            htmlDocument.LoadHtml(html); // html is loaded into the dom.
                                         // parses 

            // This code goes through the whole page and gets any data with the :
            // Html tag "article", if seen in inspect mode on the web page the word is in purple.
            // The var type "class", in the color yellow
            // and contains the name "placard" in its variable name.
            var apartmentListings = htmlDocument.DocumentNode.Descendants("article") // html tag
                               .Where(node => node.GetAttributeValue("class", "") // var type
                               .Contains("placard")).ToList(); // var Name
            // Have to consider other data that may adhere to this such as ads.

            foreach(var apartment in apartmentListings)
            {
                // Gets the name of the apartment. Still need to write exception for ads
                var apartmentName = apartment.Descendants("a") 
                               .Where(node => node.GetAttributeValue("class", "") 
                               .Equals("placardTitle js-placardTitle  "));

                // Gets the location listed for the apartment
                var apartmentLocation = apartment.Descendants("div")
                                        .Where(node => node.GetAttributeValue("class", "")
                                        .Equals("location"));

                // Price Listing
                var apartmentRent = apartment.Descendants("span")
                                    .Where(node => node.GetAttributeValue("class", "")
                                    .Equals("altRentDisplay"));

                // Phone #
                var apartmentContactInfo = apartment.Descendants("div")
                                    .Where(node => node.GetAttributeValue("class", "")
                                    .Equals("phone")).ToList();
                var apartmentPhoneNum = apartmentContactInfo[0].InnerText.Trim(); // Need to see why info is so spaced out.

                if (apartmentName != null)
                // Gives error when trying to write from an ad.
                Console.WriteLine(apartmentName.ElementAt(0).InnerHtml); // InnerHtml allows us to out the data as a string.
                Console.WriteLine(apartmentLocation.ElementAt(0).InnerHtml);
                Console.WriteLine(apartmentRent.ElementAt(0).InnerHtml);
                Console.WriteLine(apartmentPhoneNum);
            }

            Console.WriteLine();
        }
    }
}
