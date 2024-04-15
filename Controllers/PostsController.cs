﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reddit;
using Reddit.Dtos;
using Reddit.Mapper;
using Reddit.Models;
using Reddit.Repositories;
using Reddit.Requests;

namespace Reddit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : Controller
    {
        private readonly ApplcationDBContext _context;
        private readonly IMapper _mapper;
        private readonly IPostsRepository _postsRepository;
        public PostsController(ApplcationDBContext context, IMapper mapper, IPostsRepository postsRepository)
        {
            _context = context;
            _mapper = mapper;
            _postsRepository = postsRepository;
        }


        // GET: api/Posts
        [HttpGet]
        public async Task<IActionResult> GetPosts(GetPostsRequest getPostsRequest)
        {
            var pL = await _postsRepository.GetAll(getPostsRequest);
            return View(pL);
        }

        // GET: api/Posts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> GetPost(int id)
        {
            var post = await _context.Posts.FindAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            return post;
        }

        // PUT: api/Posts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPost(int id, Post post)
        {
            if (id != post.Id)
            {
                return BadRequest();
            }

            _context.Entry(post).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Posts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Post>> PostPost(CreatePostDto createPostDto)
        {
            var post = _mapper.toPost(createPostDto);

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPost", new { id = post.Id }, post);
        }

        // DELETE: api/Posts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }
    }
}
