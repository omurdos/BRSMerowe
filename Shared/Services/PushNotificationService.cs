using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Builder.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Services
{
    public class PushNotificationService
    {
       
            FirebaseMessaging firebaseMessaging;
            static FirebaseApp defaultApp = null;
            public PushNotificationService()
            {
                if (defaultApp == null) initializeApp();
                firebaseMessaging = FirebaseMessaging.DefaultInstance;
            }
            public async Task<string> SendMessage(string title, string body, string token)
            {
                var message = new Message()
                {
                    Notification = new Notification
                    {
                        Title = title,
                        Body = body,
                    },

                    Token = token
                };


                var result = await firebaseMessaging.SendAsync(message);
            return result;
            }
            public async Task<BatchResponse> SendMulticastMessage(string title, string body, List<string> tokens)
            {
                var message = new MulticastMessage()
                {
                    Notification = new Notification
                    {
                        Title = title,
                        Body = body,
                    },
                    Tokens = tokens
                };

                var messaging = FirebaseMessaging.DefaultInstance;
                var result = await messaging.SendMulticastAsync(message);
            return result;
            }
            public void initializeApp()
            {
            Console.WriteLine("");

            defaultApp = FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "uofs-e5988-firebase-adminsdk-4zomv-e6e2a44c12.json")),
                }) ;
            }
           }
}
