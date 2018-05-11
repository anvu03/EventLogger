using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
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
        private readonly EventLogger.Models.PilotDBEntities _context;

        public EventController()
        {
            _context = new PilotDBEntities();
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
            var begin = DateTime.Today.Subtract(new TimeSpan(rollback, 0, 0, 0));
            return Json((from da in _context.DailyAggregates
                where da.event_type_id == eventTypeId &&
                      EntityFunctions.TruncateTime(da.created_on) >= EntityFunctions.TruncateTime(begin)
                group da by new {app_id = da.app_id, da.event_type_id}
                into grp
                let count = grp.Sum(x => x.count)
                select new
                {
                    grp.Key.app_id,
                    grp.Key.event_type_id,
                    count,
                    grp.FirstOrDefault().app_name,
                    grp.FirstOrDefault().event_type_name
                }).ToList());
        }

        [HttpGet]
        [Route("logins")]
        public IHttpActionResult GetLogins(int rollback = 0)
        {
            return Json((from eg in _context.DailyAggregates
                where eg.app_id != null && eg.event_type_id == 8 &&
                      EntityFunctions.TruncateTime(eg.created_on) == EntityFunctions.TruncateTime(DateTime.Today)
                         select new {app_name = eg.app_name, event_type = eg.event_type_name, count = eg.count}).ToList());
        }
    }
}