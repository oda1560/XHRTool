using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using XHRTool.XHRLogic.Common;

namespace XHRTool.XHRLogic
{
    public class XHRLogicManager
    {
        public XHRResponseModel SendXHR(XHRRequestModel requestModel)
        {
            try
            {
                var requestUri = new Uri(requestModel.Url, UriKind.Absolute);
                var client = new TcpClient
                {
                    ReceiveTimeout = 60000
                };
                client.Connect(requestUri.Host, requestUri.Port);
                var httpRequest = new StringBuilder();

                // Request header
                httpRequest.AppendFormat("{0} {1} HTTP/1.1{2}", requestModel.Verb, requestUri.AbsolutePath + requestUri.Query, Environment.NewLine);

                // Other HTTP headers
                var hostHeader = requestModel.Headers.SingleOrDefault(header => header.Name.Equals("host", StringComparison.OrdinalIgnoreCase));
                if (hostHeader == null)
                {
                    requestModel.Headers.Add(new HttpHeader("Host", requestUri.Host));
                }
                if (requestModel.HasContent)
                {
                    var contentLengthHeader = requestModel.Headers.SingleOrDefault(header => header.Name.Equals("Content-Length", StringComparison.OrdinalIgnoreCase));
                    if (contentLengthHeader != null)
                    {
                        contentLengthHeader.Value = requestModel.Content.Length.ToString();
                    }
                    else
                    {
                        requestModel.Headers.Add(new HttpHeader("Content-Length", requestModel.Content.Length.ToString()));
                    }
                }
                requestModel.Headers.ForEach(h => httpRequest.AppendFormat("{0}: {1}{2}", h.Name, h.Value, Environment.NewLine));
                httpRequest.AppendLine();

                // Content
                if (requestModel.HasContent)
                {
                    httpRequest.AppendLine(requestModel.Content);
                }
                httpRequest.AppendLine();

                // Sending request
                var request = Encoding.Default.GetBytes(httpRequest.ToString());
                var netStream = client.GetStream();
                netStream.Write(request, 0, request.Length);
                netStream.Flush();

                // Parsing response
                var streamReader = new StreamReader(netStream);
                var contentLength = 0;
                var responseMessage = new StringBuilder();
                while (true)
                {
                    var line = streamReader.ReadLine();
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        break;
                    }
                    responseMessage.AppendLine(line);
                    if (line.StartsWith("content-length", StringComparison.OrdinalIgnoreCase))
                    {
                        contentLength = int.Parse(line.Substring(line.IndexOf(':') + 1, line.Length - line.IndexOf(':') - 1));
                    }
                    else if (line == Environment.NewLine)
                    {
                        break;
                    }
                }
                if (contentLength != 0)
                {
                    var buffer = new char[contentLength];
                    streamReader.Read(buffer, 0, buffer.Length);
                    responseMessage.AppendLine();
                    responseMessage.Append(new string(buffer));
                }
                netStream.Close();
                netStream.Dispose();
                client.Close();
                var responseMessageString = responseMessage.ToString();
                return parseResponseString(responseMessageString);
            }
            catch (Exception ex)
            {
                return new XHRResponseModel
                {
                    Content = ex.ToString()
                };
            }
        }

        XHRResponseModel parseResponseString(string responseString)
        {
            var lines = responseString.Split(new [] { Environment.NewLine }, StringSplitOptions.None).ToList();
            var code = int.Parse(Regex.Match(lines[0], " [0-9]{3} ").Value);
            var message = Regex.Match(lines[0], " [0-9]{3} (.+)", RegexOptions.IgnoreCase).Groups[1].Value;

            var headerStrings = new List<string>();
            int idx;
            for (idx = 1; idx < lines.Count; idx++)
            {
                if (lines[idx] == string.Empty)
                {
                    break;
                }
                headerStrings.Add(lines[idx]);
            }

            var content = string.Join(string.Empty, lines.Skip(idx));
            var headers = new List<HttpHeader>();
            headerStrings.ForEach(hs => 
                {
                    var match = Regex.Match(hs, "(.+): (.+)");
                    var name = match.Groups[1].Value;
                    var value = match.Groups[2].Value;
                    headers.Add(new HttpHeader(name, value));
                });
            var result = new XHRResponseModel
                {
                    StatusCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), code.ToString()),
                    HttpHeaders = headers,
                    StatusMessage = message,
                    Content = content
                };
            
            return result; 
        }







        [Obsolete]
        public XHRResponseModel SendXHRUsingWebClient(XHRRequestModel requestModel)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            var message = new HttpRequestMessage
            {
                Method = requestModel.Verb,
                RequestUri = new Uri(requestModel.Url, UriKind.Absolute)
            };
            if (requestModel.Content != null)
            {
                message.Content = new StringContent(requestModel.Content, Encoding.Default, "application/json");
            }
            requestModel.Headers.ForEach(h => message.Headers.TryAddWithoutValidation(h.Name, h.Value));

            try
            {
                var result = client.SendAsync(message).Result;
                var response = new XHRResponseModel
                {
                    Content = result.Content.ReadAsStringAsync().Result,
                    StatusCode = result.StatusCode,
                    HttpHeaders = result.Headers.Select(h => new HttpHeader(h.Key, h.Value.FirstOrDefault())).ToList()
                };
                return response;
            }
            catch (Exception ex)
            {
                return new XHRResponseModel
                {
                    Content = ex.ToString()
                };
            }
        }
    }
}
