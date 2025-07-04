using dweb.Data;
using dweb.Models;
using Microsoft.AspNetCore.Mvc;

namespace dweb.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReviewController : Controller
{
    private readonly AppDbContext _context;

    public ReviewController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Review>>> GetReviews()
    {
        var reviews = _context.Review.ToList();
        return Ok(reviews);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Review>> GetReview(int id)
    {
        var review = _context.Review. 
            Where(r => r.reviewID == id). 
            FirstOrDefault();

        if (review is not Review)
        {
            return NotFound();
        }
        
        return Ok(review);
    }

    [HttpPost]
    public async Task<ActionResult<Review>> PostReview(Review review)
    {
        _context.Review.Add(review);
        await _context.SaveChangesAsync();
        return Ok(review);
    }

    [HttpPut]
    public async Task<ActionResult<Review>> PutReview(Review review)
    {
        var rev = _context.Review.
            Where(r => r.reviewID == review.reviewID).
            FirstOrDefault();

        if (rev is not Review)
        {
            return NotFound();
        }
        rev.conteudo = review.conteudo;
        _context.SaveChanges();
        return Ok(rev);
    }

    [HttpDelete]
    public async Task<ActionResult<Review>> DeleteReview([FromBody] int id)
    {
        var review = _context.Review.
            Where(r => r.reviewID == id). 
            FirstOrDefault();

        if (review is not Review)
        {
            return NotFound();
        }
        
        _context.Review.Remove(review);
        await _context.SaveChangesAsync();
        return Ok("Review removida com sucesso!");
    }
}