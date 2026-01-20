using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace FrenchRevolution.Application.Controllers;

[ApiController]
[ApiVersion(1)]
[Route("api/v{version:apiVersion}/[controller]")]
public abstract class BaseApiController : ControllerBase;