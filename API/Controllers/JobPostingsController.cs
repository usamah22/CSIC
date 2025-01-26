/*using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class JobPostingsController : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<List<JobPostingDto>>> GetJobPostings([FromQuery] GetJobPostingsQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("recent")]
    public async Task<ActionResult<List<JobPostingDto>>> GetRecentJobPostings([FromQuery] int count = 5)
    {
        return await Mediator.Send(new GetRecentJobPostingsQuery { Count = count });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<JobPostingDetailDto>> GetJobPosting(Guid id)
    {
        var result = await Mediator.Send(new GetJobPostingByIdQuery { Id = id });
        
        if (result == null)
            return NotFound();
            
        return result;
    }

    [HttpGet("types/{type}")]
    public async Task<ActionResult<List<JobPostingDto>>> GetJobPostingsByType(JobType type)
    {
        return await Mediator.Send(new GetJobPostingsByTypeQuery { Type = type });
    }

    [HttpGet("company/{companyName}")]
    public async Task<ActionResult<List<JobPostingDto>>> GetJobPostingsByCompany(string companyName)
    {
        return await Mediator.Send(new GetJobPostingsByCompanyQuery { CompanyName = companyName });
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Professional")]
    public async Task<ActionResult<Guid>> Create(CreateJobPostingCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Professional")]
    public async Task<ActionResult> Update(Guid id, UpdateJobPostingCommand command)
    {
        if (id != command.Id)
            return BadRequest();

        await Mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Delete(Guid id)
    {
        await Mediator.Send(new DeleteJobPostingCommand { Id = id });
        return NoContent();
    }
}*/