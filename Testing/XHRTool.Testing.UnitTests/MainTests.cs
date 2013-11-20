using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XHRTool.BLL;
using XHRTool.BLL.Common;
using XHRTool.Testing.Service.Models;

namespace XHRTool.Testing.UnitTests
{
    [TestClass]
    public class MainTests
    {
        [TestMethod]
        public void SandboxTest()
        {
            var m = new XHRLogicManager();
            var ret = m.SendXHR(new XHRRequestModel
                {
                    Url = "http://localhost.fiddler:2032/api/values",
                    Verb = HttpMethod.Get
                });

        }

        [TestMethod]
        public void SimpleGet()
        {
            var m = new XHRLogicManager();
            var returnMessage = m.SendXHR(new XHRRequestModel
            {
                Url = "http://localhost.fiddler:2032/api/values",
                Verb = HttpMethod.Get
            });
            Assert.IsNotNull(returnMessage);
            Assert.AreEqual(returnMessage.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(returnMessage.Content);
        }

        [TestMethod]
        public void SimplePost()
        {
            var m = new XHRLogicManager();
            var testModel = new TestPostModel { Value1 = "Test1", Value2 = "Test2" };
            var testModelJson = Json.Encode(testModel);
            var returnMessage = m.SendXHR(new XHRRequestModel
            {
                Url = "http://localhost.fiddler:2032/api/values/PostTest",
                Verb = HttpMethod.Post,
                Content = testModel,
                Headers = new List<HttpHeader>
                { 

                }
            });
            Assert.IsNotNull(returnMessage);
            Assert.AreEqual(returnMessage.StatusCode, HttpStatusCode.OK);
            //Assert.IsNotNull(returnMessage.StatusCode.);
        }
    }
}
