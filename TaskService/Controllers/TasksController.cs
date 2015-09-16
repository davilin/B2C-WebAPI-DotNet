﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using TaskService.DAL;

namespace TaskService.Controllers
{
    // TODO: Secure the actions in this controller
    public class TasksController : ApiController
    {
        private TasksServiceContext db = new TasksServiceContext();

        public IEnumerable<Models.Task> Get()
        {
            // TODO: Use claims to get the tasks for the calling user.
        }

        public void Post(Models.Task task)
        {
            if (task.task == null || task.task == string.Empty)
                throw new WebException("Please provide a task description");

            string owner = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            task.owner = owner;
            task.completed = false;
            task.date = DateTime.UtcNow;
            db.Tasks.Add(task);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            string owner = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            Models.Task task = db.Tasks.Where(t => t.owner.Equals(owner) && t.TaskID.Equals(id)).FirstOrDefault();
            db.Tasks.Remove(task);
            db.SaveChanges();
        }
    }
}
