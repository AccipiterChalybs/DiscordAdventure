using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Threading;
using System.Net.Security;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class DiscordManager : MonoBehaviour
{
    
    public string token;

    public string defaultChannelID = "";

    private DiscordClient client;
    public DiscordPlayerControl discordControl;

    public float messageCheckInterval=5;
    private float timer=5;

    public string lastMessageId;

    public void Start()
    {
        client = new DiscordClient(token, defaultChannelID);
        timer = messageCheckInterval;
        client.InitMessages(handleMessages);
    }

    public void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            if (lastMessageId != null) client.CheckMessages(lastMessageId, handleMessages);
            timer = messageCheckInterval;
        }

        client.updateActions();
    }



    public void handleMessages(DiscordClient client, DiscordMessage[] messages, DiscordError error)
    {
        for (int i=messages.Length-1; i>=0; --i)
        {
            discordControl.handleMessage(messages[i]);
        }
        if (messages.Length > 0)  lastMessageId = messages[0].ID;
    }












    /**
     * The majority of the code below is from the Discord Unity project (https://github.com/DiscordUnity/DiscordUnity/), 
     * with some edits by myself to slim it down & integrate it into the project
     */

    public class DiscordClient
    {

        public string token;
        public string channelID;

        public Queue<Action> unityInvoker = new Queue<Action>();

        public DiscordClient(string t, string c)
        {
            token = t;
            channelID = c;
            ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
        }


        public void InitMessages(DiscordMessagesCallback callback)
        {
            GetMessages(channelID, 100, callback);
        }

        public void CheckMessages(string lastMessage, DiscordMessagesCallback callback)
        {
            GetMessages(channelID, 100, lastMessage, false, callback);
        }

        //fixes HTTPS issue: solution from here: http://stackoverflow.com/questions/4926676/mono-https-webrequest-fails-with-the-authentication-or-decryption-has-failed
        //Needed because apparently Mono has no CA installed by default, so validating the server's https certificate will always fail.
        private bool MyRemoteCertificateValidationCallback(System.Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            bool isOk = true;
            // If there are errors in the certificate chain, look at each error to determine the cause.
            if (sslPolicyErrors != SslPolicyErrors.None)
            {
                for (int i = 0; i < chain.ChainStatus.Length; i++)
                {
                    if (chain.ChainStatus[i].Status != X509ChainStatusFlags.RevocationStatusUnknown)
                    {
                        chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
                        chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
                        chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 1, 0);
                        chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
                        bool chainIsValid = chain.Build((X509Certificate2)certificate);
                        if (!chainIsValid)
                        {
                            isOk = false;
                        }
                    }
                }
            }
            return isOk;
        }

        public void updateActions()
        {
            if (unityInvoker == null) return;

            while (unityInvoker.Count > 0)
            {
                unityInvoker.Dequeue()();
            }
        }


        #region Http
        internal delegate void CallResult(string result);

        internal enum HttpMethod
        {
            Post,
            Get,
            Patch,
            Put,
            Delete
        }

        internal class RequestState
        {
            public HttpMethod method;
            public CallResult result;
            public CallResult error;
            public HttpWebRequest request;
        }

        internal class RequestStateJSON : RequestState
        {
            public string content;
        }

        internal static string APIurl = "https://discordapp.com/api/";

        internal void Call(HttpMethod method, string url, CallResult result = null, CallResult error = null, string content = null)
        {
            try
            {
                HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(url);
                httpRequest.Headers["Authorization"] = "Bot " + token;
                httpRequest.ContentType = "application/json";
                httpRequest.UserAgent = "DiscordBot (http://alexhawker.com, 0.0.0)";

                switch (method)
                {
                    case HttpMethod.Post:
                        httpRequest.Method = "POST";
                        if (content == null) httpRequest.BeginGetResponse(new AsyncCallback(OnGetResponse), new RequestState() { method = method, result = result, error = error, request = httpRequest });
                        else httpRequest.BeginGetRequestStream(new AsyncCallback(OnRequestStream), new RequestStateJSON() { method = method, content = content, result = result, error = error, request = httpRequest });
                        break;

                    case HttpMethod.Get:
                        httpRequest.Method = "GET";
                        httpRequest.BeginGetResponse(new AsyncCallback(OnGetResponse), new RequestState() { method = method, result = result, error = error, request = httpRequest });
                        break;

                    case HttpMethod.Patch:
                        httpRequest.Method = "PATCH";
                        httpRequest.BeginGetRequestStream(new AsyncCallback(OnRequestStream), new RequestStateJSON() { method = method, content = content, result = result, error = error, request = httpRequest });
                        break;

                    case HttpMethod.Put:
                        httpRequest.Method = "PUT";
                        if (content == null) httpRequest.BeginGetResponse(new AsyncCallback(OnGetResponse), new RequestState() { method = method, result = result, error = error, request = httpRequest });
                        else httpRequest.BeginGetRequestStream(new AsyncCallback(OnRequestStream), new RequestStateJSON() { method = method, content = content, result = result, error = error, request = httpRequest });
                        break;

                    case HttpMethod.Delete:
                        httpRequest.Method = "DELETE";
                        httpRequest.BeginGetResponse(new AsyncCallback(OnGetResponse), new RequestState() { method = method, result = result, error = error, request = httpRequest });
                        break;
                }
            }

            catch (Exception e)
            {
                Debug.LogError("#Main Call");
                Debug.LogError(e.Message);
                error(e.Message);
            }
        }

        private void OnRequestStream(IAsyncResult result)
        {
            RequestStateJSON state = (RequestStateJSON)result.AsyncState;

            try
            {
                using (StreamWriter writer = new StreamWriter(state.request.EndGetRequestStream(result)))
                {
                    //Debugger.WriteLine("Send: " + state.content);
                    writer.Write(state.content);
                    writer.Flush();
                    writer.Close();
                }

                state.request.BeginGetResponse(new AsyncCallback(OnGetResponse), state);
            }

            catch (Exception e)
            {
                Debug.LogError("#Request Call");
                Debug.LogError(e.Message);
                state.error(e.Message);
            }
        }

        private void OnGetResponse(IAsyncResult result)
        {
            RequestState state = (RequestState)result.AsyncState;

            try
            {
                HttpWebResponse httpResponse = (HttpWebResponse)state.request.EndGetResponse(result);

                using (StreamReader reader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    string response = reader.ReadToEnd();
                    //Debugger.WriteLine("Received: " + response);
                    state.result(response);
                }
            }

            catch (WebException e)
            {
                print(e.Message);
                if (e.Message.Contains("(502)")) // Bad request
                {
                    Thread.Sleep(2000);
                    Call(state.method, state.request.RequestUri.AbsolutePath, state.result, state.error, (state.GetType() == typeof(RequestStateJSON)) ? ((RequestStateJSON)state).content : null);
                    return;
                }

                state.error(e.Message);
            }
        }

        #endregion

        #region API
        //
        // Channels
        //

        private string channelurl = "https://discordapp.com/api/channels/";

        //
        // Messages
        //

        internal DiscordMessage[] GetMessagesArray(DiscordMessageJSON[] messages)
        {
            List<DiscordMessage> result = new List<DiscordMessage>();

            foreach (DiscordMessageJSON message in messages)
            {
                result.Add(new DiscordMessage(message));
            }

            return result.ToArray();
        }

        internal void GetMessages(string channelID, int limit, string messageID, bool before, DiscordMessagesCallback callback)
        {
            if (callback == null) callback = delegate { };
            string url = channelurl + channelID + "/messages?&limit=" + limit;
            if (before) url += "&before=" + messageID;
            else url += "&after=" + messageID;

            Call(HttpMethod.Get, url, (result) =>
            {
                string substring = "{\"messages\":";
                result = result.Insert(0, substring).Insert(result.Length + substring.Length, "}");
                DiscordMessageJSONWrapper wrapper = JsonUtility.FromJson<DiscordMessageJSONWrapper>(result);
                unityInvoker.Enqueue(() => callback(this, GetMessagesArray(wrapper.messages), new DiscordError()));
            }, (result) => { unityInvoker.Enqueue(() => callback(this, null, new DiscordError(result))); });
        }

        internal void GetMessages(string channelID, int limit, DiscordMessagesCallback callback)
        {
            if (callback == null) callback = delegate { };
            string url = channelurl + channelID + "/messages?&limit=" + limit;


            Call(HttpMethod.Get, url, (result) =>
            {
                string substring = "{\"messages\":";
                result = result.Insert(0, substring).Insert(result.Length + substring.Length, "}");
                DiscordMessageJSONWrapper wrapper = JsonUtility.FromJson<DiscordMessageJSONWrapper>(result);

                unityInvoker.Enqueue(() => callback(this, GetMessagesArray(wrapper.messages), new DiscordError()));
            }, (result) => { unityInvoker.Enqueue(() => callback(this, null, new DiscordError(result))); });
            
        }
    }

    public class DiscordMessage
    {
        /// <summary> The author of this message. </summary>
        public string author { get; internal set; }
        /// <summary> The content of this message. </summary>
        public string content { get; internal set; }
        /// <summary> When is this message created? </summary>
        public DateTime createdAt { get; internal set; }
        /// <summary> When is this message edited? </summary>
        public DateTime editedAt { get; internal set; }

        internal string ID;
        internal string channelID;

        internal DiscordMessage(DiscordMessageJSON e)
        {
            ID = e.id;
            channelID = e.channel_id;
            author = e.author.username;
            content = e.content;
            if (!string.IsNullOrEmpty(e.timestamp)) createdAt = DateTime.Parse(e.timestamp);
            else createdAt = DateTime.Now;
            if (!string.IsNullOrEmpty(e.edited_timestamp)) editedAt = DateTime.Parse(e.edited_timestamp);
            else editedAt = DateTime.Now;
        }

        public override bool Equals(object o)
        {
            return base.Equals(o);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(DiscordMessage a, DiscordMessage b)
        {
            if ((object)a == null && (object)b == null) return true;
            if ((object)a == null && (object)b != null) return false;
            if ((object)a != null && (object)b == null) return false;
            return a.ID == b.ID;
        }

        public static bool operator !=(DiscordMessage a, DiscordMessage b)
        {
            return !(a == b);
        }
    }

    public class DiscordError
    {
        public bool failed;
        public string message;

        public DiscordError()
        {
            failed = false;
        }

        public DiscordError(string Message)
        {
            failed = true;
            message = Message;
        }
    }

    public delegate void DiscordCallback(DiscordClient m, string content, DiscordError error);
    public delegate void DiscordMessageCallback(DiscordClient client, DiscordMessage message, DiscordError error);
    public delegate void DiscordMessagesCallback(DiscordClient client, DiscordMessage[] messages, DiscordError error);
    #endregion
}