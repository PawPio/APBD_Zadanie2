using APBD_Zadanie_4.DTOs;
using APBD_Zadanie_4.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Exercise3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimalsController : ControllerBase
    {
        private readonly IAnimalRepo _animalRepo;

        public AnimalsController(IAnimalRepo animalRepo)
        {
            _animalRepo = animalRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAnimalsAsync(string? orderBy)
        {
            if (orderBy is null) 
            { 
                orderBy = "name";
            }
            else
            {
                string[] acceptedOrdersBy = { "name", "description", "category", "area" };
                if (!acceptedOrdersBy.Contains(orderBy))
                {
                    orderBy = "name";
                }
            }

            var animals = await _animalRepo.GetSync(orderBy);

            return Ok(animals);
        }

        [HttpPost]
        public async Task<IActionResult> AddAnimal(AnimalPost animalPost)
        {
            var animalExists = await _animalRepo.DoesExist(animalPost.ID);
            if (animalExists)
            {
                return Conflict("Animal with this ID does exist");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _animalRepo.AddAnimal(animalPost);
            return Created("api/animals", animalPost);
        }

        [HttpPut]
        [Route("{animalID}")]
        public async Task<IActionResult> UpdateAnimal(int animalID, [FromBody] AnimalPut animalPut)
        {
            var animalExists = await _animalRepo.DoesExist(animalID);
            if (!animalExists)
            {
                return NotFound("Animal does not exists");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _animalRepo.UpdateAnimal(animalID, animalPut);
            return Ok(animalPut);
        }

        [HttpDelete]
        [Route("{animalID}")]
        public async Task<IActionResult> DeleteAnimal(int animalID)
        {
            var animalExists = await _animalRepo.DoesExist(animalID);
            if (!animalExists)
            {
                return NotFound("Animal does not exists");
            }
            await _animalRepo.DeleteAnimal(animalID);
            return Ok("Animal was correctly removed");
        }
    }
}