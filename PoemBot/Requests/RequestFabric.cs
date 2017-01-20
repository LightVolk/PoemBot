using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace PoemBot.Requests
{
    public class RequestFabric
    {
        public async Task<string> GetRandomPoem()
        {
            var request = WebRequest.Create(new Uri("https://poem.alv.in/api/generate")) as HttpWebRequest;
            request.Method = "GET";
            request.Accept = "application/json";
            
            WebResponse responseObject = await Task<WebResponse>.Factory.FromAsync(request.BeginGetResponse, request.EndGetResponse, request);
            var responseStream = responseObject.GetResponseStream();
            var sr = new StreamReader(responseStream);
            string received = await sr.ReadToEndAsync();

            return received;
        }

        public async Task<string> LikePoem(string poemHashId)
        {

            var request = WebRequest.Create(new Uri("https://poem.alv.in/api/poem/like")) as HttpWebRequest;
            request.Method = "POST";
            request.Accept = "application/json";
            request.ContentLength = poemHashId.Length;

            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            byte[] bytes = encoding.GetBytes(poemHashId);
            using (Stream requestStream = request.GetRequestStream())
            {
                // Send the data.
                requestStream.Write(bytes, 0, bytes.Length);
            }

            WebResponse responseObject = await Task<WebResponse>.Factory.FromAsync(request.BeginGetResponse, request.EndGetResponse, request);
            var responseStream = responseObject.GetResponseStream();
            var sr = new StreamReader(responseStream);
            string received = await sr.ReadToEndAsync();

            return received;


            //HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri("https://poem.alv.in/api/poem/like"));

            //request.Method = "POST";
            //request.ContentType = "text/plain;charset=utf-8";
            //System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            //byte[] bytes = encoding.GetBytes(poemHashId);
            //request.ContentLength = bytes.Length;

            //using (Stream requestStream = request.GetRequestStream())
            //{
            //    // Send the data.
            //    requestStream.Write(bytes, 0, bytes.Length);
            //}


            //request.BeginGetResponse((x) =>
            //{
            //    using (HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(x))
            //    {
            //        response.

            //            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Response));
            //            callback(ser.ReadObject(response.GetResponseStream()) as Response);

            //    }
            //}, null);
            //var request = WebRequest.Create(new Uri("https://poem.alv.in/api/poem/like")) as HttpWebRequest;
            //request.Method = "POST";
            //request.Accept = "application/json";
            //request.
            //WebResponse responseObject = await Task<WebResponse>.Factory.FromAsync(request.BeginGetResponse, request.EndGetResponse, request);
            //var responseStream = responseObject.GetResponseStream();
            //var sr = new StreamReader(responseStream);
            //string received = await sr.ReadToEndAsync();

            return received;
        }
    }
}