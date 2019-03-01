using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TransformFunctions
{
    public static class HL7toJSON
    {
        
        [FunctionName("HL7toJSON")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HL7toJSON HTTP trigger function fired");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            try
            {
                var metadata = HL7MetaDataLoader.Instance.GetMetaDataFromMessage(requestBody);
                JObject o = HL7ToXmlConverter.ConvertToJObject(requestBody,metadata);
                return new JsonResult(o["hl7message"]);


            } catch (Exception e)
            {
                log.LogError(e, e.Message);
                return new BadRequestObjectResult("Error: " + e.Message);
            }
            
        }
    }
}
