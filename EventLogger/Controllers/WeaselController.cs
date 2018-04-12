﻿using System;
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
        [Route("logins")]
        public IHttpActionResult GetLogins()
        {
            return Json((from eg in _context.Event_Aggregates
                where eg.App_Id != null && eg.EventType_Id == 8
                select new {app_name = eg.App.Name, event_type = eg.EventType.Name, count = eg.Count}).ToList());
        }

        [HttpGet]
        [Route("failed_logins")]
        public IHttpActionResult GetFailedLogins()
        {
            return Json((from eg in _context.Event_Aggregates
                where eg.App_Id != null && eg.EventType_Id == 9
                select new { app_name = eg.App.Name, event_type = eg.EventType.Name, count = eg.Count }).ToList());
        }

        [HttpGet]
        [Route("password_changes")]
        public IHttpActionResult GetPasswordChanges()
        {
            return Json((from eg in _context.Event_Aggregates
                where eg.App_Id != null && eg.EventType_Id == 11 // password change = 11
                select new { app_name = eg.App.Name, event_type = eg.EventType.Name, count = eg.Count }).ToList());
        }
    }
}