using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly DataContext context;
        public UsersController(DataContext context)
        {
            this.context = context;

        }

        [HttpGet]
        public async Task<IEnumerable<AppUser>> GetUsers() 
        {
            var users = await this.context.Users.ToListAsync();
            return users;
        }

        [HttpGet("{id}")]
        public async Task<AppUser> GetUser(int id) 
        {
            var user = await this.context.Users.FirstOrDefaultAsync(user => user.Id == id);
            return user;
        }
    }
}