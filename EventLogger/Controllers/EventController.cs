using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using EventLogger.Models;
using Newtonsoft.Json;

namespace EventLogger.Controllers
{
    [RoutePrefix("api/events")]
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
        [Route("")]
        public JsonResult<List<object>> GetAllEvents()
        {
            var result = new List<object>();
            var events = this._context.Events.ToList();
            events.ForEach(e => result.Add(new {id = e.Id, event_type = e.EventType.Name}));
            return Json(result);
        }
    }
}