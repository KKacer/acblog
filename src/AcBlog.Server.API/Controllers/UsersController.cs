﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AcBlog.Server.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        IUserRepository Provider { get; }

        public UsersController(IUserRepository provider)
        {
            Provider = provider;
        }

        [HttpGet("actions/read")]
        public async Task<ActionResult<bool>> CanRead()
        {
            return await Provider.CanRead();
        }

        [HttpGet("actions/write")]
        public async Task<ActionResult<bool>> CanWrite()
        {
            return await Provider.CanWrite();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<IEnumerable<string>>> All()
        {
            if (!await Provider.CanRead())
                return BadRequest();
            return Ok(await Provider.All());
        }

        [HttpPut("query")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<QueryResponse<string>>> Query([FromBody] UserQueryRequest query)
        {
            if (!await Provider.CanRead())
                return BadRequest();
            return Ok(await Provider.Query(query));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<User>> Get(string id)
        {
            if (!await Provider.CanRead())
                return BadRequest();
            if (await Provider.Exists(id))
                return Ok(await Provider.Get(id));
            else
                return NotFound();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<string>> Create([FromBody] User value)
        {
            if (!await Provider.CanWrite())
                return BadRequest();
            var result = await Provider.Create(value);
            return Created(result, result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<bool>> Update(string id, [FromBody] User value)
        {
            if (!await Provider.CanWrite())
                return BadRequest();
            value.Id = id;
            if (await Provider.Exists(value.Id))
                return Ok(await Provider.Update(value));
            else
                return NotFound();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<bool>> Delete(string id)
        {
            if (!await Provider.CanWrite())
                return BadRequest();
            if (await Provider.Exists(id))
                return Ok(await Provider.Delete(id));
            else
                return NotFound();
        }
    }
}
