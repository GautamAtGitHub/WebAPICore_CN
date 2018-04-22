using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPICore_CN.Model;

namespace WebAPICore_CN.Controllers
{
    //[Produces("application/json")]
    [Route("api/Todo")]
    public class TodoController : Controller
    {
        private readonly ToDoContext _context;

        public TodoController(ToDoContext context)
        {
            _context = context;

            if (_context.TodoItems.Count() == 0)
            {
                _context.TodoItems.Add(new TodoItem { Id = 101, Name = "Demo Task", IsComplete = false });
                _context.SaveChanges();
            }
        }

        // GET: api/Todo
        [HttpGet]
        public IEnumerable<TodoItem> GetAll()
        {
            return _context.TodoItems.ToList();
        }

        [HttpGet("{id}.{format?}", Name = "GetTodo")]
        public IActionResult GetById(long id)
        {
            var item = _context.TodoItems.FirstOrDefault(t => t.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        // POST: api/Todo
        [HttpPost]
        public void Post([FromBody]TodoItem item)
        {
            if (item != null)
            {
                _context.TodoItems.Add(item);
                _context.SaveChanges();
            }
        }

        // PUT: api/Todo/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var todo = _context.TodoItems.FirstOrDefault(t => t.Id == id);
            if (todo != null)
            {
                _context.TodoItems.Remove(todo);
                _context.SaveChanges();
            }
        }
    }
}
