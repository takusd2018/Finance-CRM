using FakeXrmEasy;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Workflow.ComponentModel;
using Xunit;

namespace PluginUnitTest
{
    public class UnitTest
    {
        private static XrmFakedContext XrmContext { get; set; }    

        private static IOrganizationService OrganizationService { get; set; }


        [Fact]
        public void CreateInstallmentOnCreateOfInvestment()
        {
            XrmContext = new XrmFakedContext { ProxyTypesAssembly = Assembly.GetAssembly(typeof(XrmFakedContext)) };
           
        }
    }
}
