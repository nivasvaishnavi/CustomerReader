using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using CustomerReader.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CustomerReader
{
    public class CustomerReader
    {
        //Static variable to hold the file path.
        private static readonly string filePath = "D:\\PROJECTS\\CustomerReaderExercise\\doc\\";
        private List<Customer> customerList;

        /// <summary>
        /// Main method.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            CustomerReader customerReader = new CustomerReader();

            //Invoking methods to read data from various files (.csv, .xml, .json).
            customerReader.ReadCustomersCsv(filePath + "customers.csv");
            customerReader.ReadCustomersXml(filePath + "customers.xml");
            customerReader.ReadCustomersJson(filePath + "customers.json");

            //Formatting the dispplay.
            Console.WriteLine("\n" + "Added this many customers: " + customerReader.GetCustomers().Count + "\n");
            customerReader.DisplayCustomers();
            Console.ReadLine();
        }

        /// <summary>
        /// Public Constructor that initializes the Customer list object<see cref="Customer"/>.
        /// </summary>
        public CustomerReader()
        {
            customerList = new List<Customer>();
        }

        /// <summary>
        /// Gets the Customer list of records read from the .csv, .xml, .json files.
        /// </summary>
        /// <returns>A list of Customer object<see cref="Customer"/></returns>
        public List<Customer> GetCustomers()
        {
            return customerList;
        }

        /// <summary>
        /// This method reads customers from a CSV file and puts them into the customers list.
        /// </summary>
        /// <param name="customerFilePath">customerFilePath gets a CSV file as input</param>
        public void ReadCustomersCsv(String customerFilePath)
        {
            try
            {
                //Reading the file path and declaring a stream reader.
                using (StreamReader streamReader = new StreamReader(File.Open(customerFilePath, FileMode.Open)))
                {
                    //Initializing the stream reader to read the header line.
                    String line = streamReader.ReadLine();
                    String[] attributes;

                    //Checking if the header line is not empty to read next lines.
                    if (line != null)
                    {
                        //Reading the line next to the header.
                        line = streamReader.ReadLine();

                        //Checking if the lines preceeding header line are empty.
                        while (line != null)
                        {
                            attributes = line.Split(',');
                            customerList.Add(new Customer
                            {
                                Email = attributes[0],
                                FirstName = attributes[1],
                                LastName = attributes[2],
                                Phone = attributes[3],
                                StreetAddress = attributes[4],
                                City = attributes[5],
                                State = attributes[6],
                                ZipCode = attributes[7]
                            });
                            line = streamReader.ReadLine();
                        }
                    }
                }
            }
            catch (IOException ex)
            {
                Console.Write("OH NO!!!!");
                Console.Write(ex.StackTrace);
            }
        }

        /// <summary>
        /// This method reads customers from an XML file and puts them into the customers list.
        /// </summary>
        /// <param name="customerFilePath">customerFilePath gets an XML file as input</param>
        public void ReadCustomersXml(String customerFilePath)
        {
            try
            {
                var xmlDocument = new XmlDocument();
                //Reading the file path.
                xmlDocument.Load(customerFilePath);

                //Setting the tag name (Node) from which the data is to be read.
                XmlNodeList xmlNodeList = xmlDocument.SelectNodes("/Customers/Customer");

                //Looping the records.
                foreach (XmlNode node in xmlNodeList)
                {
                    //Reading child nodes from each record.
                    XmlElement eElement = (XmlElement)node;
                    XmlElement aElement = (XmlElement)eElement.GetElementsByTagName("Address").Item(0);

                    //Assigning the node's inner text to a new Customer object property and adding it to the Customer List.
                    customerList.Add(new Customer
                    {
                        Email = node["Email"].InnerText,
                        FirstName = node["FirstName"].InnerText,
                        LastName = node["LastName"].InnerText,
                        Phone = node["PhoneNumber"].InnerText,

                        StreetAddress = aElement.GetElementsByTagName("StreetAddress")[0].InnerText,
                        City = aElement.GetElementsByTagName("City")[0].InnerText,
                        State = aElement.GetElementsByTagName("State")[0].InnerText,
                        ZipCode = aElement.GetElementsByTagName("ZipCode")[0].InnerText
                    });
                }
            }
            catch (Exception e)
            {
                Console.Write(e.StackTrace);
            }
        }
        
        /// <summary>
        /// This method reads customers from a JSON file and puts them into the customers list.
        /// </summary>
        /// <param name="customerFilePath">customerFilePath gets a JSON file as input</param>
        public void ReadCustomersJson(String customersFilePath)
        {
            //Initializing the Json text reader.
            JsonTextReader reader = new JsonTextReader(System.IO.File.OpenText(customersFilePath));
            try
            {
                //Reading content using the reader and storing in a Json array.
                JArray jArray = (JArray)JToken.ReadFrom(reader);

                //Loop for accessing every Json object from the Json array.
                foreach (JObject jObject in jArray)
                {
                    //Reading parent Json objects.
                    JObject record = jObject;
                    //Reading child Json objects.
                    JObject address = (JObject)record["Address"];
                    //Assigning the Json object values to a new Customer object property and adding it to the Customer List.
                    customerList.Add(new Customer
                    {
                        Email = (String)record["Email"],
                        FirstName = (String)record["FirstName"],
                        LastName = (String)record["LastName"],
                        Phone = (String)record["PhoneNumber"],
                        StreetAddress = (String)address["StreetAddress"],
                        City = (String)address["City"],
                        State = (String)address["State"],
                        ZipCode = (String)address["ZipCode"]
                    });
                }
            }
            catch (FileNotFoundException e)
            {
                Console.Write(e.StackTrace);
            }
            catch (IOException e)
            {
                Console.Write(e.StackTrace);
            }
            catch (JsonException e)
            {
                Console.Write(e.StackTrace);
            }
        }

        /// <summary>
        /// This method reads customers from a CSV file and puts them into the customers list.
        /// </summary>
        /// <param name="customerFilePath">customerFilePath gets a CSV file as input</param>
        public void DisplayCustomers()
        {
            //Looping Customer list.
            foreach (Customer customer in this.customerList)
            {
                //Initializing a string variable and building it with values.
                String customerString = "";
                customerString += "Email: " + customer.Email + "\n";
                customerString += "First Name: " + customer.FirstName + "\n";
                customerString += "Last Name: " + customer.LastName + "\n";
                customerString += "Phone Number: " + customer.Phone + "\n";
                customerString += "Street Address: " + customer.StreetAddress + "\n";
                customerString += "City: " + customer.City + "\n";
                customerString += "State: " + customer.State + "\n";
                customerString += "Zip Code: " + customer.ZipCode + "\n";
                //Printing the variable.
                Console.WriteLine(customerString);
            }
        }
    }
}
