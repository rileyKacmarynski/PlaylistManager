using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using NUnit.Framework;

namespace IntegrationTests.Playlists
{
    public class CreatePlaylistTests
    {
        private HttpClient _client;
        private ApiWebApplicationFactory _factory;

        [OneTimeSetUp]
        public void Setup()
        {
            _factory = new ApiWebApplicationFactory();
            _client = _factory.CreateClient();
        }

        [Test]
        public async Task CreatePlaylist_Succeeds_ReturnSuccess()
        {
            var values = new
            {
                name = "Test",
                userId = "1"
            };
            var json = JsonConvert.SerializeObject(values);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var res = await _client.PostAsync("/api/playlists", content);

            var stuff = await res.Content.ReadAsStringAsync();

            Assert.That(res.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _client.Dispose();
            _factory.Dispose();
        }
    }
}
