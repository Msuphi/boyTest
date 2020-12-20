using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace PushNotifications
{
    public static class FirebaseNotification
    {
        public static async Task SendNotification(string textMessage,string token)
        {
            var defaultApp = FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "notificationsKey.json"))
            });
            var fcm = FirebaseMessaging.GetMessaging(defaultApp);
            var message = new Message()
            {
                Data = new Dictionary<string, string>()
                {
                    ["Message"] = textMessage
                },
                Notification = new Notification
                {
                    Title = "Mesajınız Var",
                    Body = textMessage
                },

                //Buraya device dan elde edilen token bilgisi gelecekti. Ancak dediğim gibi Ionic uygulamsaını ayağa kaldıramadım.
                //Token = token,
                Topic = "Test"
            };
            //var messaging = FirebaseMessaging.DefaultInstance;
            var result = await fcm.SendAsync(message);
        }
    }
}
