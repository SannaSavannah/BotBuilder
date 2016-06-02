using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Utilities;

namespace Stock_LUIS
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<Message> Post([FromBody]Message message)
        {
            if (message.Type == "Message")
            {
                StockLUIS stLuis = await LUISStockClient.ParseUserInput(message.Text);
                string strRet;

                if (stLuis.intents.Any())
                {
                    switch (stLuis.intents[0].intent)
                    {
                        case "ShowLatestStampings":
                            if (stLuis.entities.Any())
                                strRet = "If I had connection to database, I would tell your latest " + stLuis.entities[0].entity + " to you";
                            else
                                strRet = "If I had connection to database, I would tell your latest stamps to you";
                            break;
                        case "TellSaldo":
                            if (stLuis.entities.Any())
                                strRet = "If I had connection to database, I would tell your "+ stLuis.entities[0].entity + " to you";
                            else
                                strRet = "If I had connection to database, I would tell your saldo to you";
                            break;
                        case "CreateStamp":
                            if (stLuis.entities.Any())
                                strRet = "If I had connection to database, I would deffinately let you create a new " + stLuis.entities[0].entity;
                            else
                                strRet = "If I had connection to database, I would deffinately let you create a new stamp";
                            break;
                        case "TellStatus":
                            strRet = "If I had connection to database, I would tell whether you are in or out";
                            break;
                        default:
                            strRet =
                                "You can ask this bot to show your latest stampings or make a new stamping. It can also show yor current status or saldo.";
                            break;
                    }
                }
                else
                {
                    strRet = "Sorry, I don't understand...";
                }


                return message.CreateReplyMessage(strRet);
            }
            return HandleSystemMessage(message);
        }
        

        private Message HandleSystemMessage(Message message)
        {
            if (message.Type == "Ping")
            {
                Message reply = message.CreateReplyMessage();
                reply.Type = "Ping";
                return reply;
            }
            else if (message.Type == "DeleteUserData")
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == "BotAddedToConversation")
            {
                return message.CreateReplyMessage("Well, I'm here - am I?");
            }
            else if (message.Type == "BotRemovedFromConversation")
            {
                return message.CreateReplyMessage("By then...");
            }
            else if (message.Type == "UserAddedToConversation")
            {
                return message.CreateReplyMessage("Welcome new user");
            }
            else if (message.Type == "UserRemovedFromConversation")
            {
            }
            else if (message.Type == "EndOfConversation")
            {
            }

            return null;
        }
    }
}