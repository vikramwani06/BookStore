using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using TechLibrary.Domain;
using TechLibrary.Models;
using TechLibrary.Services;
using TechLibrary.Contracts.Requests;

namespace TechLibrary.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BooksController : Controller
    {
        private readonly ILogger<BooksController> _logger;
        private readonly IBookService _bookService;
        private readonly IMapper _mapper;
        private readonly BookRequest _bookRequest;

        public BooksController(ILogger<BooksController> logger, IBookService bookService, IMapper mapper, BookRequest bookRequest)
        {
            _logger = logger;
            _bookService = bookService;
            _mapper = mapper;
            _bookRequest = bookRequest;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Get all books");

            var books = await _bookService.GetBooksAsync();

            var bookResponse = _mapper.Map<List<BookResponse>>(books);

            return Ok(bookResponse);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation($"Get book by id {id}");

            var book = await _bookService.GetBookByIdAsync(id);

            var bookResponse = _mapper.Map<BookResponse>(book);

            return Ok(bookResponse);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook([FromBody] BookRequest bookRequest, int id)
        {
            _logger.LogInformation($"Update book by id {id}");

            var book = await _bookService.GetBookByIdAsync(id);

            var newBook = new Book
            {
                BookId = id,
                Title = bookRequest.Title,
                ISBN = bookRequest.ISBN,
                PublishedDate = bookRequest.PublishedDate,
                ThumbnailUrl = bookRequest.ThumbnailUrl,
                ShortDescr = bookRequest.Descr,
            };

            var updatedBook = await _bookService.UpadteBookByIdAsync(newBook);

            var bookResponse = _mapper.Map<BookResponse>(updatedBook);

            return Ok(bookResponse);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBookAsync([FromBody] BookRequest bookRequest)
        {
            _logger.LogInformation($"Create book");

            var book = new Book
            {
                Title = bookRequest.Title,
                ISBN = bookRequest.ISBN,
                PublishedDate = bookRequest.PublishedDate,
                ThumbnailUrl = bookRequest.ThumbnailUrl,
                ShortDescr = bookRequest.Descr
            };

            var newBook = await _bookService.CreateBookAsync(book);

            var bookResponse = _mapper.Map<BookResponse>(newBook);

            return Ok(bookResponse);
        }
    }
}
