using AutoMapper;
using GeekShooping.ProductAPI.Data.ValueObjects;
using GeekShooping.ProductAPI.Model;
using GeekShooping.ProductAPI.Model.Context;
using Microsoft.EntityFrameworkCore;

namespace GeekShooping.ProductAPI.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly MySqlContext _db;

        private readonly IMapper _mapper;
        public ProductRepository(IMapper mapper,MySqlContext context  )
        {
            _db = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductVO>> FindAll()
        {
            return _mapper.Map<List<ProductVO>>(await _db.Products.ToListAsync());
        }
        public async Task<ProductVO> FindById(long id)
        {
            return _mapper.Map<ProductVO>(await _db.Products.Where(p => p.Id == id).FirstOrDefaultAsync());
        }
        public async Task<ProductVO> Create(ProductVO vo)
        {
            var product = _mapper.Map<Product>(vo);
            await _db.Products.AddAsync(product);
            await _db.SaveChangesAsync();
            return _mapper.Map<ProductVO>(product);
           
        }
        public async Task<ProductVO> Update(ProductVO vo)
        {
            var product = _mapper.Map<Product>(vo);
             _db.Products.Update(product);
            await _db.SaveChangesAsync();
            return _mapper.Map<ProductVO>(product);
        }
        public async Task<bool> Delete(long id)
        {
            try
            {
               var produt =  await _db.Products.Where(p => p.Id == id).FirstOrDefaultAsync();


                if(produt != null)
                {

                    _db.Products.Remove(produt);
                    await _db.SaveChangesAsync();
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
