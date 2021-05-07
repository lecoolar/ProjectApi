using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjectApi.Enums;

namespace ProjectApi.Models
{
    public class TaskItem
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public StatusTask? StatusTask { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }
        public long? ProjectId { get; set; }
        public ProjectItem Project { get; set; }
    }
}
