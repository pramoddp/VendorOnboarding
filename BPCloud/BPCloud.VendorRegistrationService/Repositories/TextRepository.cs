using BPCloud.VendorRegistrationService.DBContexts;
using BPCloud.VendorRegistrationService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud.VendorRegistrationService.Repositories
{
    public class TextRepository : ITextRepository
    {
        private readonly RegistrationContext _dbContext;

        public TextRepository(RegistrationContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<BPText> GetAllTexts()
        {
            try
            {
                return _dbContext.BPTexts.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BPText> CreateText(BPText Text)
        {
            try
            {
                Text.IsActive = true;
                Text.CreatedOn = DateTime.Now;
                var result = _dbContext.BPTexts.Add(Text);
                await _dbContext.SaveChangesAsync();
                return Text;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BPText> UpdateText(BPText Text)
        {
            try
            {
                var entity = _dbContext.Set<BPText>().FirstOrDefault(x => x.TextID == Text.TextID);
                if (entity == null)
                {
                    return entity;
                }
                //_dbContext.Entry(Text).State = EntityState.Modified;
                entity.Text = Text.Text;
                entity.ModifiedBy = Text.ModifiedBy;
                entity.ModifiedOn = DateTime.Now;
                await _dbContext.SaveChangesAsync();
                return Text;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BPText> DeleteText(BPText Text)
        {
            try
            {
                //var entity = await _dbContext.Set<BPText>().FindAsync(Text.Text, Text.Language);
                var entity = _dbContext.Set<BPText>().FirstOrDefault(x => x.TextID == Text.TextID);
                if (entity == null)
                {
                    return entity;
                }

                _dbContext.Set<BPText>().Remove(entity);
                await _dbContext.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
