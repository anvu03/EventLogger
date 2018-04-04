using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using EventLogger.Models;
using Event = EventLogger.Models.Event;
using EventType = EventLogger.Models.EventType;

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

        [Route("InsertEvents")]
        [HttpGet]
        public IHttpActionResult InsertEvents()
        {
            var olEvents = OneLogin.Client.GetEvents();
            var apps = new Dictionary<int, EventLogger.Models.App>();
            // update event types if there's a new one
            foreach (var olEvent in olEvents)
            {
                var appId = olEvent["app_id"].ToObject(typeof(int?));
                var appName = (string) olEvent["app_name"];

                if (appId != null && apps.ContainsKey((int) appId))
                {
                    apps.Add((int) appId, new EventLogger.Models.App()
                    {
                        Name = appName,
                        OL_Id = (int) appId,
                    });
                }

                return Ok(apps.Count);
            }

            return Ok("not null");
        }
    }
}