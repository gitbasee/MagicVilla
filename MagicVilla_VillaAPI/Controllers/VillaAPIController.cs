using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Logging;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController] // Model validation happens here.
    public class VillaAPIController : ControllerBase
    {
        //private readonly ILogger<VillaAPIController> _logger;
        private readonly ILogging _logger;
        
        public VillaAPIController(ILogging logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(VillaDTO))]
        [ProducesResponseType(404)]
        public ActionResult GetVillas()
        {
            _logger.Log("Getting all villas", "info");
            return Ok(VillaStore.villaList);
        }

        //[HttpGet("id")] works same
        [HttpGet("{id:int}",Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK,Type=typeof(VillaDTO))] // you can mention the return type here also
        [ProducesResponseType(StatusCodes.Status404NotFound)] // you can use StatusCodes class or just hardcode the code as below
        [ProducesResponseType(400)]
        public ActionResult<VillaDTO> GetVilla(int id)
        {
            if (id==0)
            {
                _logger.Log("ID is passed as null", "error");
                return BadRequest();
            }
            var villa = VillaStore.villaList.FirstOrDefault(item => item.ID == id);
            if (villa==null)
            {
                return NotFound();
            }
            _logger.Log(JsonConvert.SerializeObject(villa), "info");
            //_logger.LogInformation(JsonConvert.SerializeObject(villa));
            return Ok(villa);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VillaDTO))] // you can mention the return type here also
        [ProducesResponseType(StatusCodes.Status404NotFound)] // you can use StatusCodes class or just hardcode the code as below
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public ActionResult<VillaDTO> CreateVilla([FromBody]VillaDTO villaDTO)
        {
            /*if (!ModelState.IsValid) // This code will not be reached if the [ApiController] annotation in there.
            {
                return BadRequest(ModelState);
            }*/
            if (VillaStore.villaList.FirstOrDefault(item => item.Name.ToLower() == villaDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "Villa already Exisits!!!!");
                return BadRequest(ModelState);
            }
            if (villaDTO==null)
            {
                return BadRequest();
            }
            if (villaDTO.ID > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            villaDTO.ID = VillaStore.villaList.OrderByDescending(item => item.ID).FirstOrDefault().ID + 1;  
            VillaStore.villaList.Add(villaDTO);
            return CreatedAtRoute("GetVilla",new { id = villaDTO.ID},villaDTO);
        }

        [HttpDelete("{id:int}", Name ="DeleteVilla")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult DeleteVilla(int id) {
            if (id == 0)
            {
                return BadRequest();  
            }
            var villa = VillaStore.villaList.FirstOrDefault(item => item.ID == id);
            if (villa==null)
            {
                return NotFound();
            }
            VillaStore.villaList.Remove(villa);
            return StatusCode(StatusCodes.Status204NoContent);
        }

        [HttpPut]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateVilla(int id, [FromBody] VillaDTO villaDTO)
        {
            if (villaDTO ==null || id !=villaDTO.ID)
            {
                return BadRequest();
            }
            var villa = VillaStore.villaList.FirstOrDefault(item=>item.ID==id);
            if (villa == null)
            {
                return NotFound();

            }
            villa.Name = villaDTO.Name;
            villa.Sqft = villaDTO.Sqft;
            villa.Occupancy = villaDTO.Occupancy;
            return Ok(villa);
        }

        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }
            var villa = VillaStore.villaList.FirstOrDefault(item => item.ID == id);
            if (villa==null)
            {
                return NotFound();
            }
            patchDTO.ApplyTo(villa, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(villa);
        }
    }
}
