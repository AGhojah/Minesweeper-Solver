using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace GenericTwitchIRC
{
    /// <summary>
    /// Steps to use this class: <para />
    /// 1- Create an instance of TwitchIRC <para />
    /// 2- Call "Connect" Method<para />
    /// 3- Subscribe to the "NewMessage" Event<para />
    /// 4- Call "JoinChannel" Method
    /// 5- When the program closes, you must call the Disconnect method
    /// </summary>
    class TwitchIRC
    {
        #region ReadMessage Variables
        string cUser = "";
        string cChannel = "";
        string cMsg = "";
        string helper1 = "";
        string[] userChannelChat = new string[3] { "", "", "" };
        #endregion

        private bool isGuest = true;
        private bool isConnected = false;
        private bool isInChannel = false;

        private string activeChannel = null;

        private TcpClient tcpClient;
        private StreamReader MessageIn;
        private StreamWriter MessageOut;

        private Thread worker;

        private string username = "", oauth = "", serverIP = "", message = "";
        private int port = 6667;


        public delegate void NewMessageEventHandler(object source, NewMessageEventArgs e);
        /// <summary>
        /// New message contains a String array of 3 elements (Username, Channel, Message Content)... Must Invoke
        /// </summary>
        public event NewMessageEventHandler NewMessage;


        /// <summary>
        /// New message contains a string array of 3 elements (Username, Channel, Message content)
        /// </summary>
        /// <param name="msg"></param>
        protected virtual void OnNewMessage(string[] msg)
        {
            if (NewMessage != null)
                NewMessage(this, new NewMessageEventArgs() { NewMessage = msg });
        }

        /// <summary>
        /// Twitch IRC Class (Uses username and oauth key, can receive and send messages)
        /// </summary>
        /// <param name="username">The username used to login to Twitch.tv</param>
        /// <param name="oauth">A generated oAuth used instead of the password</param>
        /// <param name="port">Twitch port (6667) if it doesn't work use (80)</param>
        /// <param name="serverIP">The server IP of twitch: "irc.chat.twitch.tv"</param>
        public TwitchIRC(string username, string oauth, int port, string serverIP)
        {
            this.username = username.ToLower();
            this.oauth = oauth;
            this.port = port;
            this.serverIP = serverIP;
            this.isGuest = false;
        }

        /// <summary>
        /// Twitch IRC (Guest user), can only receive chat!
        /// </summary>
        public TwitchIRC()
        {
            this.username = "justinfan35";
            this.oauth = "";
            this.port = 6667;
            this.serverIP = "irc.chat.twitch.tv";
        }

        /// <summary>
        /// Tries to connect using the default or provided server IP, Port and Username
        /// </summary>
        /// <returns>Returns a string describing the status of the attempt</returns>
        public string Connect()
        {
            if (tcpClient == null)
            {
                try
                {
                    tcpClient = new TcpClient(this.serverIP, this.port);
                    MessageIn = new StreamReader(tcpClient.GetStream());
                    MessageOut = new StreamWriter(tcpClient.GetStream());
                    if (!this.isGuest)
                        MessageOut.WriteLine("PASS " + oauth);
                    MessageOut.WriteLine("NICK " + username);
                    MessageOut.WriteLine("USER " + username + " 8 * :" + username);
                    MessageOut.Flush();

                    isConnected = true;
                    worker = new Thread(ReadMessages);
                    worker.Start();

                    return FirstLine();
                }
                catch
                {
                    return "Error, Failed to connect";
                }
            }
            else return "You Are Already Connected";
        }

        private void ReadMessages()
        {
            while (isConnected)
                ReadMessage(true);
        }


        public void Disconnect()
        {
            try
            {
                if (tcpClient.Connected)
                {
                    ResetVariables();
                    tcpClient.Close();
                }
            }
            catch { }
            finally { isConnected = false; }
        }
        private void ResetVariables()
        {
            isConnected = false;
            isInChannel = false;
        }

        private string FirstLine()
        {
            message = MessageIn.ReadLine();
            if (message.Contains("NOTICE") == true)
                return "Error logging in, wrong Username or Password/oauth";
            else
                return "Connected as " + username;
        }

        /// <summary>
        /// Join the specified channel
        /// </summary>
        /// <param name="channel"></param>
        public void JoinChannel(string channel)
        {
            LeaveChannel();
            if (tcpClient == null)
                return;

            if (tcpClient.Connected && this.isConnected)
            {
                MessageOut.WriteLine("JOIN #" + channel.ToLower());
                MessageOut.Flush();
                this.activeChannel = channel.ToLower();
                this.isInChannel = true;
            }
        }

        /// <summary>
        /// Leave the specified channel
        /// </summary>
        /// <param name="channel"></param>
        public void LeaveChannel(string channel)
        {
            if (tcpClient == null)
                return;
            if (tcpClient.Connected && this.isInChannel)
            {
                MessageOut.WriteLine("PART #" + channel);
                MessageOut.Flush();
                this.isInChannel = false;
            }
        }

        /// <summary>
        /// Leave the current channel if you're in a channel
        /// </summary>
        public void LeaveChannel()
        {
            if (tcpClient == null)
                return;
            if (tcpClient.Connected && this.isInChannel)
            {
                MessageOut.WriteLine("PART #" + this.activeChannel);
                MessageOut.Flush();
                this.isInChannel = false;
            }
        }

        private string ReadLine()
        {
            return MessageIn.ReadLine();
        }

        private string[] ReadMessage(bool SkipStart)
        {
            try
            {
                if (tcpClient.Available > 0 || MessageIn.Peek() >= 0)
                {

                    cUser = "";
                    cChannel = "";
                    cMsg = "";
                    helper1 = "";
                    message = MessageIn.ReadLine();

                    userChannelChat = new string[3] { "", "", "" };
                    if (message.StartsWith("PING "))
                    {
                        SendIrcMessage("PONG tmi.twitch.tv\r\n");
                    }
                    if (SkipStart == true)
                        if (message.StartsWith(":tmi.twitch.tv"))
                            return userChannelChat;
                    if (message.Contains("PRIVMSG #"))
                    {
                        int start = 0, end = 0;
                        start = message.IndexOf(":");
                        end = message.IndexOf("!");
                        cUser = message.Substring(start + 1, end - start - 1);
                        helper1 = ":" + cUser + "!" + cUser + "@" + cUser + ".tmi.twitch.tv PRIVMSG #";
                        start = helper1.Length;
                        end = message.Substring(start).IndexOf(":");
                        cChannel = message.Substring(start, end - 1);
                        cMsg = message.Substring(start + end + 1);
                        userChannelChat[0] = cUser;
                        userChannelChat[1] = cChannel;
                        userChannelChat[2] = cMsg;
                        OnNewMessage(userChannelChat);
                        return userChannelChat;
                    }
                    return userChannelChat;
                }

                else
                {
                    userChannelChat = new string[3] { "", "###", "" };
                    Thread.Sleep(10);
                    return userChannelChat;
                }
            }
            catch
            {
                userChannelChat = new string[3] { "", "###", "" };
                Thread.Sleep(10);
                return userChannelChat;
            }
        }

        /// <summary>
        /// Sends a message to the channel you're connected to (Must not be a guest user)
        /// </summary>
        /// <param name="message">The content of the message (Chat line)</param>
        public void SendIrcMessage(string message)
        {
            if (tcpClient == null)
                return;
            if (isConnected)
            {
                try
                {
                    MessageOut.WriteLine(message);
                    MessageOut.Flush();
                }
                catch { }
            }
        }

        /// <summary>
        /// Logged in as guest or not
        /// </summary>
        /// <returns>Returns true if you're logged in as a guest</returns>
        public bool IsGuest()
        {
            return this.isGuest;
        }

        /// <summary>
        /// Are you in a channel
        /// </summary>
        /// <returns>returns true if you're in a channel</returns>
        public bool IsInChannel()
        {
            return this.isInChannel;
        }

        /// <summary>
        /// Is it connected
        /// </summary>
        /// <returns></returns>
        public bool IsConnected()
        {
            return this.isConnected;
        }

        /// <summary>
        /// returns Null if you aren't in any channel
        /// </summary>
        /// <returns>Name of the channel you're connected to</returns>
        public string GetCurrentChannel()
        {
            return this.activeChannel;
        }
    }

    public class NewMessageEventArgs : EventArgs
    {
        /// <summary>
        /// String array of 3 elements (Username, Channel, Message)
        /// </summary>
        public string[] NewMessage { get; set; }
        public NewMessageEventArgs()
        {
        }
    }
}
