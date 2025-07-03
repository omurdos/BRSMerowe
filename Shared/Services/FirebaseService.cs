using FirebaseAdmin.Messaging;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Runtime.InteropServices;

namespace Shared.Services
{

    public class FirebaseService
    {
        private readonly ILogger<FirebaseService> _logger;
        public FirebaseService(ILogger<FirebaseService> Logger, string CredentialFilePath)
        {
            _logger = Logger;
            // Initialize Firebase Admin SDK only once
            if (FirebaseApp.DefaultInstance == null)
            {
                FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile(CredentialFilePath)
                });

                Logger.LogInformation($"Firebase Admin {FirebaseApp.DefaultInstance.Name}");
            }
        }

        public async Task<string> SendNotificationAsync(string deviceToken, string title, string body)
        {
            try
            {
                // Create a message object
                var message = new Message()
                {
                    Token = deviceToken,
                    Notification = new Notification()
                    {
                        Title = title,
                        Body = body,
                    },
                    // Optional: Add custom data
                    Data = new Dictionary<string, string>
            {
                { "key1", "value1" },
                { "key2", "value2" },
            }
                };

                // Send the notification
                string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
                Console.WriteLine("Message sent successfully: " + response);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending notification");
                throw;
            }
        }

        public async Task<BatchResponse> SendMulticastNotificationAsync(List<string> deviceTokens, string title, string body, string route)
        {


            try
            {

                // Create a multicast message object
                var message = new MulticastMessage()
                {
                    Tokens = deviceTokens, // List of FCM device tokens
                    Notification = new Notification()
                    {
                        Title = title,
                        Body = body,
                    },
                    // Optional: Add custom data
                    Data = new Dictionary<string, string>
        {
            { "route", route },

        }
                };

                // Send the multicast message
                var response = await FirebaseMessaging.DefaultInstance.SendEachForMulticastAsync(message);

                // Log successful deliveries
                Console.WriteLine($"Messages sent: {response.SuccessCount}, Failed: {response.FailureCount}");

                // Handle failed tokens if needed
                if (response.FailureCount > 0)
                {
                    var failedTokens = new List<string>();
                    for (int i = 0; i < response.Responses.Count; i++)
                    {
                        if (!response.Responses[i].IsSuccess)
                        {
                            failedTokens.Add(deviceTokens[i]); // Keep track of the failed tokens
                            Console.WriteLine($"Failed to send to {deviceTokens[i]}: {response.Responses[i].Exception}");
                        }
                    }
                    Console.WriteLine("Failed Tokens: " + string.Join(", ", failedTokens));
                }
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending multicast notification");
                throw ex;
            }

        }



        private string GetCredentialsFilePath()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Console.WriteLine("This is a Linux environment.");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Console.WriteLine("This is a Windows environment.");
            }
            else
            {
                Console.WriteLine("This is another operating system.");
            }
            // Get the current directory
            string currentDirectory = Directory.GetCurrentDirectory();

            // Combine the current directory with the file name
            string filePath = Path.Combine(currentDirectory, "uofs-e5988-firebase-adminsdk-4zomv-e6e2a44c12.json");

            // Check if the file exists
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"File not found: {filePath}");
            }

            return filePath;

        }


    }
}
