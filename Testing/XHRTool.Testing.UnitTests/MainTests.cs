using System;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XHRTool.BLL;
using XHRTool.BLL.Common;

namespace XHRTool.Testing.UnitTests
{
    [TestClass]
    public class MainTests
    {
        [TestMethod]
        public void SandboxTest()
        {
            var m = new XHRLogicManager();
            var ret = m.SendXHR(new XHRRequestModel()
                {
                    Url = "http://localhost.fiddler:2032/api/values",
                    Verb = HttpMethod.Get
                });

        }
    }
}
