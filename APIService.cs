using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Net;
using TenmoClient.Data;
using TenmoClient.Models;

namespace TenmoClient
{
    public class APIService
    {
       
        private readonly static string API_BASE_URL = "https://localhost:44315/";
        private readonly IRestClient client = new RestClient();

        //login endpoints
        public Account GetAccount(int userId)
        {
            RestRequest request = new RestRequest(API_BASE_URL + $"accounts/{userId}");
            
            IRestResponse<Account> response = client.Get<Account>(request);

            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                Console.WriteLine("An error occurred communicating with the server.");
                return null;
            }
            else if (!response.IsSuccessful)
            {
                    Console.WriteLine("An error response was received from the server. The status code is " + (int)response.StatusCode);
                
                return null;
            }
            else
            {
                return response.Data;
            }
        }
        public List<User> GetUsers()
        {
            RestRequest request = new RestRequest(API_BASE_URL + "users");
            client.Authenticator = new JwtAuthenticator(UserService.GetToken());
            IRestResponse<List<User>> response = client.Get<List<User>>(request);
            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                throw new Exception("Error occurred - unable to reach server.");
            }
            if (!response.IsSuccessful)
            {
                if(response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new Exception("Authorization is required, please log in");
                }
                if (response.StatusCode == HttpStatusCode.Forbidden)
                {
                    throw new Exception("You do not have permission");
                }
                throw new Exception($"Error occurred: {response.StatusCode} ({(int)response.StatusCode})");
            }
            return response.Data;
        }
        public Transfer Transfer(int userIdTo, decimal amount)
        {
            Transfer transfer = new Transfer();
            transfer.AccountFrom = UserService.GetUserId();
            transfer.AccountTo = userIdTo;
            transfer.Amount = amount;
            transfer.TransferStatusId = 2;
            transfer.TransferTypeId = 2;
            
            RestRequest request = new RestRequest(API_BASE_URL + "transfers");
            request.AddJsonBody(transfer);

            client.Authenticator = new JwtAuthenticator(UserService.GetToken());

            IRestResponse<Transfer> response = client.Put<Transfer>(request);
            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                throw new Exception("Error occurred - unable to reach server.");
            }
            if (!response.IsSuccessful)
            {
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new Exception("Authorization is required, please log in");
                }
                if (response.StatusCode == HttpStatusCode.Forbidden)
                {
                    throw new Exception("You do not have permission");
                }
                throw new Exception($"Error occurred: {response.StatusCode} ({(int)response.StatusCode})");
            }
            return response.Data;

        }
    }
}
