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
using PoemBot.Requests;
using PoemBot.Poems;

namespace PoemBot
{
   
    //
    

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
                RequestFabric fablic = new RequestFabric();
                string replyStr = String.Empty;
                if (activity.Text.Contains("/getpoem"))
                {

                                      
                    string received = await fablic.GetRandomPoem();

                    
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
                        var poemObj=pHelper.MakePoem(linesColl);
                        

                        // return our reply to the user
                        Activity reply = activity.CreateReply($"{poem}");                        
                        await connector.Conversations.ReplyToActivityAsync(reply);

                        var stateClient = activity.GetStateClient();
                        BotState botState = new BotState(stateClient);
                        BotData botData = new BotData(eTag: "*");
                        botData.Data = poemObj;
                        botData.SetProperty<BotState>("LastPoem", botState);
                        BotData responseSetData = await stateClient.BotState.SetUserDataAsync(activity.ChannelId, activity.From.Id, botData);

                    }
                }
                else if(activity.Text.Contains("/like"))
                {
                    var stateClient = activity.GetStateClient();
                    BotData userData = await stateClient.BotState.GetUserDataAsync(activity.ChannelId, activity.From.Id);
                    var poemData = userData.GetProperty<BotData>("LastPoem").Data as Poem;
                    if (poemData != null)
                    {
                       var responseStr= fablic.LikePoem(poemData.HashId);
                        int ss = 0;
                    }

                    
                    
                    //ReceiptCard plCard = new ReceiptCard()
                    //{
                    //    Title = "Like previous poem?",
                    //    Buttons = cardButtons,
                    //    Items = receiptList,
                    //    Total = "275.25",
                    //    Tax = "27.52"
                    //};
                }
                else if(activity.Text.Contains("/start"))
                {
                    replyStr = @"Hello! Try to use ""/getpoem"" command! ";
                    Activity reply = activity.CreateReply($"{replyStr}");
                    await connector.Conversations.ReplyToActivityAsync(reply);
                }
                else if(activity.Text.Contains("/help"))
                {
                    replyStr = @"Hello! Try to use ""/getpoem"" command! ";
                    Activity reply = activity.CreateReply($"{replyStr}");
                    await connector.Conversations.ReplyToActivityAsync(reply);
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