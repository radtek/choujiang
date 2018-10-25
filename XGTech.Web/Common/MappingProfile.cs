using AutoMapper;
using XGTech.Model.Model;
using XGTech.Model.RequestModel;
using XGTech.Model.ResponseModel;

namespace XGTech.Web.Common
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Tbl_PrizeSetting, PrizeSettingRequestModel>();
            CreateMap<PrizeSettingRequestModel, Tbl_PrizeSetting>();

            CreateMap<Tbl_PrizeSetting, PrizeSettingExportResponseModel>();
            CreateMap<PrizeSettingExportResponseModel, Tbl_PrizeSetting>();
        }
    }
}
