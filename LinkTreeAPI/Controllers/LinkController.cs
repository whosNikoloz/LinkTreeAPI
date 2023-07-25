using LinkTreeAPI.Data;
using LinkTreeAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LinkTreeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LinkController : ControllerBase
    {
        private readonly DataDbContext _context;
        private readonly IConfiguration _configuration;

        public LinkController(DataDbContext context, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
        }
        [HttpPost("UploadUserLink")]
        public async Task<IActionResult> UploadLink(Linkrequest request, string user)
        {
            var User = await _context.Users.FirstOrDefaultAsync(u => u.UserName == user);
            if (User == null)
            {
                return BadRequest();
            }
            var link = new Links
            {
                Name = request.Name,
                Description = request.Description,
                link = request.link
            };
            User.Links.Add(link);
            await _context.SaveChangesAsync();

            return Ok("Link Uploaded");
        }

        [HttpGet("UserLinks")]
        public async Task<IActionResult> UserLinks(string user)
        {
            var userWithLinks = await _context.Users.Include(u => u.Links).FirstOrDefaultAsync(u => u.UserName == user);

            if (userWithLinks == null)
            {
                return BadRequest();
            }

            var links = userWithLinks.Links;

            return Ok(links);
        }


        [HttpPost("UserWithLinks")]
        public async Task<IActionResult> UserWithLinks([FromBody]string email)
        {
            var userWithLinks = await _context.Users.Include(u => u.Links).FirstOrDefaultAsync(u => u.Email == email);

            if (userWithLinks == null)
            {
                return BadRequest();
            }


            return Ok(userWithLinks);
        }

        [HttpPut("GetLink")]
        public async Task<IActionResult> UserLink(string user, int id)
        {
            var userWithLinks = await _context.Users.Include(u => u.Links).FirstOrDefaultAsync(u => u.UserName == user);

            if (userWithLinks == null)
            {
                return BadRequest();
            }

            var links = userWithLinks.Links;
            foreach (var link in links)
            {
                if (link.Id == id)
                {
                    return Ok(link);
                }
            }

            return BadRequest("No link Found");
        }

        [HttpPut("EditLink")]
        public async Task<IActionResult> UserLinkEdit(string username, int linkid, Linkrequest update)
        {
            var userWithLinks = await _context.Users.Include(u => u.Links).FirstOrDefaultAsync(u => u.UserName == username);

            if (userWithLinks == null)
            {
                return BadRequest();
            }

            var links = userWithLinks.Links;
            foreach (var link in links)
            {
                if (link.Id == linkid)
                {
                    link.Description = update.Description;
                    link.link = update.link;
                    link.Name = update.Name;
                    await _context.SaveChangesAsync();
                    return Ok(userWithLinks);
                }
            }

            return BadRequest("No link Found");
        }


        [HttpDelete("DeleteLink")]
        public async Task<IActionResult> UserLinkDelete(string user, int id)
        {
            var userWithLinks = await _context.Users.Include(u => u.Links).FirstOrDefaultAsync(u => u.UserName == user);

            if (userWithLinks == null)
            {
                return BadRequest();
            }

            var links = userWithLinks.Links;
            //ლინკების ლისტს მაძლევს
            foreach (var link in links)
            {
                if (link.Id == id)
                {
                    _context.Links.Remove(link);
                    await _context.SaveChangesAsync();
                    return Ok(userWithLinks);
                }
            }

            return Ok(links);
        }

        [HttpDelete("ClearLink")]
        public async Task<IActionResult> UserLinkClear(string user)
        {
            var userWithLinks = await _context.Users.Include(u => u.Links).FirstOrDefaultAsync(u => u.UserName == user);

            if (userWithLinks == null)
            {
                return BadRequest();
            }

            List<Links> links = userWithLinks.Links;
            int count = links.Count;
            for (int i = 0; i < count; i++)
            {
                _context.Links.Remove(links[0]);
                await _context.SaveChangesAsync();
            }


            return Ok("Cleared");
        }

    }
}
