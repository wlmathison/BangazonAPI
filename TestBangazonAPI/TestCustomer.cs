/* Author: Brian Jobe
 * Purpose: Creating tests for the Customer controller methods of GET, POST, PUT. 
 * Methods: Test_Get_All_Customer, Test_Single_Customer, Test_Create_and_Delete_PaymentType, Test_Modify_Customer.
 */




using System;
using System.Net;
using Newtonsoft.Json;
using Xunit;
using BangazonAPI.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace TestBangazonAPI
{
    public class TestCustomer
    {
        [Fact]
        public async Task Test_Get_All_Customers()
        {
            using (var client = new APIClientProvider().Client)
            {

                var response = await client.GetAsync("/api/customer");


                string responseBody = await response.Content.ReadAsStringAsync();
                var customers = JsonConvert.DeserializeObject<List<Customer>>(responseBody);

                /*
                    ASSERT
                */
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.True(customers.Count > 0);
            }
        }
        [Fact]
        public async Task Test_Get_Single_Customer()
        {
            using (var client = new APIClientProvider().Client)
            {

                var response = await client.GetAsync("/api/customer/1");


                string responseBody = await response.Content.ReadAsStringAsync();
                var customer = JsonConvert.DeserializeObject<Customer>(responseBody);

                /*
                    ASSERT
                */
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.Equal("Jameka", customer.FirstName);
                Assert.Equal("Echols", customer.LastName);
                Assert.NotNull(customer);

            }
        }
        [Fact]
        public async Task Test_Create_And_Delete_Customer()
        {
            using (var client = new APIClientProvider().Client)
            {
                Customer john = new Customer
                {
                    FirstName = "John",
                    LastName = "Chalmers"
                };
                var johnAsJSON = JsonConvert.SerializeObject(john);


                var response = await client.PostAsync(
                    "/api/customer",
                    new StringContent(johnAsJSON, Encoding.UTF8, "application/json")
                );

                string responseBody = await response.Content.ReadAsStringAsync();
                var newJohn = JsonConvert.DeserializeObject<Customer>(responseBody);

                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                Assert.Equal("John", newJohn.FirstName);
                Assert.Equal("Chalmers", newJohn.LastName);

                var deleteResponse = await client.DeleteAsync($"/api/customer/{newJohn.Id}");
                Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
            }
        }

        [Fact]
        public async Task Test_Modify_Customer()
        {
            // New last name to change to and test
            string newLastName = "Edwards";
            string oldLastName = "Echols";

            using (var client = new APIClientProvider().Client)
            {
                /*
                    PUT section
                 */
                Customer modifiedCustomer = new Customer
                {
                    FirstName = "Jameka",
                    LastName = newLastName,

                };
                var modifiedCustomerAsJSON = JsonConvert.SerializeObject(modifiedCustomer);

                var response = await client.PutAsync(
                    "/api/customer/1",
                    new StringContent(modifiedCustomerAsJSON, Encoding.UTF8, "application/json")
                );
                string responseBody = await response.Content.ReadAsStringAsync();

                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

                /*
                    GET section
                 */
                var getCustomer = await client.GetAsync("/api/customer/1");
                getCustomer.EnsureSuccessStatusCode();

                string getCustomerBody = await getCustomer.Content.ReadAsStringAsync();
                Customer newCustomer = JsonConvert.DeserializeObject<Customer>(getCustomerBody);

                Assert.Equal(HttpStatusCode.OK, getCustomer.StatusCode);
                Assert.Equal(newLastName, newCustomer.LastName);

                Customer originalCustomer = new Customer
                {
                    FirstName = "Jameka",
                    LastName = oldLastName,

                };
                var originalCustomerAsJSON = JsonConvert.SerializeObject(originalCustomer);

                var originalResponse = await client.PutAsync(
                                "/api/customer/1",
                                new StringContent(originalCustomerAsJSON, Encoding.UTF8, "application/json")
                            );
                string responseBodyOriginal = await response.Content.ReadAsStringAsync();

                Assert.Equal(HttpStatusCode.NoContent, originalResponse.StatusCode);


            }
        }

    }
}