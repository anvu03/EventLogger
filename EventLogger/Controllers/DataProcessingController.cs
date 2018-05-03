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
        private EventLoggerDataContext _context;
        public DataProcessingController()
        {
            _context = new EventLoggerDataContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
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
            var oneLoginClient = new OneLogin.Client();

            var events = new List<Event>();

            // update event types 
            var eventTypes = oneLoginClient.GetEventTypes();
            foreach (var eventType in eventTypes)
                try
                {
                    _context.EventTypes.InsertOnSubmit(new EventType()
                    {
                        Id = (int) eventType["id"],
                        Name = (string) eventType["name"],
                    });
                    _context.SubmitChanges();
                }
                catch (System.Data.Linq.DuplicateKeyException)
                {
                }
                catch (System.Data.SqlClient.SqlException)
                {
                }

            // list of events to report 
            var reportableEventTypes = _context.EventTypes.Where(e => e.Reportable).ToList();

            foreach (var eventType in reportableEventTypes)
            {
                // update event types if there's a new one

                string afterCursor = "";

                while (true)
                {
                    // get all events occured yesterday for a particular event_id 
                    var response = oneLoginClient.GetEvents(
                        eventTypeId: eventType.Id,
                        since: DateTime.Now.AddDays(-1),
                        until: DateTime.Now,
                        afterCursor: afterCursor);

                    var eventsRetrived = response["data"];
                    afterCursor = (string) response["pagination"]["after_cursor"];

                    // if no events, break the loop and go to the end of function
                    if (!eventsRetrived.Any())
                    {
                        break;
                    }

                    foreach (var _event in eventsRetrived)
                    {
                        var appId = _event["app_id"].ToObject(typeof(int?));
                        var appName = (string) _event["app_name"];

                        // if there's a new app, insert it into the database
                        if (appId != null) // if the event references to an app
                            try
                            {
                                var newApp = new App()
                                {
                                    Id = (int) appId,
                                    Name = appName
                                };
                                _context.Apps.InsertOnSubmit(newApp);
                                _context.SubmitChanges();
                            }
                            catch (System.Data.SqlClient.SqlException e)
                            {
                            }
                            catch (System.Data.Linq.DuplicateKeyException e)
                            {
                            }

                        // create a container
                        var newEvent = new Event()
                        {
                            Id = (long) _event["id"],
                            App_Id = (int?) _event["app_id"],
                            EventType_Id = (int) _event["event_type_id"],
                            CreatedAt = Convert.ToDateTime(_event["created_at"])
                        };

                        // insert this event into database
                        try
                        {
                            _context.Events.InsertOnSubmit(newEvent);
                            events.Add(newEvent); // for keeping track only
                            _context.SubmitChanges();
                        }
                        catch (System.Data.SqlClient.SqlException e)
                        {
                            return Ok(newEvent);
                        }
                        catch (System.Data.Linq.DuplicateKeyException e)
                        {
                            return Ok(newEvent);
                        }
                    }
                    // break if there's no nextpage
                    if (afterCursor == null)
                        break;
                }
            }

            return Ok(events);
        }
    }
}