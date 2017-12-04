using System;
using System.Threading.Tasks;
using Ankieter.Data;
using Ankieter.IRepo;
using Ankieter.Models;
using Ankieter.Models.Forms;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace Ankieter.Services
{
    public class FormService : IFormService
    {
        private readonly ApplicationDbContext _context;

        public FormService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateForm(CreatedForm form)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    await _context.CreatedForm.AddAsync(form);
                    await _context.SaveChangesAsync();

                    await _context.Questionnaires.InsertOneAsync(new Questionnaire()
                    {
                        Id = ObjectId.Parse(form.Id.ToString().PadLeft(24, '0')),
                        Name = form.Name,
                        Questions = BsonSerializer.Deserialize<BsonArray>(form.FormStructure)
                    });

                    await _context.SaveChangesAsync();

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw;
                }
            }

            return true;
        }
    }
}