using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Azure.Documents.Spatial;
using System.Linq;
using System.Net;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace TapThis.Azure_Resource
{
    class Query_Manager
    {
        //Accessing the DB. Might make Manager
        static private Uri collectionLink = UriFactory.CreateDocumentCollectionUri(Azure_Resource.VerifiedDB.Database, Azure_Resource.VerifiedDB.ItemCollection);
        static private DocumentClient Client = new DocumentClient(new Uri(Azure_Resource.VerifiedDB.EndpointUri), Azure_Resource.VerifiedDB.ReadKey);

        //Writing to a seperate collection and database. Will crash the link if invalid/empty 
        //static private Uri Write_collectionLink = UriFactory.CreateDocumentCollectionUri(Azure_Resource.WriteDB.Database, Azure_Resource.WriteDB.Submission_Collection);
        //static private DocumentClient Write_Client = new DocumentClient(new Uri(Azure_Resource.WriteDB.EndpointUri), Azure_Resource.WriteDB.Key);

        //Burn this. Testing Only
        static private Uri Write_collectionLink = UriFactory.CreateDocumentCollectionUri(Azure_Resource.VerifiedDB.Database, Azure_Resource.VerifiedDB.ItemCollection);
        static private DocumentClient Write_Client = new DocumentClient(new Uri(Azure_Resource.VerifiedDB.EndpointUri), Azure_Resource.VerifiedDB.ReadKey);


        //  Query across partition keys
        static public IDocumentQuery<Model.Document> Set_Query(Polygon Map)
        {
            Model.Filter Filter = ((App)Application.Current).Filter.Filter_Settings;
            bool TwoTypes = false;
 
            //Update Filter being used on Query based on the user switches. Helps reduce number of possible Query calls.
            //Updates Day and Time if the Right Now switch is on
            if (Filter.Right_Now)
            {
                Filter.Day = DateTime.Today.DayOfWeek.ToString();
                Filter.Time.Hour  = DateTime.Now.Hour;
                Filter.Time.Min = DateTime.Now.Minute;
            }
            //Updates Type if All Items switch is off. 
            //Will treat All Items on if all items are indivdual clicked on
            //Does handle if nothing is clicked
            if (!Filter.Type_Everything)
            {
                if (Filter.Type_Drink && Filter.Type_Food && Filter.Cover) { Filter.Type_Everything = true; }
                else if (Filter.Type_Drink)
                {
                    if (Filter.Type_Food)
                    {
                        Filter.Type2 = "Food";
                        TwoTypes = true;
                    }
                    else if (Filter.Cover)
                    {
                        Filter.Type2 = "Cover";
                        TwoTypes = true;
                    }
                    Filter.Type = "Drink";

                }
                else if (Filter.Type_Food)
                {
                    if (Filter.Cover)
                    {
                        Filter.Type2 = "Cover";
                        TwoTypes = true;
                    }
                    Filter.Type = "Food";
                }
                else if (Filter.Cover)
                {
                    Filter.Type = "Cover";
                }
                else { Filter.Type = "Nothing"; }
            }

           



            //All Items for the entire day.
            if (Filter.Type_Everything && Filter.All_Day)
            {
                return Client.CreateDocumentQuery<Model.Document>(collectionLink, new FeedOptions { EnableCrossPartitionQuery = true })
                    .Where(DB =>
                    DB.Day == Filter.Day && DB.Location.Within(Map))
                    .Take(200)
                    .AsDocumentQuery();
            }
            // All Items that are currently going
            else if (Filter.Type_Everything)
            {
                return Client.CreateDocumentQuery<Model.Document>(collectionLink, new FeedOptions { EnableCrossPartitionQuery = true })
                    .Where(DB =>
                    DB.Day == Filter.Day && DB.Start <= Filter.Time.Time_Double && Filter.Time.Time_Double < DB.End && DB.Location.Within(Map)
                    ||
                    DB.BleedDay == Filter.Day && Filter.Time.Time_Double < DB.BleedEnd && DB.Location.Within(Map))
                    .Take(200)
                    .AsDocumentQuery();
            }
            // One specified Item for the whole day
            else if (Filter.All_Day && !TwoTypes)
            {
                return Client.CreateDocumentQuery<Model.Document>(collectionLink, new FeedOptions { EnableCrossPartitionQuery = true })
                    .Where(DB =>
                    DB.Day == Filter.Day && DB.Type == Filter.Type && DB.Location.Within(Map))
                    .Take(200)
                    .AsDocumentQuery();
            }
            // One specified Item that are currently going
            else if (Filter.All_Day && !TwoTypes)
            {
                return Client.CreateDocumentQuery<Model.Document>(collectionLink, new FeedOptions { EnableCrossPartitionQuery = true })
                    .Where(DB =>
                    DB.Day == Filter.Day && DB.Start <= Filter.Time.Time_Double && Filter.Time.Time_Double < DB.End && DB.Type == Filter.Type && DB.Location.Within(Map)
                    ||
                    DB.BleedDay == Filter.Day && Filter.Time.Time_Double < DB.BleedEnd && DB.Type == Filter.Type && DB.Location.Within(Map))
                    .Take(200)
                    .AsDocumentQuery();
            }
            // Two specified Item for the whole day
            else if (Filter.All_Day && TwoTypes)
            {
                return Client.CreateDocumentQuery<Model.Document>(collectionLink, new FeedOptions { EnableCrossPartitionQuery = true })
                    .Where(DB =>
                    DB.Day == Filter.Day && DB.Type == Filter.Type && DB.Location.Within(Map)
                    ||
                    DB.Day == Filter.Day && DB.Type == Filter.Type2 && DB.Location.Within(Map))
                    .Take(200)
                    .AsDocumentQuery();
            }
            // Two specified Item that are currently going
            else if (Filter.All_Day && TwoTypes)
            {
                return Client.CreateDocumentQuery<Model.Document>(collectionLink, new FeedOptions { EnableCrossPartitionQuery = true })
                    .Where(DB =>
                    DB.Day == Filter.Day && DB.Start <= Filter.Time.Time_Double && Filter.Time.Time_Double < DB.End && DB.Type == Filter.Type && DB.Location.Within(Map)
                    ||
                    DB.BleedDay == Filter.Day && Filter.Time.Time_Double < DB.BleedEnd && DB.Type == Filter.Type && DB.Location.Within(Map)
                    ||
                    DB.Day == Filter.Day && DB.Start <= Filter.Time.Time_Double && Filter.Time.Time_Double < DB.End && DB.Type == Filter.Type2 && DB.Location.Within(Map)
                    ||
                    DB.BleedDay == Filter.Day && Filter.Time.Time_Double < DB.BleedEnd && DB.Type == Filter.Type2 && DB.Location.Within(Map))
                    .Take(200)
                    .AsDocumentQuery();
            }



            // Else nothing
            return Client.CreateDocumentQuery<Model.Document>(collectionLink, new FeedOptions { EnableCrossPartitionQuery = true })
                    .Where(DB =>
                    DB.Day == Filter.Day && DB.Start <= Filter.Time.Time_Double && Filter.Time.Time_Double < DB.End && DB.Type == Filter.Type && DB.Location.Within(Map)
                    ||
                    DB.BleedDay == Filter.Day && Filter.Time.Time_Double < DB.BleedEnd && DB.Type == Filter.Type && DB.Location.Within(Map))
                    .Take(200)
                    .AsDocumentQuery();

        }

        // Sending Suggestion
        static public async Task Send_Data()
        {
            //Making a smaller object to send to the data base. Purpose of less clutter on my DB

            try
            {
                Model.Send_Suggestion Data = new Model.Send_Suggestion
                {
                    //using deafault naming in DB. the '/' character breaks my naming convention
                    Document_Type = "Suggest",
                    Address = ((App)Application.Current).Suggest.Data.Address,
                    City_State_Zip = ((App)Application.Current).Suggest.Data.City_State_Zip,
                    Business_Name = ((App)Application.Current).Suggest.Data.Business_Name,
                    Country = ((App)Application.Current).Suggest.Data.Country,
                    Latitude = ((App)Application.Current).Suggest.Data.Latitude,
                    Longitude = ((App)Application.Current).Suggest.Data.Longitude,
                    Item_List = new List<Model.Send_Item>()

                };

                foreach (Model.Suggest_Item Item in ((App)Application.Current).Suggest.Data.Item_List)
                {
                    Data.Item_List.Add(new Model.Send_Item
                    {
                        Name = Item.Item_Name,
                        Price = Item.Price,
                        Type = Item.Type,
                        Start = Item.Start.Time_Double,
                        End = Item.End.Time_Double,
                        Day = Item.Day
                    });
                }

                await Write_Client.CreateDocumentAsync(Write_collectionLink, Data);
            }
            catch (DocumentClientException de)
            {
                Exception baseException = de.GetBaseException();
                Console.WriteLine("{0} error occurred: {1}, Message: {2}", de.StatusCode, de.Message, baseException.Message);
            }
            catch ( Exception e)
            {
                Exception baseException = e.GetBaseException();
                Console.WriteLine("Error: {0}, Message: {1}", e.Message, baseException.Message);
            }   
        }

        static public async Task Send_Report(Model.Send_Report Report)
        {
            try
            { 
            await Write_Client.CreateDocumentAsync(Write_collectionLink, Report);
            }
            catch (DocumentClientException de)
            {
                Exception baseException = de.GetBaseException();
                Console.WriteLine("{0} error occurred: {1}, Message: {2}", de.StatusCode, de.Message, baseException.Message);
            }
            catch (Exception e)
            {
                Exception baseException = e.GetBaseException();
                Console.WriteLine("Error: {0}, Message: {1}", e.Message, baseException.Message);
            } 
        }

        static public async Task Send_Contact(Model.WriteDB.Contact Contact)
        {
            try
            {
                await Write_Client.CreateDocumentAsync(Write_collectionLink, Contact);
            }
            catch (DocumentClientException de)
            {
                Exception baseException = de.GetBaseException();
                Console.WriteLine("{0} error occurred: {1}, Message: {2}", de.StatusCode, de.Message, baseException.Message);
            }
            catch (Exception e)
            {
                Exception baseException = e.GetBaseException();
                Console.WriteLine("Error: {0}, Message: {1}", e.Message, baseException.Message);
            }
        }


    }
}
