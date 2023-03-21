using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
namespace PracticePlugin
{
    public class HelloWorld : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {// Obtain the tracing service
         // Extract the tracing service for use in debugging sandboxed plug-ins.  
         // If you are not registering the plug-in in the sandbox, then you do  
         // not have to add any tracing service related code.  
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));             // Obtain the execution context from the service provider.  
            IPluginExecutionContext context = (IPluginExecutionContext)
             serviceProvider.GetService(typeof(IPluginExecutionContext));             // Obtain the organization service reference which you will need for  
            // web service calls.  
            IOrganizationServiceFactory serviceFactory =
            (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);              // The InputParameters collection contains all the data passed in the message request.  
            if (context.InputParameters.Contains("Target") &&
            context.InputParameters["Target"] is Entity)
            {
                // Obtain the target entity from the input parameters.  
                Entity investmentEntity = (Entity)context.InputParameters["Target"];
                try
                {
                    
                    // Plug-in business logic goes here.                     
                    OptionSetValue investmentTermField = investmentEntity.GetAttributeValue<OptionSetValue>("new_investment");
                    if (investmentTermField == null) return;
                    int investmentTerm = investmentTermField.Value;
                    Money amount = (Money)investmentEntity.Attributes["new_amount"];

                    if(amount==null) return;
                    decimal amountPerInvest = amount.Value / investmentTerm;
                    Money amountTotal = new Money(amountPerInvest); 
                    DateTime currentDate= DateTime.Now;


                    for (int i = 0; i < investmentTerm; i++)
                    {
                        Entity EMIEntity = new Entity("new_emi");
                        EMIEntity["new_name"] = "EMI -" + i;
                        EMIEntity["new_parentinvestment"] = new EntityReference("new_investment", investmentEntity.Id);
                        EMIEntity["new_amount"] = amountTotal;
                        EMIEntity["new_duedate"] = currentDate;
                        currentDate = currentDate.AddMonths(1);
                        Guid guid = service.Create(EMIEntity);
                    }
                }
                catch (FaultException<OrganizationServiceFault> ex)
                {
                    throw new InvalidPluginExecutionException("An error occurred in MyPlug-in.", ex);
                }
                catch (Exception ex)
                {
                    tracingService.Trace("MyPlugin: {0}", ex.ToString());
                    throw;
                }
            }
        }
    }
}
