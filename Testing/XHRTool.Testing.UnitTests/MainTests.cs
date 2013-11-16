using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XHRTool.BLL;

namespace XHRTool.Testing.UnitTests
{
    [TestClass]
    public class MainTests
    {
        [TestMethod]
        public void SandboxTest()
        {
            var m = new XHRLogicManager();
            m.Send();

        }
    }
}
