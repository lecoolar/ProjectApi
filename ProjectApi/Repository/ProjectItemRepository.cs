using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectApi.Context;
using ProjectApi.Filters;
using ProjectApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectApi.Repository
{
    public class ProjectItemRepository
    {
        private readonly ProjectTasksContext _context;

        public ProjectItemRepository(ProjectTasksContext context)
        {
            _context = context;
        }

        public async Task<ActionResult<IEnumerable<ProjectItem>>> GetProjectItemByFilters(string sortBy = null,
            string filtersSartdate = null,
            string filterEndDate = null,
            string filterStatusproject = null,
            string filterStartPriority = null,
            string filterEndPriority = null)
        {
            var result = await _context.Projects.Include(p => p.TaskItems).ToListAsync();
            if (!string.IsNullOrEmpty(sortBy))
            {
                try
                {
                    result = ProjectItemsFilters.SortProjectsBy(result, sortBy);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            if (!string.IsNullOrEmpty(filtersSartdate) && !string.IsNullOrEmpty(filterEndDate))
            {
                try
                {
                    result = ProjectItemsFilters.FilterByDate(result, filtersSartdate, filterEndDate);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            if (!string.IsNullOrEmpty(filterStatusproject))
            {
                try
                {
                    result = ProjectItemsFilters.FilterByStatusProject(result, filterStatusproject);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            if (!string.IsNullOrEmpty(filterStartPriority) && !string.IsNullOrEmpty(filterEndPriority))
            {
                try
                {
                    result = ProjectItemsFilters.FilterByPriority(result, filterStartPriority, filterEndPriority);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return result;
        }

        public async Task<ActionResult<ProjectItem>> GetById(long id)
        {
            var projectItem = await _context.Projects.Include(p => p.TaskItems).FirstOrDefaultAsync(t => t.Id == id);

            if (projectItem == null)
            {
                throw new Exception("NotFound");
            }

            return projectItem;
        }

        public async Task<ActionResult<ProjectItem>> AddProjectItem(ProjectItem projectItem)
        {
            try
            {
                _context.Projects.Add(projectItem);
                await _context.SaveChangesAsync();
                return projectItem;
            }
            catch
            {
                throw new Exception("NotAdd");
            }
        }

        public async Task<ActionResult<ProjectItem>> DeleteProjectItem(long id)
        {
            try
            {
                var projectItem = await _context.Projects.FindAsync(id);
                var task = await _context.Tasks.FirstOrDefaultAsync(t => t.ProjectId == id);
                if (projectItem == null)
                {
                    throw new Exception("NotFound");
                }
                _context.Tasks.Remove(task);
                _context.Projects.Remove(projectItem);
                await _context.SaveChangesAsync();
                return projectItem;
            }
            catch
            {
                throw new Exception("NotFound");
            }
        }

        public async Task<ActionResult<ProjectItem>> ReplaceProjectItem(long id, ProjectItem projectItem)
        {
            if (id != projectItem.Id)
            {
                throw new Exception("Incorrect Id");
            }

            _context.Entry(projectItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return projectItem;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectItemExists(id))
                {
                    throw new Exception("NotFound");
                }
                else
                {
                    throw;
                }
            }
        }
        private bool ProjectItemExists(long id)
        {
            return _context.Projects.Any(e => e.Id == id);
        }
    }
}
