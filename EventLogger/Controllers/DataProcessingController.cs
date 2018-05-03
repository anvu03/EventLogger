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
            var events = new List<Event>();

            var eventTypes = oneLoginClient.GetEventTypes();

            foreach (var eventType in eventTypes)
                try
                {
                    context.EventTypes.InsertOnSubmit(new EventType()
                    {
                        Id = (int) eventType["id"],
                        Name = (string) eventType["name"],
                    });
                    context.SubmitChanges();
                }
                catch (System.Data.Linq.DuplicateKeyException)
                {
                }
                catch (System.Data.SqlClient.SqlException)
                {
                }

            // list of events to report 
            var reportableEventTypes = context.EventTypes.Where(e => e.Reportable).ToList();

            foreach (var eventType in reportableEventTypes)
            {
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
                            try
                            {
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
                            catch (System.Data.SqlClient.SqlException e)
                            {
                            }
                            catch (System.Data.Linq.DuplicateKeyException e)
                            {
                            }
                        }

                        var newEvent = new Event()
                        {
                            Id = (long) olEvent["id"],
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
                        catch (System.Data.SqlClient.SqlException e)
                        {
                        }
                        catch (System.Data.Linq.DuplicateKeyException e)
                        {
                        }

                    }

                    if (afterCursor == null)
                        break;
                }
            }

            context.Dispose();
            return Ok(events.Count);
        }
    }
}