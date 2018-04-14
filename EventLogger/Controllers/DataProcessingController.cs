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
        [Route("UpdateEventTypes")]
        [HttpGet]
        public IHttpActionResult UpdateEventTypes()
        {
            var oneLoginClient = new OneLogin.Client();
            var olEventTypes = oneLoginClient.GetEventTypes();
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

        /// <summary>
        /// This method will be called by the scheduled windows service at 0:01 - 3:00
        /// It will retrieve all events created from 0:00 - 23:59:59 of yesterday
        /// </summary>
        /// <returns></returns>
        [Route("InsertEvents")]
        [HttpGet]
        public IHttpActionResult InsertEvents()
        {
            var context = new EventLoggerDataContext();
            var oneLoginClient = new OneLogin.Client();

            // update event types 

            var eventTypes = oneLoginClient.GetEventTypes();

            foreach (var eventType in eventTypes)
                if (context.EventTypes.FirstOrDefault(e => e.Id == (int) eventType["id"]) == null)
                {
                    context.EventTypes.InsertOnSubmit(new EventType()
                    {
                        Id = (int)eventType["id"],
                        Name = (string)eventType["name"],
                    });
                    context.SubmitChanges();
                }

            // list of events to report 
            var reportableEventTypes = context.EventTypes.Where(e => e.Reportable).ToList();

            foreach (var eventType in reportableEventTypes)
            {
                var events = new List<Event>();
                // update event types if there's a new one

                string afterCursor = "";

                while (true)
                {
                    var response = oneLoginClient.GetEvents(
                        eventTypeId: eventType.Id,
                        since: DateTime.Today.Subtract(new TimeSpan(500, 0, 0)),
                        until: DateTime.Today.Add(new TimeSpan(23, 59, 59)),
                        afterCursor: afterCursor);

                    var olEvents = response["data"];
                    afterCursor = (string) response["pagination"]["after_cursor"];

                    if (!olEvents.Any())
                    {
                        break;
                    }

                    foreach (var olEvent in olEvents)
                    {
                        var appId = olEvent["app_id"].ToObject(typeof(int?));
                        var appName = (string) olEvent["app_name"];

                        // insert a new app into database if there's one 
                        if (appId != null) // if the event references to an app
                        {
                            // does the database already have this app? 
                            if (context.Apps.FirstOrDefault(app => app.Id == (int) appId) == null)
                            {
                                // if so, insert this new app into the database
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
                            Id = (long) olEvent["id"],
                            App_Id = (int?) olEvent["app_id"],
                            EventType_Id = (int) olEvent["event_type_id"],
                            CreatedAt = Convert.ToDateTime(olEvent["created_at"])
                        };
                        if (context.Events.FirstOrDefault(e => e.Id == newEvent.Id) == null)
                        {
                            context.Events.InsertOnSubmit(newEvent);
                            context.SubmitChanges();
                            events.Add(newEvent);
                        }
                    }

                    if (afterCursor == null)
                        break;
                }
            }


            // aggregate

            var result = (from e in context.Events
//                where e.CreatedAt > new DateTime() && e.CreatedAt < new DateTime()
                group e by new {e.EventType_Id, e.App_Id}
                into g
                select new
                {
                    g.Key.EventType_Id,
                    g.Key.App_Id,
                    count = g.Count()
                }).ToList();


            // insert aggregate into database
            // TODO prevent inserting twice
            foreach (var r in result)
            {
                var instance = context.Event_Aggregates.FirstOrDefault(
                    eg => (eg.App_Id == r.App_Id || (eg.App_Id == null && r.App_Id == null)) &&
                          eg.EventType_Id == r.EventType_Id);
                if (instance == null)
                {
                    context.Event_Aggregates.InsertOnSubmit(new Event_Aggregate()
                    {
                        Date = DateTime.Today,
                        App_Id = r.App_Id,
                        EventType_Id = r.EventType_Id,
                        Count = r.count
                    });
                    context.SubmitChanges();
                }
                else
                {
                    // update count
                    instance.Count = r.count;
                    context.SubmitChanges();
                }
            }


            context.Dispose();
            return Json(result);
        }
    }
}