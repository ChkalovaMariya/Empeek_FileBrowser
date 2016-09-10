using FileBrowser_SPA.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace FileBrowser_SPA.Controllers
{
    public class WebApiController : ApiController
    {
        // GET api/<controller>
        private DataManager dataManager;

        public WebApiController()
            : this(new DataManager())
        {
        }

        public WebApiController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }

        [Route("api/FileSystemEntries/{*directory}")]
        [HttpGet]
        public async Task<HttpResponseMessage> Get(string directory)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            string dir = directory;

            try
            {
                var results = await dataManager.Get(dir);
                if (!dir.Equals("drives"))
                {
                    var counts = await dataManager.GetCount(dir);
                    response = Request.CreateResponse(HttpStatusCode.OK, new { entities = results, countMin = counts[0], countMiddle = counts[1], countMax = counts[2] });
                }
                else
                {
                    response = Request.CreateResponse(HttpStatusCode.OK, new { entities = results, countMin = "", countMiddle = "", countMax = "" });
                }
            }
            catch (ArgumentException e)
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest, new { });

            }
            catch (Exception e)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, new { });
            }
            finally
            {
                response.Headers.Add("Access-Control-Allow-Origin", "*");
            }   
            return response;
        }
    }
}