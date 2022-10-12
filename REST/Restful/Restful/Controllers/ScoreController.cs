using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restful.Models;
using Restful.Services;
using System.Data.Common;

namespace Restful.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ScoreController : Controller
	{
		private readonly DummyScoreDb dummyScoreDb;

		public ScoreController(DummyScoreDb dummyScoreDb)
		{
			this.dummyScoreDb = dummyScoreDb;
		}

		[HttpGet]
		public ActionResult<IEnumerable<Score>> Get()
		{
			return Ok(dummyScoreDb.Values);
		}

		[HttpGet("{name}")]
		public ActionResult<string> Get(string name)
		{
			if(dummyScoreDb.TryGetValue(name, out var score))
			{
				return Ok(score);
			}
			return NotFound();
		}

		[HttpPost]
		public IActionResult Post([FromBody] Score score)
		{
			if (score == null)
				return BadRequest();
			if (dummyScoreDb.ContainsKey(score.Name))
				return Conflict();
			dummyScoreDb.Add(score.Name, score);
			return Created(Request.Path + "/" + score.Name, null);
		}

		[HttpPut("{name}")]
		public ActionResult<string> Replace(string name, [FromBody] Score score)
		{
			if (string.IsNullOrWhiteSpace(name))
			{
				return NotFound();
			}
			if (dummyScoreDb.ContainsKey(name))
			{
				dummyScoreDb[name] = score;
				return Ok();
			}
			else
			{
				return NotFound();
			}
		}


		[HttpDelete]
		public IActionResult Delete()
		{
			return Unauthorized();
			dummyScoreDb.Clear();
			return Ok();
		}

		[HttpDelete("{name}")]
		public ActionResult<string> Delete(string name)
		{
			if(dummyScoreDb.ContainsKey(name))
			{
				dummyScoreDb.Remove(name);
				return Ok();
			}
			return Conflict();
		}
	}
}
