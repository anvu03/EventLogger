using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using EventLogger.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EventLogger.Controllers
{
    [RoutePrefix("api/DataProcessing")]
    public class DataProcessingController : ApiController
    {
        [Route("run")]
        [HttpGet]
        public IHttpActionResult Run()
        {
            var olEvents = OneLogin.Client.GetEvents();
            var olEventTypes = OneLogin.Client.GetEventTypes();

            using (var context = new EventLoggerDataContext())
            {
                foreach (var olEventType in olEventTypes)
                {
                    var et = new EventType()
                    {
                        OL_Id = (int) olEventType["id"],
                        Name = (string) olEventType["name"]
                    };

                    return Ok(et);
                }
                foreach (var olEvent in olEvents)
                {
                    var e = new Event()
                    {
                        CreatedAt = Convert.ToDateTime(olEvent["created_at"]),
                        OL_Id = (int) olEvent["id"]
                    };

                    return Ok(e);
                }
            }

            return Ok(OneLogin.Client.GetEvents());
        }

        [Route("UpdateEventTypes")]
        [HttpGet]
        public IHttpActionResult UpdateEventTypes()
        {
            var olEventTypes = OneLogin.Client.GetEventTypes();
            var eventTypes = new List<EventType>();

            foreach (var olEventType in olEventTypes)
            {
                eventTypes.Add(new EventType
                {
                    OL_Id = (int) olEventType["id"],
                    Name = (string) olEventType["name"]
                });
            }

            using (var context = new EventLoggerDataContext())
            {
                context.EventTypes.InsertAllOnSubmit(eventTypes);
                context.SubmitChanges();
            }

            return Ok(olEventTypes.Count());
        }
    }
}