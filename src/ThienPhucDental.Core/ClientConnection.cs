using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using ThienPhucDental.Configuration;
using ThienPhucDental.Helper;

namespace ThienPhucDental
{
    public interface IClientConnection
    {
        Task<string> GetConnectionString();
    }

    public class ClientConnection : IClientConnection
    {
        private static string _connectionStringResult;
        private readonly string _clientRequest;
        private readonly string _clientRequestContent;
        private readonly bool _isRequestServer;
        private readonly string _localConnectionName;
        private readonly bool _isBypassCert;
        private string localConnectionString;
        private readonly IDetailLoggerHelper detailLoggerHelper;


        public ClientConnection(IWebHostEnvironment env, IDetailLoggerHelper detailLoggerHelper, string localConnectionName = "Default", string uamClientRequestContentKey = "uam:ClientRequestContent")
        {
            this.detailLoggerHelper = detailLoggerHelper;
            var appConfiguration = env.GetAppConfiguration();
            _clientRequest = appConfiguration["App:uam_ClientRequestUrl"];
            this.detailLoggerHelper.StartLog("App:uam_ClientRequestUrl:" + _clientRequest);
            _clientRequestContent = appConfiguration["App:uam_ClientRequestContent"];
            this.detailLoggerHelper = detailLoggerHelper;
            this.detailLoggerHelper.StartLog("App:uam_ClientRequestContent:" + _clientRequestContent);

            var strValue = appConfiguration["App:uam_LocalConnection"];
            if (strValue == "false")
            {
                _isRequestServer = true;
            }
            this.detailLoggerHelper.StartLog("App:uam_LocalConnection:" + strValue);

            strValue = appConfiguration["App:uam_ByPassCert"];
            if (strValue == "true")
            {
                _isBypassCert = true;
            }
            _localConnectionName = localConnectionName;
            this.detailLoggerHelper.StartLog("App:uam_ByPassCert:" + strValue);

            localConnectionString = env.GetAppConfiguration().GetConnectionString("Default");
        }

        public async Task<string> GetConnectionString()
        {
            if (!string.IsNullOrEmpty(_connectionStringResult))
            {
                return _connectionStringResult;
            }

            if (_isRequestServer)
            {
                try
                {
                    using (var client = new HttpClient())
                    {
                        if (_isBypassCert)
                        {
                            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                        }

                        var content = new StringContent(_clientRequestContent, Encoding.UTF8, "application/x-www-form-urlencoded");

                        var post = await client.PostAsync(_clientRequest, content, new CancellationToken(false));

                        var resultContent = await post.Content.ReadAsStringAsync();
                        this.detailLoggerHelper.StartLog("Result:" + resultContent);

                        var xdoc = new XmlDocument();
                        xdoc.LoadXml(resultContent);
                        var resultNode = xdoc.SelectSingleNode("//response//result");
                        var contentNode = xdoc.SelectSingleNode("//response//content");
                        if (resultNode == null || contentNode == null)
                        {
                            return resultContent;
                        }

                        if (resultNode.InnerText == "000")
                            _connectionStringResult = contentNode.InnerText;
                        return _connectionStringResult;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return localConnectionString;


            //if (string.IsNullOrEmpty(_localConnectionName))
            //         {
            //             return string.Empty;
            //         }

            //         var strValue = ConfigurationManager.ConnectionStrings[_localConnectionName];
            //         _connectionStringResult = strValue.ConnectionString;
            //         return _connectionStringResult;
        }
    }
}
