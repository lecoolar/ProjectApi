using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectApi.Context;
using ProjectApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectApi.Repository
{
    public class TaskItemRepository
    {
        private readonly ProjectTasksContext _context;

        public TaskItemRepository(ProjectTasksContext context)
        {
            _context = context;
        }

        public async Task<ActionResult<IEnumerable<TaskItem>>> GetTaskItems()
        {
            try
            {
                return await _context.Tasks.Include(t => t.Project).ToListAsync();
            }
            catch
            {
                throw new Exception("Error");
            }
        }

        public async Task<ActionResult<TaskItem>> GetById(long id)
        {
            var taskItem = await _context.Tasks.Include(t => t.Project).FirstOrDefaultAsync(t => t.Id == id);

            if (taskItem == null)
            {
                throw new Exception("NotFound");
            }

            return taskItem;
        }
        public async Task<ActionResult<TaskItem>> AddTaskItem(TaskItem taskItem)
        {
            try
            {
                _context.Tasks.Add(taskItem);
                await _context.SaveChangesAsync();
                return taskItem;
            }
            catch
            {
                throw new Exception("NotAdd");
            }
        }

        public async Task<ActionResult<TaskItem>> DeleteTaskItem(long id)
        {
            try
            {
                var taskItem = await _context.Tasks.FindAsync(id);
                if (taskItem == null)
                {
                    throw new Exception("NotFound");
                }
                _context.Tasks.Remove(taskItem);
                await _context.SaveChangesAsync();
                return taskItem;
            }
            catch
            {
                throw new Exception("NotFound");
            }
        }

        public async Task<ActionResult<TaskItem>> ReplaceTaskItem(long id, TaskItem taskItem)
        {
            if (id != taskItem.Id)
            {
                throw new Exception("Incorrect Id");
            }
            _context.Entry(taskItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return taskItem;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskItemExists(id))
                {
                    throw new Exception("NotFound");
                }
                else
                {
                    throw;
                }
            }
        }

        private bool TaskItemExists(long id)
        {
            return _context.Tasks.Any(e => e.Id == id);
        }
    }
}
