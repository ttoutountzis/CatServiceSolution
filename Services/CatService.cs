using CatServiceAPI.Data;
using CatServiceAPI.Models;
using Newtonsoft.Json;
using System;
using System.Net;

namespace CatServiceAPI.Services
{
    public class CatService
    {
        private readonly AppDbContext _context;
        private readonly HttpClient _httpClient;

        public CatService(AppDbContext context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
        }

        public async Task FetchAndStoreCatsAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<CatApiResponse>>("v1/images/search?limit=25&has_breeds=true");

                foreach (var cat in response)
                {
                    var catId = cat.Id;
                    var width = cat.Width;
                    var height = cat.Height;
                    var url = cat.Url;
                    var breeds = cat.Breeds?.FirstOrDefault()?.Temperament?.Split(',');

                    if (_context.Cats.Any(c => c.CatId == catId)) continue;

                    var imageData = await _httpClient.GetByteArrayAsync(url);

                    var catEntity = new CatEntity
                    {
                        CatId = catId,
                        Width = width,
                        Height = height,
                        Image = imageData
                    };

                    _context.Cats.Add(catEntity);

                    if (breeds != null)
                    {
                        foreach (var breed in breeds)
                        {
                            var trimmedBreed = breed.Trim();
                            var tag = _context.Tags.FirstOrDefault(t => t.Name == trimmedBreed)
                                      ?? new TagEntity { Name = trimmedBreed };

                            if (tag.Id == 0) _context.Tags.Add(tag);

                            _context.CatTags.Add(new CatTag { Cat = catEntity, Tag = tag });
                        }
                    }
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching or storing cats: {ex.Message}");
                throw;
            }
        }
    }
}
