using System;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace ModelStateOrderRepro.Controllers
{
    [Route("test")]
    [ApiController]
    public class ReproController : ControllerBase
    {
        // PATCH test/unexpected/notaguid will return valid if json patch document is valid. ex. []
        // PATCH test/unexpected/notaguid will return not valid if json patch document is also invalid. ex. {}
        // This is unexpected behavior
        [HttpPatch("unexpected/{id}")]
        public IActionResult PatchUnexpected(Guid id, [FromBody] JsonPatchDocument<SimpleObj> patchDocument)
        {
            return new OkObjectResult(new { ModelState.IsValid, ModelState.ErrorCount });
        }

        // PATCH test/expected/notaguid will return not valid regardless of json patch document validity.
        // This is the expected behavior, but parameter order shouldn't matter.
        [HttpPatch("expected/{id}")]
        public IActionResult PatchExpected([FromBody] JsonPatchDocument<SimpleObj> patchDocument, Guid id)
        {
            return new OkObjectResult(new { ModelState.IsValid, ModelState.ErrorCount });
        }

        // PATCH test/expected2/notaguid will return not valid regardless of body validity.
        [HttpPatch("expected2/{id}")]
        public IActionResult PatchExpected2(Guid id, [FromBody] SimpleObj simpleObj)
        {
            return new OkObjectResult(new { ModelState.IsValid, ModelState.ErrorCount });
        }
    }
}
