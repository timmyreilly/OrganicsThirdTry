using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;


namespace OrganicsThirdTry
{
    public class CosmosService
    {
        public async Task<Rating> CreateRatingFromDocument(dynamic data, IAsyncCollector<Rating> document, HttpClient client, ILogger logger)
        {
            // Set unique id
            var id = Guid.NewGuid();

            // Set Timestamp
            var timestamp = DateTime.UtcNow;

            var rand = new Random();

            var rating = new Rating
            {
                id = id,
                userId = data.userId,
                productId = data.productId,
                timestamp = timestamp,
                locationName = data.locationName,
                rating = data.rating,
                userNotes = data.userNotes
                // ,
                // magicNumber = rand.Next(),
                // sentimentScore = sentimentScore
            };

            await document.AddAsync(rating);

            return rating;
        }

        public Rating CreateRating(dynamic data)
        {
            var id = Guid.NewGuid();
            var timestamp = DateTime.UtcNow;
            var rand = new Random();

            var rating = new Rating
            {
                id = id,
                userId = data.userId,
                productId = data.productId,
                timestamp = timestamp,
                locationName = data.locationName,
                rating = data.rating,
                userNotes = data.userNotes
                // ,
                // magicNumber = rand.Next(),
                // sentimentScore = sentimentScore
            };

            return rating; 
        }

        public DocumentClient GetClient(string endpointUrl, string authorizationKey)
        {
            var documentClient = new DocumentClient(new Uri(endpointUrl), authorizationKey);
            return documentClient;
        }


    }
}
