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
        private static readonly string filePath = "D:\\PROJECTS\\CustomerReaderExercise\\doc\\";
        private List<Customer> customerList;

        /// <summary>
        /// Main method
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            CustomerReader customerReader = new CustomerReader();

            customerReader.ReadCustomersCsv(filePath + "customers.csv");
            customerReader.ReadCustomersXml(filePath + "customers.xml");
            customerReader.ReadCustomersJson(filePath + "customers.json");

            Console.WriteLine("\n" + "Added this many customers: " + customerReader.GetCustomers().Count + "\n");
            customerReader.DisplayCustomers();
            Console.ReadLine();
        }

        /// <summary>
        /// Public Constructor that initializes the Customer list object
        /// </summary>
        public CustomerReader()
        {
            customerList = new List<Customer>();
        }

        /// <summary>
        /// Gets the Customer list of records read from the .csv, .xml, .json files
        /// </summary>
        /// <returns></returns>
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
                using (StreamReader streamReader = new StreamReader(File.Open(customerFilePath, FileMode.Open)))
                {
                    String line;
                    String[] attributes;
                    line = streamReader.ReadLine();
                    do
                    {
                        attributes = line.Split(',');

                        customerList.Add(new Customer
                        {
                            Email = attributes[0],
                            Firstname = attributes[1],
                            LastName = attributes[2],
                            Phone = attributes[3],
                            StreetAddress = attributes[4],
                            City = attributes[5],
                            State = attributes[6],
                            ZipCode = attributes[7]
                        });
                        line = streamReader.ReadLine();
                    } while (line != null);
                }

                //StreamReader br = new StreamReader(File.Open(customer_file_path, FileMode.Open));
                //String line = br.ReadLine();

                //while (line != null)
                //{
                //    //String[] attributes = line.split(" , ");
                //    String[] attributes = line.Split(',');

                //    Customer customer = new Customer();
                //    customer.email = attributes[0];
                //    customer.fn = attributes[1];
                //    customer.ln = attributes[2];
                //    customer.phone = attributes[3];
                //    customer.streetAddress = attributes[4];
                //    customer.city = attributes[5];
                //    customer.state = attributes[6];
                //    customer.zipCode = attributes[7];

                //    customers.Add(customer);
                //    line = br.ReadLine();
                //}
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
                xmlDocument.Load(customerFilePath);

                XmlNodeList xmlNodeList = xmlDocument.GetElementsByTagName("Customers");

                for (int temp = 0; temp < xmlNodeList.Count; temp++)
                {
                    XmlNode xmlNode = xmlNodeList[temp];
                    Console.WriteLine("\nCurrent Element :" + xmlNode.Name);
                    if (xmlNode.NodeType == XmlNodeType.Element)
                    {
                        XmlElement eElement = (XmlElement)xmlNode;
                        XmlElement aElement = (XmlElement)eElement.GetElementsByTagName("Address").Item(0);

                        customerList.Add(new Customer
                        {
                            Email = eElement.GetElementsByTagName("Email").Item(temp).InnerText,
                            Firstname = eElement.GetElementsByTagName("FirstName").Item(temp).InnerText,
                            LastName = eElement.GetElementsByTagName("LastName").Item(temp).InnerText,
                            Phone = eElement.GetElementsByTagName("PhoneNumber").Item(temp).InnerText,
                            StreetAddress = aElement.GetElementsByTagName("StreetAddress").Item(temp).InnerText,
                            City = aElement.GetElementsByTagName("City").Item(temp).InnerText,
                            State = aElement.GetElementsByTagName("State").Item(temp).InnerText,
                            ZipCode = aElement.GetElementsByTagName("ZipCode").Item(temp).InnerText
                        });
                    }
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
            //JsonTextReader reader = (new JsonTextReader(System.IO.File.OpenText(customersFilePath));
            //JObject reader = JObject.Parse(File.ReadAllText(customersFilePath));
            JsonTextReader reader = new JsonTextReader(System.IO.File.OpenText(customersFilePath));

            try
            {
                JArray jArray = (JArray)JToken.ReadFrom(reader);
                //JArray a = (JArray)reader);
                //

                foreach (JObject jObj in jArray)
                {
                    JObject record = jObj;
                    JObject address = (JObject)record["Address"];
                    customerList.Add(new Customer
                    {
                        Email = (String)record["Email"],
                        Firstname = (String)record["FirstName"],
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
            foreach (Customer customer in this.customerList)
            {
                String customerString = "";
                customerString += "Email: " + customer.Email + "\n";
                customerString += "First Name: " + customer.Firstname + "\n";
                customerString += "Last Name: " + customer.LastName + "\n";
                customerString += "Phone Number: " + customer.Phone + "\n";
                customerString += "Street Address: " + customer.StreetAddress + "\n";
                customerString += "City: " + customer.City + "\n";
                customerString += "State: " + customer.State + "\n";
                customerString += "Zip Code: " + customer.ZipCode + "\n";

                Console.WriteLine(customerString);
            }
        }
    }
}
