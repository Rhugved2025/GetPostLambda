using Amazon.DynamoDBv2.DataModel;

namespace LambdaFunction.Models
{
    [DynamoDBTable("Students")]
    public class Student
    {
        [DynamoDBHashKey(AttributeName = "email")] // Partition Key
        public string Email { get; set; }

        [DynamoDBProperty]
        public string Name { get; set; }

        [DynamoDBProperty]
        public int Age { get; set; }
    }
}
