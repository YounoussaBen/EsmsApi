﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceManagerApi.Interfaces;

namespace ServiceManagerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaeApiController<T> : ControllerBase
    {
        private ILogger<T> _loggerInstance;
        private IMapper _mapperInstance;
        private IUnitOfWork _unitOfWorkInstance;
        
        protected IUnitOfWork Get_UnitOfWork()
        {
            return _unitOfWorkInstance ??= HttpContext.RequestServices.GetService<IUnitOfWork>();
        }
        protected ILogger<T> _logger => _loggerInstance ??= HttpContext.RequestServices.GetService<ILogger<T>>();
        protected IMapper _mapper => _mapperInstance ??= HttpContext.RequestServices.GetService<IMapper>();
    }
}
