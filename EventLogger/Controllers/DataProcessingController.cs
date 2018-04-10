using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                        Name = (string) olEventType["name"]
                    };

                    return Ok(et);
                }

                foreach (var olEvent in olEvents)
                {
                    var e = new Event()
                    {
                        CreatedAt = Convert.ToDateTime(olEvent["created_at"]),
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
            using (var context = new EventLoggerDataContext())
            {
                foreach (var olEventType in olEventTypes)
                {
                    var eventType = new EventType
                    {
                        Id = (int) olEventType["id"],
                        Name = (string) olEventType["name"]
                    };
                    eventTypes.Add(eventType);


                    try
                    {
                        context.EventTypes.InsertOnSubmit(eventType);
                        context.SubmitChanges();
                    }
                    catch (Exception e)
                    {
                        return Ok(olEventType);
                    }
                }
            }


            return Ok();
        }

        [Route("InsertEvents")]
        [HttpGet]
        public IHttpActionResult InsertEvents()
        {
            var context = new EventLoggerDataContext();
            var olEvents = OneLogin.Client.GetEvents();
            var events = new List<Event>();
            // update event types if there's a new one

            foreach (var olEvent in olEvents)
            {
                var appId = olEvent["app_id"].ToObject(typeof(int?));
                var appName = (string) olEvent["app_name"];

                // if there's an app
                if (appId != null)
                {
                    // check with database
                    if (context.Apps.FirstOrDefault(app => app.Id == (int) appId) == null)
                    {
                        // create new app object
                        var newApp = new App()
                        {
                            Id = (int) appId,
                            Name = appName
                        };
                        // insert into context
                        context.Apps.InsertOnSubmit(newApp);
                        // save changes to database
                        context.SubmitChanges();
                    }
                }

                var newEvent = new Event()
                {
                    Id = (int) olEvent["id"],
                    App_Id = (int?) olEvent["app_id"],
                    EventType_Id = (int) olEvent["event_type_id"],
                    CreatedAt = Convert.ToDateTime(olEvent["created_at"])
                };

                try
                {
                    context.Events.InsertOnSubmit(newEvent);
                    context.SubmitChanges();
                    events.Add(newEvent);
                }
                catch (Exception e)
                {
                }
            }

            // compare with database

            // if different then insert new app

            // insert events

            context.Dispose();
            return Ok(events.Count);
        }
    }
}