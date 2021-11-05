using AutoMapper;
using BookStore_API.Contracts;
using BookStore_API.Data;
using BookStore_API.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore_API.Controllers
{
    /// <summary>
    /// Interacts with the books table
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BooksController : Controller
    {
        private readonly IBookRepository _booksRepository;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;

        public BooksController(IBookRepository booksRepository, ILoggerService loggerService, IMapper imapper)
        {
            this._booksRepository = booksRepository;
            this._logger = loggerService;
            this._mapper = imapper;
        }

        /// <summary>
        /// This get all books
        /// </summary>
        /// <returns>List odf books</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBooks()
        {
            var location = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"{location}: Attempt call");
                var books = await _booksRepository.FindeAll();
                var response = _mapper.Map<IList<BookDTO>>(books);
                _logger.LogInfo($"{location}: Attempt successful");

                return Ok(response);
            }
            catch (Exception e)
            {
                return InternalError($"{location}: {e.Message} and {e.InnerException.Message}");
            }
        }

        /// <summary>
        /// Get book by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>One book</returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBook(int id)
        {
            var location = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"{location}: Attempt call dor id = {id}");
                var book = await _booksRepository.FindById(id);
                if (book == null)
                {
                    _logger.LogWarn($"{location}: No book for id = {id}");
                    return NotFound();
                }
                var response = _mapper.Map<BookDTO>(book);
                _logger.LogInfo($"{location}: Attempt successful for id = {id}");

                return Ok(response);
            }
            catch (Exception e)
            {
                return InternalError($"{location}: {e.Message} and {e.InnerException.Message}");
            }
        }

        /// <summary>
        /// Create new book
        /// </summary>
        /// <param name="bookDTO"></param>
        /// <returns>book objecet</returns>
        /// 
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] BookCreateDTO bookDTO)
        {
            var location = GetControllerActionNames();
            try
            {
                if(bookDTO == null)
                {
                    _logger.LogError($"{location}: empty request was submitted");
                    return BadRequest(ModelState);
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError($"{location}: data was incomplete");
                    return BadRequest(ModelState);
                }

                var book = _mapper.Map<Book>(bookDTO);
                var isSuccess = await _booksRepository.Create(book);

                if (!isSuccess)
                {
                    return InternalError($"{location}: Book create faild");
                }
                
                    
                return Created("Create", new { book });
                
            }
            catch (Exception e)
            {
                return InternalError($"{location}: {e.Message} and {e.InnerException.Message}");
            }
        }

        /// <summary>
        /// Update book
        /// </summary>
        /// <param name="id"></param>
        /// <param name="bookUpdate"></param>
        /// <returns></returns>
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, [FromBody] BookUpdateDTO bookUpdate)
        {
            var location = GetControllerActionNames();
            try
            {
                if(id < 0 ||  bookUpdate == null || id != bookUpdate.Id)
                {
                    _logger.LogError($"{location}: update faild with bad data");
                    return BadRequest(ModelState);
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError($"{location}: data was incomplete");
                    return BadRequest(ModelState);
                }

                var isExist = await _booksRepository.IsExists(id);
                if (!isExist)
                {
                    _logger.LogError($"That ID dont exist! id:{id}");
                }

                var book = _mapper.Map<Book>(bookUpdate);
                var isSuccesed = await _booksRepository.Update(book);

                if (!isSuccesed)
                {
                    return InternalError($"{location}: Book create faild");
                }
                return NoContent();
            }
            catch (Exception e)
            {
                return InternalError($"{location}: {e.Message} and {e.InnerException.Message}");
            }
        }

        /// <summary>
        /// This delete user by specyfic id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete (int id)
        {
            var location = GetControllerActionNames();

            if (id < 1)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _logger.LogInfo($"{location}: Delete attempted on record with id: {id}");

                var isExists = await _booksRepository.IsExists(id);
                if (!isExists)
                {
                    _logger.LogError($"{location}: DELETE - Failed to find id {id} ");
                    return NotFound();
                }

                var book = await _booksRepository.FindById(id);
                var isSuccesed = await _booksRepository.Delete(book);

                _logger.LogInfo($"{location}: DELETE - Successed to delete book with id {id} ");
                if (!isSuccesed)
                {
                    return InternalError($"{location}: Book deletd faild with id: {id}");
                }
            }
            catch (Exception e)
            {
                return InternalError($"{e.Message} and {e.InnerException.Message}");
            }

            return Ok();
        }

        private string GetControllerActionNames()
        {
            var controller = ControllerContext.ActionDescriptor.ControllerName;
            var action = ControllerContext.ActionDescriptor.ActionName;

            return $"{controller} - {action}";
        }

        private ObjectResult InternalError(string message)
        {
            _logger.LogError(message);
            return StatusCode(500, "Something wnet wrong");
        }
    }
}
