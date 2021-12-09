using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TalentHunt.Models;

namespace TalentHunt.ModelView
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<admin, adminv>();
            CreateMap<adminv, admin>();

            CreateMap<eventrequire,eventrequirev>();
            CreateMap<eventrequirev, eventrequire>();

            CreateMap<image, imagev>();
            CreateMap<imagev, image>();

            CreateMap<plan, planv>();
            CreateMap<planv, plan>();

            CreateMap<production,productionv>();
            CreateMap<productionv, production>();

            CreateMap<productionevent,productioneventv>();
            CreateMap<productioneventv,productionevent>();

            CreateMap<rating,ratingv>();
            CreateMap<ratingv,rating>();

            CreateMap<subproduction,subproductionv>();
            CreateMap<subproductionv,subproduction>();

            CreateMap<subuser,subuserv>();
            CreateMap<subuserv,subuser>();

            CreateMap<talent,talentv>();
            CreateMap<talentv,talent>();

            CreateMap<userapply,userapplyv>();
            CreateMap<userapplyv,userapply>();

            CreateMap<userprofile,userprofilev>();
            CreateMap<userprofilev, userprofile>();

            CreateMap<userselect,userselectv>();
            CreateMap<userselectv,userselect>();

            CreateMap<user,userv>();
            CreateMap<userv,user>();

            CreateMap<video,videov>();
            CreateMap<videov,video>();
        }
    }
}