using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EventLogger.Models;

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

        public IHttpActionResult GetAllEvents()
        {
            var events = this._context.Events.ToList();
            return Ok(events);
        }
    }
}
