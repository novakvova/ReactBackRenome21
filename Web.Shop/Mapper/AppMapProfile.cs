using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Shop.Data.Entities.Identity;
using Web.Shop.Models;

namespace Web.Shop.Mapper
{
    public class AppMapProfile : Profile
    {
        public AppMapProfile()
        {
            CreateMap<AppUser, UserItemViewModel>()
                .ForMember(x => x.Image, opt => opt.MapFrom(x => "images/" 
                    + (string.IsNullOrEmpty(x.Photo) ? "noimage.jpg" : x.Photo)));
        }
    }
}
