using backend_impar.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace backend_impar.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class CarsController : ControllerBase
    {
        private readonly DataContext ctx;

        public CarsController(DataContext ctx)
        {
            this.ctx = ctx;
        }

        [HttpGet]
        public async Task<Object> Get(int offset = 0, int limit = 12)
        {
            var cars = await this.ctx.cars.Include(car => car.Photo).Skip(offset).Take(limit).ToListAsync();
            var totalCars = await this.ctx.cars.CountAsync();


            return new { cars, totalCars };
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Car>> GetById(int id)
        {
            var car = await this.ctx.cars.Include(car => car.Photo).Where(car => car.Id == id).FirstAsync();
            if (car == null)
                return BadRequest("car not found.");

            return Ok(car);
        }

        [HttpGet("search")]
        public async Task<Object> SearchCar(string name, int offset = 0, int limit = 12)
        {

            var cars = await this.ctx.cars.Include(car => car.Photo).Where(car => car.Name.Contains(name)).Skip(offset).Take(limit).ToListAsync();
            var totalCars = await this.ctx.cars.Where(car => car.Name.Contains(name)).CountAsync();

            return new { cars, totalCars };
        }


        [HttpPost]
        public async Task<ActionResult<Car>> AddCar(Car car)
        {
            this.ctx.cars.Add(car);
            await this.ctx.SaveChangesAsync();

            return CreatedAtAction("Get", new { id = car.Id }, car);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateCar(Car request)
        {
            var carData = await this.ctx.cars.FindAsync(request.Id);

            if (carData == null) return BadRequest("car not found.");

            var photoData = await this.ctx.photos.FindAsync(request.PhotoId);

            carData.Name = request.Name;
            carData.Status = request.Status;
            photoData.Base64 = request.Photo.Base64;

            try
            {
                await this.ctx.SaveChangesAsync();
                return Ok(carData);
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest("car not found.");
            }
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var car = await this.ctx.cars.FindAsync(id);
            if (car == null) return BadRequest("car not found.");

            this.ctx.cars.Remove(car);
            await this.ctx.SaveChangesAsync();
            return Ok("deleted");
        }
    }
}
