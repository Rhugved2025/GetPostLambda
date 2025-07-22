    using System.Net;
    using Amazon.DynamoDBv2;
    using Amazon.DynamoDBv2.DataModel;
    using Amazon.Lambda.APIGatewayEvents;
    using Amazon.Lambda.Core;
    using Newtonsoft.Json;
    using LambdaFunction.Models;

    // Required for Lambda to know how to serialize input/output
    [assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

    namespace LambdaFunction
    {
        public class Function
        {
            private static readonly AmazonDynamoDBClient client = new AmazonDynamoDBClient();
            private static readonly DynamoDBContext dbContext = new DynamoDBContext(client);

            public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext contextLambda)
            {
                if (request.HttpMethod == "POST")
                {
                    var student = JsonConvert.DeserializeObject<Student>(request.Body);
                    await dbContext.SaveAsync(student);

                    return new APIGatewayProxyResponse
                    {
                        StatusCode = (int)HttpStatusCode.OK,
                        Body = "Student added successfully!",
                        Headers = new Dictionary<string, string> { { "Content-Type", "text/plain" } }
                    };
                }

                if (request.HttpMethod == "GET")
                {
                    var conditions = new List<ScanCondition>(); // fetch all
                    var allStudents = await dbContext.ScanAsync<Student>(conditions).GetRemainingAsync();

                    return new APIGatewayProxyResponse
                    {
                        StatusCode = (int)HttpStatusCode.OK,
                        Body = JsonConvert.SerializeObject(allStudents),
                        Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
                    };
                }

                return new APIGatewayProxyResponse
                {
                    StatusCode = (int)HttpStatusCode.MethodNotAllowed,
                    Body = "Unsupported HTTP method"
                };
            }
        }
    }
