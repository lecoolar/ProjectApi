using ProjectApi.Enums;
using ProjectApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectApi.Filters
{

    public static class ProjectItemsFilters
    {
        private const string Id = "id";
        private const string Name = "name";
        private const string StartDate = "startdate";
        private const string CompletionDate = "completiondate";
        private const string StatusProject = "statusproject";
        private const string Priority = "priority";
        private const string TaskItems = "taskitems";


        public static List<ProjectItem> SortProjectsBy(IEnumerable<ProjectItem> projects, string sortBy)
        {
            switch (sortBy.ToLower())
            {
                case Id:
                    projects = projects.OrderBy(e => e.Id);
                    break;
                case Name:
                    projects = projects.OrderBy(e => e.Name);
                    break;
                case StartDate:
                    projects = projects.OrderBy(e => e.StartDate);
                    break;
                case CompletionDate:
                    projects = projects.OrderBy(e => e.CompletionDate);
                    break;
                case StatusProject:
                    projects = projects.OrderBy(e => e.StatusProject);
                    break;
                case Priority:
                    projects = projects.OrderBy(e => e.Priority);
                    break;
                case TaskItems:
                    projects = projects.OrderBy(e => e.TaskItems);
                    break;
                default:
                    throw new Exception("Incorrect Sort Filters");

            }
            return projects.ToList();
        }

        public static List<ProjectItem> FilterByDate(IEnumerable<ProjectItem> projects, string filtersSartdate, string filterEndDate)
        {
            if (DateTime.TryParse(filtersSartdate, out var startDate)
                && DateTime.TryParse(filterEndDate, out var endDate))
            {
                if (startDate > endDate)
                {
                    throw new Exception("Start date cannot be more that end date");
                }
                else
                {
                    projects = projects.Where(e => e.StartDate >= startDate && e.StartDate <= endDate);
                }
            }
            else
            {
                throw new Exception("Incorrect dates");
            }
            return projects.ToList();
        }

        public static List<ProjectItem> FilterByStatusProject(IEnumerable<ProjectItem> projects, string filterStatusproject)
        {
            if (Enum.TryParse<StatusProject>(filterStatusproject, true, out var statusProject))
            {
                projects = projects
                    .Where(e => e.StatusProject == statusProject);
            }
            else
            {
                throw new Exception("Incorrect Status");
            }
            return projects.ToList();
        }

        public static List<ProjectItem> FilterByPriority(IEnumerable<ProjectItem> projects, string filterStartPriority, string filterEndPriority)
        {
            if (int.TryParse(filterStartPriority, out var startPriority)
                    && int.TryParse(filterEndPriority, out var endPriority))
            {
                if (startPriority < endPriority)
                {
                    throw new Exception("Start priority cannot be more that end priority");
                }
                else
                {
                    projects = projects.Where(e => e.Priority >= startPriority && e.Priority <= endPriority);
                }
            }
            return projects.ToList();
        }
    }
}
