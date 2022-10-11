using CosmosEngine;
using CosmosEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SpaceBattle
{
    internal class ChatJSON : GameBehaviour
    {
        //variabler til chatbox
        public InputField input;
        public TextField chatOutputText;

        //URLs til at GET/POST chat
        private string chatGETUrl = "INSERT URL";
        private string chatPOSTUrl = "INSERT URL";

        //JSON
        private string chatJSONString;
        //private JsonData      LitJson lib https://litjson.net/api/LitJson/JsonData/
        public string chatOutput;
        public string newText;
        public List<Message> chatList = new List<Message>();
        public string chatNewString;
        int messageCount;

        public string displayTextString = "";

        protected override void Start()
        {
            //Task<GETChatJSON>();
        }

        //private IEnumerator GETChatJSON()
        //{
            //WWW chat = new WWW(chatGETUrl); LitJson lib
            //yield return chat;
        //    if (chat.error == null)
        //    {

        //        Debug.Log("Connection is good to: " + chatGETUrl);

        //    }
        //    else
        //    {
        //        Debug.Log("ERROR: " + chat.error);
        //    }

        //    chatJSONString = chat.text;
        //    chatStringData = JsonMapper.ToObject(chatJSONString);

        //    GetChatText();
        //}

        public void GetChatText()
        {
            string msgID;
            string msgContent;
            string msgUsername;

            //for (int i = 0; i < chatStringData["messages"].Count; i++)
            //{
            //    msgID = chatStringData["messages"][i]["chat_message_id"].ToString();
            //    msgContent = chatStringData["messages"][i]["msg_content"].ToString();
            //    msgUsername = chatStringData["messages"][i]["username"].ToString();



            //    //displayTextString = "User: " + msgUsername + "      " + "Message: " + msgContent


            //    chatList.Add(new Message
            //    {
            //        messageId = msgID,
            //        messageContent = msgContent,
            //        messageUsername = msgUsername,
            //    });
            //    messageCount = i;

            //}
            //DisplayMessage();
        }
        //void DisplayMessage()
        //{
        //    chatOutputText.text = "";
        //    IEnumerable<Message> sortedMessages =
        //         from Message in chatList
        //         select Message;

        //    foreach (Message msg in sortedMessages)
        //    {

        //        chatNewString = chatNewString + msg.messageUsername + ": " + msg.messageContent + "\n";
        //    }
        //    chatOutputText.text = chatNewString;
        //}
    }
}
