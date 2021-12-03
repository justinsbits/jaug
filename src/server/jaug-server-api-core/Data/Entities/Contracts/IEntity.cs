using System;

namespace jaug_server_api_core.Data.Entities.Contracts
{
    interface IEntity
    {
        int Id { get; set; }
        string CreatedBy { get; set; }
        DateTime CreatedOn { get; set; }
        string LastModifiedBy { get; set; }
        DateTime? LastModifiedOn { get; set; }
    }
}
