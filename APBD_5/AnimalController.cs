﻿using Microsoft.AspNetCore.Mvc;

namespace APBD_5;

public static class InMemoryData
{
    public static List<Animal> Animals { get; } = new List<Animal>();
    public static int AnimalIdCounter { get; set; } = 1;
}

[ApiController]
[Route("api/animals")]
public class AnimalController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAll() => Ok(InMemoryData.Animals);

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var animal = InMemoryData.Animals.FirstOrDefault(a => a.Id == id);
        if (animal == null) return NotFound();
        return Ok(animal);
    }

    [HttpPost]
    public IActionResult Create([FromBody] Animal animal)
    {
        animal.Id = InMemoryData.AnimalIdCounter++;
        InMemoryData.Animals.Add(animal);
        return CreatedAtAction(nameof(Get), new { id = animal.Id }, animal);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] Animal updatedAnimal)
    {
        var animal = InMemoryData.Animals.FirstOrDefault(a => a.Id == id);
        if (animal == null) return NotFound();

        animal.Name = updatedAnimal.Name;
        animal.Category = updatedAnimal.Category;
        animal.Weight = updatedAnimal.Weight;
        animal.FurColor = updatedAnimal.FurColor;

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var animal = InMemoryData.Animals.FirstOrDefault(a => a.Id == id);
        if (animal == null) return NotFound();

        InMemoryData.Animals.Remove(animal);
        return NoContent();
    }

    [HttpGet("{id}/visits")]
    public IActionResult GetVisits(int id)
    {
        var animal = InMemoryData.Animals.FirstOrDefault(a => a.Id == id);
        if (animal == null) return NotFound();
        return Ok(animal.Visits);
    }

    [HttpPost("{id}/visits")]
    public IActionResult AddVisit(int id, [FromBody] Visit visit)
    {
        var animal = InMemoryData.Animals.FirstOrDefault(a => a.Id == id);
        if (animal == null) return NotFound();

        visit.Id = animal.Visits.Count > 0 ? animal.Visits.Max(v => v.Id) + 1 : 1;
        animal.Visits.Add(visit);

        return CreatedAtAction(nameof(GetVisits), new { id = animal.Id }, visit);
    }
}