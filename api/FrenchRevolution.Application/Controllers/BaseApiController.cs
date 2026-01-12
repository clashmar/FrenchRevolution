using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FrenchRevolution.Application.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseApiController : ControllerBase;