using AutoMapper;
using JBus.Web.Models;
using JBus.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JBus.Web
{
    public class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            Mapper.CreateMap<BusOperator, BusOperatorViewModel>();

            //Mapper.CreateMap<UserViewModel, User>()
            //    .ForMember(u => u.Roles, opt => opt.Ignore());
            //Mapper.CreateMap<Role, RoleViewModel>();
            //Mapper.CreateMap<RoleViewModel, Role>()
            //    .ForMember(u => u.Permissions, opt => opt.Ignore())
            //    .ForMember(u => u.CreatedAt, opt => opt.Ignore())
            //    .ForMember(u => u.CreatedBy, opt => opt.Ignore());
        }
    }
}