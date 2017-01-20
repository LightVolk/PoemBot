using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;

namespace PoemBot
{
   

    

    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                // calculate something for us to return

                string replyStr = String.Empty;
                if (activity.Text.Contains("/getpoem"))
                {
                    var request = WebRequest.Create(new Uri("https://poem.alv.in/api/generate")) as HttpWebRequest;
                    request.Method = "GET";
                    request.Accept = "application/json";
                    WebResponse responseObject = await Task<WebResponse>.Factory.FromAsync(request.BeginGetResponse, request.EndGetResponse, request);
                    var responseStream = responseObject.GetResponseStream();
                    var sr = new StreamReader(responseStream);
                    string received = await sr.ReadToEndAsync();

                    
                    JsonTextReader jreader = new JsonTextReader(new StringReader(received));
                    if(jreader!=null)
                    {
                        List<String> linesColl = new List<string>();
                        while(jreader.Read())
                        {
                            if(jreader.Value!=null)
                            {
                                linesColl.Add(jreader.Value as String);
                               // replyStr = string.Concat(replyStr, "\n", jreader.Value as string,"\n");                                
                            }
                        }

                        PoemHelper pHelper = new PoemHelper();
                        var poem = pHelper.MakePoemReply(linesColl);

                        

                        // return our reply to the user
                        Activity reply = activity.CreateReply($"{poem}");                        
                        await connector.Conversations.ReplyToActivityAsync(reply);
                    }
                }
                else
                {
                    Activity reply = activity.CreateReply($"Cannot understand command.\nType /getpoem");
                    await connector.Conversations.ReplyToActivityAsync(reply);
                }                              
            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}