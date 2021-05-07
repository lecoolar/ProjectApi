using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectApi.Enums;

namespace ProjectApi.Models
{
    public class ProjectItem
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public StatusProject? StatusProject { get; set; }
        public int Priority { get; set; }
        public ICollection<TaskItem> TaskItems { get; set; }
    }
}
