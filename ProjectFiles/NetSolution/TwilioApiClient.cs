#region Using directives
using System;
using System.Text;
using System.Collections.Generic;
using UAManagedCore;
using OpcUa = UAManagedCore.OpcUa;
using FTOptix.UI;
using FTOptix.DataLogger;
using FTOptix.HMIProject;
using FTOptix.NativeUI;
using FTOptix.Store;
using FTOptix.InfluxDBStoreRemote;
using FTOptix.Retentivity;
using FTOptix.CoreBase;
using FTOptix.InfluxDBStore;
using FTOptix.Core;
using FTOptix.NetLogic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using FTOptix.MQTTClient;
#endregion

public class TwilioApiClient : BaseNetLogic
{
    public override void Start()
    {
        // Insert code to be executed when the user-defined logic is started
    }

    public override void Stop()
    {
        // Insert code to be executed when the user-defined logic is stopped
    }
    [ExportMethod]

    public void sendSMS(string accountSid, string authToken,string fromNumber, string toNumber, string messageBody)
    {
        string response =string.Empty;
        int statusCode = 0;

        try
        {
            string endpoint =$"https://api.twilio.com/2010-04-01/Accounts/{accountSid}/Messages.json";
            var postData =new[]{
                new KeyValuePair<string,string>("From", fromNumber),
                new KeyValuePair<string,string>("To", toNumber),
                new KeyValuePair<string,string>("Body", messageBody),
            };

            using (var client = new HttpClient())
            {
                var byteArray = Encoding.ASCII.GetBytes($"{accountSid}:{authToken}");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                var content = new FormUrlEncodedContent(postData);
                var httpResponse = client.PostAsync(endpoint,content).Result;
                response = httpResponse.Content.ReadAsStringAsync().Result;
            }
            Log.Info($"Twilios response:{response}");
            Log.Info($"Status Code: {statusCode}");
        }
        catch (Exception ex)
        {
            Log.Info($"Error enviando SMS: {ex.Message}");
        }
    }
}
