﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace PartyApi.Controllers
{
    [ApiController]
    [Route("party")]
    public class PartyController : ControllerBase
    {
        private static readonly string[] PartyStatuses = new[]
        {
            "talking", "dancing", "jumping", "drinking", "sleeping", "seducing", "laughing", "crying", "singing"
        };

        private readonly ILogger<PartyController> _logger;

        public PartyController(ILogger<PartyController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("entrance")]
        public PartyExperience GetIntoTheEntrance()
        {
            return new PartyExperience
            {
                Date = DateTime.Now,
                Location = "entrance",
                Status = PartyStatuses[new Random().Next(PartyStatuses.Length)],
                AdditionalInfo = "none"
            };
        }

        [HttpGet]
        [Route("bar")]
        public PartyExperience GetIntoTheBar()
        {
            return new PartyExperience
            {
                Date = DateTime.Now,
                Location = "bar",
                Status = PartyStatuses[new Random().Next(PartyStatuses.Length)],
                AdditionalInfo = "none"
            };
        }

        [HttpGet]
        [Route("dancefloor")]
        public PartyExperience GetIntoTheDancefloor()
        {
            return new PartyExperience
            {
                Date = DateTime.Now,
                Location = "dancefloor",
                Status = PartyStatuses[new Random().Next(PartyStatuses.Length)],
                AdditionalInfo = "none"
            };
        }

        [HttpGet]
        [Authorize(Roles = "vip")]
        [Route("vip")]
        public PartyExperience GetIntoTheVip()
        {
            return new PartyExperience
            {
                Date = DateTime.Now,
                Location = "vip",
                Status = PartyStatuses[new Random().Next(PartyStatuses.Length)],
                AdditionalInfo = "none"
            };
        }

        [HttpGet]
        [Route("private-party")]
        public PartyExperience GetIntoThePrivateParty()
        {
            HttpContext.VerifyUserHasAnyAcceptedScope("access_private_party");

            return new PartyExperience
            {
                Date = DateTime.Now,
                Location = "private-party",
                Status = PartyStatuses[new Random().Next(PartyStatuses.Length)],
                AdditionalInfo = "none"
            };
        }
    }
}
