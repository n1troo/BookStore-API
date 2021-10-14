using AutoMapper;
using BookStore_API.Contracts;
using BookStore_API.Data;
using BookStore_API.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore_API.Controllers
{
    /// <summary>
    /// Endpoint used to insteract with the Authors in the book store's database
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;
        public AuthorsController(IAuthorRepository authorRepository, ILoggerService logger, IMapper mapper)
        {
            _authorRepository = authorRepository;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all authors
        /// </summary>
        /// <returns>List of authors</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAuthors()
        {
            try
            {
                _logger.LogInfo("Get authors started");
                var authors = await _authorRepository.FindeAll();
                var response = _mapper.Map<IList<AuthorDTO>>(authors);
                _logger.LogInfo("Successed got all authors");
               
                return Ok(response);
            }
            catch (Exception e)
            {
                return InternalError($"{e.Message} and {e.InnerException.Message}");
            }
        }


        /// <summary>
        /// Get one authors by specyfic ID
        /// </summary>
        /// <returns>one of authors</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuthor(int id)
        {
            try
            {
                _logger.LogInfo($"Get author by iD {id}");
                var author = await _authorRepository.FindById(id);
                if(author == null)
                {
                    _logger.LogWarn($"Author not found {id}");
                    return NotFound();
                }
                var response = _mapper.Map<AuthorDTO>(author);
                _logger.LogInfo("Successed author");

                return Ok(response);
            }
            catch (Exception e)
            {
                return InternalError($"{e.Message} and {e.InnerException.Message}");
            }
        }

        private ObjectResult InternalError (string message)
        {
            _logger.LogError(message);
            return StatusCode(500, "Something wnet wrong");
        }
    }
}