using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext context;
        private readonly IMapper mapper;
        public UserRepository(DataContext context, IMapper mapper)
        {
            this.mapper = mapper;
            this.context = context;
        }

        public async Task<MemberDto> GetMemberByUsernameAsync(string username)
        {
            var user = await this.context.Users
                .Where(u => u.UserName == username)
                .ProjectTo<MemberDto>(this.mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
            return user;
        }

        public async Task<IEnumerable<MemberDto>> GetMembersAsync()
        {
            var users = await this.context.Users
                .ProjectTo<MemberDto>(this.mapper.ConfigurationProvider)
                .ToListAsync();
            return users;
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await this.context.Users
                .Include(p => p.Photos)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await this.context.Users
                .Include(p => p.Photos)
                .FirstOrDefaultAsync(u => u.UserName == username);
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await this.context.Users
                .Include(p => p.Photos)
                .ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await this.context.SaveChangesAsync() > 0;
        }

        public void Update(AppUser user)
        {
            this.context.Entry(user).State = EntityState.Modified;
        }
    }
}