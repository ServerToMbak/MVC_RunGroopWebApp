﻿using Microsoft.EntityFrameworkCore;
using RunGroopWebApp.Data;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.Models;

namespace RunGroopWebApp.Repository
{
    public class ClubRepoistory : IClubRepository
    {
        private readonly ApplicationDbContext _context;

        public ClubRepoistory(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Add(Club club)
        {
            _context.Add(club);
            return Save();
        }

        public bool Delete(Club club)
        {
            _context.Remove(club);
            return Save();
        }

        public async Task<IEnumerable<Club>> GetAll()
        {
           return  await _context.Clubs.ToListAsync();
        }

        public async Task<Club> GetByIdAsync(int id)
        {
            return await _context.Clubs.Include(opt => opt.Address).FirstOrDefaultAsync(opt => opt.Id == id);
        }
        public async Task<Club> GetByIdAsyncNoTracking(int id)
        {
            return await _context.Clubs.Include(opt => opt.Address).AsNoTracking().FirstOrDefaultAsync(opt => opt.Id == id);
        }

        public async Task<IEnumerable<Club>> GetClubsByCity(string city)
        {
            return await _context.Clubs./*Include(opt => opt.Address).*/Where(opt => opt.Address.City.Contains(city)).ToListAsync();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Club club)
        {
            _context.Update(club);
            return Save();
        }
    }
}
