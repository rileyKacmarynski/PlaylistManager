using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using API;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using NUnit.Framework;

namespace IntegrationTests.Playlists
{
    public class CreatePlaylistTests
    {
        private HttpClient _client;
        private WebApplicationFactory<Startup> _factory;

        [OneTimeSetUp]
        public void Setup()
        {
            _factory = new ApiWebApplicationFactory<Startup>();
            _client = _factory.CreateClient();
        }
        private async Task<HttpResponseMessage> PostData(object values)
        {
            var json = JsonConvert.SerializeObject(values);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var res = await _client.PostAsync("/api/playlists", content);
            return res;
        }
        
        [Test]
        public async Task CreatePlaylist_Succeeds_ReturnSuccess()
        {
            var values = new
            {
                name = "Test",
                userId = "1"
            };
            var res = await PostData(values);

            Assert.That(res.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task CreatePlaylist_EmptyName_ReturnBadRequest()
        {
            var values = new
            {
                name = string.Empty,
                userId = "1"
            };

            var res = await PostData(values);

            Assert.That(res.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task CreatePlaylist_UserNotFound_ReturnNotFound()
        {
            var values = new
            {
                name = "Test",
                userId = "12"
            };

            var res = await PostData(values);

            Assert.That(res.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _client.Dispose();
            _factory.Dispose();
        }
    }
}
