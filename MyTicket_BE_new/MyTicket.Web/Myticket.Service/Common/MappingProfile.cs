using AutoMapper;
using MYTICKET.WEB.DOMAIN.Entities;
using MYTICKET.WEB.SERVICE.AuthModule.Dtos.RoleDto;
using MYTICKET.WEB.SERVICE.AuthModule.Dtos.UserDto;
using MYTICKET.WEB.SERVICE.EventTypeModule.Dtos;
using MYTICKET.WEB.SERVICE.VenueModule.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYTICKET.WEB.SERVICE.Common
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            #region Role Permission
            CreateMap<RoleDto, Role>().ReverseMap();
            CreateMap<CreateRolePermissionDto, Role>().ReverseMap();

            #endregion

            //#region Customer
            //CreateMap<Createcu, Customer>().ReverseMap();
            //CreateMap<BusinessCustomerDto, BusinessCustomer>().ReverseMap();
            //CreateMap<UpdateBusinessCustomerDto, BusinessCustomer>().ReverseMap k
            //();
            //#endregion
            //#region Restaurant
            //CreateMap<CreateRestaurantDto, Restaurant>().ReverseMap();
            //CreateMap<UpdateRestaurantDto, Restaurant>().ReverseMap();
            //CreateMap<RestaurantDto, Restaurant>().ReverseMap();
            //CreateMap<Restaurant, RestaurantDto>()
            //            .ForMember(dest => dest.BusinessCustomer, opt => opt.MapFrom(src => src.BusinessCustomer)).ReverseMap();
            //#endregion
            //CreateMap<>().ReverseMap()
            CreateMap<CreateUserDto, User>().ReverseMap();
            CreateMap<UpdateUserDto, User>().ReverseMap();
            CreateMap<UserDto, User>().ReverseMap();

            //Venue
            CreateMap<CreateVenueDto,Venue>().ReverseMap();
            CreateMap<UpdateVenueDto,Venue>().ReverseMap();
            CreateMap<VenueDto,Venue>().ReverseMap();
            CreateMap<VenueDetailDto,Venue>().ReverseMap();

            //EventType
            CreateMap<EventType, EventTypeDto>().ReverseMap();
        }

    }
}