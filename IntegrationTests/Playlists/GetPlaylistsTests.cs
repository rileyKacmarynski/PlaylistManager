using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using API;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;

namespace IntegrationTests.Playlists
{
    public class GetPlaylistsTests
    {
        private HttpClient _client;
        private WebApplicationFactory<Startup> _factory;

        [OneTimeSetUp]
        public void Setup()
        {
            _factory = new ApiWebApplicationFactory<Startup>();
            _client = _factory.CreateClient();
        }

        [Test]
        public async Task GetPlaylists_UserIdInQueryString_ReturnsSuccessfully()
        {
            var uri = "/api/playlists?userId=1";
            var res = await _client.GetAsync(uri);

            Assert.That(res.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task GetPlaylists_NameInQueryString_ReturnsSuccessfully()
        {
            var uri = "/api/playlists?name=Test";
            var res = await _client.GetAsync(uri);

            Assert.That(res.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task GetPlaylists_NoQueryString_ReturnBadRequest()
        {
            var uri = "/api/playlists";
            var res = await _client.GetAsync(uri);

            Assert.That(res.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _client.Dispose();
            _factory.Dispose();
        }
    }
}
