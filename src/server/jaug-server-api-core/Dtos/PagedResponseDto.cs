using System;
using System.Collections.Generic;
using AutoMapper;
using jaug_server_api_core.Controllers;
using jaug_server_api_core.Data.Repositories;

namespace jaug_server_api_core.Dtos
{
    public class PagedResponseDto<TprEntity, TprDto> : ResponseDto<IEnumerable<TprDto>>
    {
        public static PagedResponseDto<TprEntity, TprDto> Create(IMapper mapper, PagedList<TprEntity> pagedEntityList, ToolsResourceParameters validFilter, IUriService uriService, string route)
        {
            var data = mapper.Map<IEnumerable<TprDto>>(pagedEntityList);
            var response = new PagedResponseDto<TprEntity, TprDto>(mapper, data, validFilter.PageNumber, validFilter.PageSize);
            response.NextPgNum = validFilter.PageNumber >= 1 && validFilter.PageNumber < pagedEntityList.TotalPages ? validFilter.PageNumber + 1 : 0;
            response.PrevPgNum = validFilter.PageNumber - 1 >= 1 && validFilter.PageNumber <= pagedEntityList.TotalPages ? validFilter.PageNumber - 1 : 0;
            response.FirstPgNum = pagedEntityList.TotalPages > 0 ? 1 : 0;
            response.LastPgNum = pagedEntityList.TotalPages;
            
            //!!! if implementing creation of link need to ensure includes any other params not related to paging
            //response.NextPage =
            //    validFilter.PageNumber >= 1 && validFilter.PageNumber < pagedEntityList.TotalPages
            //    ? uriService.GetPageUri(new PagedResourceParameters() { PageNumber = validFilter.PageNumber + 1, PageSize = validFilter.PageSize }, route)
            //    : null;
            //response.PreviousPage =
            //    validFilter.PageNumber - 1 >= 1 && validFilter.PageNumber <= pagedEntityList.TotalPages
            //    ? uriService.GetPageUri(new PagedResourceParameters() { PageNumber = validFilter.PageNumber - 1, PageSize = validFilter.PageSize }, route)
            //    : null;
            //response.FirstPage = uriService.GetPageUri(new PagedResourceParameters() { PageNumber = 1, PageSize = validFilter.PageSize }, route);
            //response.LastPage = uriService.GetPageUri(new PagedResourceParameters() { PageNumber = pagedEntityList.TotalPages, PageSize = validFilter.PageSize }, route);
            response.TotalPages = pagedEntityList.TotalPages;
            response.TotalObjCount = pagedEntityList.TotalCount;
            
            return response;
        }

        public int CurrentPgNum { get; set; }
        public int FirstPgNum { get; set; }
        public int LastPgNum { get; set; }
        public int NextPgNum { get; set; }
        public int PrevPgNum { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalObjCount { get; set; }

        //public Uri FirstPage { get; set; }
        //public Uri LastPage { get; set; }
        //public Uri NextPage { get; set; }
        //public Uri PreviousPage { get; set; }

        public PagedResponseDto(IMapper mapper, IEnumerable<TprDto> data, int pageNumber , int pageSize)
        {
            this.CurrentPgNum = pageNumber;
            this.PageSize = pageSize;
            this.Data = data;
            this.Message = null;
            this.Succeeded = true;
            this.Errors = null;
        }
    }

}
