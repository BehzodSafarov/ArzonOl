using ArzonOL.Dtos.ProductVoterDtos;
using ArzonOL.Models;
using ArzonOL.Repositories.Interfaces;
using ArzonOL.Services.VoterService.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ArzonOL.Controllers.Voter
{
    [ApiController]
    [Route("api/[controller]")]
    public class VoterController : ControllerBase
    {
        private readonly ILogger<VoterController> _logger;
        private readonly IVoterService _voterService;

        public VoterController(ILogger<VoterController> logger, IVoterService voterService)
        {
            _logger = logger;
            _voterService = voterService;
        }

        [HttpPost("Create")]
        public async Task<ActionResult<ProductVoterModel>> CreateProductVoter(VoterDto productVoterdto)
        {
            try
            {
                var createdVoter = await _voterService.CreateAsync(await _voterService.MupDtoToModel(productVoterdto));
                return Created($"/api/Voter/{createdVoter.UserId}", createdVoter);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, "Product voter model cannot be null.");
                return BadRequest("Product voter model cannot be null.");
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Invalid input data.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the product voter.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the product voter.");
            }
        }

        [HttpPut("UpdateForVote")]
        public async Task<ActionResult<ProductVoterModel>> UpdateForVote(VoterDto productVoterdto)
        {
            try
            {
                var updatedVoter = await _voterService.UpdateForVoteAsync(await _voterService.MupDtoToModel(productVoterdto));
                return Ok(updatedVoter);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, "Product voter model cannot be null.");
                return BadRequest("Product voter model cannot be null.");
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Invalid input data.");
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, "Product voter not found.");
                return NotFound("Product voter not found.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the product voter.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the product voter.");
            }
        }

        [HttpPut("UpdateForComment")]
        public async Task<ActionResult<ProductVoterModel>> UpdateForComment(VoterDto productVoterdto)
        {
            try
            {
                var updatedVoter = await _voterService.UpdateForCommentAsync(await _voterService.MupDtoToModel(productVoterdto));
                return Ok(updatedVoter);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, "Product voter model cannot be null.");
                return BadRequest("Product voter model cannot be null.");
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Invalid input data.");
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, "Product voter not found.");
                return NotFound("Product voter not found.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the product voter.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the product voter.");
            }
        }

        [HttpDelete("DeleteComment")]
        public async Task<ActionResult<ProductVoterModel>> DeleteComment(VoterDto productVoterdto)
        {
            try
            {
                var updatedVoter = await _voterService.DeleteCommentAsync(await _voterService.MupDtoToModel(productVoterdto));
                return Ok(updatedVoter);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, "Product voter model cannot be null.");
                return BadRequest("Product voter model cannot be null.");
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Invalid input data.");
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, "Product voter not found.");
                return NotFound("Product voter not found.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the product voter comment.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the product voter comment.");
            }
        }

        [HttpDelete("DeleteVote")]
        public async Task<ActionResult<ProductVoterModel>> DeleteVote(VoterDto productVoterdto)
        {
            try
            {
                var updatedVoter = await _voterService.DeleteVoteAsync(await _voterService.MupDtoToModel(productVoterdto));
                return Ok(updatedVoter);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, "Product voter model cannot be null.");
                return BadRequest("Product voter model cannot be null.");
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Invalid input data.");
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, "Product voter not found.");
                return NotFound("Product voter not found.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the product voter vote.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the product voter vote.");
            }
        }

    }
}
