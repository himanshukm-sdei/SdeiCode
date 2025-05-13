using AutoMapper;
using NaCore.Domain.Clipping;
using NaWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NaWebsite.Common
{
    public class GenericAutoMapperConfigure
    {
        public static Mapper InitializeAutoMapper<TSource, TDestination>()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TSource, TDestination>();
            });

            var mapper = new Mapper(mapperConfig);

            return mapper;
        }
        
    }
}