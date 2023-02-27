using AuthServer.Core.Models;
using AuthServer.Core.Repository;
using AuthServer.Core.Service;
using AuthServer.Core.UnitOfWork;
using AuthServer.Service.Mapping;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Service.Services
{
    public class ServiceGeneric<TEntity, TDto> : IServiceGeneric<TEntity, TDto> where TEntity : class where TDto : class
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<TEntity> _genericRepository;

        public ServiceGeneric(IUnitOfWork unitOfWork, IGenericRepository<TEntity> genericRepository)
        {
            _genericRepository = genericRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<TDto>> AddAsync(TDto entity)
        {
            var newEntity = ObjectMapper.Mapper.Map<TEntity>(entity);

            await _genericRepository.AddAsync(newEntity);
            await _unitOfWork.CommmitAsync();

            var dtoEntity = ObjectMapper.Mapper.Map<TDto>(entity);
            return Response<TDto>.Success(dtoEntity, 200);
        }

        public async Task<Response<IEnumerable<TDto>>> GetAllAsync()
        {
            var products = ObjectMapper.Mapper.Map<List<TDto>>(await _genericRepository.GetAllAsync());
            return Response<IEnumerable<TDto>>.Success(products, 200);
        }

        public async Task<Response<TDto>> GetByIdAsync(int id)
        {
            var entity = ObjectMapper.Mapper.Map<List<TDto>>(await _genericRepository.GetByIdAsync(id));

            if (entity == null)
                return Response<TDto>.Fail("Id Not Found", 404, true);
            
            var dtoEntity = ObjectMapper.Mapper.Map<TDto>(entity);
            return Response<TDto>.Success(dtoEntity, 200);
        }

        public async Task<Response<NoDataDto>> Remove(int id)
        {
            var entity = await _genericRepository.GetByIdAsync(id);

            if (entity == null)
                return Response<NoDataDto>.Fail("Id Not Found", 404, true);

            _genericRepository.Remove(entity);
            await _unitOfWork.CommmitAsync();

            //204 durum kodu =>  No Content  => Response body'sinde hiç bir data  olmayacak.
            return Response<NoDataDto>.Success(204);
        }

        public async Task<Response<NoDataDto>> Update(TDto entity, int id)
        {
            var isExistEntity = await _genericRepository.GetByIdAsync(id);

            if (isExistEntity == null)
                return Response<NoDataDto>.Fail("Id Not Found", 404, true);

            var updateEntity = ObjectMapper.Mapper.Map<TEntity>(entity);
            _genericRepository.Update(updateEntity);
            await _unitOfWork.CommmitAsync();

            return Response<NoDataDto>.Success(204);
        }

        public async Task<Response<IEnumerable<TDto>>> Where(Expression<Func<TEntity, bool>> predicate)
        {
            // where(x=>x.id>5) -  Take(8) ilk 8 tanesini alır.
            var list = _genericRepository.Where(predicate);
            
            // Db'ye ToListAsync deyince yansıyor cekiyor verileri
            var dtoEntity = ObjectMapper.Mapper.Map<IEnumerable<TDto>>(await list.ToListAsync());

            return Response<IEnumerable<TDto>>.Success(dtoEntity, 200);
        }

    }
}
