using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ankieter.Data;
using Ankieter.IRepo;
using Ankieter.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace Ankieter.Repo
{
    public class QuestionnaireSqlRepo : IQuestionnaireSqlRepo
    {
        private readonly ApplicationDbContext _context;

        public QuestionnaireSqlRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<QuestionnaireSql>> GetAllAsync()
            => await _context.QuestionnaireSqls.ToListAsync();

        public async Task<QuestionnaireSql> GetByIdAsync(int id)
            => await _context.QuestionnaireSqls.SingleOrDefaultAsync(x => x.Id == id);

        public async Task CreateAsync(QuestionnaireSql item)
        {
            await _context.QuestionnaireSqls.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(QuestionnaireSql item)
        {
            _context.QuestionnaireSqls.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var item = await GetByIdAsync(id);
            _context.QuestionnaireSqls.Remove(item);
            await _context.SaveChangesAsync();
        }
    }
}