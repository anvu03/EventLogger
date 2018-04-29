using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Razor.Generator;
using EventLogger.Models;
using Newtonsoft.Json;

namespace EventLogger.Controllers
{
    [RoutePrefix("api/weasel")]
    public class EventController : ApiController
    {
        private readonly EventLoggerDataContext _context;

        public EventController()
        {
            _context = new EventLoggerDataContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            base.Dispose(disposing);
        }

        [HttpGet]
        [Route("events")]
        public IHttpActionResult GetEvents(int eventTypeId, int rollback = 0)
        {
            return Json((from da in _context.DailyAggregates
                where da.event_type_id == eventTypeId &&
                      ((DateTime) da.created_on).Date >= DateTime.Today.Subtract(new TimeSpan(rollback, 0, 0, 0)).Date
                group da by new {app_id = da.app_id, da.event_type_id}
                into grp
                let count = grp.Sum(x => x.count)
                select new
                {
                    grp.Key.app_id,
                    grp.Key.event_type_id,
                    count,
                    grp.First().app_name,
                    grp.First().event_type_name
                }).ToList());
        }

        [HttpGet]
        [Route("logins")]
        public IHttpActionResult GetLogins(int rollback = 0)
        {
            return Json((from eg in _context.DailyAggregates
                where eg.app_id != null && eg.event_type_id == 8 &&
                      ((DateTime) eg.created_on).Date == DateTime.Today.Date
                select new {app_name = eg.app_name, event_type = eg.event_type_name, count = eg.count}).ToList());
        }

        [HttpGet]
        [Route("failed_logins")]
        public IHttpActionResult GetFailedLogins()
        {
            return Json((from eg in _context.Event_Aggregates
                where eg.App_Id != null && eg.EventType_Id == 9
                select new {app_name = eg.App.Name, event_type = eg.EventType.Name, count = eg.Count}).ToList());
        }

        [HttpGet]
        [Route("password_changes")]
        public IHttpActionResult GetPasswordChanges()
        {
            return Json((from eg in _context.Event_Aggregates
                where eg.App_Id != null && eg.EventType_Id == 11 // password change = 11
                select new {app_name = eg.App.Name, event_type = eg.EventType.Name, count = eg.Count}).ToList());
        }
    }
}